import 'bootstrap/dist/css/bootstrap.css';
import { useState, useEffect } from "react";
import {Picture } from "./Picture";


function Person() {
  const [query] = useState("Selena Gomez");
  const apiKey = "c8190d104e34c4f62a2be88afa477327";
  const [pictures, setPictures] = useState([]); 
  const [currentIndex] = useState(0);

  useEffect(() => {
    fetch(`https://api.themoviedb.org/3/search/person?query=${query}&api_key=${apiKey}`)
      .then((res) => res.json())
      .then((data) => setPictures(data.results || []))
      .catch((error) => console.error("Error at fetching data", error));
  }, [query]);

  const picture = pictures[currentIndex];
 
  return (
    <>
      <div className="row">
        <div className="col-4 text-center">
          {picture ? (
            <>
              <Picture picturePath={picture} width={300} height={300} />
              <p>Age: {"VORES AGE" || "Unknown"}</p>
              <p>Department: {picture.known_for_department || "Unknown"}</p>
              <p>Rating: {"VORES RATING" || "N/A"}</p>
            </>
          ) : (
            <p>No picture and stats available</p>
          )}
        </div>
        <div className="col">
          <div className="row">
            <div className="col">
              <h3>{"Vores API NAME" || "Name not available"}</h3>
            </div>
          </div>
          <div className="row">
            <div className="col">
              <p>Co actors</p>
            </div>
          </div>
          <div className="row">
            <div className="col">
              <p>Known for</p>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default Person;