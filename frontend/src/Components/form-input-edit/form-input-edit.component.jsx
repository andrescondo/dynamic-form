import { useState, useEffect } from 'react';
import axios from 'axios';

import './../../App.css'

const initialInput = {
  id: '',
  inputs: []
}
/* Todo el código para crear y editar los inputs (campos) del formulario debe colocarse en la siguiente ruta
y nombre de archivo */
const FormInputEditComponent = ({ id }) => {
  // eslint-disable-next-line
  const [inputs, setInputs] = useState(initialInput);
  const [containers, setContainers] = useState([]);


  useEffect(() => {
    // eslint-disable-next-line
    getDataForm();
  }, [])

// eslint-disable-next-line
  const getDataForm = async () => {
    try {
      const res = await axios.get(`https://localhost:7048/Manage/form/${id}/0`)
        .catch(err => {
          console.log(err);
        })

      // setForm(res.data.data)
      const data = res.data.data.map(item => ({
        ID: item.ID,
        name: item.InputsName,
        type: item.InputsType,
        IsActive: item.IsActive,
        IsDeleted: item.IsDeleted,
        IDForm: item.IDForm
      }));

      setContainers(data);


    } catch (error) {
      console.log(error)
    }
  }


  const handleAddContainer = () => {
    setContainers([...containers, { name: '', type: '', IsActive: true }]);
  };

  const handleRemoveContainer = (index) => {
    const newContainers = containers.filter((_, i) => i !== index);
    setContainers(newContainers);
  };

  const handleChangeContainer = (index, type, value) => {
    const newContainers = containers.map((container, i) => {
      if (i === index) {
        return { ...container, [type]: value };
      }
      return container;
    });
    setContainers(newContainers);
  };
  const handleCheckboxChange = (index, value) => {
    const newContainers = containers.map((container, i) => {
      if (i === index) {
        return { ...container, IsActive: value };
      }
      return container;
    });
    setContainers(newContainers);
  };

  const showConfirmChangeIsActive = (status, index, value) => {
    // eslint-disable-next-line no-alert
    const userConfirmed = window.confirm(`¿Seguro desea ${status ? 'Desactivar' : 'Activar'} este campo? \nLos usuarios ${status ? 'NO' : ''} podrán verlo`);
    if (userConfirmed) {
      handleCheckboxChange(index, value);
    } else {
      alert("Has cancelado.");
    }
  }
  const showConfirmChangeIsDelete = (name, index) => {
    // eslint-disable-next-line no-alert
    const userConfirmed = window.confirm(`¿ Seguro desea eliminar el campo "${name}" ? \nCuando se guarde el formulario esta acción será irreversible`);
    if (userConfirmed) {
      if(containers[index].ID !== undefined){
        handleChangeContainer(index, 'IsDeleted', true)
      }else {
        handleRemoveContainer(index);
      }
    } else {
      alert("Has cancelado.");
    }
  }


  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      // eslint-disable-next-line no-alert
      const userConfirmed = window.confirm(`¿ Seguro desea actualizar este formulario ?`);
      if (!userConfirmed) {
        return;
      }

      inputs.inputs = [];
      inputs.id = id;
      inputs.inputs.push(...containers);


      const res = await axios.put('https://localhost:7048/Manage/form/edit', inputs)
        .catch(err => {
          throw new Error(err.response.data.messages[0].text);
        });

      alert(res.data.messages[0].text)

    } catch (error) {
      alert(`Error: ${error.message}`)
    }
  }


  return (
    <section>

      <form className='FormView-List' onSubmit={handleSubmit}>
        <div>
          <div className='label'>
            <span>Crear inputs</span>
            <button className='button' type='button' onClick={handleAddContainer}>
              +
            </button>
          </div>
          <div>
            {containers.map((container, index) => (
              !container.IsDeleted && <div key={index} className="label-input">
                <label className='label-Inputs'>
                  Nombre del input
                  <input
                    type="text"
                    value={container.name}
                    pattern="[A-Za-z0-9 ]+"
                    required
                    onChange={(e) => handleChangeContainer(index, 'name', e.target.value)}
                  />
                </label>
                <label className='label-checkbox'>
                  Estatus
                  <input
                    type="checkbox"
                    checked={container.IsActive}
                    onChange={(e) => showConfirmChangeIsActive(container.IsActive, index, e.target.checked)}
                  />
                </label>
                <label className='label-Inputs'>
                  Tipo de input
                  <select
                    value={container.type}
                    required
                    onChange={(e) => handleChangeContainer(index, 'type', e.target.value)}
                  >
                    <option value="">Seleccione</option>
                    <option value="text">Texto</option>
                    <option value="number">Numerico (Entero)</option>
                    <option value="date">Fecha</option>
                  </select>
                </label>
                <button type="button" className='button-close' onClick={() => showConfirmChangeIsDelete(container.name, index)}>
                  X
                </button>
              </div>
            ))}
          </div>
        </div>

        <input className='button' type="submit" value="Editar" />
      </form>


    </section>
  )
}

export default FormInputEditComponent