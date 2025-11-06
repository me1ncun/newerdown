/* eslint-disable no-console */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import {
  getMonitors,
  createMonitor as createMonitorApi,
  deleteMonitor as deleteMonitorApi,
} from '../api/monitoring';
import type { Monitor, CreateMonitorRequest, MonitorsState } from '../shared/types/Monitor';

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
      });
  },
});

export const { clearError } = monitorsSlice.actions;
export default monitorsSlice.reducer;
