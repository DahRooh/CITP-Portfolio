import './App.css';
import Frontpage from './Components/Frontpage.js';
import Header from './Components/Header.js';
import 'bootstrap/dist/css/bootstrap.css';


function App() {
  return (
    <div className="container">
      <Header />
      <Frontpage />
    </div>
  );
}

export default App;
