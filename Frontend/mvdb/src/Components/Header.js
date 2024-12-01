import 'bootstrap/dist/css/bootstrap.css';
import { Button, ButtonGroup, Col, Container, Nav, Row } from 'react-bootstrap';
import { Link } from 'react-router';

function Header() {
  return (
    <Container className="fluid">
      <Row>
          <Col className='centered'>
          <Nav>
            <Link to="/">LOGO</Link>
          </Nav>
          </Col>
          <Col>
              <input placeholder="search"/>
              <Button>gogo</Button>
          </Col>
          <Col>
            <ButtonGroup style={{border: "1px solid black"}}>
              <Button className="btn-dark">sign in</Button>
              <Button className="btn-light">sign up</Button>
            </ButtonGroup>
          </Col>
      </Row>
      <hr/>
    </Container>
  );
}
  
export default Header;
  