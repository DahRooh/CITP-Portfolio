/* (22)
varierende variabler
dem der kommer til at være en del af dep. array?
hashing funktion - skal have salt. Skal gemme salt/standard salt som inge kender?

knap - post?
*/

import 'bootstrap/dist/css/bootstrap.css';
import { useEffect, useState } from 'react';
import { Container, Row, Col, FormGroup, Button } from 'react-bootstrap';
import { useNavigate } from 'react-router';
import Cookies from 'js-cookie';

/*
funktion her.
Skal setters bruges her? nok ikke...
*/

function SignIn() {

  const [logInData, setLogInData] = useState({});
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [cookies, setCookie] = useState(['user']);
  let navigate = useNavigate();
  
  useEffect(() => {
    if (logInData) {
      if (logInData.token) {
        setCookie('token', logInData.token, {path: '/'});
        navigate("/");
      }
      console.log("no user");
    }
  }, [logInData])

  const print = () => {
    console.log(logInData)
  }

  async function signIn() {
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
      return null;
    })
    .then(data => {
      if (data) {
        setLogInData(data);
      }
    })
    .catch(e => {
      console.log(e)
    }) 
  }

//  console.log(logInData)

  const handleSetUsername = e => {
    setUsername(e.target.value);
  }

  const handleSetPassword = e => {
    setPassword(e.target.value);
  }

  return (

/*
brug setters for neden her - højst sandsynligt i FormGroups
username og password som argumenter? 
*/

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
            <input className="placeholderText" placeholder="Password" onChange={handleSetPassword}/>
          </FormGroup>

          <Button style={{width: "20%"}} onClick={signIn}>
            Login
          </Button>
        </form>

      </Container>
      <Button onClick={print}>
        gogogo
      </Button>
    </div>
  );
}
  
export default SignIn;
