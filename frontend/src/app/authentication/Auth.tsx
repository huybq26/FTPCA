import React, { Component, useEffect } from 'react';
import AuthService from './Auth.service';
import { SignIn } from './SignIn';
import { SignUp } from './SignUp';

interface AuthLogin {
	onLogin: () => void;
}

const Auth: React.FC<AuthLogin> = ({ onLogin }) => {
	const [choice, setChoice] = React.useState<'signIn' | 'signUp'>('signIn');
	const [isLoggedIn, setIsLoggedIn] = React.useState(false);

	useEffect(() => {
		const token = sessionStorage.getItem('jwtToken');
		if (token) setIsLoggedIn(true);
		else setIsLoggedIn(false);
	}, []);

	const handleLogin = () => {
		if (sessionStorage.getItem('jwtToken')) {
			onLogin();
			setIsLoggedIn(true);
		}
	};

	const handleChoiceUpdate = (newChoice: 'signIn' | 'signUp') => {
		setChoice(newChoice);
	};

	return (
		<div>
			{choice == 'signIn' ? (
				<SignIn
					onChoiceUpdate={handleChoiceUpdate}
					onLoginSuccess={handleLogin}
				/>
			) : (
				<SignUp onChoiceUpdate={handleChoiceUpdate} />
			)}
		</div>
	);
};

export default Auth;
