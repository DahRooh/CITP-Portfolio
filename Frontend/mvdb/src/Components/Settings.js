import 'bootstrap/dist/css/bootstrap.css';
import './User.css';
import { Container, Row, Col, Button, Form } from 'react-bootstrap';

function UpdateInformation() {
  return (
      <Container className='blackBorder'>
        <Container>
        <Row className="textHeader">
          <Col>
              <h1>Update Information</h1>
          </Col>
        </Row>
        </Container>

        <Container className="placeholders text-center">
          <Form >
          <Form.Control  
            className="placeholderText placeholders"
            type="text"
            placeholder="Email"
          />
          <Form.Control  
              className="placeholderText placeholders"
              type="text"
              placeholder="Password"
          />
          <Button type="submut" style={{width: "20%"}}>
            Update
          </Button>
          </Form>
        </Container>
      </Container>
  );
}
  
export default UpdateInformation;