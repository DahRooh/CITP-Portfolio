import 'bootstrap/dist/css/bootstrap.css';
import { useEffect, useState } from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import { Link, useLocation } from 'react-router';
import ImageFor from './ImageFor';
import { Paging } from './Pagination';
import Cookies from 'js-cookie';


function SearchResult({ result }) {
  
  const type = result.type;
  const displayName = result.type === "series" ? result.name : result.title;
  
  return (
 
    <Row>
      <Col className="search" style={{ paddingRight: "5vw" }}>
        <Link to={`http://localhost:3000/${type}/${result.id}`}> 
          <ImageFor item={result} width="20%" />
            <span style={{ textAlign: "center", margin: "0 auto" }}>
              {displayName}
            </span>
        </Link>
      </Col>
    </Row>
  );
}

// resultet fra fetch kommer til at have item for person og item for titler
// resultaterne skal være i arrays - har gjort det for den ene
// linket skal være varierende
// col i stedet for row

function SearchResults() {
  
  const [personPage, setPersonPage] = useState(1);
  const [titlePage, setTitlePage] = useState(1);
  const [personResults, setPersonResults] = useState([]);
  const [titleResults, setTitleResults] = useState([]);
  const [loading, setLoading] = useState(true);
  const location = useLocation();

  const queryParams = new URLSearchParams(location.search);
  const keyword = queryParams.get('keyword');
  
  const pageSize = 5;

  useEffect(() => {
    
      console.log(Cookies.get("token"));
      fetch(`http://localhost:5001/api/search/titles?keyword=${keyword}&page=${titlePage}&pageSize=${pageSize}`, {
        headers: {
          Authorization: "Bearer " + Cookies.get("token")
        }
      })
      .then(res => {
        console.log(res);
        if (res.ok) {
          return res.json();          
        } else {
          throw new Error(`Cannot fetch data ${res.status}`)
        }
      })
      .then(data => {
        console.log(data);
        if (data) {
          setTitleResults(data);
          setLoading(false);
        } else {
          throw new Error("no results");
        }
      })
      .catch(e => {
        console.log(e);
      })
    }, [keyword, titlePage]);

    useEffect(() => {
    
      console.log(Cookies.get("token"));
      fetch(`http://localhost:5001/api/search/people?keyword=${keyword}&page=${personPage}&pageSize=${pageSize}`, {
        headers: {
          Authorization: "Bearer " + Cookies.get("token")
        }
      })
      .then(res => {
        console.log(res);
        if (res.ok) {
          return res.json();          
        } else {
          throw new Error(`Cannot fetch data ${res.status}`)
        }
      })
      .then(data => {
        console.log(data);
        if (data) {
          setPersonResults(data);
          setLoading(false);
        } else {
          throw new Error("no results");
        }
      })
      .catch(e => {
        console.log(e);
      })
    }, [keyword, personPage]);

    
  return (
      <Container>

        <Row className="textHeader">
          <Col>
            <p>Searched for: {keyword}</p>
          </Col>

        </Row>
        <Row>
        <Col className="search centered" >
          {
              (!loading) ? (titleResults.items)  // if loading, then if there are results
              ? titleResults.items.map(result => <SearchResult result={result}/>) 
              : <h2 className='centered'>No results!</h2>
              : <h2 className='centered'>Loading results!</h2>
            }            
          <Row >
            <Col>
              <Paging index={titlePage} total={titleResults.totalNumberOfPages} setIndex={setTitlePage}/>
            </Col>
          </Row>
        </Col>
        
            
          <Col className="search centered">

            {
            (!loading) ? (personResults.items)  // if loading, then if there are results
            ? personResults.items.map(result => <SearchResult result={result}/>) 
            : <h2 className='centered'>No results!</h2>
            : <h2 className='centered'>Loading results!</h2>
            }            
         <Row >
            <Col>
              <Paging index={personPage} total={personResults.totalNumberOfPages} setIndex={setPersonPage}/>
            </Col>
          </Row>
        </Col>

        </Row>
      </Container>
  );
}
  
export default SearchResults;