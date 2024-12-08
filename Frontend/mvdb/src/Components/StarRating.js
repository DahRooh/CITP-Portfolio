import 'bootstrap/dist/css/bootstrap.css';
import { Container, Row, Col, Button, ButtonGroup, InputGroup, Form } from 'react-bootstrap';
import { useState } from 'react';

export function StarRating({ amountOfStars }) {
    const [rating, setRating] = useState(0);
  
    const onClickStar = (index) => {
      const newRating = index + 1;
      setRating(newRating);
    };
  
    const buttons = [];
    for (let i = 0; i < amountOfStars; i++) {
      buttons.push(
        <Button
          key={i}
          className={`btn ${
            i < rating ? "btn-warning" : "btn-light"
          } rounded-circle`}
          onClick={() => onClickStar(i)}
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
        </Button>
      );
    }
  
    return <ButtonGroup>{buttons}</ButtonGroup>;
  }
  