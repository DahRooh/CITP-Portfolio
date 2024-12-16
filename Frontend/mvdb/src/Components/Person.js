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
    <Container style={{height: "80vh"}}>
      <Row>
        <Col>
          <ImageFor item={person} width={'50%'} />
        </Col>
      </Row>

      <Row>
        <Col>
          <h3>{person.name || "Name not available"}</h3>
          <p>Age: {calculateAge()}</p>
          <p>Rating: {person.personRating || "Has no rating"}</p>
        </Col>
      </Row>
    </Container>
  )
  
}



function Person() {
  const [person, setPersons] = useState([]);


  const [coActors, setCoActors] = useState(false);
  const [coActorsPage, setcoActorsPage] = useState(1);


  const [knownFor, setKnownFor] = useState(false);
  const [knownForPage, setKnownForPage] = useState(1);

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
    fetch(`http://localhost:5001/api/person/${p_id}/coactors?page=${coActorsPage}&pageSize=${itemsPerPage}`)
    .then(res => {
      if (res.ok) return res.json();
      throw new Error("Could not fect"); // no results
    })
    .then(data => {
      if (data) {setCoActors(data);}
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [coActorsPage, p_id]);




  useEffect(() => {
    fetch(`http://localhost:5001/api/person/${p_id}/knownfor?page=${knownForPage}&pageSize=${itemsPerPage}`)
    .then(res => {
      if (res.ok) return res.json();
      throw new Error("Could not fect"); // no results
    })
    .then(data => {
      if (data) {setKnownFor(data);}
      else return new Error("No data");
    }) 
    .catch(e => console.log("error", e))
  }, [knownForPage, p_id]);




  let coActorsPaging = coActors.totalNumberOfPages;
  let knownForPaging = knownFor.totalNumberOfPages;

  if(coActors.totalNumberOfPages === 0){
    coActorsPaging = 1;
  } else if(knownFor.totalNumberOfPages === 0){
    knownForPaging = 1;
  }


  return (
    <Container>
      <Row>
        <Col className="text-center InfoBox">
          <PersonInformation person={person}/> 
        </Col>        
        <Col>
        <SelectionPane items={coActors.items} path={`/person`} currentIndex={coActorsPage} name={"Co-actors"} amountOfPages={coActorsPaging} function={setcoActorsPage}/> 
        <br/>
        <SelectionPane items={knownFor.items} path={"/title"} currentIndex={knownForPage} name={"Known for"} amountOfPages={knownForPaging} function={setKnownForPage}/>
        </Col>
      </Row>
    </Container>
  );
}

export default Person;
