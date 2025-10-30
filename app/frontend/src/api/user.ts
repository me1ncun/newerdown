import type { UserInformation } from '../shared/types/User';
import { client } from '../utils/axiosClient';

export const getUserInformation = () => {
  return client.get<UserInformation>('/api/users/me');
};

export const setUserInformation = (data: UserInformation) => {
  return client.patch<UserInformation>('/api/users/me', data);
};

export const deleteUserInformation = () => {
  return client.delete('/api/users/me');
};

// export const uploadUserAvatar = () => {
//   return client.post<UserInformation>('/api/users/me/upload-photo');
// };

// export const deleteUserAvatar = () => {
//   return client.delete('/api/users/me/upload-photo');
// };
