
import { useParams } from 'react-router-dom';
import './../../../App.css'
import { Link } from "react-router-dom";
import FormInputListComponent from '../../../Components/form-input-list/form-input-list.component';


const FormView = () => {
    const { id } = useParams();

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
            <FormInputListComponent id={id} />
            
        </main>
    )
}

export default FormView