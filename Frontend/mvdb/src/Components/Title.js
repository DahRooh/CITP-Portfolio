import 'bootstrap/dist/css/bootstrap.css';
import { Link, useParams } from 'react-router';
import SelectionPane from './SelectionPane.js'
import { Button, ButtonGroup, Col, Container, Row } from 'react-bootstrap';
import { useEffect, useState } from 'react';

import { convertCookie } from './Header.js';
import InfoBox from './InfoBox.js';
import TitleReview from './TitleReview.js';
import TitleReviews from './TitleReview.js';



function Title() {
  const pageSize = 5;
  
  const {id} = useParams();
  const [title, setTitle] = useState({});
  

  const [similarTitles, setSimilarTitles] = useState(false);
  const [similarTitlesPage, setSimilarTitlesPage] = useState(1);


  const [cast, setCast] = useState(false);
  const [castPage, setCastPage] = useState(1);

  const [crew, setCrew] = useState(false);
  const [crewPage, setCrewPage] = useState(1);
  

  const [reviews, setReviews] = useState(false);
  const [cookies] = useState(() => convertCookie());

  const [updater, setUpdater] = useState(null);
    
  
  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) setTitle(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [id]);

  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}/reviews`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) setReviews(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [id, updater]);

  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}/crew?page=${crewPage}&pageSize=${pageSize}`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) setCrew(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [crewPage, id]);

  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}/cast?page=${castPage}&pageSize=${pageSize}`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) {setCast(data);}
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [castPage, id]);

  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}/similartitles?page=${similarTitlesPage}&pageSize=${pageSize}`)
    .then(res => {
      if (res.ok) return res.json();
      return {}; // no results
    })
    .then(data => {
      if (data && data.items) setSimilarTitles(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [similarTitlesPage, id]);


  return (
    <Container className='centered'>
      <Row>
        <InfoBox updater={setUpdater} key={title.id} title={title} cookies={cookies} />

        <Col>
          <Row>
            <Col className='centered'>
              <h2>{title._Title}</h2>
            </Col>
          </Row>

          <SelectionPane items={similarTitles.items} path={"/title"} currentIndex={similarTitlesPage} name={"Similar titles"} amountOfPages={(similarTitles.totalNumberOfPages !== 0) ? similarTitles.totalNumberOfPages : 1} function={setSimilarTitlesPage}/> 
          
          <br/>
          <SelectionPane items={cast.items} path={"/person"} currentIndex={castPage} name={"Cast"} amountOfPages={cast.totalNumberOfPages} function={setCastPage}/> 
          
          <br/>
          <SelectionPane items={crew.items} path={"/person"} currentIndex={crewPage} name={"Crew"} amountOfPages={crew.totalNumberOfPages} function={setCrewPage}/> 


        </Col>
      </Row>
      <br/>
      <TitleReviews updater={setUpdater} reviews={reviews} cookies={cookies}/>
    </Container>
  );
}
  
export default Title;