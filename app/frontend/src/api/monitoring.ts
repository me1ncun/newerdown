import { client, instance } from '../utils/axiosClient';
import type {
  Monitor,
  CreateMonitorRequest,
  UpdateMonitorRequest,
  DeleteMonitorRequest,
  PauseResumeRequest,
} from '../shared/types/Monitor';

export const getMonitors = () => client.get<Monitor[]>('/api/monitors');

export const getMonitorById = (id: string) => client.get<Monitor>(`/api/monitors/${id}`);

export const createMonitor = (data: CreateMonitorRequest) =>
  client.post<Monitor>('/api/monitors', data);

export const updateMonitor = (id: string, data: UpdateMonitorRequest) =>
  client.put<Monitor>(`/api/monitors/${id}`, data);

export const deleteMonitor = async (data: DeleteMonitorRequest) => {
  const response = await instance.delete('/api/monitors', { data });
  return response.data;
};

export const pauseMonitor = (data: PauseResumeRequest) => client.post('/api/monitors/pause', data);

export const resumeMonitor = (id: string, data: PauseResumeRequest) =>
  client.post(`/api/monitors/${id}/resume`, data);

export const importMonitors = async (file: File) => {
  const response = await client.postForm<void>('/api/monitors/import', file, 'file');
  return response;
};

export const exportMonitor = async (id: string) => {
  const response = await instance.get(`/api/monitors/${id}/export`, {
    responseType: 'blob',
  });

  const url = window.URL.createObjectURL(new Blob([response.data]));
  const link = document.createElement('a');
  link.href = url;
  link.setAttribute('download', `monitor_${id}.csv`);
  document.body.appendChild(link);
  link.click();
  link.remove();
  window.URL.revokeObjectURL(url);

  return response.data;
};
