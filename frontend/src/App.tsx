import React, { useEffect, useState } from 'react';
import {
	BrowserRouter as Router,
	Route,
	Routes,
	useLocation,
	Navigate,
} from 'react-router-dom';
import Auth from './app/authentication/Auth';
import LandingPage from './app/landing/Landing';
import SearchBar from './app/friends/SearchBar';
import FriendsListing from './app/friends/FriendListing';
import FriendRequest from './app/friends/FriendRequest';
import VerticalNav from './app/navigation/VerticalNav';
import { fetchToken } from './app/authentication/JwtUtils';
import MessageChat from './app/messaging/MessageChat';

function App() {
	const [loggedIn, setLoggedIn] = useState(false);

	useEffect(() => {
		const checkLoggedIn = () => {
			const token = sessionStorage.getItem('jwtToken');
			if (token) {
				setLoggedIn(true);
			} else {
				setLoggedIn(false);
			}
		};

		checkLoggedIn();
	}, []);

	const handleLogout = () => {
		sessionStorage.removeItem('jwtToken');
		setLoggedIn(false);
		// window.location.href = '/login';
	};

	const handleLogin = () => {
		setLoggedIn(true);
	};
	//  need to pass logged in state from the login
	return (
		<Router>
			<div style={{ display: 'flex' }}>
				{loggedIn && <VerticalNav onLogout={handleLogout} />}
				<div style={{ flex: 1 }}>
					<Routes>
						<Route path='/' element={<Navigate to='/message' />} />
						<Route path='/login' element={<Auth onLogin={handleLogin} />} />
						<Route path='/landing' element={<LandingPage />} />
						<Route path='/friendlist' element={<FriendsListing />} />
						<Route path='/friendrequest' element={<FriendRequest />} />
						<Route path='/searchfriend' element={<SearchBar />} />
						<Route path='/message' element={<MessageChat />} />
					</Routes>
				</div>
			</div>
		</Router>
	);
}

export default App;
