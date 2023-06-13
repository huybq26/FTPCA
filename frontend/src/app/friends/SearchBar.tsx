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
	relationshipStatus: string;
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
				const parsedResults: SearchResult[] = [];

				for (const item of data) {
					const [userid, username, phoneNumber, name, email] = item.split(',');
					const relationshipStatus = await fetchRelationshipStatus(
						userInfo?.userid || '',
						Number(userid)
					);

					if (Number(userid) !== Number(userInfo?.userid)) {
						parsedResults.push({
							userid: Number(userid),
							username,
							phoneNumber,
							name,
							email,
							isFriendRequestSent: false,
							relationshipStatus,
						});
					}
				}

				setResults(parsedResults);
			} else {
				console.log('Error:', response.status);
			}
		} catch (error) {
			console.log('An error occurred:', error);
		}
	};

	const fetchRelationshipStatus = async (userid1: string, userid2: number) => {
		try {
			const response = await fetch(
				`http://localhost:5169/checkrelationshipstatus?userid1=${userid1}&userid2=${userid2}`,
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
				return data.data;
			} else {
				console.log('Error:', response.status);
				return '';
			}
		} catch (error) {
			console.log('An error occurred:', error);
			return '';
		}
	};

	const handleSendFriendRequest = async (
		senderId: string,
		receiverId: string,
		index: number
	) => {
		console.log('Sending friend request to:', receiverId);

		try {
			const response = await fetch(
				`http://localhost:5169/addfriendrequest?senderid=${senderId}&receiverid=${receiverId}`,
				{
					method: 'POST',
					headers: {
						Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
					},
				}
			);

			if (response.ok) {
				console.log('Friend request sent successfully');
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

	const handleAcceptFriendRequest = async (index: number) => {
		const friendRequest = results[index];
		console.log('Accepting friend request from:', friendRequest.username);

		try {
			const response = await fetch(
				`http://localhost:5169/acceptfriendrequest?senderid=${
					friendRequest.userid
				}&receiverid=${Number(userInfo?.userid)}`,
				{
					method: 'POST',
					headers: {
						'Content-Type': 'application/json',
						Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
					},
					// body: JSON.stringify({
					// 	senderId: friendRequest.userid,
					// 	receiverId: Number(userInfo?.userid),
					// }),
				}
			);

			if (response.ok) {
				console.log('Friend request accepted successfully');
				const updatedResults = [...results];
				updatedResults[index].relationshipStatus = 'Friend';
				setResults(updatedResults);
			} else {
				console.log('Error accepting friend request:', response.status);
			}
		} catch (error) {
			console.log('An error occurred:', error);
		}
	};

	const handleCancelFriendRequest = async (index: number) => {
		const friendRequest = results[index];
		console.log('Canceling friend request to:', friendRequest.username);

		try {
			const response = await fetch(
				`http://localhost:5169/declinefriendrequest?senderid=${Number(
					userInfo?.userid
				)}&receiverid=${friendRequest.userid}`,
				{
					method: 'POST',
					headers: {
						'Content-Type': 'application/json',
						Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
					},
					// body: JSON.stringify({
					// 	senderId: Number(userInfo?.userid),
					// 	receiverId: friendRequest.userid,
					// }),
				}
			);

			if (response.ok) {
				console.log('Friend request canceled successfully');
				const updatedResults = [...results];
				updatedResults[index].relationshipStatus = 'Add Friend';
				setResults(updatedResults);
			} else {
				console.log('Error canceling friend request:', response.status);
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
						{result.relationshipStatus === 'Friend' && <p>Friend</p>}
						{result.relationshipStatus === 'Cancel Friend Request' && (
							<button onClick={() => handleCancelFriendRequest(index)}>
								Cancel Friend Request
							</button>
						)}
						{result.relationshipStatus === 'Accept Friend Request' && (
							<button onClick={() => handleAcceptFriendRequest(index)}>
								Accept Friend Request
							</button>
						)}
						{result.relationshipStatus === 'Add Friend' && (
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
						)}
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
