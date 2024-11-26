import { Button } from 'bootstrap';
import 'bootstrap/dist/css/bootstrap.css';

function Header() {
  return (
    <div className="container">
      <div className="row">
          <div className="col">
              LOGO
          </div>
          <div className="col">
              <input placeholder="search"/>
              <button>gogo</button>
          </div>
          <div className="col">
              <button>sign in</button>
              <button>sign up</button>
          </div>
      </div>
      <hr/>
    </div>
  );
}
  
export default Header;
  