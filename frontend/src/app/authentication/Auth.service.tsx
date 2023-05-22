import HttpService, { methodType } from '../../api/httpService';
import User from '../../user/user';

export default class AuthService extends HttpService {
	private static _user: User;
	private static backendAddress: string = 'http://localhost:5169';
	private static _requestResult: Record<string, string> = {
		success: 'true',
		error: 'none',
	};

	public static registerUser(
		username: string,
		phoneNumber: string,
		email: string,
		rawPassword: string,
		name: string
	): Record<string, string> {
		// call the register user api from the backend, return all the hashed password and token (can decide to call here or call later)
		console.log(
			'Submitting request of registering ' + username + ' to the backend'
		);
		let hashedPassword: string = '';
		let token: string = '';
		// pretend that we have the new user already
		AuthService._user = new User(
			username,
			phoneNumber,
			name,
			email,
			hashedPassword,
			token
		);
		return AuthService._requestResult;
	}

	public static async loginUser(username: string, rawPassword: string) {
		// return the boolean-type function login check from the backend
		// note that the function should be able to check username's password from database

		// if (AuthService._requestResult.success == 'true') {
		// 	let username: string = username;
		// 	let phoneNumber: string = '';
		// 	let name: string = '';
		// 	let email: string = '';
		// 	let hashedPassword: string = '';
		// 	let token: string = '';

		// 	AuthService._user = new User(
		// 		username,
		// 		phoneNumber,
		// 		name,
		// 		email,
		// 		hashedPassword,
		// 		token
		// 	);
		// }
		try {
			// const response = await this.httpRequest(
			// 	'/login',
			// 	methodType.post,
			// 	{},
			// 	null,
			// 	{
			// 		Username: username,
			// 		HashedPassword: rawPassword,
			// 	}
			// );

			const response = await fetch('http://localhost:5169/login', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json',
					Accept: '*/*',
				},
				body: JSON.stringify({
					Username: username,
					HashedPassword: rawPassword,
				}),
			});

			if (response.ok) {
				const data = await response.json();
				console.log('Login successful:', data);
			} else {
				const error = await response.json();
				console.log('Login failed:', error);
			}
		} catch (error) {
			console.log(error);
		}
		// return AuthService._requestResult;
	}

	public static logoutUser(): Record<string, string> {
		AuthService._user = new User('', '', '', '', '', '');
		return AuthService._requestResult;
	}

	public static changePassword(
		username: string,
		oldPassword: string,
		newPassword: string
	): Record<string, string> {
		// call the api to changePassword function on the backend
		let hashedPassword: string = '';
		AuthService._user.hashedPassword = hashedPassword;
		return AuthService._requestResult;
	}

	public static editUserDetails(
		username: string,
		newPhoneNumber: string,
		newEmail: string,
		newName: string
	): Record<string, string> {
		// note that the username is unchangable

		AuthService._user.phoneNumber = newPhoneNumber;
		AuthService._user.email = newEmail;
		AuthService._user.name = newName;
		// call backend api with the similar, or pass the User directly
		return AuthService._requestResult;
	}

	public static get user(): User {
		return AuthService._user;
	}
}
