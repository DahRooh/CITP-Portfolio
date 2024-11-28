import './App.css';
import Header from './Components/Header.js';
import 'bootstrap/dist/css/bootstrap.css';
import { useEffect, useState } from 'react';


function App() {
  const [movies, setMovies] = useState([]);

  useEffect(() => {
    fetch("http://localhost:5001/api/title/movies")
    .then(res => {
      if (res.ok) return res.json();
      return {}; // no results
    })
    .then(data => {
      if (data && data.items) setMovies(data.items);
      else return new Error("No data");
      
    }) // check if items
    .catch(e => console.log("error", e))
  }, []);

  return (
    <div className="container">
      <Header />
      {movies.map(m => <p key={m._Title}> {m._Title} </p>)}
     {// <Frontpage /> 
      //<Person />
      //<SearchResult />
     }
     {/*<UserPage />*/}
    </div>
  );
}

export default App;
