
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
        <div className="col" style={{"text-align": "center"}}>
          FRONT PAGE
        </div>
      </div>
      <br/>
      <div className="row">
        <div className="col">
          <button>{"<-"}</button>
          {titles.map(t => <TitleOption title={t}/>)}
          <button>{"->"}</button>
        </div>
      </div>
      <br/>

      <div className="row">
        <div className="col">
          <button>{"<-"}</button>
          {titles.map(t => <TitleOption title={t}/>)}
          <button>{"->"}</button>
        </div>
      </div>
      <br/>

      <div className="row">
        <div className="col">
          <button>{"<-"}</button>
            {titles.map(t => <TitleOption title={t}/>)}
          <button>{"->"}</button>
        </div>
      </div>


    </>
  );
}

export default Frontpage;
