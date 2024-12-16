import 'bootstrap/dist/css/bootstrap.css';
import { useEffect, useState } from "react"; 
import { Row, Col, Container } from "react-bootstrap";
import { Link } from 'react-router';
import Cookies from 'js-cookie';

import './User.css';
import { StarRatingFixed } from './StarRatingFixed.js';
import ImageFor from './ImageFor.js';
import { Paging } from './Pagination.js';
import { Popup } from './Popup.js';

function UserReview( {review, updater} ) {
  async function deleteReview() {
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
          <Popup deleter={deleteReview} functionMsg={"Delete review"} message={"Are you sure you want to delete the review?"}/>
          
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
        setReviews(data);
      } else {
        setReviews([]);
      }
    });
  }, [updater, user.token, user.userid])

    return (
      <Container className='blackBorder text-center'>
        <Container>
          <Row>
            <Col>
              <h2 className='col text-center'>Reviews</h2>

                  {(reviews) ? (reviews.length > 0) ? reviews.map( (item) => <UserReview key={item.reviewId} review={item} updater={setUpdater}/>  ) : <div className='review'>You have no reviews.</div> 
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