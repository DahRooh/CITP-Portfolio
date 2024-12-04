import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap.css';
import { useParams } from 'react-router';
import SelectionPane from './SelectionPane.js'
import { Button, ButtonGroup, Col, Container, Row } from 'react-bootstrap';
import { useEffect, useState } from 'react';

function Series() {
  const {id} = useParams();
  const [title, setTitle] = useState({});
  const [series, setSeries] = useState({});
  const [currentSeason, setCurrentSeason] = useState("Extra");
  const [episodeOptionIndex, setEpisodeOptionIndex] = useState(1);


  useEffect(() => { // get the series' title
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

  useEffect(() => { // get episodes
    fetch(`http://localhost:5001/api/title/series/${id}`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) {
        setSeries(data);
      console.log(data)}
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [id]);

  let titleImage = (title.poster !== "N/A") ? title.poster : "https://media.istockphoto.com/id/911590226/vector/movie-time-vector-illustration-cinema-poster-concept-on-red-round-background-composition-with.jpg?s=612x612&w=0&k=20&c=QMpr4AHrBgHuOCnv2N6mPUQEOr5Mo8lE7TyWaZ4r9oo="  
  return (
    <Container className='centered'>
      <Row>
        <Col xs={4}>
          <Row>
          <Col>
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
          </Row>
        </Col>

        <Col>
          <Row>
            <Col className='centered'>
              <h2>{title._Title}</h2>
            </Col>

          </Row>
          <br/>
          <Row>
            <Col>
              <ButtonGroup>
              {Object.keys(series).length > 0 // if there are available keys
              ? Object.keys(series.seasons).map((seasonKey, index) => ( // then map the key to the button
                  <Button key={index}  
                  onClick={() => setCurrentSeason(seasonKey)} 
                  disabled={currentSeason === seasonKey}>
                    {seasonKey}
                  </Button>
                ))
              : "No seasons available" // else no seasons
              }
              </ButtonGroup>
            </Col>
          </Row>
          <br/>
          <Row>
            <Col> 
              {
                (series.seasons && series.seasons[currentSeason]) 
                ? (<SelectionPane items={series.seasons[currentSeason]} path={"/title"} currentIndex={episodeOptionIndex} name={"Episodes"} amountOfPages={5} function={setEpisodeOptionIndex}/> 

                ) 
                : "Loading!"
              }
              
            </Col>
          </Row>
          <br/>
        
        </Col>
      </Row>

      <Container>
        <Row>
          <Col>
          <h3>Reviews</h3>
          </Col>
        </Row>


      </Container>
    </Container>
  
  );
}
  
export default Series;