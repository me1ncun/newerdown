/* eslint-disable no-console */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import {
  getMonitors,
  getMonitorById,
  createMonitor as createMonitorApi,
  updateMonitor as updateMonitorApi,
  deleteMonitor as deleteMonitorApi,
  pauseMonitor as pauseMonitorApi,
  resumeMonitor as resumeMonitorApi,
  importMonitors as importMonitorsApi,
} from '../api/monitoring';
import type {
  Monitor,
  CreateMonitorRequest,
  UpdateMonitorRequest,
  MonitorsState,
} from '../shared/types/Monitor';

const initialState: MonitorsState = {
  monitors: [],
  loading: false,
  error: null,
  hasMore: true,
};

export const fetchMonitors = createAsyncThunk(
  'monitors/fetchMonitors',
  async (_, { rejectWithValue }) => {
    try {
      const response: Monitor[] = await getMonitors();
      return response;
    } catch (error: any) {
      console.error('Error in fetchMonitors:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown error fetching monitors');
    }
  },
);

export const createMonitor = createAsyncThunk(
  'monitors/createMonitor',
  async (data: CreateMonitorRequest, { rejectWithValue }) => {
    try {
      const response: Monitor = await createMonitorApi(data);
      return response;
    } catch (error: any) {
      console.error('Error in createMonitor:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown error creating monitor');
    }
  },
);

export const deleteMonitor = createAsyncThunk(
  'monitors/deleteMonitor',
  async (id: string, { rejectWithValue }) => {
    try {
      await deleteMonitorApi({ id });
      return id;
    } catch (error: any) {
      console.error('Error in deleteMonitor:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown error deleting monitor');
    }
  },
);

export const fetchMonitorById = createAsyncThunk(
  'monitors/fetchMonitorById',
  async (id: string, { rejectWithValue }) => {
    try {
      const response: Monitor = await getMonitorById(id);
      return response;
    } catch (error: any) {
      console.error('Error in fetchMonitorById:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown error fetching monitor');
    }
  },
);

export const updateMonitor = createAsyncThunk(
  'monitors/updateMonitor',
  async ({ id, data }: { id: string; data: UpdateMonitorRequest }, { rejectWithValue }) => {
    try {
      const response: Monitor = await updateMonitorApi(id, data);
      return response;
    } catch (error: any) {
      console.error('Error in updateMonitor:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown error updating monitor');
    }
  },
);

export const pauseMonitor = createAsyncThunk(
  'monitors/pauseMonitor',
  async (id: string, { rejectWithValue }) => {
    try {
      await pauseMonitorApi({ id });
      return id;
    } catch (error: any) {
      console.error('Error in pauseMonitor:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown error pausing monitor');
    }
  },
);

export const resumeMonitor = createAsyncThunk(
  'monitors/resumeMonitor',
  async (id: string, { rejectWithValue }) => {
    try {
      await resumeMonitorApi(id, { id });
      return id;
    } catch (error: any) {
      console.error('Error in resumeMonitor:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown error resuming monitor');
    }
  },
);

export const importMonitors = createAsyncThunk(
  'monitors/importMonitors',
  async (file: File, { rejectWithValue, dispatch }) => {
    try {
      await importMonitorsApi(file);
      dispatch(fetchMonitors());
      return;
    } catch (error: any) {
      console.error('Error in importMonitors:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown error importing monitors');
    }
  },
);

export const monitorsSlice = createSlice({
  name: 'monitors',
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchMonitors.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchMonitors.fulfilled, (state, action) => {
        state.loading = false;
        state.monitors = action.payload;
        state.hasMore = false;
      })
      .addCase(fetchMonitors.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createMonitor.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createMonitor.fulfilled, (state, action) => {
        state.loading = false;
        state.monitors.unshift(action.payload);
      })
      .addCase(createMonitor.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteMonitor.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteMonitor.fulfilled, (state, action) => {
        state.loading = false;
        state.monitors = state.monitors.filter((monitor) => monitor.id !== action.payload);
      })
      .addCase(deleteMonitor.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateMonitor.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateMonitor.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.monitors.findIndex((m) => m.id === action.payload.id);
        if (index !== -1) {
          state.monitors[index] = action.payload;
        }
      })
      .addCase(updateMonitor.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(pauseMonitor.fulfilled, (state, action) => {
        const monitor = state.monitors.find((m) => m.id === action.payload);
        if (monitor) {
          monitor.isActive = false;
        }
      })
      .addCase(pauseMonitor.rejected, (state, action) => {
        state.error = action.payload as string;
      })
      .addCase(resumeMonitor.fulfilled, (state, action) => {
        const monitor = state.monitors.find((m) => m.id === action.payload);
        if (monitor) {
          monitor.isActive = true;
        }
      })
      .addCase(resumeMonitor.rejected, (state, action) => {
        state.error = action.payload as string;
      })
      .addCase(importMonitors.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(importMonitors.fulfilled, (state) => {
        state.loading = false;
      })
      .addCase(importMonitors.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const { clearError } = monitorsSlice.actions;
export default monitorsSlice.reducer;
