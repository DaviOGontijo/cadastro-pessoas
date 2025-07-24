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
import { Link as RouterLink } from 'react-router-dom';
import { Visibility, VisibilityOff } from '@mui/icons-material';

import { useLogin } from '../../hooks/useLogin';

type FormData = { login: string; password: string; remember: boolean };

export default function LoginPage() {
  const { login, error, loading } = useLogin();
  const [showPassword, setShowPassword] = useState(false);

  const { control, handleSubmit, formState: { errors } } = useForm<FormData>({
    defaultValues: { login: '', password: '', remember: false },
  });

  const onSubmit = (data: FormData) => login(data);

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
      <Typography variant="h4" textAlign="center" mb={2}>Acesso ao Sistema</Typography>

      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <Box component="form" onSubmit={handleSubmit(onSubmit)} noValidate sx={{ width: '100%' }}>
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
              disabled={loading}
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
              disabled={loading}
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton
                      aria-label="toggle password visibility"
                      onClick={togglePasswordVisibility}
                      edge="end"
                      disabled={loading}
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
              control={<Checkbox {...field} color="primary" disabled={loading} />}
              label="Lembrar-me"
            />
          )}
        />

        <Button
          type="submit"
          fullWidth
          variant="contained"
          color="primary"
          disabled={loading}
          sx={{ mt: 2, py: 1.5 }}
          endIcon={loading ? <CircularProgress size={24} color="inherit" /> : null}
        >
          {loading ? 'Entrando...' : 'Entrar'}
        </Button>
      </Box>

      <Box sx={{ mt: 2, textAlign: 'center' }}>
        <Typography variant="body2">
          Ainda não tem conta? {' '}
        </Typography>
        <RouterLink to="/register" style={{ textDecoration: 'none', color: '#1976d2' }}>
          Registre-se
        </RouterLink>
      </Box>

      <Box textAlign="center" mt={4}>
        <Typography variant="body2" color="text.secondary">
          © {new Date().getFullYear()}.
        </Typography>
      </Box>
    </Container>
  );
}
