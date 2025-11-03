import type { UserInformation } from '../shared/types/User';
import { client } from '../utils/axiosClient';

export const getUserInformation = () => {
  return client.get<UserInformation>('/api/users/me');
};

export const setUserInformation = (data: UserInformation) => {
  return client.patch<UserInformation>('/api/users/me', data);
};

export const deleteUserAccount = () => {
  return client.delete('/api/users/me');
};

export const uploadUserAvatar = (file: File): Promise<UserInformation> => {
  return client.postForm<UserInformation>('/api/users/me/upload-photo', file, 'file');
};

export const deleteUserAvatar = () => {
  return client.delete('/api/users/me/delete-photo');
};
