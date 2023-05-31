import jwtDecode from 'jwt-decode';

interface UserInfo {
	userid: string;
	username: string;
	name: string;
	email: string;
	phonenumber: string;
}

export const fetchToken = (): UserInfo | null => {
	const token = sessionStorage.getItem('jwtToken');
	console.log('Retrieved token: ' + token);

	if (token) {
		try {
			const decodedToken: any = jwtDecode(token);
			console.log(decodedToken);

			// const { userid, username, name, email } = decodedToken;
			return {
				userid: decodedToken['UserId'],
				username: decodedToken['Username'],
				name: decodedToken['Name'],
				email: decodedToken['Email'],
				phonenumber: decodedToken['PhoneNumber'],
			};
		} catch (error) {
			console.log('Error decoding JWT token:', error);
		}
	}

	return null;
};
