import { useEffect, useState } from 'react';
import axios from 'axios';
import './App.css';

function App() {
  const [data, setData] = useState([]);

  useEffect(() => {
    const res = axios.get('https://localhost:7048/Manage/all-form')
      .catch((err) => {
        console.log(err)
      });
    console.log(res);
  }, [])


  return (
    <div className="App">
      <header className="App-header">
        Lista de formularios
      </header>
      <main>


      </main>
    </div>
  );
}

export default App;
