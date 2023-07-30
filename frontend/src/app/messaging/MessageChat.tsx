import React, { useState, useEffect, useRef, ChangeEvent } from 'react';
import { UserInfo, fetchToken } from '../authentication/JwtUtils';

import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import {
	AppBar,
	Box,
	Button,
	IconButton,
	InputBase,
	Modal,
	Paper,
	TextField,
	Toolbar,
	Typography,
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import CheckIcon from '@mui/icons-material/Check';
import ClearIcon from '@mui/icons-material/Clear';
import AddIcon from '@mui/icons-material/Add';
import Mood from '@mui/icons-material/Mood';
import SendIcon from '@mui/icons-material/Send';
import './style.css';
import * as signalR from '@microsoft/signalr';

interface Conversation {
	id: number;
	name: string;
	lastSender: string;
	lastMessage: string;
	groupImage: string;
	creator: string;
}

interface Message {
	id: number;
	sender: string;
	content: string;
	timestamp: string;
	fileid: string | null;
	filename: string | null;
	filepath: string | null;
	filetype: string | null;
	senderid: string | null;
	senderusername: string | null;
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
	const [file, setFile] = useState<File | null>(null);
	const fileInputRef = useRef<HTMLInputElement | null>(null);

	// useEffect(() => {
	// 	const newConnection = new signalR.HubConnectionBuilder()
	// 		.withUrl('http://localhost:5169/chatHub') // Specify the hub URL
	// 		.build();

	// 	setConnection(newConnection);
	// }, []);

	// useEffect(() => {
	// 	if (connection) {
	// 		connection
	// 			.start()
	// 			.then(() => {
	// 				console.log('Connected to SignalR hub');

	// 				// Subscribe to the 'ReceiveMessage' event from the hub
	// 				connection.on('ReceiveMessage', (sender: string, message: string) => {
	// 					setMessages((prevMessages) => [
	// 						...prevMessages,
	// 						{
	// 							id: prevMessages.length + 1,
	// 							sender,
	// 							content: message,
	// 							timestamp: getCurrentDateTime(),
	// 							fileid
	// 						},
	// 					]);
	// 				});
	// 			})
	// 			.catch((error) => console.error('SignalR connection error:', error));
	// 	}
	// }, [connection]);

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
						creator: userInfo?.name ?? '',
						lastMessage: item[2],
						groupImage: '',
						lastSender: item[3],
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

	// const getCurrentDateTime = (): string => {
	// 	const currentDate = new Date();
	// 	const gmtPlus8Offset = 8 * 60; // GMT+8 offset in minutes
	// 	const gmtPlus8DateTime = new Date(
	// 		currentDate.getTime() + gmtPlus8Offset * 60000
	// 	); // Add offset in milliseconds
	// 	const formattedDateTime = gmtPlus8DateTime.toISOString().slice(0, 19);

	// 	return formattedDateTime;
	// };
	const getCurrentDateTime = (): string => {
		const currentDate = new Date();
		const gmtPlus8Offset = 8 * 60; // GMT+8 offset in minutes
		const gmtPlus8DateTime = new Date(
			currentDate.getTime() + gmtPlus8Offset * 60000
		); // Add offset in milliseconds

		// Get the individual components of the date and time
		const year = gmtPlus8DateTime.getFullYear();
		const month = gmtPlus8DateTime.getMonth() + 1; // Months are 0-based, so add 1
		const day = gmtPlus8DateTime.getDate();
		const hours = gmtPlus8DateTime.getHours();
		const minutes = gmtPlus8DateTime.getMinutes();
		const seconds = gmtPlus8DateTime.getSeconds();
		const ampm = hours >= 12 ? 'PM' : 'AM';
		const formattedDateTime = `${month}/${day}/${year} ${formatTwoDigits(
			hours
		)}:${formatTwoDigits(minutes)}:${formatTwoDigits(seconds)} ${ampm}`;

		return formattedDateTime;
	};

	const formatTwoDigits = (num: number): string => {
		return num.toString().padStart(2, '0');
	};

	const handleMoodClick = () => {};

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
				const parsedResults: Message[] = await Promise.all(
					data.data.map(async (item: any) => {
						let fileid = item[4];
						console.log(fileid);
						if (fileid !== 'null') {
							try {
								const fileResponse = await fetch(
									`http://localhost:5169/getfile?fileid=${fileid}`,
									{
										method: 'GET',
										headers: {
											Authorization: `Bearer ${sessionStorage.getItem(
												'jwtToken'
											)}`,
										},
									}
								);

								if (fileResponse.ok) {
									const fileData: any = await fileResponse.json();
									console.log('File detected');
									console.log(fileData.data);

									return {
										id: item[0],
										sender: item[1],
										content: item[2],
										timestamp: item[3],
										fileid: item[4],
										senderid: item[5],
										senderusername: item[6],
										filename: fileData.data[1],
										filepath: fileData.data[2],
										filetype: fileData.data[4],
									};
								} else {
									console.log('Error:', fileResponse.status);
								}
							} catch (e) {
								console.log('An error occurred while fetching file:', e);
							}
						}

						return {
							id: item[0],
							sender: item[1],
							content: item[2],
							timestamp: item[3],
							fileid: item[4],
							senderid: item[5],
							senderusername: item[6],
						};
					})
				);
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

	const handleFileChange = async (e: ChangeEvent<HTMLInputElement>) => {
		const selectedFile = e.target.files?.[0];
		if (selectedFile) {
			setFile(selectedFile);
		}
		if (selectedFile) {
			try {
				const formData = new FormData();
				formData.append('file', selectedFile);

				const response = await axios.post(
					'http://localhost:5169/upload',
					formData,
					{
						headers: { 'Content-Type': 'multipart/form-data' },
					}
				);

				if (response.status === 200) {
					console.log(response.data.data);
					let fileId = response.data.data.fileId; // Handle the response from the backend if needed
					let fileName = response.data.data.fileName;
					let filePath = response.data.data.filePath;
					let fileType = response.data.data.fileType;

					// send message with file (without content)
					if (selectedConversation) {
						console.log('Sending message with file');
						try {
							const response = await axios.post(
								`http://localhost:5169/sendmessage?senderid=${userInfo?.userid}&convid=${selectedConversation.id}&fileid=${fileId}&content=Sent a file`,
								{},
								{
									headers: {
										Authorization: `Bearer ${sessionStorage.getItem(
											'jwtToken'
										)}`,
									},
								}
							);

							if (response.status === 200) {
								const newMessageObject: Message = {
									id: response.data.messageid,
									sender: userInfo?.name || '',
									content: fileName,
									timestamp: getCurrentDateTime(),
									senderid: userInfo?.userid || '',
									senderusername: userInfo?.username || '',
									fileid: fileId,
									filename: fileName,
									filepath: filePath,
									filetype: fileType,
								};
								// SignalR part
								// if (connection) {
								// 	connection
								// 		.invoke('SendMessage', userInfo?.username, fileName)
								// 		.catch((error) =>
								// 			console.error('SignalR sendMessage error:', error)
								// 		);
								// }

								setMessages([...messages, newMessageObject]);
								setNewMessage('');
								let updateConversation = selectedConversation;
								updateConversation.lastMessage = 'Sent a file';
								updateConversation.lastSender = userInfo?.name ?? '';
								setSelectedConversation(updateConversation);
							} else {
								console.log('Error:', response.status);
							}
						} catch (error) {
							console.log('An error occurred:', error);
						}
					}
				}
			} catch (error) {
				console.error(error);
			}
		} else {
			console.log('Please select a file first.');
		}
	};

	const handleUpload = async () => {
		if (file) {
			try {
				const formData = new FormData();
				formData.append('file', file);

				const response = await axios.post(
					'http://localhost:5169/upload',
					formData,
					{
						headers: { 'Content-Type': 'multipart/form-data' },
					}
				);

				if (response.status === 200) {
					console.log(response.data.data);
					let fileId = response.data.data.fileId; // Handle the response from the backend if needed
					let fileName = response.data.data.fileName;
					let filePath = response.data.data.filePath;
					let fileType = response.data.data.fileType;

					// send message with file (without content)
					if (selectedConversation) {
						console.log('Sending message with file');
						try {
							const response = await axios.post(
								`http://localhost:5169/sendmessage?senderid=${userInfo?.userid}&convid=${selectedConversation.id}&fileid=${fileId}&content=Sent a file`,
								{},
								{
									headers: {
										Authorization: `Bearer ${sessionStorage.getItem(
											'jwtToken'
										)}`,
									},
								}
							);

							if (response.status === 200) {
								const newMessageObject: Message = {
									id: response.data.messageid,
									sender: userInfo?.name || '',
									content: fileName,
									timestamp: getCurrentDateTime(),
									senderid: userInfo?.userid || '',
									senderusername: userInfo?.username || '',
									fileid: fileId,
									filename: fileName,
									filepath: filePath,
									filetype: fileType,
								};
								// SignalR part
								// if (connection) {
								// 	connection
								// 		.invoke('SendMessage', userInfo?.username, fileName)
								// 		.catch((error) =>
								// 			console.error('SignalR sendMessage error:', error)
								// 		);
								// }

								setMessages([...messages, newMessageObject]);
								setNewMessage('');
								let updateConversation = selectedConversation;
								updateConversation.lastMessage = 'Sent a file';
								updateConversation.lastSender = userInfo?.name ?? '';
								setSelectedConversation(updateConversation);
							} else {
								console.log('Error:', response.status);
							}
						} catch (error) {
							console.log('An error occurred:', error);
						}
					}
				}
			} catch (error) {
				console.error(error);
			}
		} else {
			console.log('Please select a file first.');
		}
	};

	const handleSendMessage = async () => {
		if (selectedConversation && newMessage.trim() !== '') {
			try {
				const response = await axios.post(
					`http://localhost:5169/sendmessage?senderid=${
						userInfo?.userid
					}&convid=${
						selectedConversation.id
					}&fileid=${null}&content=${newMessage}`,
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
						sender: userInfo?.name || '',
						content: newMessage,
						timestamp: getCurrentDateTime(),
						senderid: userInfo?.userid || '',
						senderusername: userInfo?.username || '',
						fileid: 'null',
						filename: 'null',
						filepath: 'null',
						filetype: 'null',
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
					let updateConversation = selectedConversation;
					updateConversation.lastMessage = newMessage;
					updateConversation.lastSender = userInfo?.name ?? '';
					setSelectedConversation(updateConversation);
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
					`http://localhost:5169/createconv?convname=${newChatName}&creatorname=${
						userInfo?.name
					}&creatorid=${userInfo?.userid}&participantList=${selectedUsers
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
						creator: userInfo?.userid ?? '',
						lastMessage: 'Created group ' + newChatName,
						groupImage: '',
						lastSender: userInfo?.name ?? '',
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
						style={{
							display: 'flex',
							justifyContent: 'center',
							width: '100%',
						}}
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
					<div
						style={{
							maxWidth: '100%',
							flex: 1,
							overflowY: 'auto',
							height: 'calc(100vh - 50px)',
						}}
					>
						{conversations.map((conversation) => (
							<>
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
										// borderBottom: '1px solid black',
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
											{`${conversation.lastSender}: ${
												conversation.lastMessage.length > 30
													? conversation.lastMessage.slice(0, 30) + '...'
													: conversation.lastMessage
											}`}
										</div>
									</div>
								</div>
								<hr
									style={{
										width: '200px',
										marginBottom: '-3px',
										marginTop: '0px',
										borderColor: '#ebeae8',
										color: '#ebeae8',
										height: '0.01px',
									}}
								/>
							</>
						))}
					</div>
				</div>
				<div
					className='messages'
					style={{
						marginLeft: '20px',
						maxWidth: '100%',
						flex: 1,
						overflowY: 'auto',
						height: 'calc(100vh - 50px)',
						backgroundImage:
							'url("https://i.pinimg.com/736x/8c/98/99/8c98994518b575bfd8c949e91d20548b.jpg")',
						// backgroundSize: 'cover',
						// opacity: 0.7,
					}}
					ref={messagesContainerRef}
				>
					{selectedConversation ? (
						<div className='conversation-messages'>
							<AppBar position='sticky' style={{ background: '#34568B' }}>
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
							<div
								style={{
									backgroundColor: 'rgba(217, 217, 217,0.5)',
									height: '100%',
								}}
							>
								<div
									style={{
										display: 'flex',
										flexDirection: 'column',
									}}
								>
									{messages.map((message, index) => {
										const currentDate = new Date(
											message.timestamp
										).toLocaleDateString();

										// Get the date of the previous message (if it exists)
										const prevMessage =
											index > 0
												? new Date(
														messages[index - 1].timestamp
												  ).toLocaleDateString()
												: null;

										// Check if the date of the current message is different from the previous message
										const showDateHeader = prevMessage !== currentDate;
										return (
											<div
												key={message.id}
												className='message-container'
												style={{
													position: 'relative',
													marginBottom:
														index == messages.length - 1 ? '35px' : '10px',
													marginTop: index == 0 ? '10px' : '0px',
													maxWidth: '60%',
													alignSelf:
														userInfo?.userid === message.senderid
															? 'flex-end'
															: 'flex-start',
													display: 'flex',
													flexDirection: 'column',
													marginLeft: '40px',
												}}
											>
												{
													// showDateHeader && (
													// <div
													// 	style={{
													// 		fontSize: '0.8rem',
													// 		color: '#616161',
													// 		textAlign: 'center',
													// 		backgroundColor: '#ECE5DD',
													// 		padding: '5px',
													// 		borderRadius: '5px',
													// 		width: 'fit-content', // Make the date header width fit its content
													// 		margin: '0 auto', // Center the date header horizontally
													// 	}}
													// >
													// 	{/* Render the date here */}
													// 	{currentDate}
													// </div>
													// )
												}
												{userInfo?.userid != message.senderid && (
													<img
														src='./default-ava.png'
														height='40px'
														alt='ava'
														style={{
															position: 'absolute',
															// left: '-80px', // Adjust this value as needed
															top: 0,
															borderRadius: '50%', // Round shape
														}}
													/>
												)}

												<div
													className='message'
													style={{
														backgroundColor:
															userInfo?.userid === message.senderid
																? '#D7E6FF'
																: 'white',
														padding: '10px',
														borderRadius: '5px',
														marginLeft:
															userInfo?.userid !== message.senderid
																? '50px'
																: '0',
														marginRight:
															userInfo?.userid == message.senderid
																? '20px'
																: '0',
														minWidth: `${Math.min(
															150,
															message.content.length * 10
														)}px`,
													}}
												>
													<div className='message-sender'>
														{userInfo?.userid !== message.senderid && (
															<>
																<span style={{ fontWeight: 'bold' }}>
																	{message.sender}
																</span>
																<span
																	style={{
																		fontSize: '0.7rem',
																		marginLeft: '5px',
																		color: '#616161',
																	}}
																>
																	{'@' + message.senderusername}
																</span>
															</>
														)}
													</div>
													<div className='message-content'>
														{message.fileid !== 'null' ? (
															// Check if the file type is an image (you can customize this check based on actual image MIME types)
															message.filetype?.startsWith('image/') ? (
																<img
																	src={`http://localhost:5169/Uploads/${
																		message.filename
																			? message.fileid + '_' + message.filename
																			: ''
																	}`}
																	alt={message.filename ?? ''}
																	style={{
																		maxHeight: '300px',
																		maxWidth: '100%',
																	}} // Set appropriate height
																/>
															) : (
																<a
																	href={`http://localhost:5169/Uploads/${
																		message.filename
																			? message.fileid + '_' + message.filename
																			: ''
																	}`}
																	download
																	target='_blank'
																	rel='noopener noreferrer'
																>
																	{message.filename}
																</a>
															)
														) : (
															message.content
														)}
													</div>
													<div
														className='message-timestamp'
														style={{
															fontSize: '0.7rem',
															// color: 'gray',
															textAlign: 'right',
															color: '#616161',
															marginBottom: '-5px',
														}}
													>
														{message.timestamp.split(' ')[1] +
															' ' +
															message.timestamp.split(' ')[2]}
														{/* {message.timestamp} */}
													</div>
												</div>
											</div>
										);
									})}
								</div>
							</div>

							<div
								className='message-input-container'
								style={{
									marginTop: '50px',
									position: 'fixed',
									// left: 0,
									bottom: 0,

									width: '75%',
									backgroundColor: '#f5f5f5',
									// padding: '10px',
									display: 'flex',
									flexDirection: 'row',
									alignItems: 'center',
									height: '60px',
									borderRadius: '15px',
								}}
							>
								{/* <div>
									<input type='file' onChange={handleFileChange} />
									<button onClick={handleUpload}>Upload File</button>
								</div> */}

								<div style={{ marginLeft: '10px' }}>
									<input
										ref={fileInputRef}
										id='file-input'
										type='file'
										style={{ display: 'none' }}
										onChange={handleFileChange}
									/>
									<label htmlFor='file-input'>
										<IconButton component='span'>
											<AddIcon fontSize='large' />
										</IconButton>
									</label>
								</div>
								<div>
									<IconButton onClick={handleMoodClick}>
										<Mood fontSize='medium' />
									</IconButton>
								</div>
								{/* <input
									type='text'
									className='message-input'
									placeholder='Type your message...'
									value={newMessage}
									onChange={(e) => setNewMessage(e.target.value)}
								/>
								<button onClick={handleSendMessage}>Send</button> */}
								<div>
									<TextField
										type='text'
										variant='outlined'
										size='small'
										placeholder='Type your message...'
										value={newMessage}
										onChange={(e) => setNewMessage(e.target.value)}
										style={{
											flex: 1,
											width: '750px',
											marginRight: '10px',
											marginLeft: '20px',
											borderRadius: '20px',
											verticalAlign: '10px',
										}}
									/>
								</div>
								<Button
									variant='contained'
									color='primary'
									onClick={handleSendMessage}
								>
									<SendIcon />
								</Button>
							</div>
						</div>
					) : (
						<h2 style={{ textAlign: 'center' }}>Select a Conversation</h2>
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
