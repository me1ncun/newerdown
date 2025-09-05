/* eslint-disable @typescript-eslint/no-explicit-any */
import axios, { type AxiosRequestConfig, type Method } from 'axios';

const BASE_URL = 'https://app-newerdown.azurewebsites.net';

function wait(delay: number) {
  return new Promise((resolve) => setTimeout(resolve, delay));
}

const instance = axios.create({
  baseURL: BASE_URL,
  headers: {
    'Content-Type': 'application/json; charset=UTF-8',
  },
});

instance.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token && config.headers) {
    config.headers['Authorization'] = `Bearer ${token}`;
  }
  return config;
});

async function request<T>(url: string, method: Method = 'GET', data: any = null): Promise<T> {
  await wait(300);

  const config: AxiosRequestConfig = {
    url,
    method,
    data,
  };

  try {
    const response = await instance.request<T>(config);
    return response.data;
  } catch (error: any) {
    if (error.response) {
      console.error('Request error:', error.response.data);
      throw new Error(JSON.stringify(error.response.data));
    } else if (error.request) {
      throw new Error('No response from server');
    } else {
      throw new Error(error.message);
    }
  }
}

export const client = {
  get: <T>(url: string) => request<T>(url, 'GET'),
  post: <T>(url: string, data?: any) => request<T>(url, 'POST', data),
  patch: <T>(url: string, data: any) => request<T>(url, 'PATCH', data),
  put: <T>(url: string, data?: any) => request<T>(url, 'PUT', data),
  delete: (url: string) => request(url, 'DELETE'),
};
