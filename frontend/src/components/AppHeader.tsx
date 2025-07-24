import React from 'react';
import { AppBar, Box, Toolbar, Typography } from '@mui/material';
import { UserMenu } from './UserMenu';

interface AppHeaderProps {
  onToggle: () => void;
  title?: string;
  onLogout: () => void;
  username: string;
}

export default function AppHeader({
  title = 'Gestão de Clientes',
  onLogout,
  username,
}: AppHeaderProps) {
  return (
    <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
      <Toolbar>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, flexGrow: 1 }}>
          <Typography variant="h6" noWrap component="div">{title}</Typography>
        </Box>

        <UserMenu username={username} onLogout={onLogout} />
      </Toolbar>
    </AppBar>
  );
}
