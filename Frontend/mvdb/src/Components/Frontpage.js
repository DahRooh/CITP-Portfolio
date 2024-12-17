import { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { Container, Row, Col } from 'react-bootstrap';

import SelectionPane from "./SelectionPane";


function Frontpage() {
  const [moviesIndex, setMoviesIndex] = useState(1);
  const [movies, setMovies] = useState(false);

  const [seriesIndex, setSeriesIndex] = useState(1);
  const [series, setSeries] = useState(false);

  const [peopleIndex, setPeopleIndex] = useState(1);
  const [people, setPeople] = useState(false);
  

  const itemsPerPage = 5;
  const maxItems = 100;

  useEffect(() => {
    setMovies(false);
    fetch(`http://localhost:5001/api/title/movies?page=${moviesIndex}&pageSize=${itemsPerPage}`)
    .then(res => {
      if (res.ok) return res.json();
      throw new Error("Could not fect"); // no results
    })
    .then(data => {
      if (data && data.items) {
        
        setMovies(data.items);
      }
      else throw new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [moviesIndex]);

  useEffect(() => {
    setSeries(false);
    fetch(`http://localhost:5001/api/title/series?page=${seriesIndex}&pageSize=${itemsPerPage}`)
    .then(res => {
      if (res.ok) return res.json();
      throw new Error("Could not fetch"); // no results
    })
    .then(data => {
      if (data && data.items){
         setSeries(data.items);
        }
      else throw new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [seriesIndex]);

  useEffect(() => {
    setPeople(false);
    fetch(`http://localhost:5001/api/person?page=${peopleIndex}&pageSize=${itemsPerPage}`)
    .then(res => {
      if (res.ok) return res.json();
      throw new Error("Could not fect"); // no results
    })
    .then(data => {
      if (data && data.items) setPeople(data.items);
      else throw new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [peopleIndex]);


  const amountOfPages = Math.ceil(maxItems/itemsPerPage);

  return (
    <Container className="centered">

      { // rows for our links to pages
      }
      <Row>
        <Col>
          <h1>MVDb</h1>
        </Col>
      </Row>

      <br/>

      <Row>
        <Col>

            <SelectionPane items={movies} path={"/title"} currentIndex={moviesIndex} name={"Popular movies"} amountOfPages={amountOfPages} function={setMoviesIndex}/> 
            <br/>
            <SelectionPane items={series} path={"/series"} currentIndex={seriesIndex} name={"Popular series"} amountOfPages={amountOfPages} function={setSeriesIndex}/> 
            <br/>
            <SelectionPane items={people} path={"/person"} currentIndex={peopleIndex} name={"Popular people"} amountOfPages={amountOfPages} function={setPeopleIndex}/> 

        </Col>
      </Row>



    </Container>
  );
}

export default Frontpage;
