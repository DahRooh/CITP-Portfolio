import 'bootstrap/dist/css/bootstrap.css';
import { Link } from 'react-router';
import { Button, Col, Row } from 'react-bootstrap';

import { StarRatingFixed } from './StarRatingFixed.js';
import { useEffect, useState } from 'react';


function InfoBox({ title, cookies }) {
  let titleImage = (title.poster !== "N/A") ? title.poster : "https://media.istockphoto.com/id/911590226/vector/movie-time-vector-illustration-cinema-poster-concept-on-red-round-background-composition-with.jpg?s=612x612&w=0&k=20&c=QMpr4AHrBgHuOCnv2N6mPUQEOr5Mo8lE7TyWaZ4r9oo=";
  const[userBookmarks, setUserBookmarks] = useState(false);
  useEffect(() => {
        if (cookies){
          fetch(`http://localhost:5001/api/user/${cookies.userid}/bookmarks`, {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
              "Authorization": "Bearer " + cookies.token
            }
          })
          .then(res => {
            if (res.ok) return res.json();
            return null; // no user
          })
          .then(data => {
            if (data) {
              data.forEach(bookmark => {
                if (bookmark.id === title.id) setUserBookmarks(bookmark); 
              }) 
            }
          })}
        }, [cookies, title]);


    function bookmark() {
        if (cookies && cookies.token) {
            fetch(`http://localhost:5001/api/title/${title.id}/bookmark`, {
              method: "POST",
              headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + cookies.token
              }
            })
            .then(res =>{
              if (res.ok) {
                return res.json();
              }
              return null;
            })
            .then ( data => {
              if (data) {
                setUserBookmarks(data);
              }
            });
          }
      }
    
      function deleteBookmark() {
    
    /*    if (userLoggedIn && userLoggedIn.token) {
          fetch(`http://localhost:5001/api/user/${userLoggedIn.id}/bookmark`, {
            method: "DELETE",
            headers: {
              "Authorization": "Bearer " + userLoggedIn.token
            }
          })
          .then(res =>{
             console.log(res);
          })
        }*/
      }
    
    function BookmarkButton() {
    if (cookies.token) {
        if (!userBookmarks) {
        return (<>
        <Button onClick={bookmark} disabled={!cookies.token}>Bookmark</Button>
        </>)
        }
        return (
        <>
        <Button className='danger-btn' onClick={() => deleteBookmark}>Bookmarked</Button>
        </>
        )
    }
    return <>
        <p style={{backgroundColor: "grey"}}> To bookmark a title please log in or sign up</p>
        </>
    }

    return (
        <Col xs={4} className='titleInfo'>
        <Row>
        <Col>
          <img
            src={titleImage}
            alt="Title"
          />
        </Col>
          <Row>
            <Col>
              {(title.rating) ? <StarRatingFixed key={title.id} titleRating={title.rating}/> : "loading rating"}

              <p>Total rating: {title.rating}</p>
            </Col>

          </Row>

          <Row>
            <Col>
              <p>Release: {title.released}</p>
            </Col>
            <Col>
              <p>Language: {title.language}</p>
            </Col>
          </Row>

          <Row>
            <Col>
              <p>Runtime: {title.runTime} minutes</p>
            </Col>
            <Col>
              <p>Country: {title.country}</p>
            </Col>
          </Row>

          <Row>
            <Col>
              <span>Plot:</span>
              <p>{(title.plot) || "No plot for this title"}</p> {/*  equivalent to: (title.plot) ? title.plot : "string" */}
            </Col>
          </Row>

          <Row>
            <Col>
              <BookmarkButton />
            </Col>
            <Col>  
              {(cookies.token) ? <Link to={`/title/${title.id}/newReview`}>
                <Button>Create Review</Button> </Link>: <p style={{backgroundColor: "grey"}}>To create a review please log in</p>}
              
            </Col>
          </Row>

        </Row>
      </Col>
    );
}

export default InfoBox;