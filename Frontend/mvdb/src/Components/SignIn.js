import 'bootstrap/dist/css/bootstrap.css';
import { Container, Row, Col, FormGroup, Button } from 'react-bootstrap';

function SignIn() {
  return (

    <Container>
      <Row className="signInHeader">
        <Col>
            MVDb
        </Col>
      </Row>

      <form className="centered">
        <FormGroup className="placeholders">
          <input className="placeholderText" placeholder="Username"/>
        </FormGroup>

        <FormGroup className="placeholders">
          <input className="placeholderText" placeholder="Password"/>
        </FormGroup>

        <Button style={{width: "20%"}}>
          Login
        </Button>
      </form>

    </Container>
  );
}
  
export default SignIn;