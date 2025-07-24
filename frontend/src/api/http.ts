import axios from 'axios';

const http = axios.create({
   baseURL: 'https://localhost:5001/api/v2',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptador para incluir token JWT nas requisições
http.interceptors.request.use(
  (config) => {
    const userRaw = localStorage.getItem('user');
    if (userRaw) {
      const user = JSON.parse(userRaw);
      const token = user.token;
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);


export default http;
