/* eslint-disable no-console */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { login, singUp, refreshToken, changePassword } from '../api/auth';
import type { Login, SingUp, User, ChangePassword, AuthResponse } from '../shared/types/Auth';

interface AuthState {
  token: string | null;
  user: User | null;
  loading: boolean;
  error: string | null;
}

const storedToken = localStorage.getItem('token') || null;
const storedUser = localStorage.getItem('user');

const initialState: AuthState = {
  token: storedToken,
  user: storedUser ? JSON.parse(storedUser) : null,
  loading: false,
  error: null,
};

export const refreshAccessToken = createAsyncThunk(
  'auth/refreshAccessToken',
  async (_, { rejectWithValue }) => {
    try {
      const response: AuthResponse = await refreshToken();

      if (response?.accessToken) {
        localStorage.setItem('token', response.accessToken);
        return { token: response.accessToken };
      }

      return rejectWithValue('No token returned');
    } catch (error: any) {
      console.error('Error in refreshAccessToken:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      localStorage.removeItem('token');
      localStorage.removeItem('user');
      return rejectWithValue(error.message || 'Unknown refresh token error');
    }
  },
);

export const loginUser = createAsyncThunk(
  'auth/loginUser',
  async (credentials: Login, { rejectWithValue }) => {
    try {
      const response: any = await login(credentials);

      if (
        typeof response === 'string' &&
        response.includes('.') &&
        response.split('.').length === 3
      ) {
        return { token: response };
      } else if (
        typeof response === 'object' &&
        response.token &&
        typeof response.token === 'string' &&
        response.token.includes('.') &&
        response.token.split('.').length === 3
      ) {
        return response;
      } else {
        return rejectWithValue(response || 'Authentication error');
      }
    } catch (error: any) {
      console.error('Error in loginUser:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown login error');
    }
  },
);

export const singUpUser = createAsyncThunk(
  'auth/singUpUser',
  async (userData: SingUp, { rejectWithValue }) => {
    try {
      const response = await singUp(userData);
      return response;
    } catch (error: any) {
      console.error('Error in singUpUser:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown signup error');
    }
  },
);

export const changePasswordUser = createAsyncThunk(
  'auth/changePasswordUser',
  async (passwordData: ChangePassword, { rejectWithValue }) => {
    try {
      const response = await changePassword(passwordData);
      return response;
    } catch (error: any) {
      console.error('Error in changePasswordUser:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown change password error');
    }
  },
);

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
    logout: (state) => {
      state.token = null;
      state.user = null;
      localStorage.removeItem('token');
      localStorage.removeItem('user');
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(loginUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(loginUser.fulfilled, (state, action) => {
        state.loading = false;
        state.token = action.payload.token;

        if (action.payload.user) {
          state.user = action.payload.user;
          localStorage.setItem('user', JSON.stringify(action.payload.user));
        }
        localStorage.setItem('token', action.payload.token);
      })
      .addCase(loginUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(singUpUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(singUpUser.fulfilled, (state, action) => {
        state.loading = false;
        if (action.payload.accessToken) {
          state.token = action.payload.accessToken;
          localStorage.setItem('token', action.payload.accessToken);
          if (action.payload.user) {
            state.user = action.payload.user;
            localStorage.setItem('user', JSON.stringify(action.payload.user));
          }
        }
      })
      .addCase(singUpUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(changePasswordUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(changePasswordUser.fulfilled, (state) => {
        state.loading = false;
        state.token = null;
        state.user = null;
        localStorage.removeItem('token');
        localStorage.removeItem('user');
      })
      .addCase(changePasswordUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(refreshAccessToken.pending, (state) => {
        state.loading = true;
      })
      .addCase(refreshAccessToken.fulfilled, (state, action) => {
        state.loading = false;
        state.token = action.payload.token;
      })
      .addCase(refreshAccessToken.rejected, (state, action) => {
        state.loading = false;
        state.token = null;
        state.user = null;
        state.error = action.payload as string;
      });
  },
});

export const { clearError, logout } = authSlice.actions;
export default authSlice.reducer;
