/* (22)
varierende variabler
dem der kommer til at være en del af dep. array?
hashing funktion - skal have salt. Skal gemme salt/standard salt som inge kender?
*/

import 'bootstrap/dist/css/bootstrap.css';
import { useEffect, useState } from 'react';
import { Container, Row, Col, FormGroup, Button } from 'react-bootstrap';

function SignIn() {

  const [logInData, setLogInData] = useState({});

  useEffect(() => {
    fetch(`http://localhost:5001/api/user/sign_in`, {
      method: "PUT", 
      body: JSON.stringify(
        {
          username: "eray",
          password: "eray"

          
        }
      ),
      headers: {
        "Content-Type": "application/json"
      }
    })
    .then(res => {
      if (res.ok) return res.json();
      return {};
    })
    .then(data => {
      if (data.token) setLogInData(data);
      else new Error("Cannot log in");
    })
    .catch(e => {
      console.log(e)
    })
  }, []);

  console.log(logInData)

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
            <input className="placeholderText" placeholder="Password"/>
          </FormGroup>

          <Button style={{width: "20%"}}>
            Login
          </Button>
        </form>

      </Container>
    </div>
  );
}
  
export default SignIn;