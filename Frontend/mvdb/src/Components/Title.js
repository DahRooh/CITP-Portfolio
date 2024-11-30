import 'bootstrap/dist/css/bootstrap.css';
import { useParams } from 'react-router';
import SelectionPane from './SelectionPane.js'
import { Button, Col, Container, Row } from 'react-bootstrap';
import { useEffect, useState } from 'react';
function Review({review}) {

}

function Title() {
  const {id} = useParams();
  const [title, setTitle] = useState({});

  useEffect(() => {
    fetch("http://localhost:5001/api/title/" + id)
    .then(res => {
      if (res.ok) return res.json();
      return {}; // no results
    })
    .then(data => {
      if (data) setTitle(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, []);

  let titleImage = (title.poster !== "N/A") ? title.poster : "https://media.istockphoto.com/id/911590226/vector/movie-time-vector-illustration-cinema-poster-concept-on-red-round-background-composition-with.jpg?s=612x612&w=0&k=20&c=QMpr4AHrBgHuOCnv2N6mPUQEOr5Mo8lE7TyWaZ4r9oo="
  return (
    <Container>
      <Row>
        <Col xs={4}>
          <Row>
          <Col style={{ maxWidth: "100%" }}>
            <img
              src={titleImage}
              alt="Title"
            />
          </Col>
            <Row>
              <Col>
                <p>Total rating: {title.rating}</p> 
              </Col>
              <Col>
                <p>Total amount of voters: ??</p> 
              </Col>
            </Row>

            <Row>
              <Col>
                <span>Plot:</span>
                <p>{(title.plot) || "No plot for this title"}</p> {/*  equivalent to: (title.plot) ? title.plot : "string" */}
              </Col>
            </Row>

            <Row>
              <Col>
                <Button>Create Review</Button>
              </Col>
            </Row>

          </Row>
        </Col>
        <Col>
          <Row>
            <Col className='text-center'>
              <h2>{title._Title}</h2>
            </Col>
          </Row>
          <br/>
          <Row>
            <Col>
              <h3>Main cast</h3>
              <SelectionPane items={["item1", "item2", "item3", "item4", "item5", "item6"]}/>
            </Col>
          </Row>

          <Row>
            <Col>
              <h3>Main crew</h3>
              <SelectionPane items={["item1", "item2", "item3", "item4", "item5", "item6"]}/>
            </Col>
          </Row>

          <Row>
            <Col>
              <h3>Similar titles</h3>
              <SelectionPane items={["item1", "item2", "item3", "item4", "item5", "item6"]}/>
            </Col>
          </Row>

        </Col>
      </Row>

      <Row>
        <Col>
          REVIEWS GO HERE
        </Col>
      </Row>
    </Container>
  );
}
  
export default Title;