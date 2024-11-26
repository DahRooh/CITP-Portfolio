import 'bootstrap/dist/css/bootstrap.css';

function Person() {
  return (
    <>
      <div className="row">
        <div className="col" style={{"text-align": "center"}}>
          PERSON PAGE
        </div>
      </div>
      <br/>

      <div className="row">
        <div className="col-4">
          <img src="https://media.istockphoto.com/id/492529287/photo/portrait-of-happy-laughing-man.jpg?s=612x612&w=0&k=20&c=0xQcd69Bf-mWoJYgjxBSPg7FHS57nOfYpZaZlYDVKRE="></img>
          <p>age</p>
          <p>department </p>
          <p>rating: </p>
        </div>
        <div className="col">
          <div className="row">
            <div className="col">
              name
            </div>
            <div className="row">
              <div className="col">
                box1
              </div>
            </div>
            <div className="row">

              <div className="col">
                box2
              </div>
            </div>
          </div>
        </div>
      </div>
    
    
    </>
  );
}
  
export default Person;