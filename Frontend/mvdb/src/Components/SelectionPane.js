import { useEffect, useState } from "react";
import { Link } from "react-router";
import { Button, Row, Col } from 'react-bootstrap';

function Option({item, path}) {
    let poster = (item.poster !== "N/A") ? item.poster :"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpHrvCwx8k_WyIOri6iWKD443_4bR3_zwUCw&s"; 

    
     if (item.name) { 
        poster = "https://thumbs.dreamstime.com/b/portrait-handsome-smiling-young-man-folded-arms-smiling-joyful-cheerful-men-crossed-hands-isolated-studio-shot-172869765.jpg";
     }
    return (
        <Col>
            <Row>
                <Link to={path}> 
                <Col>
                    <img src={poster} style={{width: "100%"}}/>
                </Col>
                <Col>
                    <p>{item._Title}{item.name}</p>
                    <p>Rating: {item.rating}</p>
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
        <h2 style={{textAlign: "center"}}>{name}</h2>
    </Row>

    <Row className="selectionPane">
        <div className="col-1 selectionButtonCon">
            <Button className="selectionButton" onClick={() => setPage(() => page - 1)} disabled={page === 1}>{"<-"}</Button>
        </div>

        <Col>
            <Row>
            {items.slice(currentPage, endIndex).map((item, i) => <Option key={i} item={item} path={path + `/${item.id}`}/>)}
            </Row>
        </Col>

        <div className="col-1 selectionButtonCon">
            <Button className="selectionButton" onClick={() => setPage(() => page + 1)} disabled={page === pages}>{"->"}</Button>
        </div>
        </Row>
    </>
    );
}

export default SelectionPane;
