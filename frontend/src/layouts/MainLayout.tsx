// src/layouts/MainLayout.tsx
import React, { useState } from 'react';
import { Box, CssBaseline, Toolbar } from '@mui/material';
import { useNavigate, Outlet } from 'react-router-dom';
import TopBar from '../components/TopBar';
import Sidebar from '../components/Sidebar/SideBar';
import { PeopleOutline } from '@mui/icons-material';

const navGroups = [
  {
    title: 'Usuários',
    icon: <PeopleOutline />,
    items: [{ title: 'Gerenciar Usuários', path: '/', icon: <PeopleOutline /> }],
  },
];

export default function MainLayout() {
  const [open, setOpen] = useState(true);
  const navigate = useNavigate();

  const handleLogout = () => {
    navigate("/login");
  };

  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />
      <TopBar open={open} onToggle={() => setOpen(o => !o)} onLogout={handleLogout} />
      <Sidebar open={open} navGroups={navGroups} />
      <Box component="main" sx={{ flexGrow: 1, p: 3, height: '100vh', overflow: 'auto' }}>
        <Toolbar />
        <Outlet />
      </Box>
    </Box>
  );
}
