import React from 'react'
import { Routes, Route } from "react-router-dom"
import App from '../App'
import FormCreate from '../pages/Form/Create'
import FormEdit from '../pages/Form/Edit'
import FormView from '../pages/Form/View'


const RouterApp = () => {
    return (
        <div>
            <Routes>
                <Route path="/" element={<App />} />
                <Route path="/form/:id" element={<FormView />} />
                <Route path="/form/create" element={<FormCreate />} />
                <Route path="/form/edit/:id" element={<FormEdit />} />
            </Routes>
        </div>
    )
}

export default RouterApp