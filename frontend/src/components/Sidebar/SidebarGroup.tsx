import { useState } from 'react';
import {
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Collapse,
  Tooltip,
} from '@mui/material';
import { ExpandLess, ExpandMore } from '@mui/icons-material';
import { NavLink, useLocation } from 'react-router-dom';
import SidebarItem from './SidebarItem';

interface SidebarGroupProps {
  group: {
    title: string;
    icon: React.ReactNode;
    items: { title: string; path: string; icon: React.ReactNode }[];
  };
  open: boolean;
}

export default function SidebarGroup({ group, open }: SidebarGroupProps) {
  const [expanded, setExpanded] = useState(false);
  const location = useLocation();

  const isGroup = group.items.length > 1;
  const groupActive = group.items.some(i => i.path === location.pathname);

  const handleToggle = (e: React.MouseEvent) => {
    if (isGroup) {
      e.preventDefault();
      setExpanded(!expanded);
    }
  };

  return (
    <>
      <Tooltip title={open ? '' : group.title} placement="right">
        <ListItemButton
          component={NavLink}
          to={group.items[0].path}
          onClick={handleToggle}
          selected={groupActive}
          sx={{
            ...(groupActive && {
              bgcolor: 'primary.light',
              color: 'primary.main',
              '& .MuiListItemIcon-root': { color: 'primary.main' },
            }),
          }}
        >
          <ListItemIcon sx={{ color: groupActive ? 'primary.main' : 'inherit' }}>
            {group.icon}
            {isGroup && !open && (expanded ? <ExpandLess /> : <ExpandMore />)}
          </ListItemIcon>
          {open && (
            <>
              <ListItemText primary={group.title} />
              {isGroup && (expanded ? <ExpandLess /> : <ExpandMore />)}
            </>
          )}
        </ListItemButton>
      </Tooltip>

      {isGroup && (
        <Collapse in={expanded} timeout="auto" unmountOnExit>
          {group.items.map(item => (
            <SidebarItem key={item.title} item={item} open={open} />
          ))}
        </Collapse>
      )}
    </>
  );
}
