import 'bootstrap/dist/css/bootstrap.css';
import { useEffect, useState } from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import { Link, useLocation } from 'react-router';
import ImageFor from './ImageFor';
import { Paging } from './Pagination';
import Cookies from 'js-cookie';

function SearchResult({ result }) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
    const fetchData = async () => {
      try {
        let response;
        if (result.id) {
          
          response = await fetch(`http://localhost:5001/api/title/${result.id}`, {
            headers: {
              Authorization: "Bearer: " + Cookies.get("token")
            }
          }
         );
        } else if (result.p_id) {
          
          response = await fetch(`http://localhost:5001/api/person/${result.p_id}`, {
            headers: {
              Authorization: "Bearer: " + Cookies.get("token")
            }
          });
        }
        const fetchedData = await response.json();
        console.log("DATA: ", data);
        setData(fetchedData);
      } catch (error) {
        console.error('Fejl ved hentning af data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [result]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (!data) {
    return <div>No data found!</div>;
  }

  const displayName = result.type === "series" ? result.name : result.title;

  return (
    <Row>
      {(() => {
        if (result.id) {
          return (
            <Link to={`http://localhost:3000/title/${result.id}`}>
              <Col className="search" style={{ paddingRight: "5vw" }}>
                <ImageFor item={data} width="20%" />
                <span style={{ display: "block", textAlign: "center", margin: "0 auto" }}>
                  {displayName}
                </span>
              </Col>
            </Link>
          );
        } else if (result.p_id) {
          return (
            <Link to={`http://localhost:3000/person/${result.p_id}`}>
              <Col className="search" style={{ paddingRight: "5vw" }}>
                <ImageFor item={data} width="20%" />
                <span style={{ display: "block", textAlign: "center", margin: "0 auto" }}>
                  {result.name}
                </span>
              </Col>
            </Link>
          );
        }
      })()}
    </Row>
  );
}

function SearchResults() {

  const [searchResults, setSearchResults] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [loading, setLoading] = useState(true);
  const location = useLocation();

  const queryParams = new URLSearchParams(location.search);
  const keyword = queryParams.get('keyword');
  
  const pageSize = 5;
  useEffect(() => {
    const fetchData = async () => {
      const res = await fetch(`http://localhost:5001/api/search?keyword=${keyword}&page=${page}&pageSize=${pageSize}`, {
        headers: {
          Authorization: "Bearer " + Cookies.get("token")
        }
      });
      try {
        console.log("fetch data")
        console.log(res);
        if (res.ok) {
          var data = await res.json(); 
          if (data) {
            setSearchResults(data.items);
            setTotalPages(data.totalNumberOfPages);
            setLoading(false);
          } else {
            throw new Error("no results");
          }
        } else {
          throw new Error(`Cannot fetch data ${res.status}`)
        }
      } catch (error) {
        console.log(error);
      }
      
    }
    fetchData();}, [keyword, page]);

    

  return (

     
      <Container>
        <Row className="textHeader">
          <Col>
            <p>Searched for: {keyword}</p>
          </Col>

          

        </Row>
          <Row className="search centered" style={{paddingLeft: "5vw", paddingRight: "5vw", paddingTop: "5vh"}}>

            {
            (!loading) ? (searchResults.length > 0)  // if loading, then if there are results
            ? searchResults.map(result => <SearchResult result={result}/>) 
            : <h2 className='centered'>No results!</h2>
            : <h2 className='centered'>Loading results!</h2>
            }            
            <Row >
              <Col>
                <Paging index={page} total={totalPages} setIndex={setPage}/>
              </Col>
            </Row>
          </Row>


        
      </Container>
  );
}
  
export default SearchResults;