import { useState, useEffect } from 'react'
import { Link } from "react-router-dom";
import './../../../App.css'

const initialInput = {
    name: '',
    inputs: []
}
const FormCreate = () => {
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

    const handleSubmit = (e) => {
        try {
            e.preventDefault();
            inputs.inputs.push(...containers)
            console.log(inputs)
            console.log(containers)
            
        } catch (error) {
            console.log(error);
        }
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
                    Crear Formulario
                </h2>
                <div>
                    
                </div>
            </header>
            <section>

                <form className='FormView-List' onSubmit={handleSubmit}>
                    <label htmlFor="name" className="label">
                        <span>Nombre</span>
                        <input
                            value={inputs.name}
                            name="name"
                            id='name'
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
                                            onChange={(e) => handleChangeContainer(index, 'name', e.target.value)}
                                        />
                                    </label>
                                    <label className='label-Inputs'>
                                        Tipo de input
                                        <select
                                            value={container.type}
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
        </main>
    )
}

export default FormCreate