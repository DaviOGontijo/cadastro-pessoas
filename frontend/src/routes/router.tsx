// src/routes/router.tsx
import { createBrowserRouter, Navigate } from 'react-router-dom';
import App from '../App';
import Home from '../pages/home/Home';
import MainLayout from '../layouts/MainLayout';
import LoginPage from '../pages/login/Login';
import LoginLayout from '../layouts/LoginLayout';

function getUser() {
  const raw = localStorage.getItem('user');
  if (!raw) return null;
  try {
    return JSON.parse(raw);
  } catch {
    return null;
  }
}

function isAuthenticated() {
  const user = getUser();
  return !!user?.token;
}

const router = createBrowserRouter([
  {
    element: <LoginLayout />,
    children: [
      {
        children: [
          { path: 'login', element: <LoginPage /> },
        ],
      },
    ],
  },
  {
    element: (
        <MainLayout />
    ),
    children: [
      {
        children: [
          { path: '/', element: <Home /> },
        ],
      },
    ],
  },
]);

export default router;
