import 'bootstrap/dist/css/bootstrap.css';
import './User.css';
import { Link, Outlet, useParams } from 'react-router';
import { Row, Col, Container } from "react-bootstrap";
import { useState } from "react"; 
import { Pagination } from './Pagination.js';
import {Button} from 'react-bootstrap';


function Reviews({ title, rating, review }) {
  const [index, setIndex] = useState(0);

  return (
    <Container className='reviewContainer'>
      <Row>
      <Col>
        {title.map((item, index) => (
          <div key={index}>
            <Container className='singleReview'>
              <Container className='whatever'>
                <Row>
                <Col className='text-center'>
                  <h5 className='breakWord'>{item}</h5>
                </Col>
                <Col className="hej">
                  <Button>Delete</Button>
                </Col>
                </Row>
               </Container>


            <Row>
            <Col className='col text-start'>
              <p style={{ fontWeight: 'bold' }}>Rating: {rating[index]}</p>
            </Col>
            <Col className='col-9'>
              <p className='breakWord'>{review[index]}</p>
            </Col>
            </Row>
            </Container>
          </div>
            
          ))}
          <Row>
          <Col className='col text-center'>
            <Pagination index={index} total={5} setIndex={setIndex} />
          </Col>
        </Row>
      </Col>
      </Row>
    </Container>
  );
}

function SearchHistory({keyword, timestamp}){
  const [index, setIndex] = useState(0);

  return(
    <Row>
      <Col>
        {keyword.map((item, index) => (
          <div key={index} className='container' style={{marginTop: 20, marginBottom: 20 }}>
            <Row>
              <Col className='col-5 text-start'>
              <p style={{ fontWeight: 'bold', wordBreak: 'break-word' }}>{timestamp[index]}</p>
              </Col>
              <Col className='col-5'>
              <h6 style={{ wordBreak: 'break-word' }}>
                {item}
              </h6>
              </Col>
              </Row>
            </div>
        ))}
        <Row>
          <Col className='text-center'>
            <Pagination index={index} total={5} setIndex={setIndex} />
          </Col>
        </Row>
      </Col>
    </Row>
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