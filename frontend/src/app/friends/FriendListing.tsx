import React, { useState, useEffect } from 'react';
import { UserInfo, fetchToken } from '../authentication/JwtUtils';

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

  useEffect(() => {
    getUserInfo();
    fetchFriends();
  }, []);

  const getUserInfo = async () => {
    const user = fetchToken();
    if (user) {
      setUserInfo(user);
      console.log('Stored user info:');
      console.log(userInfo);
    }
  };
  const fetchFriends = async () => {
    try {
      const response = await fetch(
        `http://localhost:5169/friendlisting?userid=${userInfo?.userid}`,
        {
          method: 'GET',
          headers: {
            Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
          },
        }
      );

      if (response.ok) {
        const data: Friend[] = await response.json();
        setFriends(data);
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
