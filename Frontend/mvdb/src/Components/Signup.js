import 'bootstrap/dist/css/bootstrap.css';
import { useState } from 'react';
import { Container, Row, Col, Button, Form } from 'react-bootstrap';
import { useNavigate } from 'react-router';

function SignUp() {

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [passwordIsOk, setPasswordIsOk] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");


  let navigate = useNavigate();
  

  async function signUp() {
    await fetch(`http://localhost:5001/api/user/new`, {
      method: "POST", 
      body: JSON.stringify(
        {
          username: username,
          password: password,
          email: email
        }
      ),
      headers: {
        "Content-Type": "application/json"
      }
    })
    .then(res => {
      return res.json();
    })
    .then(data => {
      if (data.id) {
          navigate("/signin");
      } else if (data.error){
        setErrorMessage(data.error);
      }
    })
    .catch(e => {
      console.log(e)
    })
  }


  const handleSetUsername = e => {
    setUsername(e.target.value);
  }

  const handleSetPassword = e => {
    var newPassword = e.target.value;
    setPassword(newPassword);
    setPasswordIsOk(newPassword.length >= 8); // avoiding branching by doing so
  }

  const handleSetEmail = e => {
    setEmail(e.target.value);
  }

  return (

      <Container style={{height: "100vh"}}>
        <Row className="textHeader">
          <Col>
            Sign Up
            {(errorMessage) ? <p>{errorMessage}</p> : null}
          </Col>
        </Row>
        
        <Form className="centered">

          <br/>
          <br/>
          <label>Username:</label>
          <Form.Control  
                    className="placeholderText"
                    type="text"
                    placeholder="Username"
                    onChange={handleSetUsername}
                  />


          <br/>
          <br/>
          <label>Email:</label>
          <Form.Control  
                    className="placeholderText"
                    type="text"
                    placeholder="Email"
                    onChange={handleSetEmail}
                  />

          <br/>
          <br/>
          <label>Password:</label>
          <Form.Control  
                    className="placeholderText"
                    type="password"
                    placeholder="Password"
                    onChange={handleSetPassword}
                  />
                  
            {(password.length > 0)  
            ? <p style={{color: (!passwordIsOk && password.length > 0) ? "red" : "green"}}>Password must be at least 8 characters long.</p>
            : null}
          <br/>
          <br/>
          <Button style={{width: "20%"}} onClick={signUp} disabled={!passwordIsOk}>
            Sign up
          </Button>
        </Form>

      </Container>
  );
}
  
export default SignUp;

