import type { UserInformation, UserResponse } from '../shared/types/User';
import { client } from '../utils/axiosClient';

export const getUserInformation = () => {
  return client.get<UserResponse>('/api/users/me');
};

export const setUserInformation = (data: UserInformation) => {
  return client.patch<UserResponse>('/api/users/me', data);
};

export const deleteUserInformation = () => {
  return client.delete('/api/users/me');
};

// export const uploadUserAvatar = () => {
//   return client.post<UserResponse>('/api/users/me/upload-photo');
// };

// export const deleteUserAvatar = () => {
//   return client.delete('/api/users/me/upload-photo');
// };
