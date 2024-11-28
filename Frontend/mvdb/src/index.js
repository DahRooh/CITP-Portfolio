import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter, Route, Routes, useParams } from 'react-router';
import Frontpage from './Components/Frontpage';
import Series from './Components/Series';
import Title from './Components/Title';
import SearchResult from './Components/SearchResults';
import CreateReview from './Components/CreateReview';
import SignUp from './Components/Signup';
import SignIn from './Components/SignIn';
import Header from './Components/Header';
import Settings from './Components/Settings';
import UserPage from './Components/User';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <Header/>
    <BrowserRouter> {/* from react package */}
      <Routes> {/* we must include our routes here somehow*/}
        <Route path="/test" element={<App />}/>
        <Route path="/" element={<Frontpage />}/>
        <Route path="/search" element={<SearchResult />}/>
        <Route path="/series" element={<Series />}/>
        <Route path="/title" element={<Title />}>
          <Route path="createReview" element={<CreateReview />}/>
        </Route>

        <Route path="/user/:id" element={<UserPage />}> 
           <Route path="settings" element={<Settings />}/>
        </Route>
           
          
        <Route path="/signIn" element={<SignIn />}/>
        <Route path="/signUp" element={<SignUp />}/>
        
        


      </Routes>
    </BrowserRouter>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
