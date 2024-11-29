import { Link } from "react-router";
import SelectionPane from "./SelectionPane";


function Frontpage() {
  let items = ["title1", "title2", "title3", "title4", "title5", "title6"];
  let path = "/title";

  return (
    <div className="container">
      { // rows for our links to pages
      }
      <div className="row">
        <div className="col" style={{"textAlign": "center"}}>
          <h1>MVDb</h1>
        </div>
      </div>
      <br/>
      <div className="row">
        <div className="col">
          <SelectionPane items={items} path={path} name="Popular Movies"/> {/* row and col inside selection pane*/}
          <br/>
          <SelectionPane items={items} path={path} name="Popular Series"/> 
          <br/>
          <SelectionPane items={items} path={path} name="Popular Actors"/> 
        </div>
      </div>

    </div>
  );
}

export default Frontpage;
