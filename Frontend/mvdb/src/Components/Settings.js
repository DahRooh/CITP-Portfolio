import 'bootstrap/dist/css/bootstrap.css';
import { Container, Row, Col, Button, Form } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router';
import Cookies from 'js-cookie';

import './User.css';
import { Popup } from './Popup';

function UpdateInformation() {
  const [user, setUser] = useState(null);
  const [updater, setUpdater] = useState(false);

  const [password, setPassword] = useState("");
  const [passwordError, setPaswordError] = useState(false);
  const [passwordSuccess, setPaswordSuccess] = useState(false);

  
  const [email, setEmail] = useState("");
  const [emailError, setEmailError] = useState(false);
  const [emailSuccess, setEmailSuccess] = useState(false);

  const cookies = Cookies.get();

  let navigate = useNavigate();

  // Fetch user details by ID
  useEffect(() => {
    fetch(`http://localhost:5001/api/user/${cookies.userid}`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${cookies.token}`,
      }
    })
      .then((res) => {
        if (res.ok) return res.json();
        return {}; // no results
      })
      .then((data) => {
        setUser(data);
      })
      .catch((error) => console.log("Error fetching user", error));
  }, [updater, cookies.token, cookies.userid]);

  async function updateMail(value) {
    await fetch(`http://localhost:5001/api/user/${cookies.userid}/update_email`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + cookies.token
      },
      body: JSON.stringify(
        {
          email: value
        }
      )
    })
    .then(res => {
        if (res.ok) {
          setEmailError(false);
          setEmailSuccess(true);
          setUpdater(c => !c)
        } else {
          setEmailSuccess(false);
          setEmailError(true);
        }
    })
    .catch(e => {
      console.log(e);
    });
  }

  async function updatePassword(value) {
    await fetch(`http://localhost:5001/api/user/${cookies.userid}/update_password`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + cookies.token
      },
      body: JSON.stringify(
        {
          password: value
        }
      )
    }).then(res => {
      if (res.ok) {
        setPaswordError(false);
        setPaswordSuccess(true);
        setUpdater(c => !c);
      }
      else {
        setPaswordError(true);
        setPaswordSuccess(false);
      }
    });
  }

  async function deleteAccount() {
    await fetch(`http://localhost:5001/api/user/${cookies.userid}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + cookies.token
      },
    })
    .then(res => {
        if (res.ok) {
          Cookies.remove("token");
          Cookies.remove("username");
          Cookies.remove("userid");
          navigate("/");
        }
    })
    .catch(e => {
      console.log(e);
    });
  }

  if (!user) return (
    <>Fetching user details</>
  )
  return (
      <Container className='blackBorder'>
        <Container>
        <Row>
          <Col className="textHeader">
              <h1>Update Information</h1>
          </Col>
          <Popup deleter={deleteAccount} message={"Do you really want to delete your account?"} functionMsg={"Delete account"}/>
        </Row>
        </Container>

        <Container className="placeholders text-center">
          <Form onSubmit={(e) => {
                e.preventDefault();
                updateMail(email);
                setEmail("");

                e.target.childNodes[1].value = '';
              }}>
            <label>Email:</label>
            <Form.Control  
              className="placeholderText placeholders"
              type="text"
              placeholder={user.email}
              onChange={e => {
                setEmail(e.target.value); 
              }}
            />
            <br/>

            <Button type="submit" style={{width: "20%"}}>
              Update Email
            </Button>
          </Form>
          {(emailError) ? <p style={{color: "red"}}>Email cannot be updated: Already exists</p> : null}
          {(emailSuccess) ? <p style={{color: "green"}}>Email updated</p> : null}
          <br/>
          <br/>
          <Form onSubmit={(e) => {
                e.preventDefault();
                updatePassword(password);
                setPassword("");
                e.target.childNodes[1].value = '';
              }}>

            <label>Password:</label>
            <Form.Control  
                className="placeholderText placeholders"
                type="password"
                placeholder="************"
                onChange={e => {
                  setPassword(e.target.value); 
                }}/>
            <br/>
            <Button type="submit" style={{width: "20%"}}>
              Update Password
            </Button>
          </Form>
          {(passwordError) ? <p style={{color: "red"}}>Password cannot be updated: Too short</p> : null}
          {(passwordSuccess) ? <p style={{color: "green"}}>Password updated</p> : null}
        </Container>
      </Container>
  );
}
  
export default UpdateInformation;