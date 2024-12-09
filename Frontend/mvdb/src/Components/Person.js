import 'bootstrap/dist/css/bootstrap.css';
import SelectionPane from "./SelectionPane";
import { Row, Col, Container } from "react-bootstrap";
import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { Picture } from "./Picture";

function Person() {
  const apiKey = "c8190d104e34c4f62a2be88afa477327";
  const [personIDs, setPersonID] = useState([]);
  const [personPictures, setPersonPictures] = useState([]);

  const [coActorsIndex, setcoActorsIndex] = useState(1);
  const [coActors, setCoActors] = useState([]);
  const [coActorsDetails, setcoActorsDetails] = useState([]);


  const [knownForIndex, setKnownForIndex] = useState(1);
  const [knownFor, setKnownFor] = useState([]);
  const [knownForDetails, setknownForDetails] = useState([]);

  const itemsPerPage = 5;
  const maxItems = 100;


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





useEffect(() => {
  fetch(`http://localhost:5001/api/person/${p_id}/coactors?page=${coActorsIndex}&pageSize=${itemsPerPage}`)
  .then(res => {
    if (res.ok) return res.json();
    throw new Error("Could not fect"); // no results
  })
  .then(data => {
    if (data && data.items) {
      setCoActors(data.items);
      setcoActorsDetails(data.totalNumberOfPages);
    }
    else throw new Error("No data");
  }) 
  .catch(e => console.log("error", e))
}, [p_id, coActorsIndex]);


useEffect(() => {
  fetch(`http://localhost:5001/api/person/${p_id}/knownfor?page=${knownForIndex}&pageSize=${itemsPerPage}`)
  .then(res => {
    if (res.ok) return res.json();
    throw new Error("Could not fect"); // no results
  })
  .then(data => {
    if (data && data.items) {
      setKnownFor(data.items);
      setknownForDetails(data.totalNumberOfPages)
    }
    else throw new Error("No data");
  }) 
  .catch(e => console.log("error", e))
}, [p_id, knownForIndex]);


console.log("CoActors: ", coActors);
console.log("Known for: ", knownFor);

console.log("Coactors details: ", coActorsDetails);
console.log("Known for details: ", knownForDetails);

let coActorsPaging = coActorsDetails;
let knownForPaging = knownForDetails;

if(coActorsDetails == 0){
  coActorsPaging = 1;
} else if(knownForDetails == 0){
  knownForPaging = 1;
}


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
            <SelectionPane items={coActors} path={`/person`} currentIndex={coActorsIndex} name={"Co-actors"} amountOfPages={coActorsPaging} function={setcoActorsIndex}/> 
            </Col>
          </Row>
          <Row>
            <Col>
            <SelectionPane items={knownFor} path={"/title"} currentIndex={knownForIndex} name={"Known for"} amountOfPages={knownForPaging} function={setKnownForIndex}/>
            </Col>
          </Row>
        </Col>
      </Row>
    </Container>
  );
}

export default Person;
