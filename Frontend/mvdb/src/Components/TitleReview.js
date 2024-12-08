import 'bootstrap/dist/css/bootstrap.css';
import { Button, ButtonGroup, Col, Container, Row } from 'react-bootstrap';
import { useEffect, useState } from 'react';

function TitleReview({ review, userLoggedIn }) {
    const [liked, setLiked] = useState(false);
  
    useEffect(() => {
      fetch(`http://localhost:5001/api/user/${userLoggedIn.id}/likes`)
      .then(res => {
        if (res.ok) return res.json();
        return null; // no results
      })
      .then(data => {
        if (data) setLiked(data);
        else return new Error("No data");
      }) 
      .catch(e => console.log("error", e))
    }, [userLoggedIn.id]);
  
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
          console.log(res);
          if (res.ok) {
            return res.json;
          }
          return null;
        }) 
        .catch(e => console.log("error", e))
      }
  
      return (
        <Row className='review'>
          <Col>
            <Row>
              <Col>
                <p>Username: {review.username}</p>
                <p>Rating: {review.rating}</p>
              </Col>
              <Col >
                <p>{review.caption}</p>
                <p>{review.text}</p>
              </Col>
              <Col>
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
function TitleReviews( {reviews, cookies} ) {
    return (
        <Container className='reviewContainer'>
            <Row>
            <Col>
            <h3>Reviews</h3>
            </Col>
            <hr/>
            </Row>

            {(reviews.length > 0) 
            ? reviews.map(review => <TitleReview review={review} key={review.reviewId} userLoggedIn={cookies} />)
            : <Row><Col><p>No reviews</p></Col></Row>}

      </Container>
    )
}
export default TitleReviews;