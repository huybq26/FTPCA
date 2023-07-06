import React, { useState, useEffect, useRef } from 'react';
import { UserInfo, fetchToken } from '../authentication/JwtUtils';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import {
	AppBar,
	Box,
	IconButton,
	InputBase,
	Modal,
	Paper,
	Toolbar,
	Typography,
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import CheckIcon from '@mui/icons-material/Check';
import ClearIcon from '@mui/icons-material/Clear';
import './style.css';
import * as signalR from '@microsoft/signalr';

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
	const [newChatName, setNewChatName] = useState<string>('');
	const [showNewChatModal, setShowNewChatModal] = useState<boolean>(false);
	const [searchedUsers, setSearchedUsers] = useState<UserInfo[]>([]);
	const [selectedUsers, setSelectedUsers] = useState<UserInfo[]>([]);
	const [searchQuery, setSearchQuery] = useState<string>('');

	const navigate = useNavigate();
	const messagesContainerRef = useRef<HTMLDivElement>(null);

	const [connection, setConnection] = useState<signalR.HubConnection | null>(
		null
	);

	useEffect(() => {
		const newConnection = new signalR.HubConnectionBuilder()
			.withUrl('http://localhost:5169/chatHub') // Specify the hub URL
			.build();

		setConnection(newConnection);
	}, []);

	useEffect(() => {
		if (connection) {
			connection
				.start()
				.then(() => {
					console.log('Connected to SignalR hub');

					// Subscribe to the 'ReceiveMessage' event from the hub
					connection.on('ReceiveMessage', (sender: string, message: string) => {
						setMessages((prevMessages) => [
							...prevMessages,
							{
								id: prevMessages.length + 1,
								sender,
								content: message,
								timestamp: getCurrentDateTime(),
							},
						]);
					});
				})
				.catch((error) => console.error('SignalR connection error:', error));
		}
	}, [connection]);

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

	const modalStyle = {
		position: 'absolute' as 'absolute',
		top: '50%',
		left: '50%',
		transform: 'translate(-50%, -50%)',
		width: 400,
		bgcolor: 'background.paper',
		// border: '2px solid #000',
		boxShadow: 24,
		p: 4,
	};

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
			console.log('An error occurred:', error);
		}
	};

	const handleNewMessageChange = (
		event: React.ChangeEvent<HTMLInputElement>
	) => {
		setNewMessage(event.target.value);
	};

	const handleSendMessage = async () => {
		if (selectedConversation && newMessage.trim() !== '') {
			try {
				const response = await axios.post(
					`http://localhost:5169/sendmessage?senderid=${userInfo?.userid}&convid=${selectedConversation.id}&content=${newMessage}`,
					{},
					{
						headers: {
							Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
						},
					}
				);

				if (response.status === 200) {
					const newMessageObject: Message = {
						id: response.data.messageid,
						sender: userInfo?.username || '',
						content: newMessage,
						timestamp: getCurrentDateTime(),
					};
					if (connection) {
						connection
							.invoke('SendMessage', userInfo?.username, newMessage)
							.catch((error) =>
								console.error('SignalR sendMessage error:', error)
							);
					}

					setMessages([...messages, newMessageObject]);
					setNewMessage('');
				} else {
					console.log('Error:', response.status);
				}
			} catch (error) {
				console.log('An error occurred:', error);
			}
		}
	};

	const handleNewChatClick = () => {
		setShowNewChatModal(true);
	};

	const handleNewChatNameChange = (
		event: React.ChangeEvent<HTMLInputElement>
	) => {
		setNewChatName(event.target.value);
	};

	const handleSearchQueryChange = (
		event: React.ChangeEvent<HTMLInputElement>
	) => {
		setSearchQuery(event.target.value);
	};

	const handleSearchUsers = async () => {
		if (searchQuery.trim() !== '') {
			try {
				const response = await fetch(
					`http://localhost:5169/searchfriend?term=${searchQuery}`,
					{
						method: 'GET',
						headers: {
							Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
							'Access-Control-Allow-Origin': '*',
						},
					}
				);

				if (response.ok) {
					const data: any = await response.json();
					console.log('Get successful:', data);
					const parsedResults: UserInfo[] = data.map((item: string) => {
						const [userid, username, phoneNumber, name, email] =
							item.split(',');
						return {
							userid: Number(userid),
							username,
							phoneNumber,
							name,
							email,
						};
					});
					console.log(parsedResults);
					const filteredUsers = parsedResults.filter(
						(user) =>
							!selectedUsers.some(
								(selectedUser) => selectedUser.userid === user.userid
							)
					);
					setSearchedUsers(filteredUsers);
				} else {
					console.log('Error:', response.status);
				}
			} catch (error) {
				console.log('An error occurred:', error);
			}
		}
	};

	const handleUserClick = (user: UserInfo) => {
		setSelectedUsers((prevSelectedUsers) => [...prevSelectedUsers, user]);
		setSearchedUsers([]);
		setSearchQuery('');
	};

	const handleRemoveUser = (user: UserInfo) => {
		setSelectedUsers((prevSelectedUsers) =>
			prevSelectedUsers.filter((u) => u.userid !== user.userid)
		);
	};

	const handleCreateChat = async () => {
		if (newChatName.trim() !== '' && selectedUsers.length > 0) {
			try {
				const response = await fetch(
					`http://localhost:5169/createconv?convname=${newChatName}&participantList=${selectedUsers
						.map((user) => user.userid)
						.toString()}`,
					{
						method: 'POST',
						headers: {
							Authorization: `Bearer ${sessionStorage.getItem('jwtToken')}`,
						},
					}
				);

				if (response.status === 200) {
					const data: any = await response.json();
					const newConversation: Conversation = {
						id: data.convid,
						name: newChatName,
						lastMessage: '',
						groupImage: '',
					};

					setConversations([...conversations, newConversation]);
					setShowNewChatModal(false);
					setNewChatName('');
					setSelectedUsers([]);
				} else {
					console.log('Error:', response.status);
				}
			} catch (error) {
				console.log('An error occurred:', error);
			}
		}
	};

	return (
		<div>
			<div
				className='message-chat-container'
				style={{ display: 'flex', flexDirection: 'row' }}
			>
				<div
					className='conversations'
					style={{
						width: '25%',
					}}
				>
					<div
						style={{
							display: 'flex',
							flexDirection: 'row',
							justifyContent: 'space-between',
							marginTop: '10px',
							marginLeft: '10px',
							alignItems: 'center',
						}}
					>
						<div>
							<Typography
								variant='h5'
								component='h3'
								style={{
									fontWeight: 'bold',
								}}
							>
								Chats
							</Typography>
						</div>
						<div>
							<IconButton
								type='button'
								aria-label='create-chat'
								onClick={() => setShowNewChatModal(true)}
							>
								<AddCircleOutlineIcon />
							</IconButton>
						</div>
					</div>

					<div
						style={{ display: 'flex', justifyContent: 'center', width: '100%' }}
					>
						<Paper
							component='form'
							sx={{
								// p: '2px 4px',
								display: 'flex',
								alignItems: 'center',
								width: '95%',
								backgroundColor: '#f5f3f2',
								marginBottom: '15px',
								marginTop: '10px',
								borderRadius: '10px',
								justifyContent: 'center',
							}}
						>
							<IconButton type='button' sx={{ p: '10px' }} aria-label='search'>
								<SearchIcon />
							</IconButton>
							<InputBase
								sx={{
									ml: 1,
									flex: 1,
									'&::placeholder': {
										fontSize: '14px', // Adjust the placeholder text size
									},
									'& input': {
										fontSize: '14px', // Adjust the input text size
									},
								}}
								placeholder='Search Conversations'
								inputProps={{ 'aria-label': 'Search Conversations' }}
							/>
						</Paper>
					</div>

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
								marginLeft: '5px',
								cursor: 'pointer',
								borderRadius: '5px',
								height: '55px',
								// backgroundColor:
								// 	selectedConversation === conversation ? '#f0f0f0' : 'initial',
							}}
						>
							<div className='group-image' style={{ marginLeft: '5px' }}>
								<img
									// src={conversation.groupImage}
									src='./chat.jpg'
									height='30px'
									alt='Group Image'
								/>
							</div>
							<div
								className='conversation-details'
								style={{ marginLeft: '3px' }}
							>
								<div
									className='conversation-name'
									style={{ fontSize: 18, fontWeight: 500 }}
								>
									{conversation.name}
								</div>
								{/* <div className='last-message'>{conversation.lastMessage}</div> */}
								<div
									className='last-message'
									style={{ fontWeight: 300, fontSize: 14, color: 'gray' }}
								>
									Lorem ipsum: asdfa sdafsdf a
								</div>
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
							<AppBar position='sticky'>
								<Toolbar>
									{selectedConversation && (
										<>
											<div className='group-image'>
												<img
													// src={selectedConversation.groupImage}
													src='./chat.jpg'
													height='30px'
													alt='Group Image'
												/>
											</div>
											<Typography
												variant='h6'
												style={{ flexGrow: 1, marginLeft: '10px' }}
											>
												{selectedConversation.name}
											</Typography>
										</>
									)}
									<IconButton color='inherit'>
										<SearchIcon />
									</IconButton>
									<IconButton color='inherit'>
										<MoreVertIcon />
									</IconButton>
								</Toolbar>
							</AppBar>
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
									<div
										className='message-sender'
										style={{ fontWeight: 'bold' }}
									>
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
								<button onClick={handleSendMessage}>Send</button>
							</div>
						</div>
					) : (
						<h2>Select a Conversation</h2>
					)}
				</div>
			</div>
			<div>
				{showNewChatModal && (
					<Modal
						open={showNewChatModal}
						onClose={() => setShowNewChatModal(false)}
						aria-labelledby='modal-modal-title'
						aria-describedby='modal-modal-description'
					>
						<Box sx={modalStyle}>
							<h2>Create New Chat</h2>
							<input
								type='text'
								placeholder='Chat Name'
								value={newChatName}
								onChange={handleNewChatNameChange}
								style={{
									borderRadius: '5px',
									height: '25px',
									marginBottom: '10px',
								}}
							/>
							<div className='search-users'>
								<input
									type='text'
									placeholder='Search Users'
									value={searchQuery}
									onChange={handleSearchQueryChange}
									style={{
										borderRadius: '5px',
										height: '25px',
										marginBottom: '10px',
										marginRight: '5px',
									}}
								/>
								<button onClick={handleSearchUsers}>Search</button>
								{searchedUsers.length > 0 && (
									<ul>
										{searchedUsers.map((user) => (
											<div
												style={{
													display: 'flex',
													flexDirection: 'row',
													alignItems: 'center',
													gap: '5px',
												}}
											>
												<li
													className='li-create'
													key={user.userid}
													onClick={() => handleUserClick(user)}
													style={{ cursor: 'pointer' }}
												>
													{user.username}
												</li>

												<IconButton
													color='inherit'
													onClick={() => handleUserClick(user)}
													style={{}}
												>
													<CheckIcon />
												</IconButton>
											</div>
										))}
									</ul>
								)}
							</div>

							<div className='selected-users'>
								<h3>Selected Users:</h3>
								<ul>
									{selectedUsers.map((user) => (
										<li key={user.userid} style={{ marginBottom: '5px' }}>
											<div
												style={{
													display: 'flex',
													flexDirection: 'row',
													gap: '5px',
													alignItems: 'center',
												}}
											>
												{user.username}
												<IconButton
													color='inherit'
													onClick={() => handleRemoveUser(user)}
													style={{}}
												>
													<ClearIcon />
												</IconButton>
											</div>
										</li>
									))}
								</ul>
							</div>
							<div
								className='create-chat-buttons'
								style={{ display: 'flex', flexDirection: 'row', gap: '10px' }}
							>
								<button onClick={handleCreateChat}>Create</button>
								<button onClick={() => setShowNewChatModal(false)}>
									Cancel
								</button>
							</div>
						</Box>
					</Modal>
				)}
			</div>
		</div>
	);
};

export default MessageChat;
