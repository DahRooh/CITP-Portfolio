import 'bootstrap/dist/css/bootstrap.css';
import { useParams } from 'react-router';
import SelectionPane from './SelectionPane.js'
function Review({review}) {

}

function Title() {
  const {id} = useParams();
  return (
    <div className='container'>
      <div className='row'>
        <div className='col-4'>
          <div className='row'>
            <div className='col'>
              <img style={{width: "100%", height: "100%"}} src="https://media.istockphoto.com/id/911590226/vector/movie-time-vector-illustration-cinema-poster-concept-on-red-round-background-composition-with.jpg?s=612x612&w=0&k=20&c=QMpr4AHrBgHuOCnv2N6mPUQEOr5Mo8lE7TyWaZ4r9oo="/>
            </div>
            <div className='row'>
              <div className='col'>
                <p>Total rating: 1.0</p> 
              </div>
              <div className='col'>
                <p>Total amount of voters: 1245</p> 
              </div>
            </div>
            <div className='row'>
              <div className='col'>
                <span>Plot:</span>
                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum</p> 
              </div>
            </div>
            <div className='row'>
              <div className='col centered'>
                <button>Create Review</button>
              </div>
            </div>

          </div>
        </div>
        <div className='col'>
          <div className='row'>
            <div className='col' style={{textAlign: "center"}}>
              <h2>name of title</h2>
            </div>
          </div>
          <br/>
          <div className='row'>
            <div className='col centered'>
              <h3>Main cast</h3>
              <SelectionPane items={["item1", "item2", "item3", "item4", "item5", "item6"]}/>
            </div>
          </div>

          <div className='row'>
            <div className='col centered'>
              <h3>Main crew</h3>
              <SelectionPane items={["item1", "item2", "item3", "item4", "item5", "item6"]}/>
            </div>
          </div>

          <div className='row'>
            <div className='col centered'>
              <h3>Similar titles</h3>
              <SelectionPane items={["item1", "item2", "item3", "item4", "item5", "item6"]}/>
            </div>
          </div>

        </div>
      </div>

      <div className='row'>
        <div className='col centered'>
          REVIEWS GO HERE
        </div>
      </div>
    </div>
  );
}
  
export default Title;