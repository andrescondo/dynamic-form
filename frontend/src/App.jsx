import { useEffect, useState } from 'react';
import axios from 'axios';
import './App.css';
import { Link } from "react-router-dom";


function App() {
  const [data, setData] = useState([]);

  useEffect(() => {
    async function getForm() {
      const res = await axios.get('https://localhost:7048/Manage/all-form')
        .catch((err) => {
          console.log(err)
        });
      setData(res.data.data);
    }

    getForm();
  }, [])


  return (
    <div className="App">
      <header className="App-header">
        Lista de formularios
      </header>
      <main>

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
              <div key={index} className="App-ContainerList-items">
                <span>{data.FormName}</span>
                <Link className="button" to={`/form/${data.ID}`}>Ver</Link>
              </div>
            ))
          }

        </div>


      </main>
    </div>
  );
}

export default App;
