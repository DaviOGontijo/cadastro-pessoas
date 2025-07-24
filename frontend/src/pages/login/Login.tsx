import React, { useState } from 'react';
import {
  Box,
  Button,
  Checkbox,
  Container,
  CssBaseline,
  FormControlLabel,
  IconButton,
  InputAdornment,
  TextField,
  Typography,
  Alert,
  CircularProgress,
} from '@mui/material';
import { useForm, Controller } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { Visibility, VisibilityOff } from '@mui/icons-material';

type FormData = { login: string; password: string; remember: boolean };

export default function LoginPage() {
  const navigate = useNavigate();
  const [errorMessage, setErrorMessage] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const { control, handleSubmit, formState: { errors } } = useForm<FormData>({
    defaultValues: { login: '', password: '', remember: false },
  });

  const onSubmit = async (formData: FormData) => {
    setErrorMessage('');
    setIsSubmitting(true);

    // Simulação de autenticação fake
    await new Promise(res => setTimeout(res, 1000)); // delay 1s

    // Exemplo simples: login == "admin" e senha == "123456"
    if (formData.login === 'admin' && formData.password === '123456') {
      // Redireciona para home ou dashboard
      navigate('/');
    } else {
      setErrorMessage('Usuário ou senha inválidos');
    }

    setIsSubmitting(false);
  };

  const togglePasswordVisibility = () => setShowPassword(prev => !prev);

  return (
    <Container
      component="main"
      maxWidth="xs"
      sx={{
        backgroundColor: '#FAF9F6',
        border: '1px solid #ccc',
        borderRadius: 2,
        p: 4,
        boxShadow: 2,
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'space-between',
        minHeight: '70vh',
      }}
    >
      <CssBaseline />

      <Typography component="h1" variant="h4" sx={{ display: 'flex', alignItems: 'center', justifyContent:'center',  mb: 2 }}>
        Acesso ao Sistema
      </Typography>

      <Box sx={{ textAlign: 'center' }}>
        {errorMessage && (
          <Alert severity="error" sx={{ width: '100%', mb: 2 }}>
            {errorMessage}
          </Alert>
        )}

        <Box component="form" noValidate onSubmit={handleSubmit(onSubmit)} sx={{ width: '100%' }}>
          <Controller
            name="login"
            control={control}
            rules={{ required: 'Usuário é obrigatório' }}
            render={({ field }) => (
              <TextField
                {...field}
                label="Usuário ou E‑mail"
                fullWidth
                margin="normal"
                error={!!errors.login}
                helperText={errors.login?.message}
                disabled={isSubmitting}
                autoFocus
              />
            )}
          />

          <Controller
            name="password"
            control={control}
            rules={{
              required: 'Senha obrigatória',
              minLength: { value: 6, message: 'Mínimo 6 caracteres' },
            }}
            render={({ field }) => (
              <TextField
                {...field}
                label="Senha"
                type={showPassword ? 'text' : 'password'}
                fullWidth
                margin="normal"
                error={!!errors.password}
                helperText={errors.password?.message}
                disabled={isSubmitting}
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        aria-label="toggle password visibility"
                        onClick={togglePasswordVisibility}
                        edge="end"
                      >
                        {showPassword ? <VisibilityOff /> : <Visibility />}
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
              />
            )}
          />

          <Controller
            name="remember"
            control={control}
            render={({ field }) => (
              <FormControlLabel
                control={<Checkbox {...field} color="primary" disabled={isSubmitting} />}
                label="Lembrar-me"
              />
            )}
          />

          <Button
            type="submit"
            fullWidth
            variant="contained"
            color="primary"
            disabled={isSubmitting}
            sx={{ mt: 2, py: 1.5 }}
            endIcon={isSubmitting ? <CircularProgress size={24} color="inherit" /> : null}
          >
            {isSubmitting ? 'Entrando...' : 'Entrar'}
          </Button>
        </Box>
      </Box>

      <Box sx={{ textAlign: 'center', mt: 4 }}>
        <Typography variant="body2" color="text.secondary">
          © {new Date().getFullYear()}.
        </Typography>
      </Box>
    </Container>
  );
}
