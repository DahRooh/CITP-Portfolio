import 'bootstrap/dist/css/bootstrap.css';
import { useParams } from 'react-router';
import SelectionPane from './SelectionPane.js'
import { Button, ButtonGroup, Col, Container, Row } from 'react-bootstrap';
import { useEffect, useState } from 'react';

import { convertCookie } from './Header.js';


function Review({ review, userLoggedIn }) {
  
    return (
        <Row className="review">
          <Col>
            <Row>
              <Col>
                <p>Username: {review.user}</p>
                <p>Rating: {review.rating}</p>

              </Col>
              <Col>
                <p>{review.reviewTitle}</p>
              </Col>
              <Col>
              <p>Likes: {review.likes}</p>
              <ButtonGroup>
                  <Button disabled={!userLoggedIn}>
                    +1
                  </Button>
                  <Button disabled={!userLoggedIn}>
                    -1
                  </Button>
              </ButtonGroup>
              </Col>
            </Row>
            <hr/>
            <Row>
              <Col>
                {review.review}
              </Col>
            </Row>
          </Col>
        </Row>
    )
}

function Title() {
  const {id} = useParams();
  const [title, setTitle] = useState({});
  
  const [similarTitles, setSimilarTitles] = useState(false);
  const [similarTitlesPage, setSimilarTitlesPage] = useState(1);
  const [cast, setCast] = useState([]);
  const [castPage, setCastPage] = useState(1);
  const [crew, setCrew] = useState([]);
  const [crewPage, setCrewPage] = useState(1);
  
  const [reviews, setReviews] = useState([]);
  const [userLoggedIn, setUserLoggedIn] = useState(convertCookie());
  
  console.log(userLoggedIn);

  var fakeReviews = [{reviewTitle: "this is title", review: "this is a review", user: "username", rating: "rating", likes: "likes"}, {reviewTitle: "this is title", review: "this is a review", user: "username", rating: "rating", likes: "likes"}];
  
  
  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) setTitle(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [id]);

  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}/crew`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) setCrew(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [id]);

  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}/cast`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) setCast(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [id]);

  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}/similartitles`)
    .then(res => {
      if (res.ok) return res.json();
      return {}; // no results
    })
    .then(data => {
      if (data) setSimilarTitles(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [id]);

  function TitlePane() {
    
    if (similarTitles) {
      if ((similarTitles.length) > 0) {
        return (
          <SelectionPane items={similarTitles} 
          path={"/title"} currentIndex={similarTitlesPage} 
          name="Similar Titles" amountOfPages={similarTitles.length}
          function={setSimilarTitlesPage}/>)
      }
      else {
        return <p>Loading!</p>
      }
    }
    return <p>No similar titles!</p>
  }


  let titleImage = (title.poster !== "N/A") ? title.poster : "https://media.istockphoto.com/id/911590226/vector/movie-time-vector-illustration-cinema-poster-concept-on-red-round-background-composition-with.jpg?s=612x612&w=0&k=20&c=QMpr4AHrBgHuOCnv2N6mPUQEOr5Mo8lE7TyWaZ4r9oo="
  return (
    <Container className='centered'>
      <Row>
        <Col xs={4} className='titleInfo'>
          <Row>
          <Col>
            <img
              src={titleImage}
              alt="Title"
            />
          </Col>
            <Row>
              <Col>
                <p>Total rating: {title.rating}</p> 
              </Col>
              <Col>
                <p>Total amount of voters: ??</p> 
              </Col>
            </Row>

            <Row>
              <Col>
                <span>Plot:</span>
                <p>{(title.plot) || "No plot for this title"}</p> {/*  equivalent to: (title.plot) ? title.plot : "string" */}
              </Col>
            </Row>

            <Row><Col>
                {(userLoggedIn) ? <Button>Create Review</Button> : <p style={{backgroundColor: "grey"}}>To create a review please log in</p>}
            </Col></Row>

          </Row>
        </Col>

        <Col>
          <Row>
            <Col className='centered'>
              <h2>{title._Title}</h2>
            </Col>
          </Row>



          <Row>
            <Col>
              <TitlePane />
            </Col>
          </Row>
        </Col>
      </Row>

      <Container>
        <Row>
          <Col>
          <h3>Reviews</h3>
          </Col>
        </Row>

        {fakeReviews.map(r => <Review review={r} userLoggedIn={userLoggedIn} />)}

      </Container>
    </Container>
  );
}
  
export default Title;