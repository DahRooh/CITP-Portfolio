import 'bootstrap/dist/css/bootstrap.css';

function SignIn() {
  return (
    <div style={{display: "flex", flexDirection: "column", justifyContent: "center", height: "100vh", margin: 0, transform: "translateY(-200px)"}}>

      <div className="row" style={{"textAlign": "center", 
        "fontWeight": "bold", marginBottom: "12%", marginTop: "15%", 
        fontSize: "220%"}}>

        <div className="col">
            OMBd
        </div>
      </div>

      <div className="row placeholders">
        <div className="col">
          <input className="placeholderText" placeholder="Username"/>
        </div>
      </div>

      <div className="row placeholders">
        <div className="col">
          <input className="placeholderText" placeholder="Password"/>
        </div>
      </div>

      <div className="row" style={{textAlign: "center"}}>
        <div className="col">
          <button style={{width: "20%"}}>
            Login
          </button>
        </div>
      </div>
    </div>
  );
}
  
export default SignIn;