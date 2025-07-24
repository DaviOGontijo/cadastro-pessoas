import { styled, CSSObject, Theme } from '@mui/material/styles';
import MuiDrawer from '@mui/material/Drawer';
import { Toolbar, List } from '@mui/material';
import SidebarGroup from './SidebarGroup';

const drawerWidth = 240;
const miniWidth = 72;

const openedMixin = (theme: Theme): CSSObject => ({
  width: drawerWidth,
  transition: theme.transitions.create('width', {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.enteringScreen,
  }),
  overflowX: 'hidden',
});

const closedMixin = (theme: Theme): CSSObject => ({
  width: miniWidth,
  transition: theme.transitions.create('width', {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  overflowX: 'hidden',
});

const StyledDrawer = styled(MuiDrawer, {
  shouldForwardProp: (prop) => prop !== 'open',
})<{ open: boolean }>(({ theme, open }) => ({
  width: open ? drawerWidth : miniWidth,
  flexShrink: 0,
  whiteSpace: 'nowrap',
  boxSizing: 'border-box',
  '& .MuiDrawer-paper': open ? openedMixin(theme) : closedMixin(theme),
}));

interface SidebarProps {
  open: boolean;
  navGroups: {
    title: string;
    icon: React.ReactNode;
    items: { title: string; path: string; icon: React.ReactNode }[];
  }[];
}

export default function Sidebar({ open, navGroups }: SidebarProps) {
  return (
    <StyledDrawer variant="permanent" open={open}>
      <Toolbar />
      <List>
        {navGroups.map((group) => (
          <SidebarGroup key={group.title} group={group} open={open} />
        ))}
      </List>
    </StyledDrawer>
  );
}
