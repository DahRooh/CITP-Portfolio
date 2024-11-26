import './App.css';
import Person from './Components/Person.js';
import Frontpage from './Components/Frontpage.js';
import Header from './Components/Header.js';
import 'bootstrap/dist/css/bootstrap.css';
import SearchResult from './Components/SearchResults.js';
import UserPage from './Components/User.js';


function App() {
  return (
    <div className="container">
      <Header />
     {// <Frontpage />
      //<Person />
      //<SearchResult />
     }
     <UserPage />
    </div>
  );
}

export default App;
