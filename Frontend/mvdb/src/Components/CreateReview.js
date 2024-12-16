import 'bootstrap/dist/css/bootstrap.css';

import { Container, Row, Col, Button, Form } from 'react-bootstrap';
import { Link, useParams, useNavigate } from 'react-router';
import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';

import {StarRating} from './StarRating'; 
import './User.css';


function CreateReview() {
  let navigate = useNavigate();
  const { id } = useParams();
  const [rating, setRating] = useState(0);
  const [reviewText, setReviewText] = useState("");
  const [captionText, setCaptionText] = useState("");
  const [loading, setLoading] = useState(false);
  const [title, setTitile] = useState([]);

  const cookieToken = Cookies.get('token'); 

  if(cookieToken === undefined){
    navigate(-1);
  }

  async function handleSubmit() {
    setLoading(true);
    await fetch(`http://localhost:5001/api/title/${id}/review`, {
      method: "POST",
      body: JSON.stringify({
        reviewText: reviewText,
        rating: rating,
        captionText: captionText,
      }),
      headers: {
        "Authorization": `Bearer ${cookieToken}`,
        "Content-Type": "application/json"
      },
    })
    .then(res => {
        if (res.ok) {
          navigate(-1);
        }
        setLoading(false)
      })
    .catch((e) => {
      console.log(e);
    });
  }
  
  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}`)
    .then(res => {
      if (res.ok) return res.json();
      throw new Error("Could not fetch"); // no results
    })
    .then(data => {
      if (data) {
        setTitile(data._Title)
      }
      else throw new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [id]);
    
  
  const handleSetRating = (newRating) => {
    setRating(newRating);
  }

  const handleSetReviewText = e => {
    setReviewText(e.target.value);
  }

  const handleSetCaptionText = e => {
    setCaptionText(e.target.value);
  }

  return (
    <Container>
      <Row>
      <Col>

        <Container>
        <Row>
          <Col className="text-center">
            <Link to={`../title/${id}`} className='removeLink'><h1>{title}</h1></Link>
            {(cookieToken === undefined)  
                ? <h1 style={{color:"red"}}>You must sign in to create a review</h1>
                : null}
          </Col>
        </Row>
      </Container>

        <Container>
          <Row>
            <Col className='blackBorder text-center'>
              <Form onSubmit={(e) => {
                      e.preventDefault();
                      handleSubmit();
                      }}>
              <StarRating amountOfStars={10} getRating={handleSetRating} startValue={0} />
              <Form.Control  
                  className="newReviewMargin text-center"
                  type="text"
                  placeholder="Caption"
                  value={captionText}
                  onChange={handleSetCaptionText}
                />
              <Form.Control
                  as="textarea" rows={5} 
                  className="newReviewMargin text-center"
                  type="text"
                  placeholder="Review text"
                  value={reviewText}
                  onChange={handleSetReviewText}
                />
              <Button className='newReviewMargin' disabled={loading} type="submit" style={{width: "20%"}}>
                  {(!loading) ? "Create Review" : "Creating review..."}
              </Button>
              </Form>
            </Col>
          </Row>
        </Container>
        </Col>
      </Row>
    </Container>
  );
}
  
export default CreateReview;