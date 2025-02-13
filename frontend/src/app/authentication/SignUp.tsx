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
import { useNavigate } from 'react-router-dom';
import User from '../../user/user';

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

type SignUpProps = {
	onChoiceUpdate: (newChoice: 'signIn' | 'signUp') => void;
};

export const SignUp: React.FC<SignUpProps> = ({ onChoiceUpdate }) => {
	const navigate = useNavigate();
	const registerUser = async (
		event: React.FormEvent<HTMLFormElement>
	): Promise<void> => {
		event.preventDefault();
		const data = new FormData(event.currentTarget);
		let username: string = (data.get('username') as string) ?? '';
		let phoneNumber: string = (data.get('phoneNumber') as string) ?? '';
		let name: string = (data.get('fullName') as string) ?? '';
		let email: string = (data.get('email') as string) ?? '';
		let rawPassword: string = (data.get('password') as string) ?? '';
		console.log('Data submitting: ');
		console.log({
			username: username,
			phoneNumber: phoneNumber,
			email: email,
			rawPassword: rawPassword,
			name: name,
		});
		let result = await AuthService.registerUser(
			username,
			phoneNumber,
			email,
			rawPassword,
			name
		);
		if (result) {
			onChoiceUpdate('signIn');
		}
		// console.log(user);
		// if (result.success == 'true') {
		// 	navigate('/landing');
		// } else {
		// 	window.alert(
		// 		'Sorry, we have some problems signing you up. Please try again later.'
		// 	);
		// }
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
						Sign up
					</Typography>
					<Box
						component='form'
						noValidate
						onSubmit={registerUser}
						sx={{ mt: 3 }}
					>
						<Grid container spacing={2}>
							{/* <Grid item xs={12} sm={6}>
								<TextField
									autoComplete='given-name'
									name='firstName'
									required
									fullWidth
									id='firstName'
									label='First Name'
									autoFocus
								/>
							</Grid>
							<Grid item xs={12} sm={6}>
								<TextField
									required
									fullWidth
									id='lastName'
									label='Last Name'
									name='lastName'
									autoComplete='family-name'
								/>
							</Grid> */}
							<Grid item xs={12}>
								<TextField
									required
									fullWidth
									id='username'
									label='Username'
									name='username'
									autoComplete='Username'
								/>
							</Grid>
							<Grid item xs={12}>
								<TextField
									required
									fullWidth
									id='fullName'
									label='Your Full Name'
									name='fullName'
									autoComplete='Your Full Name'
								/>
							</Grid>
							<Grid item xs={12}>
								<TextField
									required
									fullWidth
									id='phoneNumber'
									label='PhoneNumber'
									name='phoneNumber'
									autoComplete='Phone Number'
								/>
							</Grid>

							<Grid item xs={12}>
								<TextField
									required
									fullWidth
									id='email'
									label='Email Address'
									name='email'
									autoComplete='email'
								/>
							</Grid>
							<Grid item xs={12}>
								<TextField
									required
									fullWidth
									name='password'
									label='Password'
									type='password'
									id='password'
									autoComplete='new-password'
								/>
							</Grid>
							<Grid item xs={12}>
								<FormControlLabel
									control={
										<Checkbox value='allowExtraEmails' color='primary' />
									}
									label='I want to receive inspiration, marketing promotions and updates via email.'
								/>
							</Grid>
						</Grid>
						<Button
							type='submit'
							fullWidth
							variant='contained'
							sx={{ mt: 3, mb: 2 }}
						>
							Sign Up
						</Button>
						<Grid
							container
							justifyContent='flex-end'
							style={{ marginBottom: '30px' }}
						>
							<Grid item>
								<Link variant='body2' onClick={() => onChoiceUpdate('signIn')}>
									{'Already have an account? Sign in'}
								</Link>
							</Grid>
						</Grid>
					</Box>
				</Box>
				{/* <Copyright sx={{ mt: 5 }} /> */}
			</Container>
		</ThemeProvider>
	);
};
