import { ListItemButton, ListItemIcon, ListItemText, Tooltip } from '@mui/material';
import { NavLink, useLocation } from 'react-router-dom';

interface SidebarItemProps {
  item: {
    title: string;
    path: string;
    icon: React.ReactNode;
  };
  open: boolean;
}

export default function SidebarItem({ item, open }: SidebarItemProps) {
  const location = useLocation();
  const selected = location.pathname === item.path;

  return (
    <ListItemButton
      component={NavLink}
      to={item.path}
      selected={selected}
      sx={{
        justifyContent: open ? 'initial' : 'center',
        px: 2,
        ...(selected && {
          bgcolor: 'primary.light',
          color: 'primary.main',
          '& .MuiListItemIcon-root': {
            color: 'primary.main',
          },
        }),
      }}
    >
      <Tooltip title={open ? '' : item.title} placement="right">
        <ListItemIcon
          sx={{
            minWidth: 0,
            mr: open ? 3 : 'auto',
            justifyContent: 'center',
          }}
        >
          {item.icon}
        </ListItemIcon>
      </Tooltip>
      {open && <ListItemText primary={item.title} />}
    </ListItemButton>
  );
}
