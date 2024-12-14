import 'bootstrap/dist/css/bootstrap.css';
import './User.css';
import { Paging } from './Pagination.js';
import {Button, ButtonGroup, Form,  Row, Col, Container, Image} from 'react-bootstrap';
import { useEffect, useState } from "react"; 
import Cookies from 'js-cookie';
import { Timestamp } from './Time.js';

function Search( {searchHistory, updater} ) {

  return (
    <Container className='review' style={{marginTop: 20, marginBottom: 20 }}>
      <Row>
        <Col className='text-start' md={1}>
          <Form>
            <Form.Check
                name="group1"
                type="checkbox"
                id={`checkbox`}
            />
          </Form>
        </Col>

        <Col className='text-start' md={2}>
          {(searchHistory) ? <Timestamp time={searchHistory.createdAt}/>
          : "Loading"}
        </Col>

        <Col className='text-start' md={8}>
          <h6 style={{wordBreak: 'break-word'}}>
            Search keyword: {searchHistory.keyword}
          </h6>
        </Col>

        <Col>
          <Button onClick={e => console.log(e)} >
            <Image src="../../trash.png" roundedCircle/>
          </Button>
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
      console.log(res);
      if (res.ok) {
        setUpdater(c => !c);
      }      
    })
  }
  

  return(
    <Container className='blackBorder'>
      <Row>
        <Col>
        <h3 className="col text-center breakWord">Search History</h3>
        <Col className='text-end'>
          <ButtonGroup>
            <Button variant="secondary">Clear Selected History</Button>
            <Button onClick={clearHistory} variant="secondary">Clear History</Button>
          </ButtonGroup>
        </Col>
      <Row>
        <Col>
          {(searchHistory) ? searchHistory.map((search, i) => (i % 2 === 0) ? <Search key={search.searchId} searchHistory={search}/> : null) : "Loading..."}
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