import 'bootstrap/dist/css/bootstrap.css';
import './User.css';
import { Paging } from './Pagination.js';
import {Button, ButtonGroup, Form,  Row, Col, Container, Image} from 'react-bootstrap';
import { useEffect, useState } from "react"; 
import { Link, Outlet, useParams } from 'react-router';
import Cookies from 'js-cookie';
import ImageFor from './ImageFor.js';
import { Timestamp } from './Time.js';
import { Popup } from './Popup.js';



function Bookmark( {item, updater} ) {

  async function deleteBookmark() {
    await fetch(`http://localhost:5001/api/user/${Cookies.get("userid")}/bookmark/${item.id}` , {
      method: "DELETE",
      headers: {
        Authorization: "Bearer " + Cookies.get("token")
      }
    })
    .then(res => {
      if (res.ok) updater(b => !b)
    });
  }
  let type = (item.type === 'movie') ? "title" : item.type; 
  return (
    <div key={item.id} className='container' style={{marginTop: 20, marginBottom: 20 }}>
      <Container className='singlehistoryContainer'>
        <Row>


          <Col className='text-start' md={2}>
            <p style={{fontWeight: 'bold', wordBreak: 'break-word'}}>
              Bookmarked at
              </p>
            <Timestamp time={item.createdAt}/>
          </Col>
            <Col className='text-start'>
              <Link to={`/${type}/${item.titleId}`}>
                <h6 style={{wordBreak: 'break-word'}}>
                  <ImageFor item={item} width='10%'/>
                  <span>Name of Title: {item.title}</span>
                </h6>
              </Link>
            </Col>
            <Popup deleter={deleteBookmark} functionMsg={"Delete bookmark"} message={"Are you sure you want to delete the bookmark?"}/>
        </Row>
      </Container>
    </div>


  )
}


function Bookmarks(){
  const [index, setIndex] = useState(0);
  const [bookmarks, setBookmarks] = useState(null);
  const [updater, setUpdater] = useState(false);

  let cookies = Cookies.get();
  useEffect(() => {
    fetch(`http://localhost:5001/api/user/${cookies.userid}/bookmarks`, { 
      headers: {
        Authorization: "Bearer " + cookies.token
      }
    })
    .then(res => res.json())
    .then (data => {
      if (data) {
        return setBookmarks(data);
      } else {
        setBookmarks([]);
      }
      throw new Error("no bookmarks")
    })
    .catch(e => {
      console.log(e);
    })
  }, [updater]);
  
  return(
  <Container className='blackBorder'>
    <Row>
      <Col>
        <Container>
          <h2 className="col text-center breakWord">Bookmarks</h2>

          <Row className='review'>
            <Col>
              {(bookmarks) 
              ? (bookmarks.length > 0) 
              ? bookmarks.map((bookmark) => ( <Bookmark key={bookmark.id} item={bookmark} updater={setUpdater} /> )) 
              : "You have no bookmarks" : "Loading"}
              <Container className='paging'>
              <Row>
                <Col className='text-center'>
                {(bookmarks) ? <Paging index={index} total={Math.ceil(bookmarks.length / 20)} setIndex={setIndex} /> 
                : null}
                </Col>
              </Row>
              </Container>
            </Col>
          </Row>
        </Container>
        
      </Col>
    </Row>
  </Container>
  );

}

export default Bookmarks;