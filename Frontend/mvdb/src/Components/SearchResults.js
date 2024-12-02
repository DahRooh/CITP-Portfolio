import 'bootstrap/dist/css/bootstrap.css';
import { useEffect, useState } from 'react';
import { Container, Row, Col, Button, ButtonGroup } from 'react-bootstrap';

function SearchResult({result, name}) {
  return (
    <Row>
      <Col className="search" style={{paddingRight: "5vw"}}>
        <img className="searchResultPictures" src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpHrvCwx8k_WyIOri6iWKD443_4bR3_zwUCw&s"/>
        <span style={{ display: "block", textAlign: "center", margin: "0 auto" }}>
          {name}
        </span>
      </Col>
    </Row>)
}

function SearchResults() {

  const [searchResults, setSearchResults] = useState([]);

  useEffect(() => {
    fetch(`http://localhost:5001/api/search?keyword=spielberg&page=1&pageSize=20`)
    .then(res => {
      if (res.ok) return res.json(); 
      return {};
    })
    .then(data => {
      if (data) setSearchResults(data.items);
      else new Error("No results found");
    })
    .catch(e => {
      console.log(e)
    })}, []);

    let fake = ["search reasult 1", "search reasult 2", "search reasult 3"];
    

  return (

    <div className="fullscreen">
     
      <Container>
        <Row className="textHeader">
          <Col>
              Keyword: "keyword"
          </Col>

          

        </Row>
          <Row className="search" style={{paddingLeft: "5vw", paddingRight: "5vw", paddingTop: "5vh"}}>

            {fake.map(f => <SearchResult name={f}/>)}            

          </Row>

          <Row style={{textAlign: "center"}}>
            <Col>
              <ButtonGroup >
                <Button className="buttonGroup">{"<"}</Button>
                <Button className="buttonGroup">1</Button>
                <Button className="buttonGroup">2</Button>
                <Button className="buttonGroup">3</Button>
                <Button className="buttonGroup">{">"}</Button>
              </ButtonGroup>
            </Col>
          </Row>
        
      </Container>
    </div>
  );
}
  
export default SearchResults;