import 'bootstrap/dist/css/bootstrap.css';
import { Container, Row, Col, FormGroup, Button } from 'react-bootstrap';

function SignUp() {
  return (

    <div className="fullscreen">
      <Container>
        <Row className="textHeader">
          <Col>
              MVDb
          </Col>
        </Row>

        <form className="centered">
          <FormGroup className="placeholders">
            <input className="placeholderText" placeholder="Username"/>
          </FormGroup>

          <FormGroup className="placeholders">
            <input className="placeholderText" placeholder="Email"/>
          </FormGroup>

          <FormGroup className="placeholders">
            <input className="placeholderText" placeholder="Password"/>
          </FormGroup>

          <Button style={{width: "20%"}}>
            Sign up
          </Button>
        </form>

      </Container>
    </div>
  );
}
  
export default SignUp;