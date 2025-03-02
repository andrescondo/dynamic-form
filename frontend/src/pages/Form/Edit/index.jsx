import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Link } from "react-router-dom";
import axios from 'axios';
import './../../../App.css'

const initialInput = {
    id: '',
    inputs: []
}
const FormEdit = () => {
    const [form, setForm] = useState([]);
    const [inputs, setInputs] = useState(initialInput);
    const [containers, setContainers] = useState([]);
    const { id } = useParams();

    useEffect(() => {
        getDataForm();
    }, [])

    const getDataForm = async () => {
        try {
            const res = await axios.get(`https://localhost:7048/Manage/form/${id}`)
                .catch(err => {
                    console.log(err);
                })


            // setForm(res.data.data)
            const data = res.data.data.map(item => ({
                ID: item.ID,
                name: item.InputsName,
                type: item.InputsType,
                IsActive: item.IsActive,
                IsDeleted: item.IsDeleted,
                IDForm: item.IDForm
            }));

            console.log(data);

            setContainers(data);


        } catch (error) {
            console.log(error)
        }
    }


    const handleAddContainer = () => {
        setContainers([...containers, { name: '', type: '', IsActive: true }]);
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
    const handleCheckboxChange = (index, value) => {
        const newContainers = containers.map((container, i) => {
            if (i === index) {
                return { ...container, IsActive: value };
            }
            return container;
        });
        setContainers(newContainers);
    };

    const showConfirmChangeIsActive = (status, index, value) => {
        // eslint-disable-next-line no-alert
        const userConfirmed = window.confirm(`¿Seguro desea ${status ? 'Desactivar' : 'Activar'} este campo? \nLos usuarios ${status ? 'NO' : ''} podrán verlo`);
        if (userConfirmed) {
            handleCheckboxChange(index, value);
        } else {
            alert("Has cancelado.");
        }
    }
    const showConfirmChangeIsDelete = (name, index) => {
        // eslint-disable-next-line no-alert
        const userConfirmed = window.confirm(`¿ Seguro desea eliminar el campo "${name}" ? \nEsta es una acción irreversible`);
        if (userConfirmed) {
            handleRemoveContainer(index);
        } else {
            alert("Has cancelado.");
        }
    }
    

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            inputs.inputs = [];
            inputs.id = id;
            inputs.inputs.push(...containers);
            console.log(inputs)

            const res = await axios.put('https://localhost:7048/Manage/form/edit', inputs)
                .catch(err => {
                    throw new Error(err.response.data.messages[0].text);
                });


            console.log(res);
            setContainers([]);
            const updatedInputs = { name: '', inputs: [] };
            setInputs(updatedInputs);

            // alert(res.data.messages[0].text)

        } catch (error) {
            console.log(error);
            alert(`Error: ${error.message}`)

        }
        finally {

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
                    Editar Formulario
                </h2>
                <div>
                    {/* {data.name} */}
                </div>
            </header>
            <section>

                <form className='FormView-List' onSubmit={handleSubmit}>
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
                                            value={container.name}
                                            pattern="[A-Za-z0-9 ]+"
                                            required
                                            onChange={(e) => handleChangeContainer(index, 'name', e.target.value)}
                                        />
                                    </label>
                                    <label className='label-checkbox'>
                                        Estatus
                                        <input
                                            type="checkbox"
                                            checked={container.IsActive}
                                            onChange={(e) => showConfirmChangeIsActive(container.IsActive, index, e.target.checked)}
                                        />
                                    </label>
                                    <label className='label-Inputs'>
                                        Tipo de input
                                        <select
                                            value={container.type}
                                            required
                                            onChange={(e) => handleChangeContainer(index, 'type', e.target.value)}
                                        >
                                            <option value="">Seleccione</option>
                                            <option value="text">Texto</option>
                                            <option value="number">Numerico (Entero)</option>
                                            <option value="date">Fecha</option>
                                        </select>
                                    </label>
                                    <button type="button" className='button-close' onClick={() => showConfirmChangeIsDelete(container.name, index)}>
                                        X
                                    </button>
                                </div>
                            ))}
                        </div>
                    </div>

                    <input className='button' type="submit" value="Editar" />
                </form>


            </section>
        </main>
    )
}

export default FormEdit