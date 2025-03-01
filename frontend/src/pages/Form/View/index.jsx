import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom';
import './../../../App.css'
import { Link } from "react-router-dom";


const FormView = () => {
    const [form, setForm] = useState([]);
    const [inputs, setInputs] = useState({});
    const { id } = useParams();

    useEffect(() => {
        async function getDataForm() {
            const res = await axios.get(`https://localhost:7048/Manage/form/${id}`)
                .catch(err => {
                    console.log(err);
                })

            console.log(res.data.data);
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


    return (
        <main className='FormView'>
            <header>
                <div>
                    <Link className="button" to="/">
                    Regresar
                    </Link>
                </div>
                <h2>
                Formulario
                </h2>
                <div>
                    <Link className='button' to={`/form/edit/${id}`}>
                        Editar
                    </Link>
                </div>
            </header>
            <section>

                <form className='FormView-List'>
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
        </main>
    )
}

export default FormView