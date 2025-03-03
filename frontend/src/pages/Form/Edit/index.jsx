import { useParams } from 'react-router-dom';
import { Link } from "react-router-dom";
import FormInputEditComponent from '../../../Components/form-input-edit/form-input-edit.component';

import './../../../App.css'

const FormEdit = () => {
    const { id } = useParams();

    return (
        <main className='FormView'>
            <header>
                <div>
                    <Link className="button" to={`/form/${id}`}>
                        Regresar
                    </Link>
                </div>
                <h2>
                    Editar Formulario
                </h2>
                <div>
                </div>
            </header>
            <FormInputEditComponent id={id} />

        </main>
    )
}

export default FormEdit