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


  let navigate = useNavigate();
  
  useEffect(() => {
    if (signUpData) {
      if (signUpData.token) {
        Cookies.set('token', signUpData.token, {secure: false, expires: 7});
        Cookies.set('userid', signUpData.id, {secure: false, expires: 7});
        Cookies.set('username', signUpData.username, {secure: false, expires: 7});

        navigate("/");
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
      if (res.ok) return res.json();
      return null;
    })
    .then(data => {
      if (data) {
        setSignUpData(data);
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
              MVDb
          </Col>
        </Row>

        <form className="centered">
          <FormGroup className="placeholders">
            <input className="placeholderText" placeholder="Username" onChange={handleSetUsername}/>
          </FormGroup>

          <FormGroup className="placeholders">
            <input className="placeholderText" placeholder="Email" onChange={handleSetEmail}/>
          </FormGroup>

          <FormGroup className="placeholders">
            <input className="placeholderText" placeholder="Password" onChange={handleSetPassword} type="password"/>
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

