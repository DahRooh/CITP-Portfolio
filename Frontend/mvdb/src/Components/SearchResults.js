import 'bootstrap/dist/css/bootstrap.css';

function SearchResult() {
  return (
    <div style={{display: "flex", flexDirection: "column", 
    justifyContent: "center", height: "80vh", margin: 0, 
    transform: "translateY(-10px)"}}>

      <div className="row" style={{"textAlign": "center", 
        "fontWeight": "bold", marginBottom: "12%", marginTop: "15%", 
        fontSize: "220%"}}>

        <div className="col">
          Keyword: "Keyword"
        </div>
      </div>
      
      <div className="row search" style={{marginLeft: "20%", marginRight: "20%"}}>
        <div className="col">
          <img className="searchResultPictures" src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpHrvCwx8k_WyIOri6iWKD443_4bR3_zwUCw&s"/>
          <hr/>
        </div>
        <div className="col">
            "search result 1"
        </div>
      </div>

      <div className="row search" style={{marginLeft: "20%", marginRight: "20%"}}>
        <div className="col">
          <img className="searchResultPictures" src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpHrvCwx8k_WyIOri6iWKD443_4bR3_zwUCw&s"/>
          <hr/>
        </div>
        <div className="col">
            "search result 2"
        </div>
      </div>

      <div className="row search" style={{marginLeft: "20%", marginRight: "20%"}}>
        <div className="col">
          <img className="searchResultPictures" src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpHrvCwx8k_WyIOri6iWKD443_4bR3_zwUCw&s"/>
          <hr/>
        </div>
        <div className="col">
            "search result 3"
        </div>
      </div>

      <div className="row"  style={{"textAlign": "center"}}>
        <div className="col">
          <button> &larr; </button>
          <button>1</button>
          <button>2</button>
          <button>3</button>
          <button> &rarr; </button>
        </div>
      </div>
    </div>
  );
}

/*
Som den aller første:
 <div style={{display: "flex", flexDirection: "column", justifyContent: "center", height: "100vh", margin: 0, transform: "translateY(-200px)"}}>

 Fungerer ikke her??
*/

export default SearchResult;