import 'bootstrap/dist/css/bootstrap.css';
import { Link, Outlet, useParams } from 'react-router';

function UserPage() {
  const { id } = useParams();
  return (
    <>
    this is the user page for user: {id}
    <br/>
    <nav>
      <Link to={`/user/${id}/settings`}/> settings <Link/>
    </nav>
    <Outlet/> 

    </>
  );
}
  
export default UserPage;