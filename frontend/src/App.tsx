import React from 'react';
import logo from './logo.svg';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Auth from './app/authentication/Auth';
import './App.css';
import LandingPage from './app/landing/Landing';
import User from './user/user';
import AuthService from './app/authentication/Auth.service';

function App() {
	return (
		<Router>
			<div>
				<Routes>
					<Route path='/signin' element={<Auth />} />
					<Route path='/landing' element={<LandingPage />} />
				</Routes>
			</div>
		</Router>
	);
}

export default App;
