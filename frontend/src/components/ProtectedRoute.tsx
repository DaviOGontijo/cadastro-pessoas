import { JSX } from 'react';
import { Navigate } from 'react-router-dom';

function getUser() {
  const raw = localStorage.getItem('user');
  if (!raw) return null;
  try {
    return JSON.parse(raw);
  } catch {
    return null;
  }
}

const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
  const user = getUser();

  if (!user || !user.token) {
    return <Navigate to="/login" replace />;
  }

  return children;
};

export default ProtectedRoute;
