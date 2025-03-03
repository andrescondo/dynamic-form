import axios from 'axios';
import React, { useEffect, useState } from 'react'
import './../../App.css'
/* Todo el código para listar los inputs (campos) del formulario debe colocarse en la siguiente ruta y
nombre de archivo */
const FormInputListComponent = ({ id }) => {
  const [form, setForm] = useState([]);
  const [inputs, setInputs] = useState({});

  useEffect(() => {
    async function getDataForm() {
      const res = await axios.get(`https://localhost:7048/Manage/form/${id}/1`)
        .catch(err => {
          console.log(err);
        })

      setForm(res.data.data)
    }

    getDataForm();
  }, [])

  const handleChange = (e) => {
    setInputs({
      ...inputs,
      [e.target.name]: e.target.value,
    });
  }

  const handleSubmit = (e) => {
    e.preventDefault()

    const newBody = handleDivideObject(inputs)
  }

  function handleDivideObject(objeto) {
    let columns = "";
    let values = "";

    for (const clave in objeto) {
      columns += `${clave}, `;
      values += `${objeto[clave]}, `;
    }

    // Eliminar la coma y el espacio final
    columns = columns.slice(0, -2);
    values = values.slice(0, -2);

    return { columns: columns, values: values };
  }


  return (
    <section>

      <form className='FormView-List' onSubmit={handleSubmit}>
        {
          form.map((data, index) => (
            <label htmlFor={data.ID} className="label">
              <span>{data.InputsName}</span>
              <input
                key={index}
                id={data.ID}
                type={data.InputsType}
                name={data.InputsName}
                value={inputs[data.InputsName] || ''}
                onChange={handleChange}
              />
            </label>
          ))
        }
        <input className='button' type="submit" value="Guardar" />
      </form>


    </section>
  )
}

export default FormInputListComponent