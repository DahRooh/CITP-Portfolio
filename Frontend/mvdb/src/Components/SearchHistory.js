import 'bootstrap/dist/css/bootstrap.css';
import './User.css';
import { Paging } from './Pagination.js';
import {Button, ButtonGroup, Form,  Row, Col, Container, Image} from 'react-bootstrap';
import { useEffect, useState } from "react"; 
import Cookies from 'js-cookie';
import { Timestamp } from './Time.js';
import trashIcon from '../trash.png';
import { Popup } from './Popup.js';

function Search( {searchHistory} ) {

  return (
    <Container className='review' style={{marginTop: 20, marginBottom: 20 }}>
      <Row>
        <Col className='text-start' md={2}>
          {(searchHistory) ? <Timestamp time={searchHistory.createdAt}/>
          : "Loading"}
        </Col>

        <Col className='text-center'>
          <h6 style={{wordBreak: 'break-word'}}>
            Search keyword: {searchHistory.keyword}
          </h6>
        </Col>
      </Row>

    </Container>

  )
}

function SearchHistory(){
  const [index, setIndex] = useState(0);
  const [searchHistory, setSearchHistory] = useState(null);
  const [updater, setUpdater] = useState(false);
  let cookies = Cookies.get();
  useEffect(() => {
    fetch(`http://localhost:5001/api/user/${cookies.userid}/search_history`, { 
      headers: {
        Authorization: "Bearer " + cookies.token
      }
    })
    .then(res =>  { 
        if (res.ok) return res.json();
      })
    .then (data => {
      setSearchHistory(data)})
  }, [updater]);

  const clearHistory = () => {
    fetch(`http://localhost:5001/api/user/${cookies.userid}/search/clear`, { 
      method: "DELETE",
      headers: {
        Authorization: "Bearer " + cookies.token
      }
    })
    .then(res => {
      if (res.ok) {
        setUpdater(c => !c);
      }      
    })
  }
  

  return(
    <Container className='blackBorder'>
      <Row>
        <Col>
          <h2 className="col text-center breakWord">Search History</h2>
          {(searchHistory && searchHistory.length > 0) ? <Popup deleter={clearHistory}  functionMsg={"Delete search history"} message={"Are you sure you want to clear the search history?"}/> : null}
          <Row>
            <Col>
              {(searchHistory) ? (searchHistory.length > 0) ? 
              searchHistory.map((search, i) => (i % 2 === 0) ? 
              <Search key={search.searchId} searchHistory={search}/> : null) : 
              <p className='review'>No results</p> : "Loading..."}

              <Container className='paging'>
                <Row>
                  <Col>
                    {(searchHistory) ? <Paging index={index} total={Math.ceil(searchHistory.length / 20)} setIndex={setIndex} /> 
                      : null}
                  </Col>
                </Row>
              </Container>
            </Col>
          </Row>
        </Col>
      </Row>
    </Container>
  );

}

  export default SearchHistory;