import { Link } from "react-router";
import SelectionPane from "./SelectionPane";
import { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { Container, Row, Col } from 'react-bootstrap';

function Frontpage() {
  const [movies, setMovies] = useState([]);
  const [series, setSeries] = useState([]);
  const [people, setPeople] = useState([]);

  useEffect(() => {
    fetch("http://localhost:5001/api/title/movies")
    .then(res => {
      if (res.ok) return res.json();
      return {}; // no results
    })
    .then(data => {
      if (data && data.items) setMovies(data.items.slice(0, 100));
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, []);

  useEffect(() => {
    fetch("http://localhost:5001/api/title/episodes")
    .then(res => {
      if (res.ok) return res.json();
      return {}; // no results
    })
    .then(data => {
      if (data && data.items) setSeries(data.items.slice(0, 100));
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, []);

  useEffect(() => {
    fetch("http://localhost:5001/api/person")
    .then(res => {
      if (res.ok) return res.json();
      return {}; // no results
    })
    .then(data => {
      if (data && data.items) setPeople(data.items.slice(0, 100));
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, []);

  return (
    <Container>
      { // rows for our links to pages
      }
      <Row>
        <Col style={{"textAlign": "center"}}>
          <h1>MVDb</h1>
        </Col>
      </Row>

      <br/>

      <Row>
        <Col className="centered">
          <SelectionPane items={movies} path={"/title"} name="Popular Movies"/> 
          <br/>
          <SelectionPane items={series} path={"/title"} name="Popular Series"/> 
          <br/>
          <SelectionPane items={people} path={"/person"} name="Popular Actors"/> 
        </Col>
      </Row>

    </Container>
  );
}

export default Frontpage;
