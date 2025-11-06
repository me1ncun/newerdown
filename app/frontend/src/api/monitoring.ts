import { client, instance } from '../utils/axiosClient';
import type { Monitor, CreateMonitorRequest, DeleteMonitorRequest } from '../shared/types/Monitor';

export const getMonitors = () => client.get<Monitor[]>('/api/monitors');

export const createMonitor = (data: CreateMonitorRequest) =>
  client.post<Monitor>('/api/monitors', data);

export const deleteMonitor = async (data: DeleteMonitorRequest) => {
  const response = await instance.delete('/api/monitors', { data });
  return response.data;
};
