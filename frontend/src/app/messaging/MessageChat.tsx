import React, { useState, useEffect } from 'react';
import { UserInfo, fetchToken } from '../authentication/JwtUtils';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

interface Conversation {
	id: number;
	name: string;
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
	const navigate = useNavigate();

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

	const fetchConversations = async (userInfo: UserInfo) => {
		try {
			const response = await fetch(
				`http://localhost:5169/queryconversation?userid=${
					userInfo?.userid
				}&lastmessage=${new Date().toISOString()}`,
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

	const handleConversationClick = async (conversation: Conversation) => {
		setSelectedConversation(conversation);

		try {
			const response = await fetch(
				`http://localhost:5169/querymessage?convid=${
					conversation.id
				}&lastmessagetime=${new Date().toISOString()}`,
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

	return (
		<div className='message-chat-container'>
			<div className='conversations'>
				{conversations.map((conversation) => (
					<div
						key={conversation.id}
						className={`conversation ${
							selectedConversation === conversation ? 'active' : ''
						}`}
						onClick={() => handleConversationClick(conversation)}
					>
						{conversation.name}
					</div>
				))}
			</div>
			<div className='messages'>
				{selectedConversation ? (
					<div className='conversation-messages'>
						{messages.map((message) => (
							<div key={message.id} className='message'>
								<div className='message-sender'>{message.sender}</div>
								<div className='message-content'>{message.content}</div>
								<div className='message-timestamp'>{message.timestamp}</div>
							</div>
						))}
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
