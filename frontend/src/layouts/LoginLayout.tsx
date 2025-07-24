import React from 'react';
import { Outlet } from 'react-router-dom';
import { Box } from '@mui/material';

export default function LoginLayout() {
  return (
    <Box
      sx={{
        height: '100vh',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        p: 2,
      }}
    >
      <Outlet />
    </Box>
  );
}
