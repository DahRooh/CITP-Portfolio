import { Link } from "react-router";

function TitleOption({title}) {
  return (
      <>
        {title}
      </>

  ); 
}

function Frontpage() {
  let titles = ["title1", "title2", "title3"]
  return (
    <>
      { // rows for our links to pages
      }
      <div className="row">
        <div className="col" style={{"textAlign": "center"}}>
          FRONT PAGE
        </div>
      </div>
      <br/>
      <div className="row">
        <div className="col">
          <button>{"<-"}</button>
          {titles.map(t => <TitleOption key={t} title={t}/>)}
          <button>{"->"}</button>
        </div>
      </div>
      <br/>

      <div className="row">
        <div className="col">
          <button>{"<-"}</button>
          {titles.map(t => <TitleOption key={t} title={t}/>)}
          <button>{"->"}</button>
        </div>
      </div>
      <br/>

      <div className="row">
        <div className="col">
          <button>{"<-"}</button>
            {titles.map(t => <TitleOption key={t} title={t}/>)}
          <button>{"->"}</button>
        </div>
      </div>
      <Link to="/search"> search </Link>
      <Link to="/series"> series </Link>
      <Link to="/title"> title </Link>
      <Link to="/signUp"> signUp </Link>
      <Link to="/signIn"> signIn </Link>
    </>
  );
}

export default Frontpage;
