import 'bootstrap/dist/css/bootstrap.css';
import { Link, useParams } from 'react-router';
import SelectionPane from './SelectionPane.js'
import { Button, ButtonGroup, Col, Container, Row } from 'react-bootstrap';
import { useEffect, useState } from 'react';

import { convertCookie } from './Header.js';
import { StarRatingFixed } from './StarRatingFixed.js';


function Review({ review, userLoggedIn }) {
  const [liked, setLiked] = useState(false);

  useEffect(() => {
    fetch(`http://localhost:5001/api/user/${userLoggedIn.id}/likes`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) setLiked(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [userLoggedIn.id]);

    if (!review) {
      return <Row>
        <Col>
          No Reviews
        </Col>
      </Row>
    }
    function likeReview( reaction ) {
      fetch(`http://localhost:5001/api/title/${review.titleId}/review/${review.reviewId}/like`, {
        method: "POST",
        body: JSON.stringify({
          "like": reaction
        }),
        headers: {
          "Content-Type": "application/json",
          "Authorization": "Bearer " + userLoggedIn.token
        }
      })
      .then(res => {
        console.log(res);
        if (res.ok) {
          return res.json;
        }
        return null;
      }) 
      .catch(e => console.log("error", e))
    }

    return (
      <Row className='review'>
        <Col>
          <Row>
            <Col>
              <p>Username: {review.username}</p>
              <p>Rating: {review.rating}</p>
            </Col>
            <Col >
              <p>{review.caption}</p>
              <p>{review.text}</p>
            </Col>
            <Col>
            <p>Likes: {review.liked}</p>
            <ButtonGroup>
                <Button  onClick={() => likeReview(1)} disabled={!userLoggedIn}>
                  Like
                </Button>
                <Button onClick={() => likeReview(-1)} disabled={!userLoggedIn}>
                  Dislike
                </Button>
            </ButtonGroup>
            </Col>
          </Row>
          <hr/>
          <Row>
            <Col>
              {review.review}
            </Col>
          </Row>
        </Col>
      </Row>
  )
}



function Title() {
  const pageSize = 5;
  
  const {id} = useParams();
  const [title, setTitle] = useState({});
  

  const [similarTitles, setSimilarTitles] = useState(false);
  const [similarTitlesPage, setSimilarTitlesPage] = useState(1);


  const [cast, setCast] = useState(false);
  const [castPage, setCastPage] = useState(1);

  const [crew, setCrew] = useState(false);
  const [crewPage, setCrewPage] = useState(1);
  

  const [reviews, setReviews] = useState([]);
  const [cookies] = useState(() => convertCookie());

  const[userBookmarks, setUserBookmarks] = useState(false);
    
  
  useEffect(() => {
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

  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}/reviews`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) setReviews(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [id]);

  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}/crew?page=${crewPage}&pageSize=${pageSize}`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) setCrew(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [crewPage, id]);

  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}/cast?page=${castPage}&pageSize=${pageSize}`)
    .then(res => {
      if (res.ok) return res.json();
      return null; // no results
    })
    .then(data => {
      if (data) setCast(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [castPage, id]);

  useEffect(() => {
    fetch(`http://localhost:5001/api/title/${id}/similartitles?page=${similarTitlesPage}&pageSize=${pageSize}`)
    .then(res => {
      if (res.ok) return res.json();
      return {}; // no results
    })
    .then(data => {
      if (data && data.items) setSimilarTitles(data);
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [similarTitlesPage, id]);

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
            if (bookmark.id === id) setUserBookmarks(bookmark); 
          }) 
        }
      })}
    }, [cookies, id]);

  function bookmark() {
    if (cookies && cookies.token) {
        fetch(`http://localhost:5001/api/title/${id}/bookmark`, {
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
    if (cookies) {
      if (!userBookmarks) {
        return (<>
        <Button onClick={bookmark}>Bookmark</Button>
        </>)
      }
  
      return (
        <>
        <Button className='danger-btn' onClick={() => deleteBookmark}>Bookmarked</Button>
        </>
      )
    }
    return <>
     <p style={{backgroundColor: "grey"}}> To bookmark a review please log in</p>
     </>
  }

  let titleImage = (title.poster !== "N/A") ? title.poster : "https://media.istockphoto.com/id/911590226/vector/movie-time-vector-illustration-cinema-poster-concept-on-red-round-background-composition-with.jpg?s=612x612&w=0&k=20&c=QMpr4AHrBgHuOCnv2N6mPUQEOr5Mo8lE7TyWaZ4r9oo="
  return (
    <Container className='centered'>
      <Row>
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
                {(cookies) ? <Link to={`/title/${id}/newReview`}>
                  <Button>Create Review</Button> </Link>: <p style={{backgroundColor: "grey"}}>To create a review please log in</p>}
                
              </Col>
            </Row>

          </Row>
        </Col>

        <Col>
          <Row>
            <Col className='centered'>
              <h2>{title._Title}</h2>
            </Col>
          </Row>

          {(similarTitles.items)?  <SelectionPane items={similarTitles.items} path={"/title"} currentIndex={similarTitlesPage} name={"Similar titles"} amountOfPages={similarTitles.totalNumberOfPages} function={setSimilarTitlesPage}/> 
            : "Loading!"}
          <br/>
          {(cast.items) ? <SelectionPane items={cast.items} path={"/person"} currentIndex={castPage} name={"Cast"} amountOfPages={cast.totalNumberOfPages} function={setCastPage}/> 
          : "Loading!"}
          <br/>
          {(crew.items) ? <SelectionPane items={crew.items} path={"/person"} currentIndex={crewPage} name={"Crew"} amountOfPages={crew.totalNumberOfPages} function={setCrewPage}/> 
          : "Loading!"}

        </Col>
      </Row>
      <br/>
      <Container className='reviewContainer'>
        <Row>
          <Col>
          <h3>Reviews</h3>
          </Col>
          <hr/>
        </Row>
        {reviews.map(review => <Review review={review} userLoggedIn={cookies} />)}

      </Container>
    </Container>
  );
}
  
export default Title;