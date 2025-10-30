import type { AuthResponse, Login, SingUp, ChangePassword } from '../shared/types/Auth';
import { client } from '../utils/axiosClient';

export const login = (data: Login) => {
  return client.post<AuthResponse>('/api/auth/login', data);
};

export const singUp = (data: SingUp) => {
  return client.post<AuthResponse>('/api/auth/signup', data);
};

export const changePassword = (data: ChangePassword) => {
  return client.post<AuthResponse>('/api/auth/change-password', data);
};

export const refreshToken = () => {
  return client.post<AuthResponse>('/api/auth/token/refresh');
};
