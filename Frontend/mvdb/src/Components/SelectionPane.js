import { Link } from "react-router";
import { Button, Row, Col, Container } from 'react-bootstrap';
import ImageFor from "./ImageFor";



function Option({item, path}) {
    return (
        <Col className="selectionOption" md={2} >
            <Link to={path}>
                <Row>
                    <Col>
                        <ImageFor item={item} />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <p style={{fontSize: "0.8em", marginBottom: "0px"}}>{item.name || item.person || item._Title || item.knownForTitle}</p>

                        <p style={{fontSize: "0.8em", margin: 0}}><small>Rating {item.rating || item.personRating || item.titleRating}</small></p>

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
                <Button className="selectionButton" onClick={() => setIndex(c => c - 1)} disabled={currentIndex === 1}>&larr;</Button>
            </Col>
            <Col>
                {(items)
                ? <Row className="centered">
                    {(items.length > 0) 
                    ? items.map((item, index) => <Option key={`${item.id}-${index}`} item={item} path={`${path}/${item.id}`}/>)
                    : "No items"}
                </Row>
                : "Loading!"}
            </Col>
            <Col md={1}>
                <Button className="selectionButton" onClick={() => setIndex(c => c + 1)} disabled={currentIndex === amountOfPages}>&rarr;</Button>
            </Col>
            </Row>
            <p className="centered">Page: {currentIndex}</p>
        </Container>

    );
}

export default SelectionPane;
