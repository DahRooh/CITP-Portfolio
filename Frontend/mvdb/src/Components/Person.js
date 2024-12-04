import 'bootstrap/dist/css/bootstrap.css';
import { Row, Col, Container } from "react-bootstrap";
import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { Picture } from "./Picture";

function Person() {
  const apiKey = "c8190d104e34c4f62a2be88afa477327";
  const [personIDs, setPersonID] = useState([]);
  const [personPictures, setPersonPictures] = useState([]);
  const [coActors, setCoActors] = useState([]);

  const [currentIndex] = useState(0);
  const [titleImages, setTitleImages] = useState([]);
  const { t_id, p_id } = useParams();

  // Fetch person details by person ID
  useEffect(() => {
    console.log("PERSON ID :", p_id);
    if (p_id) {
      fetch(`http://localhost:5001/api/person/${p_id}`)
        .then((res) => {
          console.log("Person Data: ", res);
          if (res.ok) return res.json();
          return {}; // no results
        })
        .then((data) => {
          setPersonID([data]);
          console.log("Person Data: ", data);
        })
        .catch((error) => console.log("Error fetching person ID", error));
    }
  }, [p_id]);

 const personID = personIDs[0];
 console.log("personID:", personID);

  // Fetch pictures when person data is available
  useEffect(() => {
    console.log("personID?.name:", personID?.name);
    if (personID?.name) {
      fetch(`https://api.themoviedb.org/3/search/person?query=${personID.name}&api_key=${apiKey}`)
        .then((res) => res.json())
        .then((data) => setPersonPictures(data.results || []))
        .catch((error) => console.error("Error fetching person data:", error));
    }
  }, [personID]);

  const personPicture = personPictures[currentIndex];
  console.log("HAA:", personPicture);



// Fetch pictures of Coactors when person data is available
  useEffect(() => {
    if (p_id) {
      fetch(`http://localhost:5001/api/person/${p_id}`)
        .then((res) => res.json())
        .then((data) => setCoActors([data] || []))
        .catch((error) => console.error("Error fetching person data:", error));
    }
  }, [personID]);

  const coActor = coActors[currentIndex];


/*
  // Fetch title details by title ID
  useEffect(() => {
    if (t_id) {
      console.log("t_id:", t_id);
      fetch(`http://localhost:5001/api/title/${t_id}`)
        .then((res) => {
          if (res.ok) return res.json();
          return {}; // no results
        })
        .then((data) => setTitleImages([data]))
        .catch((error) => console.log("Error fetching title data", error));
    }
  }, [t_id]);

  const titleImage = titleImages[0];
*/



  return (
    <Container>
      <Row>
        <Col className="text-center">
          {personPicture ? (
            <>
              <Picture type="person" personPath={personPicture} width={300} height={300} />
              <p>Age: {!(personID.deathYear == null) ? personID.deathYear - personID.birthYear : (new Date().getFullYear()) - personID.birthYear || "Unknown"}</p>
              <p>Department: {personPicture.known_for_department || "Unknown"}</p>
              <p>Rating: {personID.personRating || "Has no rating"}</p>
            </>
          ) : (
            <p>Picture and stats are not available</p>
          )}
        </Col>
        <Col>
          <Row>
            <Col>
              <h3>{personPicture?.name || "Name not available"}</h3>
            </Col>
          </Row>
          <Row>
            <Col>
              <p>Co-actors</p>
            </Col>
          </Row>
          <Row>
            <Col>
              <p>Known for</p>
            </Col>
          </Row>
        </Col>
      </Row>
{/*
      <Container>
        <Row>
          <Col>
            <h1>{titleImage?._Title || "Title not available"}</h1>
            {titleImage?.id ? (
              <Picture type="title" titlePath={titleImage} width={200} height={400} />
            ) : (
              <p>Image not available</p>
            )}
          </Col>
        </Row>
      </Container> */}
    </Container>
  );
}

export default Person;
