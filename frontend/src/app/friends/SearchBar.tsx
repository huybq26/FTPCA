import React, { useState, useEffect } from 'react';
import './SearchBar.styles.css';
import { UserInfo } from '../authentication/JwtUtils';
import { fetchToken } from '../authentication/JwtUtils';
import { useNavigate } from 'react-router-dom';

interface SearchResult {
	userid: number;
	username: string;
	name: string;
	email: string;
	phoneNumber: string;
	isFriendRequestSent: boolean;
}

const SearchBar: React.FC = () => {
	const [keyword, setKeyword] = useState('');
	const [results, setResults] = useState<SearchResult[]>([]);
	const [selectedResult, setSelectedResult] = useState<SearchResult | null>(
		null
	);
	const [userInfo, setUserInfo] = useState<UserInfo | null>(null);
	const navigate = useNavigate();
	useEffect(() => {
		const user = fetchToken();
		if (user) {
			setUserInfo(user);
			console.log('Stored user info: ');
			console.log(userInfo);
		} else {
			navigate('/login');
		}
	}, []);

	const handleSearch = async () => {
		console.log('Performing search with keyword:', keyword);

		try {
			const response = await fetch(
				`http://localhost:5169/searchfriend?term=${keyword}`,
				{
					method: 'GET',
					headers: {
						Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
					},
				}
			);

			if (response.ok) {
				const data: string[] = await response.json();
				console.log('Get successful:', data);
				const parsedResults: SearchResult[] = data
					.map((item) => {
						const [userid, username, phoneNumber, name, email] =
							item.split(',');
						return {
							userid: Number(userid),
							username,
							phoneNumber,
							name,
							email,
							isFriendRequestSent: false,
						};
					})
					.filter((result) => result.userid !== Number(userInfo?.userid));

				setResults(parsedResults);
			} else {
				console.log('Error:', response.status);
			}
		} catch (error) {
			console.log('An error occurred:', error);
		}
	};

	const handleSendFriendRequest = async (
		senderId: string,
		receiverId: string,
		index: number
	) => {
		// Logic to send friend request from the senderId to the receiverId
		console.log('Sending friend request ' + 'to:', receiverId);

		try {
			// Make the POST request to send the friend request
			const response = await fetch(
				'http://localhost:5169/addfriendrequest?senderid=' +
					senderId +
					'&receiverid=' +
					receiverId,
				{
					method: 'POST',
					headers: {
						// 'Content-Type': 'application/json',
						Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
					},
				}
			);

			if (response.ok) {
				console.log('Friend request sent successfully');
				// Update the state to mark the friend request as sent
				const updatedResults = [...results];
				updatedResults[index].isFriendRequestSent = true;
				setResults(updatedResults);
			} else {
				console.log('Error sending friend request:', response.status);
			}
		} catch (error) {
			console.log('An error occurred:', error);
		}
	};

	const handleInfoClick = (result: SearchResult) => {
		setSelectedResult(result);
	};

	const closeInfoCard = () => {
		setSelectedResult(null);
	};

	return (
		<div>
			<div>
				<input
					type='text'
					value={keyword}
					onChange={(e) => setKeyword(e.target.value)}
				/>
				<button onClick={handleSearch}>Search</button>
			</div>
			<div>
				{results.map((result, index) => (
					<div key={index}>
						<p>Username: {result.username}</p>
						<p>Name: {result.name}</p>
						<button
							onClick={() =>
								handleSendFriendRequest(
									userInfo?.userid || '',
									result.userid.toString(),
									index
								)
							}
							disabled={result.isFriendRequestSent}
						>
							{result.isFriendRequestSent ? 'Sent!' : 'Send Friend Request'}
						</button>
						<button onClick={() => handleInfoClick(result)}>Info</button>
					</div>
				))}
			</div>
			{selectedResult && (
				<div className='info-card'>
					<p>Username: {selectedResult.username}</p>
					<p>Name: {selectedResult.name}</p>
					<p>Email: {selectedResult.email}</p>
					<p>Phone Number: {selectedResult.phoneNumber}</p>
					<button onClick={closeInfoCard}>Close</button>
				</div>
			)}
		</div>
	);
};

export default SearchBar;
