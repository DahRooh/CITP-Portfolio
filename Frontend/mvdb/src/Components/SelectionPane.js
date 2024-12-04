import { useEffect, useState } from "react";
import { Link } from "react-router";
import { Button, Row, Col, Container } from 'react-bootstrap';

function handleData(data, setData) {
    var keys = Object.keys(data);
    keys.forEach(key => {
        if (data[key].length > 0) {
            setData((data[key][0].profile_path) ? data[key][0].profile_path : data[key][0].poster_path)
        }
    });
}

function Option({item, path}) {
    const [poster, setPoster] = useState(false);

    const apiKey = "c8190d104e34c4f62a2be88afa477327";
    var posterUrl = "https://image.tmdb.org/t/p/w500";
    
    const fetchPoster = `https://api.themoviedb.org/3/find/${item.id}?api_key=${apiKey}&external_source=imdb_id`;

    useEffect(() => {
        fetch(fetchPoster)
        .then(res =>  res.json())
        .then(data => handleData(data, setPoster));
    })


    return (
        <Col className="selectionOption" md={2}>
            <Link to={path}>
                <Row>
                    <Col>
                        {<img src={(poster) ? posterUrl+poster : "https://media.istockphoto.com/id/911590226/vector/movie-time-vector-illustration-cinema-poster-concept-on-red-round-background-composition-with.jpg?s=612x612&w=0&k=20&c=QMpr4AHrBgHuOCnv2N6mPUQEOr5Mo8lE7TyWaZ4r9oo="}
                         width={"100%"} height={"100%"}/>}
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <p>{item.name || item._Title}</p>
                    </Col>
                </Row>
            </Link>
        </Col>

    ); 
}

function SelectionPane({items, path, currentIndex, name, amountOfPages, function: setIndex}) {


    return (

        <Container className="selectionPane centered">
            <Row className="centered">
                <h2>{name}</h2>
            </Row>
            <Row>
            <Col md={1}>
                <Button onClick={() => setIndex(c => c - 1)} disabled={currentIndex === 1}>&larr;</Button>
            </Col>
            <Col>
                <Row className="centered">
                    {items.map(item => <Option key={item.id} item={item} path={`${path}/${item.id}`}/>)}
                </Row>
            </Col>
            <Col md={1}>
                <Button onClick={() => setIndex(c => c + 1)} disabled={currentIndex === amountOfPages}>&rarr;</Button>
            </Col>
            </Row>
            <p className="centered">Page: {currentIndex}</p>
        </Container>

    );
}

export default SelectionPane;
