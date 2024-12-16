import 'bootstrap/dist/css/bootstrap.css';
import {  ButtonGroup } from 'react-bootstrap';
import { useEffect, useState } from 'react';

export function StarRatingFixed({ titleRating }) {
    const [rating] = useState(titleRating);
    const [buttons, setButtons] = useState([]);
  
    useEffect(() => {
        var newButtons = [];
        for (let i = 0; i < 10; i++) {
            newButtons.push(
              <button
                key={i}
                className={` ${
                  i < rating ? "stargold" : "stardull"
                } rounded-circle star`}
                style={{
                  width: "20px",
                  height: "25px",
                  marginRight: "5px",
                  display: "flex",
                  justifyContent: "center",
                  alignItems: "center",
                  border: "none",
                  fontSize: 20,
                }}
              >
                ☆
              </button>
            );
        }
        setButtons(newButtons);
    }, [rating])

  
    return <ButtonGroup>{buttons}</ButtonGroup>;
  }