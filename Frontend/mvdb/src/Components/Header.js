import 'bootstrap/dist/css/bootstrap.css';
import { useState } from 'react';
import { Button, ButtonGroup, Col, Container, Nav, Row } from 'react-bootstrap';
import Cookies from 'js-cookie';
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
  const [cookies, setCookie] = useState(Cookies.get());

  setInterval(() => {
    var userCookies = convertCookie();
    if (userCookies && userCookies.token !== cookies.token) {
      setCookie(userCookies);
    }
  }, 500);

  function clearCookies() {
    var cookieNames = Object.keys(cookies);
    cookieNames.map(name => document.cookie = name + '=; Max-Age=0; expires=Thu, 01 Jan 1970 00:00:00 UTC"');
    setCookie(Cookies.get());
  }

  return (
    <Container key={cookies} className="fluid">
      <Row>
          <Col className='centered'>
          <Nav>
            <Link to="/"><h2>MVDb</h2></Link>
          </Nav>
          </Col>
          <Col>
            <form action='/search'>
              <input name="keyword" placeholder="search"/>
                <Button type='submit'>Search</Button>
            </form>
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
  