import React from 'react'
import { Routes, Route } from "react-router-dom"
import App from '../App'


const RouterApp = () => {
    return (
        <div>
            <Routes>
                <Route path="/" element={<App />} />
            </Routes>
        </div>
    )
}

export default RouterApp