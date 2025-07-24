import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

type LoginData = { login: string; password: string; remember: boolean };

export function useLogin() {
  const navigate = useNavigate();
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const login = async (data: LoginData) => {
    setError('');
    setLoading(true);
    try {
      // const response = await fetch('https://cadastropessoasapi.azurewebsites.net/api/Auth/login', {
      const response = await fetch('https://localhost:5001/api/Auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username: data.login, password: data.password }),
      });

      if (!response.ok) {
        throw new Error('Usuário ou senha inválidos');
      }

      const result = await response.json();

      localStorage.setItem('user', JSON.stringify({
        token: result.token,
        username: data.login,
      }));

      navigate('/');
    } catch (e: any) {
      setError(e.message || 'Erro ao fazer login');
    } finally {
      setLoading(false);
    }
  };

  return { login, error, loading };
}
