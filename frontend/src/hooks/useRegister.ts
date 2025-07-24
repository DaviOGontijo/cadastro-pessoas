import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

type RegisterData = {
  username: string;
  password: string;
};

export function useRegister() {
  const navigate = useNavigate();
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);

  const register = async (data: RegisterData) => {
    setError('');
    setSuccess('');
    setLoading(true);

    try {
      const response = await fetch('https://cadastropessoasapi.azurewebsites.net/api/Auth/register', {
      // const response = await fetch('https://localhost:5001/api/Auth/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
      });

      if (!response.ok) {
        const errorData = await response.text();
        throw new Error(errorData || 'Erro ao registrar');
      }

      setSuccess('Usuário registrado com sucesso!');
      setTimeout(() => navigate('/login'), 1500);
    } catch (e: any) {
      setError(e.message || 'Erro ao registrar');
    } finally {
      setLoading(false);
    }
  };

  return { register, error, success, loading };
}
