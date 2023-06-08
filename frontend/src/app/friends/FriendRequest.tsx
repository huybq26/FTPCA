import React, { useState, useEffect } from 'react';
import { UserInfo, fetchToken } from '../authentication/JwtUtils';
import './FriendRequest.css';

interface FriendRequest {
  senderid: number;
  username: string;
  phoneNumber: string;
  name: string;
  email: string;
}

const FriendRequest: React.FC = () => {
  const [userInfo, setUserInfo] = useState<UserInfo | null>(null);
  const [friendRequestList, setFriendRequestList] = useState<FriendRequest[]>(
    []
  );
  const [selectedRequest, setSelectedRequest] = useState<FriendRequest | null>(
    null
  );

  useEffect(() => {
    const getUserInfo = async () => {
      const user = fetchToken();
      if (user) {
        setUserInfo(user);
        console.log('Stored user info:');
        console.log(userInfo);
      }
    };

    const fetchFriendRequests = async () => {
      try {
        const response = await fetch(
          `http://localhost:5169/queryfriendrequest?userid=${userInfo?.userid}`,
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
          const parsedResults: FriendRequest[] = data.map((item) => {
            const [userid, username, phoneNumber, name, email] =
              item.split(',');
            return {
              senderid: Number(userid),
              username,
              phoneNumber,
              name,
              email,
            };
          });
          setFriendRequestList(parsedResults);
        } else {
          console.log('Error:', response.status);
        }
      } catch (error) {
        console.log('An error occurred:', error);
      }
    };

    getUserInfo();
    fetchFriendRequests();
  }, []);

  const handleAcceptRequest = async (senderId: number) => {
    try {
      const response = await fetch(
        'http://localhost:5169/acceptfriendrequest',
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
          },
          body: JSON.stringify({
            senderId,
            receiverId: Number(userInfo?.userid),
          }),
        }
      );

      if (response.ok) {
        console.log('Friend request accepted successfully');
        // Perform any necessary actions upon successful acceptance
      } else {
        console.log('Error accepting friend request:', response.status);
      }
    } catch (error) {
      console.log('An error occurred:', error);
    }
  };

  const handleRejectRequest = async (senderId: number) => {
    try {
      const response = await fetch(
        'http://localhost:5169/rejectfriendrequest',
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
          },
          body: JSON.stringify({
            senderId,
            receiverId: Number(userInfo?.userid),
          }),
        }
      );

      if (response.ok) {
        console.log('Friend request accepted successfully');
        // Perform any necessary actions upon successful acceptance
      } else {
        console.log('Error rejecting friend request:', response.status);
      }
    } catch (error) {
      console.log('An error occurred:', error);
    }
  };

  const handleInfoClick = (request: FriendRequest) => {
    setSelectedRequest(request);
  };

  const closeInfoCard = () => {
    setSelectedRequest(null);
  };

  return (
    <div className='friend-request-container'>
      {friendRequestList.map((friendRequest) => (
        <div className='friend-request-item' key={friendRequest.senderid}>
          <p>Username: {friendRequest.username}</p>
          <p>Name: {friendRequest.name}</p>
          <button onClick={() => handleRejectRequest(friendRequest.senderid)}>
            Decline
          </button>
          <button onClick={() => handleAcceptRequest(friendRequest.senderid)}>
            Accept
          </button>
          <button onClick={() => handleInfoClick(friendRequest)}>Info</button>
        </div>
      ))}
      {selectedRequest && (
        <div className='info-card'>
          <h3>Friend Request Info</h3>
          <p>
            <strong>UserID:</strong> {selectedRequest.senderid}
          </p>
          <p>
            <strong>Username:</strong> {selectedRequest.username}
          </p>
          <p>
            <strong>Name:</strong> {selectedRequest.name}
          </p>
          <p>
            <strong>Phone Number:</strong> {selectedRequest.phoneNumber}
          </p>
          <p>
            <strong>Email:</strong> {selectedRequest.email}
          </p>
          <button onClick={closeInfoCard}>Close</button>
        </div>
      )}
    </div>
  );
};

export default FriendRequest;
