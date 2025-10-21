import axios from 'axios';

const apiClient = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_BASE_URL,
  timeout: 10000
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error('Erro na chamada à API NutriScan', error);
    return Promise.reject(error);
  }
);

export default apiClient;
