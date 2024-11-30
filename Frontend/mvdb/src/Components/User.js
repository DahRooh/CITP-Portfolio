import 'bootstrap/dist/css/bootstrap.css';
import { Link, Outlet, useParams } from 'react-router';
import { useState } from "react"; 
import { Pagination } from './Pagination.js';
import {Button} from 'react-bootstrap';


function Reviews({ title, rating, review }) {
  const [index, setIndex] = useState(0);

  return (
    <div className='row'>
      <div className='col'>
        <div className='container'>
          {title.map((item, index) => (
            <div key={index}>
              <div className='container' style={{ marginTop: 20, marginBottom: 20 }}>
              <div className='row'>
                <h5 className='col-10 text-center' style={{ wordBreak: "break-word" }}>{item}</h5>
                <Button className='col-auto btn'>Delete</Button>
              </div>
              <div className='row'>
                <div className='col text-start'>
                  <p style={{ fontWeight: 'bold' }}>Rating: {rating[index]}</p>
                </div>
                <div className='col-9'>
                  <p style={{ wordBreak: "break-word" }}>{review[index]}</p>
                </div>
                </div>
              </div>
            </div>
            
          ))}
          <div className='row'>
           <div className='col text-center'>
            <Pagination index={index} total={5} setIndex={setIndex} />
           </div>
         </div>
        </div>
      </div>
    </div>

  );
}

function SearchHistory({keyword, timestamp}){
  const [index, setIndex] = useState(0);

  return(
    <div className='row'>
      <div className='col'>
        {keyword.map((item, index) => (
          <div key={index} className='container' style={{marginTop: 20, marginBottom: 20 }}>
            <div className='row'>
            <div className='col-3 text-start'>
                  <p style={{ fontWeight: 'bold' }}>{timestamp[index]}</p>
                </div>
            
              <h6 className='col-5' style={{ wordBreak: 'break-word' }}>
                {item}
              </h6>
              </div>
            </div>
        ))}
        <div className='row'>
          <div className='col text-center'>
            <Pagination index={index} total={5} setIndex={setIndex} />
          </div>
        </div>
      </div>
    </div>
  );

}





function UserPage() {
  const { id } = useParams();
  let title = ["selena", "orusbo KLAMME SO JEG HÅBER DU BLIVER GRAVID BLA BLA ", "luder"];
  let rating = [8, 4, 3];
  let review = ["HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf", "STOP", "NO U"];
  let keyword = ["HEJ MED DIG", "BIG MONKEY", "MONEY"];
  let timestamp = [322334676567, 343434, 3432423];

  return (
    <div className="row">
      <div className="col text-center">
        <p>This is the user page for user: {id}</p>
        <Outlet />
      </div>
      <div className="col-1 text-end">
        <nav>
          <Link to={`/user/${id}/settings`}>Settings</Link>
        </nav>
      </div>
      
      <div className='container'>
        <div className="row">
          <div className="col text-start">
            <div className='container'>
              <h3 className='col text-center'>Reviews</h3>
              <Reviews title={title} rating={rating} review={review} />
            </div>
          </div>
          <div className="col text-end">
            <div className="container">
              <div className="d-flex">
                <h3 className="col text-center">Search History</h3>
                <button className="btn">Clear History</button>
              </div>
              <div className="container">
                <SearchHistory keyword={keyword} timestamp={timestamp} />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}


export default UserPage;