import { useState } from 'react';
import axios from 'axios';
import './../../App.css'


const initialInput = {
  name: '',
  inputs: []
}
/* Todo el código para crear y editar los formularios debe colocarse en la siguiente ruta y nombre de
archivo */
const FormEditComponent = () => {
  const [inputs, setInputs] = useState(initialInput);
  const [containers, setContainers] = useState([]);


  const handleAddContainer = () => {
    setContainers([...containers, { name: '', type: '' }]);
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

  const handleChange = (e) => {
    setInputs({
      ...inputs,
      [e.target.name]: e.target.value,
    });
  }

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      inputs.inputs = [];
      inputs.inputs.push(...containers);

      const res = await axios.post('https://localhost:7048/Manage/form/create', inputs)
        .catch(err => {
          throw new Error(err.response.data.messages[0].text);
        });


      setContainers([]);
      const updatedInputs = { name: '', inputs: [] };
      setInputs(updatedInputs);

      alert(res.data.messages[0].text)

    } catch (error) {
      alert(`Error: ${error.message}`)

    }
  }


  return (
    <section>

      <form className='FormView-List' onSubmit={handleSubmit}>
        <label htmlFor="name" className="label">
          <span>Nombre</span>
          <input
            value={inputs.name}
            name="name"
            id='name'
            pattern="[A-Za-z0-9 ]+"
            required
            onChange={handleChange} />
        </label>
        <div>
          <div className='label'>
            <span>Crear inputs</span>
            <button className='button' type='button' onClick={handleAddContainer}>
              +
            </button>
          </div>
          <div>
            {containers.map((container, index) => (
              <div key={index} className="label-input">
                <label className='label-Inputs'>
                  Nombre del input
                  <input
                    type="text"
                    value={container.text}
                    pattern="[A-Za-z0-9 ]+"
                    required
                    onChange={(e) => handleChangeContainer(index, 'name', e.target.value)}
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
                <button type="button" className='button-close' onClick={() => handleRemoveContainer(index)}>
                  X
                </button>
              </div>
            ))}
          </div>
        </div>

        <input className='button' type="submit" value="Guardar" />
      </form>


    </section>
  )
}

export default FormEditComponent