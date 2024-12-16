import 'bootstrap/dist/css/bootstrap.css';
import { useState } from 'react';
import { Container, Row, Col, Button, Form } from 'react-bootstrap';
import { useNavigate } from 'react-router';
import Cookies from 'js-cookie';

function SignIn() {

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const [errorMessage, setErrorMessage] = useState(false);
  let navigate = useNavigate();
  
  function login(logInData) {
    if (logInData) {
      if (logInData.token) {
        Cookies.set('token', logInData.token, {secure: false, expires: 7});
        Cookies.set('userid', logInData.id, {secure: false, expires: 7});
        Cookies.set('username', logInData.username, {secure: false, expires: 7});

        setErrorMessage(false);
        navigate("/");
      } 
    } else {
      setErrorMessage(true);
    }
  }


  async function signIn(e) {
    await fetch(`http://localhost:5001/api/user/sign_in`, {
      method: "PUT", 
      body: JSON.stringify(
        {
          username: username,
          password: password
        }
      ),
      headers: {
        "Content-Type": "application/json"
      }
    })
    .then(res => {
      if (res.ok) return res.json();
      setErrorMessage(true);
    })
    .then(data => {
      if (data) {
        login(data);
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
    setPassword(e.target.value);
  }

  return (

    <div className="fullscreen">
      <Container>
        <Row className="textHeader">
          <Col>
            Sign In
          </Col>
        </Row>

        <Form className="centered"
              onSubmit={(e) => {
                ;
              }}>
            <label>Username:</label>
            <Form.Control  
                  className="newReviewMargin text-center"
                  type="text"
                  placeholder="Username"
                  onChange={handleSetUsername}
                />
            <br/>
            {(errorMessage) ? <p style={{color: "red"}}>Username or password is incorrect.</p> : null}
            <label>Password:</label>
            <Form.Control  
                  className="newReviewMargin text-center"
                  type="password"
                  placeholder="Password"
                  onChange={handleSetPassword}
                />
          <br/>

          <Button onClick={signIn} style={{width: "20%"}}>
            Login
          </Button>
        </Form>

      </Container>

    </div>
  );
}
  
export default SignIn;
