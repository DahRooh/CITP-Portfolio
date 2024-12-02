import { useEffect, useState } from "react";
import { Link } from "react-router";
import { Button, Row, Col, Container } from 'react-bootstrap';

function Option({item, path}) {
    let poster = (item.poster !== "N/A") ? item.poster :"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpHrvCwx8k_WyIOri6iWKD443_4bR3_zwUCw&s"; 
    console.log(item);
    
     if (item.name) { 
        poster = "https://thumbs.dreamstime.com/b/portrait-handsome-smiling-young-man-folded-arms-smiling-joyful-cheerful-men-crossed-hands-isolated-studio-shot-172869765.jpg";
     }
    return (
        <Col className="selectionOption">
            <Row>
                <Link to={path}> 

                <Col>
                    <img src={poster} className="selectionImage"/>
                </Col>
                
                <Col>
                    <span>{item._Title}{item.name}</span>
                    <br/>
                    <span>Rating: {item.rating}</span>
                </Col>
                </Link>
            </Row>
        </Col>
    ); 
}

const SelectionPane = ({items, path, name}) => {

    const [page, setPage] = useState(1);
    let total = items.length;
    const itemsPerPage = 5;
    let pages = Math.ceil(total/itemsPerPage);

    let currentPage = (page - 1) * itemsPerPage;
    let endIndex = currentPage + itemsPerPage;

    return (
        <>
    <Row>
        <h2>{name}</h2>
    </Row>
    <Container className="selectionPane">

    <Row>
        <Col md={1} className="selectionButtonCon">
            <Button className="selectionButton" variant="secondary" onClick={() => setPage(() => page - 1)} disabled={page === 1}>{"<"}</Button>
        </Col>

        <Col>
            <Row>
            {items.slice(currentPage, endIndex).map((item, i) => <Option key={i} item={item} path={path + `/${item.id}`}/>)}
            </Row> 
        </Col>

        <Col md={1} className="selectionButtonCon">
            <Button className="selectionButton" variant="secondary" onClick={() => setPage(() => page + 1)} disabled={page === pages}>{">"}</Button>
        </Col>
        
        </Row>
    </Container>
    </>
    );
}

export default SelectionPane;
