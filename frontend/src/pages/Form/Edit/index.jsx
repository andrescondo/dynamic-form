import { useEffect, useState } from 'react'
import axios from 'axios';

import { useParams } from 'react-router-dom';
import { Link } from "react-router-dom";
import FormInputEditComponent from '../../../Components/form-input-edit/form-input-edit.component';

import './../../../App.css'

const FormEdit = () => {
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
                    <Link className="button" to={`/form/${id}`}>
                        Regresar
                    </Link>
                </div>
                <h2>
                    Editar Formulario "{nameForm}"
                </h2>
                <div>
                </div>
            </header>
            <FormInputEditComponent id={id} />

        </main>
    )
}

export default FormEdit