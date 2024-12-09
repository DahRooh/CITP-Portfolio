import 'bootstrap/dist/css/bootstrap.css';
import { useEffect, useState } from 'react';
import { Container, Row, Col, FormGroup, Button } from 'react-bootstrap';
import { useNavigate } from 'react-router';
import Cookies from 'js-cookie';

function SignUp() {

  const [signUpData, setSignUpData] = useState({});
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [passwordIsOk, setPasswordIsOk] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");


  let navigate = useNavigate();
  
  useEffect(() => {
    if (signUpData) {
      if (signUpData.id) {
        navigate("/signin");
      } 
    }
  }, [signUpData])


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
        setSignUpData(data);
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

    <div className="fullscreen">
      <Container>
        <Row className="textHeader">
          <Col>
            Sign Up
            {(errorMessage) ? <p>{errorMessage}</p> : null}
          </Col>
        </Row>
        
        <form className="centered">
          <FormGroup className="placeholders">
            <label>Username:</label>
            <br/>
            <input className="placeholderText" placeholder="Username" onChange={handleSetUsername}/>
          </FormGroup>

          <FormGroup className="placeholders">
            <label>Email:</label>
            <br/>
            <input className="placeholderText" placeholder="Email" onChange={handleSetEmail}/>
          </FormGroup>

          <FormGroup className="placeholders">
            <label>Password:</label>
            <br/>
            <input className="placeholderText" placeholder="Password" onChange={handleSetPassword} type="password"/>
            {(password.length > 0)  
            ? <p style={{color: (!passwordIsOk && password.length > 0) ? "red" : "green"}}>Password must be at least 8 characters long.</p>
            : null}
            
          </FormGroup>

          <Button style={{width: "20%"}} onClick={signUp} disabled={!passwordIsOk}>
            Sign up
          </Button>
        </form>

      </Container>
    </div>
  );
}
  
export default SignUp;

