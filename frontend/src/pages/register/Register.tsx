import React from 'react';
import {
    Box,
    Button,
    Container,
    CssBaseline,
    TextField,
    Typography,
    Alert,
    CircularProgress,
} from '@mui/material';
import { useForm, Controller } from 'react-hook-form';
import { Link as RouterLink } from 'react-router-dom';
import { useRegister } from '../../hooks/useRegister';

type RegisterData = {
    username: string;
    password: string;
};

export default function RegisterPage() {
    const { register: doRegister, error, success, loading } = useRegister();

    const { control, handleSubmit, formState: { errors } } = useForm<RegisterData>({
        defaultValues: { username: '', password: '' },
    });

    const onSubmit = (data: RegisterData) => doRegister(data);

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
            <Typography component="h1" variant="h4" textAlign="center" mb={2}>
                Criar Conta
            </Typography>

            {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
            {success && <Alert severity="success" sx={{ mb: 2 }}>{success}</Alert>}

            <Box component="form" noValidate onSubmit={handleSubmit(onSubmit)}>
                <Controller
                    name="username"
                    control={control}
                    rules={{ required: 'Usuário é obrigatório' }}
                    render={({ field }) => (
                        <TextField
                            {...field}
                            label="Usuário"
                            fullWidth
                            margin="normal"
                            error={!!errors.username}
                            helperText={errors.username?.message}
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
                            type="password"
                            fullWidth
                            margin="normal"
                            error={!!errors.password}
                            helperText={errors.password?.message}
                            disabled={loading}
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
                    {loading ? 'Registrando...' : 'Registrar'}
                </Button>

                <Box sx={{ mt: 2, textAlign: 'center' }}>
                    <Typography variant="body2">
                        Já tem uma conta?{' '}
                        <RouterLink to="/login" style={{ textDecoration: 'none', color: '#1976d2' }}>
                            Faça login
                        </RouterLink>
                    </Typography>
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
