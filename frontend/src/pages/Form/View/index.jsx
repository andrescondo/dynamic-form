import { useEffect, useState } from 'react'
import axios from 'axios';

import { useParams } from 'react-router-dom';
import './../../../App.css'
import { Link } from "react-router-dom";
import FormInputListComponent from '../../../Components/form-input-list/form-input-list.component';


const FormView = () => {
    const { id } = useParams();
    const [nameForm, setNameForm] = useState("");

    useEffect(() => {
        getData();
    }, [])


    async function getData() {
        const res = await axios.get('https://localhost:7048/Manage/all-form')
            .catch((err) => {
                console.log(err)
            });

        const result = res.data.data.find(form => form.ID == id);
        setNameForm(result.FormName);
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
                    Formulario "{nameForm}"
                </h2>
                <div>
                    <Link className='button' to={`/form/edit/${id}`}>
                        Editar
                    </Link>
                </div>
            </header>
            <FormInputListComponent id={id} />

        </main>
    )
}

export default FormView