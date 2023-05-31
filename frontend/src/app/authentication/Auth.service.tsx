import HttpService, { methodType } from '../../api/httpService';
import User from '../../user/user';

export default class AuthService extends HttpService {
	public static async registerUser(
		username: string,
		phoneNumber: string,
		email: string,
		rawPassword: string,
		name: string
	): Promise<boolean> {
		try {
			console.log('Registering...');
			console.log(
				JSON.stringify({
					Username: username,
					Email: email,
					Name: name,
					PhoneNumber: phoneNumber,
					HashedPassword: rawPassword,
				})
			);
			const response = await fetch('http://localhost:5169/register', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json',
				},
				body: JSON.stringify({
					Username: username,
					Email: email,
					Name: name,
					PhoneNumber: phoneNumber,
					HashedPassword: rawPassword,
				}),
			});

			if (response.ok) {
				const data = await response.json();
				console.log('Register successful:', data);
				return true;
			} else {
				const error = await response.json();
				window.alert(
					'Your credentials did not meet our requirements. Please check again.'
				);
				return false;
			}
		} catch (error) {
			console.log(error);
			window.alert('Internal errors. Please try again later.');
			return false;
		}
	}

	public static async loginUser(
		username: string,
		rawPassword: string
	): Promise<boolean> {
		try {
			console.log('Logging in...');
			console.log(
				JSON.stringify({
					Username: username,
					HashedPassword: rawPassword,
				})
			);
			const response = await fetch('http://localhost:5169/login', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json',
				},
				body: JSON.stringify({
					Username: username,
					HashedPassword: rawPassword,
				}),
			});

			if (response.ok) {
				const data = await response.json();
				console.log('Login successful:', data);
				console.log(data.data.token);
				sessionStorage.setItem('jwtToken', data.data.token);
				// Store the token or user data as needed
				return true;
			} else {
				const error = await response.json();
				window.alert('Incorrect username or password!');
				return false;
			}
		} catch (error) {
			console.log(error);
			window.alert('Internal errors. Please try again later.');
			return false;
		}
	}

	// public static editUserDetails(
	// 	username: string,
	// 	newPhoneNumber: string,
	// 	newEmail: string,
	// 	newName: string
	// ): Record<string, string> {
	// 	// note that the username is unchangable

	// 	AuthService._user.phoneNumber = newPhoneNumber;
	// 	AuthService._user.email = newEmail;
	// 	AuthService._user.name = newName;
	// 	// call backend api with the similar, or pass the User directly
	// 	return AuthService._requestResult;
	// }

	// public static get user(): User {
	// 	return AuthService._user;
	// }
}
