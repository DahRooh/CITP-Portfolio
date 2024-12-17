import 'bootstrap/dist/css/bootstrap.css';
import { useEffect, useState } from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import { Link, useLocation } from 'react-router-dom';
import ImageFor from './ImageFor';
import { Paging } from './Pagination';
import Cookies from 'js-cookie';


function SearchResult({ result }) {
  
  const type = result.type;
  return (
 
    <Link style={{border: "none", outline: "none", textDecoration: "none"}} to={`http://localhost:3000/${type}/${result.id}`}> 
      <Row  className='review'>
          <Col className="search" md={3}>
            <ImageFor item={result} width='100%' height='100%'/>
          </Col>
          <Col className='text-center'>
            <h4>
              {result.text}
            </h4>
            {(result.type !== 'person') ? <p>{(result.type === 'title') ? "movie" : result.type} </p>: null}
            {(result.rating) ? "Rating: " + result.rating : null}
          </Col>

      </Row>
    </Link>
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
  const [loadingTitle, setLoadingTitle] = useState(true);
  const [loadingPerson, setLoadingPerson] = useState(true);
  const location = useLocation();

  const queryParams = new URLSearchParams(location.search);
  const keyword = queryParams.get('keyword');
  
  const pageSize = 5;

  useEffect(() => {
    setLoadingTitle(true);
    fetch(`http://localhost:5001/api/search/titles?keyword=${keyword}&page=${titlePage}&pageSize=${pageSize}`, {
      headers: {
        Authorization: (Cookies.get("token")) ? "Bearer " + Cookies.get("token") : null
      }
    })
    .then(res => {
      if (res.ok) {
        return res.json();          
      } else {
        throw new Error(`Cannot fetch data ${res.status}`)
      }
    })
    .then(data => {
      if (data) {
        console.log(data);
        setTitleResults(data);
        setLoadingTitle(false);
      } else {
        throw new Error("no results");
      }
    })
    .catch(e => {
      console.log(e);
    })
  }, [keyword, titlePage]);

    useEffect(() => {
      setLoadingPerson(true);
      fetch(`http://localhost:5001/api/search/people?keyword=${keyword}&page=${personPage}&pageSize=${pageSize}`)
      .then(res => {
        if (res.ok) {
          return res.json();          
        } else {
          throw new Error(`Cannot fetch data ${res.status}`)
        }
      })
      .then(data => {
        if (data) {
          setPersonResults(data);
          setLoadingPerson(false);
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
          <Col className='border'>
            <h2>Titles:</h2>
            {
              (!loadingTitle) ? (titleResults.items && titleResults.items.length > 0)  // if loading, then if there are results
              ? titleResults.items.map(result => <SearchResult key={result.id} result={result}/>) 
              : <h2 className='centered'>No results!</h2>
              : <h2 className='centered'>Loading results!</h2>
            }            
            <Row>
              <Col>
                <Paging index={titlePage} total={titleResults.totalNumberOfPages} setIndex={setTitlePage}/>
              </Col>
            </Row>
          </Col>
          <br/>
          <Col className='border'>
            <h2>People:</h2>
            {
              (!loadingPerson) ? (personResults.items && personResults.items.length > 0)  // if loading, then if there are results
              ? personResults.items.map(result => <SearchResult key={result.id} result={result}/>) 
              : <h2 className='centered'>No results!</h2>
              : <h2 className='centered'>Loading results!</h2>
            }            
            <Row>
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