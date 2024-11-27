import './App.css';
import Person from './Components/Person.js';
import Frontpage from './Components/Frontpage.js';
import Header from './Components/Header.js';
import 'bootstrap/dist/css/bootstrap.css';
import SearchResult from './Components/SearchResults.js';
import UserPage from './Components/User.js';
import { useEffect, useState } from 'react';


function App() {
  const [movies, setMovies] = useState([]);
  useEffect( fetch("http://localhost:5001/api/title/movies")
              .then(res => res.json())
              .then(data => {console.log(data); setMovies(data.items)}))
  return (
    <div className="container">
      <Header />
      {movies.map(m => <p> {m._Title} </p>)}
     {// <Frontpage /> 
      //<Person />
      //<SearchResult />
     }
     {/*<UserPage />*/}
    </div>
  );
}

export default App;
