import React from 'react'
import { Routes, Route } from "react-router-dom"
import App from '../App'
import FormView from '../pages/Form/View'


const RouterApp = () => {
    return (
        <div>
            <Routes>
                <Route path="/" element={<App />} />
                <Route path="/form/:id" element={<FormView />} />
            </Routes>
        </div>
    )
}

export default RouterApp