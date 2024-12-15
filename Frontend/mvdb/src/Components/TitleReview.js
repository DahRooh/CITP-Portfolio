import 'bootstrap/dist/css/bootstrap.css';
import { Button, ButtonGroup, Col, Container, Row } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import { StarRatingFixed } from './StarRatingFixed';

function TitleReview({ updater, review, userLoggedIn }) {


    useEffect(() => {
      fetch(`http://localhost:5001/api/user/${userLoggedIn.userid}/likes`, {
        headers: {
          Authorization: "Bearer " + userLoggedIn.token
        }
      })
      .then(res => {
        if (res.ok) return res.json();
        return null; // no results
      })
      .then(data => {
        if (!data) return new Error("No data");
      }) 
      .catch(e => console.log("error", e))
    }, [userLoggedIn.userid]);
  
      if (!review) {
        return <Row>
          <Col>
            No Reviews
          </Col>
        </Row>
      }
      function likeReview( reaction ) {
        fetch(`http://localhost:5001/api/title/${review.titleId}/review/${review.reviewId}/like`, {
          method: "POST",
          body: JSON.stringify({
            "like": reaction
          }),
          headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + userLoggedIn.token
          }
        })
        .then(res => {
          if (res.ok) {
            updater(c => !c);
            return res.json;
          }
        }) 
        .catch(e => console.log("error", e))
      }
      let style = (userLoggedIn.username === review.username) ? { backgroundColor: "rgb(173,255,47)" } : null;
      


      return (
        <Row className='review'>
          <Col>
            <Row>
              <Col md={2}>
                <StarRatingFixed titleRating={review.rating} />
                <br/>
                <p>Rating given: {review.rating}</p>
                <p style={style}>Username: {review.username}</p>

              </Col>
              <Col >
                <h2>{review.caption}</h2>
                <hr/>
                <p style={{ backgroundColor: "rgb(191, 215, 215)" }}>{review.text}</p>
              </Col>
              <Col md={2}>
              <p>Likes: {review.liked}</p>
              <ButtonGroup>
                  <Button onClick={() => likeReview(1)} disabled={!userLoggedIn.token}>
                    Like
                  </Button>
                  <Button onClick={() => likeReview(-1)} disabled={!userLoggedIn.token}>
                    Dislike
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
function TitleReviews( {updater, reviews, cookies} ) {
    return (
        <Container className='reviewContainer'>
            <Row>
            <Col>
            <h1>Reviews</h1>
            </Col>
            <hr/>
            </Row>

            {(reviews.length > 0) 
            ? reviews.map(review => <TitleReview updater={updater} review={review} key={review.reviewId} userLoggedIn={cookies} />)
            : <Row><Col><p>No reviews</p></Col></Row>}

      </Container>
    )
}
export default TitleReviews;