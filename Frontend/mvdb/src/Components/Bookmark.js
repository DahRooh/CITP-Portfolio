import 'bootstrap/dist/css/bootstrap.css';
import './User.css';
import { Paging } from './Pagination.js';
import {Button, ButtonGroup, Form,  Row, Col, Container, Image} from 'react-bootstrap';
import { useEffect, useState } from "react"; 
import { Link, Outlet, useParams } from 'react-router';




function Bookmarks(){
const [index, setIndex] = useState(0);
    let titles = ["Money", "BIG MONKEY", "Lil Monkey"];
    let bookmarkAt = [23434245, 345345345433434, 534534544];
    return(
    <Container className='blackBorder'>
    <Row>
    <Col>
      <Container>
        <h3 className="col text-center breakWord">Bookmarks</h3>
        <Col className='text-end'>
        <ButtonGroup>
        <Button variant="secondary">Clear Selected Bookmarks</Button>
        <Button variant="secondary">Clear Bookmarks</Button>
        </ButtonGroup>
        </Col>
      <Row>
        <Col>
          {titles.map((item, index) => (
            <div key={index} className='container' style={{marginTop: 20, marginBottom: 20 }}>
              <Container className='singlehistoryContainer'>
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
                <Col className='text-start' md={2}>
                <p style={{fontWeight: 'bold', wordBreak: 'break-word'}}>{bookmarkAt[index]}</p>
                </Col>
                <Col className='text-start' md={8}>
                <h6 style={{wordBreak: 'break-word'}}>
                  {item}
                </h6>
                </Col>
                <Col>
                    <Button>
                    <Image src="../../trash.png" roundedCircle />
                    </Button>
                    </Col>
              </Row>
              </Container>
              </div>
          ))}
          <Container className='paging'>
          <Row>
            <Col className='text-center'>
              <Paging index={index} total={5} setIndex={setIndex} />
            </Col>
          </Row>
          </Container>
        </Col>
      </Row>
      </Container>
      
    </Col>
    </Row>
    </Container>
    );

}

export default Bookmarks;