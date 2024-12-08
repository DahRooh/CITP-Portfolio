import 'bootstrap/dist/css/bootstrap.css';
import { Button, ButtonGroup } from 'react-bootstrap';
import { useEffect, useState } from 'react';

export function StarRatingFixed({ titleRating }) {
    const [rating] = useState(titleRating);
    const [buttons, setButtons] = useState([]);
  
    useEffect(() => {
        var newButtons = [];
        for (let i = 0; i < 10; i++) {
            newButtons.push(
              <Button
                key={i}
                className={`btn ${
                  i < rating ? "btn-warning" : "btn-light"
                } rounded-circle`}
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
        setButtons(newButtons);
    }, [rating])

  
    return <ButtonGroup>{buttons}</ButtonGroup>;
  }