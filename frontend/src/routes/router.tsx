import { createBrowserRouter} from 'react-router-dom';
import Home from '../pages/home/Home';
import MainLayout from '../layouts/MainLayout';
import LoginPage from '../pages/login/Login';
import RegisterPage from '../pages/register/register';
import LoginLayout from '../layouts/LoginLayout';
import ProtectedRoute from '../components/ProtectedRoute';

const router = createBrowserRouter([
  {
    element: <LoginLayout />,
    children: [
      {
        children: [
          { path: 'login', element: <LoginPage /> },
          { path: 'register', element: <RegisterPage /> },
        ],
      },
    ],
  },
  {
    element: <MainLayout />,
    children: [
      {
        path: '/',
        element: (
          <ProtectedRoute>
            <Home />
          </ProtectedRoute>
        ),
      },
    ],
  },
]);

export default router;
