import 'bootstrap/dist/css/bootstrap.css';
import { Container, Row, Col, Button, ButtonGroup } from 'react-bootstrap';

function SearchResult() {
  return (

    <div className="fullscreen">
      <Container>
        <Row className="textHeader">
          <Col>
              Keyword: "keyword"
          </Col>
        </Row>

          <Row className="search" style={{paddingLeft: "5vw", paddingRight: "5vw", paddingTop: "5vh"}}>

            <Row>
              <Col className="search" style={{paddingRight: "5vw"}}>
                <img className="searchResultPictures" src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpHrvCwx8k_WyIOri6iWKD443_4bR3_zwUCw&s"/>
                <span style={{ display: "block", textAlign: "center", margin: "0 auto" }}>
                  Search result 1
                </span>
              </Col>
            </Row>

            <Row>
              <Col className="search" style={{paddingRight: "5vw"}}>
                <img className="searchResultPictures" src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpHrvCwx8k_WyIOri6iWKD443_4bR3_zwUCw&s"/>
                <span style={{ display: "block", textAlign: "center", margin: "0 auto" }}>
                  Search result 2
                </span>
              </Col>
            </Row>

            <Row>
              <Col className="search" style={{paddingRight: "5vw"}}>
                <img className="searchResultPictures" src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpHrvCwx8k_WyIOri6iWKD443_4bR3_zwUCw&s"/>
                <span style={{ display: "block", textAlign: "center", margin: "0 auto" }}>
                  Search result 3
                </span>
              </Col>
            </Row>
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
  
export default SearchResult;