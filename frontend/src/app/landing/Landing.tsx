import React, { useEffect, useState } from 'react';
import jwtDecode from 'jwt-decode';
import { fetchToken } from '../authentication/JwtUtils';

interface UserInfo {
	userid: string;
	username: string;
	name: string;
	email: string;
	phonenumber: string;
}

const Landing: React.FC = () => {
	const [userInfo, setUserInfo] = useState<UserInfo | null>(null);

	useEffect(() => {
		const user = fetchToken();
		if (user) {
			setUserInfo(user);
			console.log('Stored user info: ');
			console.log(userInfo);
		}
	}, []);

	return (
		<div>
			{userInfo ? (
				<div>
					<p>User ID: {userInfo.userid}</p>
					<p>Username: {userInfo.username}</p>
					<p>Name: {userInfo.name}</p>
					<p>Email: {userInfo.email}</p>
					<p>Phone Number: {userInfo.phonenumber}</p>
				</div>
			) : (
				<p>No user info available</p>
			)}
		</div>
	);
};

export default Landing;
