import React, { useState, useEffect, useRef } from 'react';
import { UserInfo, fetchToken } from '../authentication/JwtUtils';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

interface Conversation {
	id: number;
	name: string;
	lastMessage: string;
	groupImage: string;
}

interface Message {
	id: number;
	sender: string;
	content: string;
	timestamp: string;
}

const MessageChat: React.FC = () => {
	const [userInfo, setUserInfo] = useState<UserInfo | null>(null);
	const [conversations, setConversations] = useState<Conversation[]>([]);
	const [selectedConversation, setSelectedConversation] =
		useState<Conversation | null>(null);
	const [messages, setMessages] = useState<Message[]>([]);
	const [newMessage, setNewMessage] = useState<string>('');
	const navigate = useNavigate();
	const messagesContainerRef = useRef<HTMLDivElement>(null);

	useEffect(() => {
		const user = fetchToken();
		console.log(user);
		if (user) {
			setUserInfo(user);
			fetchConversations(user);
		} else {
			navigate('/login');
		}
	}, []);

	useEffect(() => {
		if (messagesContainerRef.current) {
			messagesContainerRef.current.scrollTop =
				messagesContainerRef.current.scrollHeight;
		}
	}, [messages]);

	const fetchConversations = async (userInfo: UserInfo) => {
		try {
			const response = await fetch(
				`http://localhost:5169/queryconversation?userid=${
					userInfo?.userid
				}&lastmessage=${getCurrentDateTime()}`,
				{
					method: 'GET',
					headers: {
						Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
					},
				}
			);

			if (response.ok) {
				const data: any = await response.json();
				console.log('Get successful:', data);
				const parsedResults: Conversation[] = data.data.map((item: any) => {
					return {
						id: item[0],
						name: item[1],
						lastMessage: item[2],
						groupImage: item[3],
					};
				});
				console.log(parsedResults);
				setConversations(parsedResults);
			} else {
				console.log('Error:', response.status);
			}
		} catch (error) {
			console.log('An error occurred:', error);
		}
	};

	const getCurrentDateTime = (): string => {
		const currentDate = new Date();
		const gmtPlus8Offset = 8 * 60; // GMT+8 offset in minutes
		const gmtPlus8DateTime = new Date(
			currentDate.getTime() + gmtPlus8Offset * 60000
		); // Add offset in milliseconds
		const formattedDateTime = gmtPlus8DateTime.toISOString().slice(0, 19);

		return formattedDateTime;
	};

	const handleConversationClick = async (conversation: Conversation) => {
		setSelectedConversation(conversation);

		try {
			const response = await fetch(
				`http://localhost:5169/querymessage?convid=${
					conversation.id
				}&lastmessagetime=${getCurrentDateTime()}`,
				{
					method: 'GET',
					headers: {
						Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
					},
				}
			);

			if (response.ok) {
				const data: any = await response.json();
				console.log('Get successful:', data);
				const parsedResults: Message[] = data.data.map((item: any) => {
					return {
						id: item[0],
						sender: item[1],
						content: item[2],
						timestamp: item[3],
					};
				});
				console.log(parsedResults);
				setMessages(parsedResults);
			} else {
				console.log('Error:', response.status);
			}
		} catch (error) {
			console.log(
				`Error fetching messages for conversation ${conversation.id}:`,
				error
			);
		}
	};

	const handleSendMessage = async () => {
		// Add your logic to send the newMessage value
		// and update the messages state accordingly
		try {
			const response = await fetch(
				`http://localhost:5169/sendmessage?senderid=${userInfo?.userid}&convid=${selectedConversation?.id}&content=${newMessage}`,
				{
					method: 'POST',
					headers: {
						Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
					},
				}
			);

			if (response.ok) {
				console.log('Sent message successfully!');
			} else {
				console.log('Error:', response.status);
			}
		} catch (error) {
			console.log(`Error sending message:`, error);
		}
	};

	return (
		<div
			className='message-chat-container'
			style={{
				display: 'flex',
				flexDirection: 'row',
			}}
		>
			<div className='conversations'>
				{conversations.map((conversation) => (
					<div
						key={conversation.id}
						className={`conversation ${
							selectedConversation === conversation ? 'active' : ''
						}`}
						onClick={() => handleConversationClick(conversation)}
						style={{
							display: 'flex',
							flexDirection: 'row',
							alignItems: 'center',
						}}
					>
						<div className='group-image'>
							<img src='chat.jpg' height='30px' alt='Group Image' />
						</div>
						<div className='conversation-details'>
							<div className='conversation-name'>
								<b>{conversation.name}</b>
							</div>
							<div className='last-message'>Contributor1: Last Message</div>
						</div>
					</div>
				))}
			</div>
			<div
				className='messages'
				style={{
					marginLeft: '20px',
					maxWidth: '100%',
					flex: 1,
					overflowY: 'auto',
					maxHeight: 'calc(100vh - 20px)',
				}}
				ref={messagesContainerRef}
			>
				{selectedConversation ? (
					<div className='conversation-messages'>
						{messages.map((message) => (
							<div
								key={message.id}
								className='message'
								style={{
									backgroundColor: '#ECE5DD',
									marginBottom: '10px',
									padding: '10px',
									borderRadius: '5px',
								}}
							>
								<div className='message-sender' style={{ fontWeight: 'bold' }}>
									{message.sender}
								</div>
								<div className='message-content'>{message.content}</div>
								<div
									className='message-timestamp'
									style={{ fontSize: '0.8rem', color: 'gray' }}
								>
									{message.timestamp}
								</div>
							</div>
						))}
						<div
							className='message-input-container'
							style={{
								marginTop: '20px',
								position: 'fixed',
								// left: 0,
								bottom: 0,
								width: '100%',
								backgroundColor: '#ffffff',
								padding: '10px',
							}}
						>
							<input
								type='text'
								className='message-input'
								placeholder='Type your message...'
								value={newMessage}
								onChange={(e) => setNewMessage(e.target.value)}
							/>
							<button className='send-button' onClick={handleSendMessage}>
								<i className='fa fa-paper-plane'></i>
							</button>
						</div>
					</div>
				) : (
					<div className='no-conversation-selected'>
						Select a conversation to view messages.
					</div>
				)}
			</div>
		</div>
	);
};

export default MessageChat;
