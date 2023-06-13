import React, { useState, useEffect } from 'react';
import { UserInfo, fetchToken } from '../authentication/JwtUtils';
import { useNavigate } from 'react-router-dom';

interface Friend {
	userid: number;
	username: string;
	name: string;
	phoneNumber: string;
	email: string;
}

const FriendsListing: React.FC = () => {
	const [userInfo, setUserInfo] = useState<UserInfo | null>(null);
	const [friends, setFriends] = useState<Friend[]>([]);
	const [selectedFriend, setSelectedFriend] = useState<Friend | null>(null);
	const navigate = useNavigate();

	useEffect(() => {
		const user = fetchToken();
		console.log(user);
		if (user) {
			setUserInfo(user);
			fetchFriends(user);
		} else {
			navigate('/login');
		}
	}, []);

	const fetchFriends = async (userInfo: UserInfo) => {
		try {
			const response = await fetch(
				`http://localhost:5169/friendlisting?userid=${userInfo.userid}`,
				{
					method: 'GET',
					headers: {
						Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
					},
				}
			);

			if (response.ok) {
				const data: any = await response.json();
				console.log('Get data: ', data.data);
				setFriends(data.data);
			} else {
				console.log('Error:', response.status);
			}
		} catch (error) {
			console.log('An error occurred:', error);
		}
	};

	const handleInfoClick = (friend: Friend) => {
		setSelectedFriend(friend);
	};

	const handleDeleteFriend = async (friend: Friend) => {
		try {
			const response = await fetch(
				`http://localhost:5169/deletefriend?userid1=${userInfo?.userid}&userid2=${friend.userid}`,
				{
					method: 'POST',
					headers: {
						'Content-Type': 'application/json',
						Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
					},
				}
			);

			if (response.ok) {
				console.log('Friend deleted successfully');
				fetchFriends(userInfo!); // Fetch friends again to update the list
			} else {
				console.log('Error deleting friend:', response.status);
			}
		} catch (error) {
			console.log('An error occurred:', error);
		}
	};

	const closeInfoCard = () => {
		setSelectedFriend(null);
	};

	return (
		<div>
			<h1>Friends List</h1>
			{friends.length > 0 ? (
				<ul>
					{friends.map((friend) => (
						<li key={friend.userid}>
							<p>Username: {friend.username}</p>
							<p>Name: {friend.name}</p>
							<button onClick={() => handleInfoClick(friend)}>Info</button>
							<button onClick={() => handleDeleteFriend(friend)}>Delete</button>
						</li>
					))}
				</ul>
			) : (
				<p>No friends found.</p>
			)}

			{selectedFriend && (
				<div className='info-card'>
					<p>UserID: {selectedFriend.userid}</p>
					<p>Username: {selectedFriend.username}</p>
					<p>Name: {selectedFriend.name}</p>
					<p>Phone Number: {selectedFriend.phoneNumber}</p>
					<p>Email: {selectedFriend.email}</p>
					<button onClick={closeInfoCard}>Close</button>
				</div>
			)}
		</div>
	);
};

export default FriendsListing;
