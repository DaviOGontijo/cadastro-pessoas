import React, { useState } from 'react';
import { Avatar, Box, Divider, IconButton, Menu, MenuItem, Typography, Tooltip } from '@mui/material';
import { Person, LogoutOutlined } from '@mui/icons-material';

interface UserMenuProps {
  username: string;
  onLogout: () => void;
}

export function UserMenu({ username, onLogout }: UserMenuProps) {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  const handleMenuOpen = (e: React.MouseEvent<HTMLElement>) => setAnchorEl(e.currentTarget);
  const handleMenuClose = () => setAnchorEl(null);
  const handleLogout = () => {
    handleMenuClose();
    onLogout();
  };

  return (
    <>
      <Typography variant="subtitle1" sx={{ color: 'white', mr: 1 }} noWrap>{username}</Typography>
      <Tooltip title="Configurações da Conta">
        <IconButton onClick={handleMenuOpen} size="small">
          <Avatar sx={{ bgcolor: 'primary.main' }}>
            <Person />
          </Avatar>
        </IconButton>
      </Tooltip>

      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleMenuClose}
        onClick={handleMenuClose}
        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
      >
        <MenuItem disabled>
          <Avatar sx={{ bgcolor: 'secondary.main', mr: 1 }}>
            <Person />
          </Avatar>
          <Box>
            <Typography variant="subtitle1" noWrap>{username}</Typography>
          </Box>
        </MenuItem>
        <Divider />
        <MenuItem onClick={handleLogout} sx={{ gap: 1 }}>
          <LogoutOutlined fontSize="small" />
          Logout
        </MenuItem>
      </Menu>
    </>
  );
}
