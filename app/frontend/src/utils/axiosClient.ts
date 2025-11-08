/* eslint-disable no-console */
/* eslint-disable @typescript-eslint/no-explicit-any */
import axios, { type AxiosRequestConfig, type Method } from 'axios';
import { refreshToken } from '../api/auth';
import type { AuthResponse } from '../shared/types/Auth';

const BASE_URL = 'https://app-newerdown.azurewebsites.net';

function wait(delay: number) {
  return new Promise((resolve) => setTimeout(resolve, delay));
}

export const instance = axios.create({
  baseURL: BASE_URL,
  headers: {
    'Content-Type': 'application/json; charset=UTF-8',
  },
  withCredentials: true,
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

function subscribeTokenRefresh(cb: (token: string) => void) {
  refreshSubscribers.push(cb);
}

function onRefreshed(newToken: string) {
  refreshSubscribers.forEach((cb) => cb(newToken));
  refreshSubscribers = [];
}

instance.interceptors.response.use(
  (res) => res,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && error.config && !originalRequest._isRetry) {
      originalRequest._isRetry = true;

      if (isRefreshing) {
        return new Promise((resolve) => {
          subscribeTokenRefresh((newToken) => {
            originalRequest.headers['Authorization'] = `Bearer ${newToken}`;
            resolve(instance(originalRequest));
          });
        });
      }

      isRefreshing = true;

      try {
        console.log('Attempting to refresh token...');
        const response: AuthResponse = await refreshToken();
        console.log('REFRESH RESPONSE:', response);

        const newToken = response.accessToken;
        localStorage.setItem('token', newToken);
        console.log('TOKEN UPDATED IN LOCAL STORAGE:', newToken);

        onRefreshed(newToken);
        isRefreshing = false;

        originalRequest.headers['Authorization'] = `Bearer ${newToken}`;
        return instance(originalRequest);
      } catch (err: unknown) {
        isRefreshing = false;

        if (axios.isAxiosError(err)) {
          const detail = err.response?.data?.detail;
          console.error('Refresh token error:', detail);

          if (detail === 'Invalid refresh token. Please login again.') {
            localStorage.removeItem('token');
          }
        } else {
          console.error('Unexpected error:', err);
        }

        throw err;
      }
    }

    if (error.config?.url?.includes('/token/refresh') && error.response?.status === 400) {
      console.error('Refresh token invalid. Logging out.');
      localStorage.removeItem('token');
    }

    throw error;
  },
);

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

async function requestForm<T>(url: string, file: File, fieldName: string = 'file'): Promise<T> {
  await wait(300);

  const formData = new FormData();
  formData.append(fieldName, file);

  try {
    const response = await instance.post<T>(url, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  } catch (error: any) {
    if (error.response) {
      console.error('Request form error:', error.response.data);
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

  postForm: <T>(url: string, file: File, fieldName?: string) =>
    requestForm<T>(url, file, fieldName),
};
