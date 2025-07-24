import React, { useState } from 'react';
import { Box, CssBaseline, Toolbar } from '@mui/material';
import { useNavigate, Outlet } from 'react-router-dom';
import AppHeader from '../components/AppHeader';

export default function MainLayout() {
  const [open, setOpen] = useState(true);
  const navigate = useNavigate();

  const storedUser = localStorage.getItem('user');
  const username = storedUser ? JSON.parse(storedUser).username : 'Usuário';

  const handleLogout = () => {
    localStorage.removeItem('user');
    navigate('/login');
  };

  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />
      <AppHeader
        onToggle={() => setOpen(o => !o)}
        onLogout={handleLogout}
        username={username} 
      />
      <Box component="main" sx={{ flexGrow: 1, p: 3, height: '100vh', overflow: 'auto' }}>
        <Toolbar />
        <Outlet />
      </Box>
    </Box>
  );
}
