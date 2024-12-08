import 'bootstrap/dist/css/bootstrap.css';
import { Container, Row, Col, Button, ButtonGroup, InputGroup, Form } from 'react-bootstrap';
import { Link, Outlet, useParams } from 'react-router';
import './User.css';
import { useState } from 'react';
import {StarRating} from './StarRating'; 



function CreateReview() {
  const { title } = useParams();
  return (
    <Container>
    <Row>
    <Col>

      <Container>
      <Row>
      <Col className="text-center">
        <Link to={`../title`}>{title}</Link>
        <h1>HEJ MED DIG</h1>
      </Col>
      </Row>
      </Container>

      <Container>
      <Row>
      <Col className='blackBorder text-center'>
      <StarRating amountOfStars={10} startValue={0} />

      <Form >
          <Form.Control  
              className="newReviewMargin text-center"
              type="text"
              placeholder="Caption"
          />
          <Form.Control
              as="textarea" rows={5} 
              className="newReviewMargin text-center"
              type="text"
              placeholder="Review text"
          />
          <Button className='newReviewMargin' type="submit" style={{width: "20%"}}>
            Confirm Review
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