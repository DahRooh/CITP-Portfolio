import 'bootstrap/dist/css/bootstrap.css';
import { Link, Outlet, useParams } from 'react-router';




function CreateReview() {
  const { title } = useParams();



  return (
    
    <div className="row">
      <div className="col text-center">
        <nav>
          <Link to={`../title`}>{title}</Link>
        </nav>
      </div>
    
    <div className="row">
    <div className="col text-center">
      <h1>HEJ MED DIG</h1>
    </div>
    </div>
    </div>
  );
}
  
export default CreateReview;