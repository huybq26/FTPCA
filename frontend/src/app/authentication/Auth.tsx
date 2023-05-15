import React, { Component } from 'react';
import AuthService from './Auth.service';
import { SignIn } from './SignIn';
import { SignUp } from './SignUp';

export default function Auth() {
	const [choice, setChoice] = React.useState<'signIn' | 'signUp'>('signIn');

	const handleChoiceUpdate = (newChoice: 'signIn' | 'signUp') => {
		setChoice(newChoice);
	};

	return (
		<div>
			{choice == 'signIn' ? (
				<SignIn onChoiceUpdate={handleChoiceUpdate} />
			) : (
				<SignUp onChoiceUpdate={handleChoiceUpdate} />
			)}
		</div>
	);
}
