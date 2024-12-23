import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap.css';
import { useParams } from 'react-router';
import SelectionPane from './SelectionPane.js'
import { Button, ButtonGroup, Col, Container, Row } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import InfoBox from './InfoBox.js';
import TitleReviews from './TitleReview.js';
import Cookies from 'js-cookie';



function Series() {
  const { id } = useParams();
  const [title, setTitle] = useState({});
  const [series, setSeries] = useState({});
  const [currentSeason, setCurrentSeason] = useState(1);
  const [episodeOptionIndex, setEpisodeOptionIndex] = useState(1);
  const [currentItems, setCurrentItems] = useState(false);
  const [cookies] = useState(Cookies.get());
  const [updater, setUpdater] = useState(null);

  useEffect(() => { // get the series' title
    setUpdater(c => !c);
    setEpisodeOptionIndex(1);
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
    fetch(`http://localhost:5001/api/series/${id}`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) {
        setSeries(data);
        setCurrentSeason((data.seasons && data.seasons["1"]) ? "1" : "Extra")
      }
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [id]);


  useEffect(() => {
    if (series && series.seasons && series.seasons[currentSeason]) {
      var newItems = series.seasons[currentSeason].slice((episodeOptionIndex - 1) * pageSize, ((episodeOptionIndex - 1) * pageSize) + pageSize);
      setCurrentItems(newItems);
    }
  }, [currentSeason, series, episodeOptionIndex]);


  const pageSize = 5;
  return (
    <Container className='centered'>
      <Row>
        <InfoBox updater={setUpdater} title={title} cookies={cookies}/>

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
              {(Object.keys(series).length > 0) // if there are available keys
              ? Object.keys(series.seasons).map((seasonKey, index) => ( // then map the key to the button
                  <Button key={index}
                  onClick={
                    () => {
                      setCurrentSeason(seasonKey);
                      setEpisodeOptionIndex(1);
                    }
                  } 
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
                (series && series.seasons && series.seasons[currentSeason]) 
                ? (<SelectionPane items={currentItems} path={"/title"} 
                currentIndex={episodeOptionIndex} name={"Episodes"} 
                amountOfPages={Math.ceil(series.seasons[currentSeason].length / pageSize)} function={setEpisodeOptionIndex}/> 
                ) 
                : "Loading!"
              }
            </Col>
          </Row>
          <br/>
        </Col>
      </Row>
      <TitleReviews updater={updater} setUpdater={setUpdater}  id={id}/>
      </Container>
  );
}
  
export default Series;