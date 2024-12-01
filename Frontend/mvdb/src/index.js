import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter, Route, Routes } from 'react-router';
import Frontpage from './Components/Frontpage';
import Series from './Components/Series';
import Title from './Components/Title';
import SearchResult from './Components/SearchResults';
import CreateReview from './Components/CreateReview';
import Person from './Components/Person';
import SignUp from './Components/Signup';
import SignIn from './Components/SignIn';
import Header from './Components/Header';
import Settings from './Components/Settings';
import UserPage from './Components/User';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <div className='all'>
    <BrowserRouter> {/* from react package */}
    <Header/>    

      <Routes> {/* we must include our routes here somehow*/}
        <Route path="/" element={<Frontpage />}/>


        <Route path="/search" element={<SearchResult />}/>
        <Route path="/series/:id" element={<Series />}/>

        <Route path="/title/:id" element={<Title />} />
        <Route path="newReview" element={<CreateReview />}/>

        {/* <Route path="/person/:t_id" element={<Person />}/> */}
        <Route path="/person/:p_id" element={<Person />}/>


        <Route path="/user/:id" element={<UserPage />} /> 
        <Route path="settings" element={<Settings />}/>
           
          
        <Route path="/signIn" element={<SignIn />}/>
        <Route path="/signUp" element={<SignUp />}/>
      

      </Routes>
    </BrowserRouter>
    </div>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
