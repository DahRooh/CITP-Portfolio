import 'bootstrap/dist/css/bootstrap.css';
import './User.css';
import { Link, Outlet, useParams } from 'react-router';
import { Row, Col, Container, Button, ButtonGroup } from "react-bootstrap";
import { useEffect, useState } from "react"; 
import Cookies from 'js-cookie';

function UserPage() {
    const { u_id } = useParams();
    const [userIDs, setuserID] = useState([]);
    const [currentPage, setCurrentPage] = useState("Settings");

  // Fetch user details by ID
  useEffect(() => {
    if (u_id) {
      fetch(`http://localhost:5001/api/user/${u_id}`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${Cookies.get("token")}`,
        }
      })
        .then((res) => {
          if (res.ok) return res.json();
          return {}; // no results
        })
        .then((data) => {
          setuserID(data);
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
              <Col className="user text-center"> 
                <h1>{userIDs.username}</h1>
              </Col>
            </Row>
          </Container>
  
          <Container>
            <Row>
              <Col className="text-center">
                <Container className="buttons">
                  <Row className="buttons-margin">
                    <Col className="user" md={3}>
                      <h2>{currentPage}</h2>
                    </Col>
                    <Col>
                      <ButtonGroup>  
                          <Link to="settings">
                            <Button onClick={() => setCurrentPage("Settings")} className="buttons-margin" disabled={currentPage === "Settings"}>Update information</Button>
                          </Link>
                          <Link to="review">
                            <Button onClick={() => setCurrentPage("Reviews")} className="buttons-margin" disabled={currentPage === "Reviews"}>Reviews</Button>
                          </Link>
                          <Link to="history">
                            <Button onClick={() => setCurrentPage("Search History")} className="buttons-margin" disabled={currentPage === "Search History"}>Search History</Button>
                          </Link>
                          <Link to="bookmark">
                            <Button onClick={() => setCurrentPage("Bookmarks")} className="buttons-margin" disabled={currentPage === "Bookmarks"}>Bookmarks</Button>
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