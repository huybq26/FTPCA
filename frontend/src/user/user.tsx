export default class User {
	private _username!: string;
	private _phoneNumber!: string;
	private _name!: string;
	private _email!: string;
	private _hashedPassword!: string;
	private _token!: string; // not sure why we need this token

	constructor(
		username: string,
		phoneNumber: string,
		name: string,
		email: string,
		hashedPassword: string,
		token: string
	) {
		this._username = username;
		this._phoneNumber = phoneNumber;
		this._name = name;
		this._email = email;
		this._hashedPassword = hashedPassword;
		this._token = token;
	}

	public get username(): string {
		return this._username;
	}

	public set username(usernameInput: string) {
		this._username = usernameInput;
	}
	public get phoneNumber(): string {
		return this._phoneNumber;
	}

	public set phoneNumber(phoneNumber: string) {
		this._phoneNumber = phoneNumber;
	}
	public get name(): string {
		return this._name;
	}

	public set name(name: string) {
		this._name = name;
	}
	public get email(): string {
		return this._email;
	}

	public set email(email: string) {
		this._email = email;
	}
	public get hashedPassword(): string {
		return this._hashedPassword;
	}

	public set hashedPassword(hashedPassword: string) {
		this._hashedPassword = hashedPassword;
	}
	public get token(): string {
		return this._token;
	}

	public set token(token: string) {
		this._token = token;
	}

	// public getUserByUsername(username: string): object{

	// }
}
