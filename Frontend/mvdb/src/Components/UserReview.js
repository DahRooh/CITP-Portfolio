import 'bootstrap/dist/css/bootstrap.css';
import './User.css';
import { Paging } from './Pagination.js';
import {Button} from 'react-bootstrap';
import { useEffect, useState } from "react"; 
import { Row, Col, Container, Form, Image } from "react-bootstrap";
import Cookies from 'js-cookie';
import { StarRatingFixed } from './StarRatingFixed.js';
import { Link } from 'react-router';
import ImageFor from './ImageFor.js';

function UserReview( {review, updater} ) {
  async function deleteReview() {
    console.log(review);
    await fetch(`http://localhost:5001/api/user/${Cookies.get("userid")}/review/${review.reviewId}` , {
      method: "DELETE",
      headers: {
        Authorization: "Bearer " + Cookies.get("token")
      }
    })
    .then(res => {
      if (res.ok) updater(b => !b)
    });
  }
  let type = (review.type === 'movie') ? "title" : review.type; 
  return (
    <Row className='review'>
      <Col>
        <Row>
          <Col md={3}>
            <StarRatingFixed titleRating={review.rating} />
            <p>Rating given: {review.rating}</p>
          </Col>
          <Col><Link to={`/${type}/${review.titleId}`}>
           <h2 className='left'> <ImageFor width='10%' item={{id: review.titleId}}/>{review.title}</h2>
           </Link></Col>

        </Row>
        <Row>
          <Col md={2}>
            <br/>
            <p>Username: {review.username}</p>

          </Col>
          <Col style={{ backgroundColor: "rgb(191, 215, 215)" }}>
            <h3> {review.caption}</h3>
            <hr/>
            <p >{review.text}</p>
          </Col>
          <Col md={1}>
            <Button onClick={deleteReview}>
              <Image src="../trash.png" roundedCircle />
            </Button>
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


function UserReviews() {
  const [index, setIndex] = useState(0);
  const[reviews, setReviews] = useState(null);
  const [updater, setUpdater] = useState(false);
  let user = Cookies.get();
  useEffect(() => {
    fetch(`http://localhost:5001/api/user/${user.userid}/reviews`, {
      headers: {
        Authorization: "Bearer " + user.token
      }
    })
    .then(res => {
        if (res.ok) return res.json() 
      })
    .then(data => {
      if (data) {
        console.log(data);
        setReviews(data);
      } else {
        setReviews([]);
      }
    });
  }, [updater])

    return (
      <Container className='blackBorder text-center'>
        <Container>
          <Row>
            <Col>
              <h3 className='col text-center'>Reviews</h3>
                <Col className="text-end">
                    <Button>Delete Selected Reviews</Button>
                </Col>
                  {(reviews) ? (reviews.length > 0) ? reviews.map( (item) => <UserReview key={item.reviewId} review={item} updater={setUpdater}/>  ) : "You have no reviews."
                    : "Loading"}
                  <Container className='paging'>
                    <Row>
                      <Col>
                        {(reviews) ? <Paging index={index} total={Math.ceil(reviews.length / 20)} setIndex={setIndex} /> : null}
                      </Col>
                    </Row>
                  </Container>
            </Col>
          </Row>
        </Container>    
      </Container>

      
    );
  }


  export default UserReviews;