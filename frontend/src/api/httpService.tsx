export enum methodType {
	get = 'GET',
	post = 'POST',
	put = 'PUT',
	delete = 'DELETE',
}

export default class HttpService {
	public static httpRequest(
		api: string,
		method: methodType,
		headers: Record<string, string>,
		options: object | null,
		content: object | null
	): Promise<any> {
		const host = 'http://localhost:5169'; // fix later to make env
		let requestOptions: any = {
			method: method,
			headers: { ...headers, 'Content-type': 'application/json' },
			...options,
		};

		if (method == methodType.post || method == methodType.put) {
			requestOptions.body = JSON.stringify(content);
		}

		return fetch(host + api, requestOptions).then((response) =>
			response.json()
		);
	}
}
