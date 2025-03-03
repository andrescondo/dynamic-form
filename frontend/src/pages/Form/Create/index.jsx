import { Link } from "react-router-dom";
import './../../../App.css'
import FormEditComponent from '../../../Components/form-edit/form-edit.component';


const FormCreate = () => {
    return (
        <main className='FormView'>
            <header>
                <div>
                    <Link className="button" to="/">
                        Regresar
                    </Link>
                </div>
                <h2>
                    Crear Formulario
                </h2>
                <div>

                </div>
            </header>
            <FormEditComponent />

        </main>
    )
}

export default FormCreate