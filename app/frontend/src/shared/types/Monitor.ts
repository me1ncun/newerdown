export enum MonitorType {
  HTTP = 0,
  HTTPS = 1,
  TCP = 2,
  PING = 3,
}

export interface Monitor {
  id: string;
  name: string;
  url: string;
  checkIntervalSeconds: number;
  isActive: boolean;
  type: MonitorType;
  createdAt: string;
  userId: string;
}

export interface CreateMonitorRequest {
  name: string;
  target: string;
  type: MonitorType;
  port: number;
  intervalSeconds: number;
  isActive: boolean;
}

export interface DeleteMonitorRequest {
  id: string;
}

export interface MonitorsState {
  monitors: Monitor[];
  loading: boolean;
  error: string | null;
  hasMore: boolean;
}
