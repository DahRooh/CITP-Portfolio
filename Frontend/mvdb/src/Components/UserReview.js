import 'bootstrap/dist/css/bootstrap.css';
import './User.css';
import { Paging } from './Pagination.js';
import {Button} from 'react-bootstrap';
import { useEffect, useState } from "react"; 
import { Row, Col, Container, Form, Image } from "react-bootstrap";

function UserReviews() {
    const [index, setIndex] = useState(0);
    const[reviewsData, setReviewsData] = useState({
        reviews: [
          { name: "selena", rating: 8, review: "HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf" },
          { name: "dviohweriogvhoiwerhvhvh  hewio hioweh iwei vhweio vhiwehv weio", rating: 4, review: "STOP" },
          { name: " hewiofh weioh fihewi hwei hiweh hewoi hfewi hiowe", rating: 3, review: "HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf HEJ MED DIG HERfhufhuwfhuwefuwehufhweqhfuiqwehfuihqwefhwehfuwehfuihweu ff hwerif hweuifh uiweh fwe feqwf hqweui wf " },
        ],
    })

    return (
      <Container className='blackBorder text-center'>
        <Container>
        <Row>
        <Col>
          <h3 className='col text-center'>Reviews</h3>
             <Col className="text-end">
                <Button>Delete Selected Reviews</Button>
             </Col>
            {reviewsData.reviews.map((item, i) => (
              <div key={i}>
                <Container className='singleReview'>
                  <Container className='whatever'>
                    <Row>
                    <Col className='text-start' md={1}>
                    <Form>
                      <Form.Check
                          name="group1"
                          type="checkbox"
                          id={`checkbox`}
                      />
                    </Form>
                     </Col>
                    <Col className='text-center' md={10}>
                      <h5 className='breakWord'>{item.name}</h5>
                    </Col>
                    <Col>
                    <Button>
                    <Image src="../trash.png" roundedCircle />
                    </Button>
                    </Col>

                    </Row>
                   </Container>
    
                <Container className='singleReview'>
                <Row>
                <Col className='text-start'  md={1}>
                  <p style={{ fontWeight: 'bold', wordBreak: 'break-word' }}>Rating: {item.rating}</p>
                </Col>
                <Col  md={11}>
                  <p className='breakWord'>{item.review}</p>
                </Col>
                </Row>
                </Container>
                </Container>
              </div>
                
              ))}
              <Container className='paging'>
                <Row>
                <Col>
                  <Paging index={index} total={5} setIndex={setIndex} />
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