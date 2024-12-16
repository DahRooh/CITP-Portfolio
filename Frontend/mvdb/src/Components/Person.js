import 'bootstrap/dist/css/bootstrap.css';
import { Row, Col, Container } from "react-bootstrap";
import { useEffect, useState } from "react";
import { useParams } from "react-router";

import SelectionPane from "./SelectionPane";
import ImageFor from './ImageFor';


function PersonInformation({person}){
  const calculateAge = () => {
    if(person.deathYear && person.birthYear){
      return person.deathYear - person.birthYear;
    } else if (!person.deathYear && person.birthYear) {
      return (new Date().getFullYear()) - person.birthYear;
    } else {
      return "Unknown";
    }
  }

  return (
    <>
      <Row>
        <Col>
          <ImageFor item={person} width={'50%'} />
        </Col>
      </Row>

      <Row>
        <Col>
          <h3>{person.name || "Name not available"}</h3>
          <p>Age: {calculateAge()}</p>
          {/*}<p>Department: {personPicture.known_for_department || "Unknown"}</p>{*/}
          <p>Rating: {person.personRating || "Has no rating"}</p>
        </Col>
      </Row>
    </>
  )
  
}



function Person() {
  const [person, setPersons] = useState([]);

  const [coActorsIndex, setcoActorsIndex] = useState(1);
  const [coActors, setCoActors] = useState([]);
  const [coActorsDetails, setcoActorsDetails] = useState([]);


  const [knownForIndex, setKnownForIndex] = useState(1);
  const [knownFor, setKnownFor] = useState([]);
  const [knownForDetails, setknownForDetails] = useState([]);

  const itemsPerPage = 5;

  const { p_id } = useParams();





  // Fetch person details by person ID
  useEffect(() => {
    if (p_id) {
      fetch(`http://localhost:5001/api/person/${p_id}`)
        .then((res) => {
          if (res.ok) return res.json();
          return {}; // no results
        })
        .then((data) => {
          setPersons(data);
        })
        .catch((error) => console.log("Error fetching person ID", error));
    }
  }, [p_id]);



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



  let coActorsPaging = coActorsDetails;
  let knownForPaging = knownForDetails;

  if(coActorsDetails === 0){
    coActorsPaging = 1;
  } else if(knownForDetails === 0){
    knownForPaging = 1;
  }


  return (
    <Container>
      <Row>
        <Col className="text-center InfoBox">
          <PersonInformation person={person}/> 
        </Col>        
        <Col>
        <SelectionPane items={coActors} path={`/person`} currentIndex={coActorsIndex} name={"Co-actors"} amountOfPages={coActorsPaging} function={setcoActorsIndex}/> 
        <br/>
        <SelectionPane items={knownFor} path={"/title"} currentIndex={knownForIndex} name={"Known for"} amountOfPages={knownForPaging} function={setKnownForIndex}/>
        </Col>
      </Row>
    </Container>
  );
}

export default Person;
