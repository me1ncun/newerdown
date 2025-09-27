import type {
  AuthResponse,
  Login,
  SingUp,
  ChangePassword,
  // RefreshToken,
} from '../shared/types/Auth';
import { client } from '../utils/axiosClient';

export const login = (data: Login) => {
  return client.post<AuthResponse>('/login', data);
};

export const singUp = (data: SingUp) => {
  return client.post<AuthResponse>('/singup', data);
};

export const changePassword = (data: ChangePassword) => {
  return client.post<AuthResponse>('/change-password', data);
};

// export const refreshToken = (data: RefreshToken) => {
//   return client.post<AuthResponse>('/token/refresh', data);
// };

// export const logout = () => {
//   return client.post('/logout');
// };
