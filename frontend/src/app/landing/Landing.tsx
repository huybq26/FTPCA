import { useLocation } from 'react-router-dom';
import User from '../../user/user';
import AuthService from '../authentication/Auth.service';
// interface LandingPageProps {
// 	location: {
// 		state: {
// 			user: User;
// 		};
// 	};
// }

const LandingPage: React.FC = () => {
	const user: User = AuthService.user;
	return (
		<div>
			<h1>Welcome to the Landing Page</h1>
			<p>Username: {user.username}</p>
			<p>Name: {user.name}</p>
			<p>Phone Number: {user.phoneNumber}</p>
			<p>Password: {user.hashedPassword}</p>
		</div>
	);
};

export default LandingPage;
