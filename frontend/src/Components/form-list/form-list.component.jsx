import { useEffect, useState } from 'react';
import axios from 'axios';
import './../../App.css';
import { Link } from "react-router-dom";
/* Todo el código para listar los botones con el nombre del formulario debe colocarse en la siguiente ruta y
nombre de archivo */
const FormListComponent = () => {
  const [data, setData] = useState([]);

  useEffect(() => {
    getForm();
  }, [])

  async function getForm() {
    const res = await axios.get('https://localhost:7048/Manage/all-form')
      .catch((err) => {
        console.log(err)
      });
    setData(res.data.data);
  }


  return (
    <section>
      <div className='App-ButtonForm--create'>
        <div></div>
        <Link to="/form/create" className='button'>
          Crear Formulario
        </Link>
      </div>
      <div className='App-Container--list'>
        {
          data.map((data, index) =>
          (
            <div key={index} className="App-ContainerList--items">
              <Link className="button" to={`/form/${data.ID}`}>{data.FormName}</Link>
            </div>
          ))
        }

      </div>
    </section>
  )
}

export default FormListComponent