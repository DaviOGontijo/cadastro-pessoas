import React, { useState } from 'react';
import {
  AppBar,
  Toolbar,
  IconButton,
  Typography,
  Menu,
  MenuItem,
  Avatar,
  Box,
  Tooltip,
  Divider,
  Button,
} from '@mui/material';
import {
  Menu as MenuIcon,
  ChevronLeft,
  Person,
  ShieldOutlined,
  LogoutOutlined,
} from '@mui/icons-material';

interface TopBarProps {
  open: boolean;
  onToggle: () => void;
  title?: string;
  onLogout: () => void;
}

export default function TopBar({
  open,
  onToggle,
  title = 'Gestão de Clientes',
  onLogout,
}: TopBarProps) {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    handleMenuClose();
    onLogout();
  };

  return (
    <AppBar position="fixed" sx={{ zIndex: theme => theme.zIndex.drawer + 1 }}>
      <Toolbar>
        <IconButton
          color="inherit"
          edge="start"
          onClick={onToggle}
          aria-label={open ? 'Close menu' : 'Open menu'}
          sx={{ mr: 2 }}
          size="large"
        >
          {open ? <ChevronLeft /> : <MenuIcon />}
        </IconButton>

        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            flexGrow: 1,
            gap: 1,
          }}
        >
          <ShieldOutlined fontSize="medium" />
          <Typography variant="h6" noWrap component="div">
            {title}
          </Typography>
        </Box>

        <Tooltip title="Configurações da Conta">
          <IconButton
            disableRipple
            onClick={handleMenuOpen}
            size="small"
            aria-controls={anchorEl ? 'account-menu' : undefined}
            aria-haspopup="true"
            aria-expanded={anchorEl ? 'true' : undefined}
          >
            <Typography
              variant="body1"
              sx={{
                mr: 1,
                fontWeight: 500,
                whiteSpace: 'nowrap',
                color: 'white',
              }}
            >
            </Typography>
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
          <MenuItem disabled sx={{ opacity: 1, pointerEvents: 'none' }}>
            <Avatar sx={{ bgcolor: 'secondary.main', mr: 1 }}>
              <Person />
            </Avatar>
            <Box>
              <Typography variant="subtitle1" noWrap>
              </Typography>
              <Typography variant="body2" color="text.secondary" noWrap>
              </Typography>
            </Box>
          </MenuItem>
          <Divider />
          <MenuItem onClick={handleLogout} sx={{ gap: 1 }}>
            <LogoutOutlined fontSize="small" />
            Logout
          </MenuItem>
        </Menu>
      </Toolbar>
    </AppBar>
  );
}
