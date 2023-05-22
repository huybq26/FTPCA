import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import AuthService from './Auth.service';
import User from '../../user/user';
import { useNavigate } from 'react-router-dom';

function Copyright(props: any) {
	return (
		<Typography
			variant='body2'
			color='text.secondary'
			align='center'
			{...props}
		>
			{'Copyright © '}
			<Link color='inherit' href='https://mui.com/'>
				Your Website
			</Link>{' '}
			{new Date().getFullYear()}
			{'.'}
		</Typography>
	);
}

const theme = createTheme();

type SignInProps = {
	onChoiceUpdate: (newChoice: 'signIn' | 'signUp') => void;
};
export const SignIn: React.FC<SignInProps> = ({ onChoiceUpdate }) => {
	const navigate = useNavigate();

	const loginUser = (event: React.FormEvent<HTMLFormElement>): void => {
		const data = new FormData(event.currentTarget);
		let username: string = (data.get('username') as string) ?? '';
		let rawPassword: string = (data.get('password') as string) ?? '';

		console.log('Data signing in: ');
		console.log({
			username: username,
			rawPassword: rawPassword,
		});
		try {
			AuthService.loginUser(username, rawPassword);
		} catch (error) {
			console.log(error);
		}

		// console.log('Result of signing in is: ' + result.success);
		// let user: User = AuthService.user;
		// console.log(user);

		// if (result.success == 'true') {
		// 	navigate('/landing');
		// } else {
		// 	window.alert(
		// 		'Sorry, we have some problems signing you up. Please try again later.'
		// 	);
		// }
	};

	const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
		event.preventDefault();
		const data = new FormData(event.currentTarget);
		console.log({
			username: data.get('username'),
			password: data.get('password'),
		});
	};

	return (
		<ThemeProvider theme={theme}>
			<Container component='main' maxWidth='xs'>
				<CssBaseline />
				<Box
					sx={{
						marginTop: 8,
						display: 'flex',
						flexDirection: 'column',
						alignItems: 'center',
					}}
				>
					<Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
						<LockOutlinedIcon />
					</Avatar>
					<Typography component='h1' variant='h5'>
						Sign in
					</Typography>
					<Box component='form' onSubmit={loginUser} noValidate sx={{ mt: 1 }}>
						<TextField
							margin='normal'
							required
							fullWidth
							id='username'
							label='Username'
							name='username'
							autoComplete='Username'
							autoFocus
						/>
						<TextField
							margin='normal'
							required
							fullWidth
							name='password'
							label='Password'
							type='password'
							id='password'
							autoComplete='current-password'
						/>
						<FormControlLabel
							control={<Checkbox value='remember' color='primary' />}
							label='Remember me'
						/>
						<Button
							type='submit'
							fullWidth
							variant='contained'
							sx={{ mt: 3, mb: 2 }}
						>
							Sign In
						</Button>
						<Grid container>
							<Grid item xs>
								<Link href='#' variant='body2'>
									Forgot password?
								</Link>
							</Grid>
							<Grid item>
								<Link variant='body2' onClick={() => onChoiceUpdate('signUp')}>
									{"Don't have an account? Sign Up"}
								</Link>
							</Grid>
						</Grid>
					</Box>
				</Box>
				<Copyright sx={{ mt: 8, mb: 4 }} />
			</Container>
		</ThemeProvider>
	);
};
