import 'bootstrap/dist/css/bootstrap.css';
import './User.css';
import { Link, Outlet, useParams } from 'react-router';
import { Row, Col, Container, Button, ButtonGroup } from "react-bootstrap";
import { useEffect, useState } from "react"; 


function UserPage() {
    const { u_id } = useParams();
    const [userIDs, setuserID] = useState([]);
    const[token] = useState("hdehw")

  // Fetch user details by ID
  useEffect(() => {
    console.log("User ID :", u_id);
    if (u_id) {
      fetch(`http://localhost:5001/api/user/${u_id}`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        }
      })
        .then((res) => {
          console.log("User response: ", res);
          if (res.ok) return res.json();
          return {}; // no results
        })
        .then((data) => {
          setuserID([data]);
          console.log("User Data: ", data);
        })
        .catch((error) => console.log("Error fetching person ID", error));
    }
  }, [u_id]);

  return (
    <Container>
      <Row>
        <Col>
          <Container>
            <Row>
              <Col className="user" md={1}>
                <h2>Setting</h2>
              </Col>
              <Col className="user  text-center" md={11}>
                <h1>{userIDs.username || "HEJ"}</h1>
              </Col>
            </Row>
          </Container>
  
          <Container>
            <Row>
              <Col className="text-center">
                <Container className="buttons">
                  <Row className="buttons-margin">
                    <Col>
                      <ButtonGroup>  
                          <Link to="settings">
                            <Button className="buttons-margin">Update information</Button>
                          </Link>
                          <Link to="review">
                            <Button className="buttons-margin">Reviews</Button>
                          </Link>
                          <Link to="history">
                            <Button className="buttons-margin">Search History</Button>
                          </Link>
                          <Link to="bookmark">
                            <Button className="buttons-margin">Bookmarks</Button>
                          </Link>
                          
                        </ButtonGroup>
                    </Col>
                  </Row>
                </Container>
              </Col>
              </Row>
              <Row>
              <Col className='text-center'>
                <Outlet />
              </Col>
            </Row>
          </Container>
        </Col>
      </Row>
    </Container>
  );
}


export default UserPage;