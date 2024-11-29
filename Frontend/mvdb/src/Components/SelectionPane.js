import { useEffect, useState } from "react";
import { Link } from "react-router";

function Option({item, path}) {
    return (
        <div className="col">
        <Link to={path}> 
            <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpHrvCwx8k_WyIOri6iWKD443_4bR3_zwUCw&s"
                style={{width: "60px"}}/>
            <p>{item}</p>
            <p>Rating: NaN</p>
        </Link>

        </div>
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
    <div className="row">
        <h2 style={{textAlign: "center"}}>{name}</h2>
    </div>

    <div className="row">
        <div className="col-2 selectionButtonCon">
            <button className="selectionButton" onClick={() => setPage(() => page - 1)} disabled={page == 1}>{"<-"}</button>
        </div>

        <div className="col">
            <div className="row">
            {items.slice(currentPage, endIndex).map((item, i) => <Option key={i} item={item} path={path + ""}/>) /* ved "" skal der være path til id, "/" + t.id*/}
            </div>
        </div>

        <div className="col-2 selectionButtonCon">
            <button className="selectionButton" onClick={() => setPage(() => page + 1)} disabled={page == pages}>{"->"}</button>
        </div>
        </div>
    </>
    );
}

export default SelectionPane;
