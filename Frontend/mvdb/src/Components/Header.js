import 'bootstrap/dist/css/bootstrap.css';
import { useEffect, useState } from 'react';
import { Button, ButtonGroup, Col, Container, Nav, Row } from 'react-bootstrap';
import { Link } from 'react-router';



export function convertCookie() {
  var cookie = {};
  const splitCookie = document.cookie.split(";");
  splitCookie.forEach(data => {
    var keyValuePair = data.split("=");
    cookie[keyValuePair[0].trim()] = keyValuePair[1]; 
  })
  return cookie;
}

function Header() {
  const [cookies, setCookie] = useState(() => convertCookie());

  setInterval(() => {
    var userCookies = convertCookie();
    if (userCookies && userCookies.token !== cookies.token) {
      setCookie(userCookies);
    }
  }, 500);

  function clearCookies() {
    var cookieNames = Object.keys(cookies);
    cookieNames.map(name => document.cookie = name + '=; Max-Age=0');
    setCookie(false);
  }

  return (
    <Container key={cookies} className="fluid">
      <Row>
          <Col className='centered'>
          <Nav>
            <Link to="/">LOGO</Link>
          </Nav>
          </Col>
          <Col>
              <input placeholder="search"/>
              <Button>Search</Button>
          </Col>
          <Col>
            {(cookies.username) ? 
              <>
                <span>Username: {cookies["username"]}</span>
                <Link to={`/user/${cookies["userid"]}`}>
                  <Button className="btn-dark">User Page</Button>
                </Link>
                <Link to={`/`}>
                  <Button className="btn-light" onClick={clearCookies}>Logout</Button>
                </Link>

              </>
            : (<ButtonGroup style={{border: "1px solid black"}}>
              <Link to="/signin">
                <Button className="btn-dark">sign in</Button>
              </Link>
              <Link to="/signup">
                <Button className="btn-light">sign up</Button>
              </Link>
            </ButtonGroup>)}
          </Col>
      </Row>
      <hr/>
    </Container>
  );
}
  
export default Header;
  