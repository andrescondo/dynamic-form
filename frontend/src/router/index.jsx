import React from 'react'
import { Routes, Route } from "react-router-dom"
import App from '../App'
import FormCreate from '../pages/Form/Create'
import FormView from '../pages/Form/View'


const RouterApp = () => {
    return (
        <div>
            <Routes>
                <Route path="/" element={<App />} />
                <Route path="/form/:id" element={<FormView />} />
                <Route path="/form/create" element={<FormCreate />} />
            </Routes>
        </div>
    )
}

export default RouterApp