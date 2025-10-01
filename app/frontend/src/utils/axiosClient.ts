/* eslint-disable @typescript-eslint/no-explicit-any */
import axios, { type AxiosRequestConfig, type Method } from 'axios';
import { refreshToken } from '../api/auth';
import type { AuthResponse } from '../shared/types/Auth';

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

let isRefreshing = false;
let refreshSubscribers: ((token: string) => void)[] = [];

function onRefreshed(token: string) {
  refreshSubscribers.forEach((cb) => cb(token));
  refreshSubscribers = [];
}

instance.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (
      error.response?.status === 401 &&
      !originalRequest._retry &&
      !originalRequest.url.includes('/token/refresh')
    ) {
      if (isRefreshing) {
        return new Promise((resolve) => {
          refreshSubscribers.push((token: string) => {
            originalRequest.headers['Authorization'] = `Bearer ${token}`;
            resolve(instance(originalRequest));
          });
        });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const response: AuthResponse = await refreshToken();

        const newToken = response.token;
        if (newToken) {
          localStorage.setItem('token', newToken);
          instance.defaults.headers.common['Authorization'] = `Bearer ${newToken}`;
          onRefreshed(newToken);
        }

        return instance(originalRequest);
      } catch (refreshError) {
        console.error('update token error', refreshError);
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        window.location.href = '/login';
        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

async function request<T>(url: string, method: Method = 'GET', data: any = null): Promise<T> {
  await wait(300);

  const config: AxiosRequestConfig = {
    url,
    method,
    data,
  };

  const response = await instance.request<T>(config);
  return response.data;
}

export const client = {
  get: <T>(url: string) => request<T>(url, 'GET'),
  post: <T>(url: string, data?: any) => request<T>(url, 'POST', data),
  patch: <T>(url: string, data: any) => request<T>(url, 'PATCH', data),
  put: <T>(url: string, data?: any) => request<T>(url, 'PUT', data),
  delete: (url: string) => request(url, 'DELETE'),
};

