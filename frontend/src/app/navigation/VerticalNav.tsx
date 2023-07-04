import React from 'react';
import { Link } from 'react-router-dom';
import { List, ListItem, ListItemIcon, ListItemText } from '@mui/material';
import ExitToAppIcon from '@mui/icons-material/ExitToApp';
import PersonIcon from '@mui/icons-material/Person';
import GroupIcon from '@mui/icons-material/Group';
import SearchIcon from '@mui/icons-material/Search';
import { useNavigate } from 'react-router-dom';

interface VerticalNavProps {
	onLogout: () => void;
}

const VerticalNav: React.FC<VerticalNavProps> = ({ onLogout }) => {
	const navigate = useNavigate();

	const navStyle: React.CSSProperties = {
		width: '60px',
		backgroundColor: '#f0f0f0',
		display: 'flex',
		flexDirection: 'column',
		height: '100%',
	};

	const listItemStyle: React.CSSProperties = {
		display: 'flex',
		justifyContent: 'center',
		alignItems: 'center',
		height: '60px',
	};

	const logoutItemStyle: React.CSSProperties = {
		display: 'flex',
		justifyContent: 'center',
		alignItems: 'center',
		height: '60px',
		cursor: 'pointer',
	};

	const handleLogout = () => {
		sessionStorage.removeItem('jwtToken');
		onLogout();
		navigate('/login');
	};

	return (
		<div style={navStyle}>
			<List component='nav'>
				<ListItem
					button
					component={Link}
					to='/friendlist'
					style={listItemStyle}
				>
					<ListItemIcon>
						<GroupIcon />
					</ListItemIcon>
				</ListItem>
				<ListItem
					button
					component={Link}
					to='/friendrequest'
					style={listItemStyle}
				>
					<ListItemIcon>
						<PersonIcon />
					</ListItemIcon>
				</ListItem>
				<ListItem
					button
					component={Link}
					to='/searchfriend'
					style={listItemStyle}
				>
					<ListItemIcon>
						<SearchIcon />
					</ListItemIcon>
				</ListItem>
				<ListItem button onClick={handleLogout} style={logoutItemStyle}>
					<ListItemIcon>
						<ExitToAppIcon />
					</ListItemIcon>
				</ListItem>
			</List>
		</div>
	);
};

export default VerticalNav;
