/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { login, singUp, refreshToken, changePassword } from '../api/auth';
import type { Login, SingUp, User, RefreshToken, ChangePassword } from '../shared/types/Auth';

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
      return rejectWithValue('Error network or server');
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
      console.error('Error in singUpUser:', error.message);

      try {
        const parsedError = JSON.parse(error.message);
        if (Array.isArray(parsedError)) {
          const errorMessages = parsedError.map((err) => err.description).join('\n');
          return rejectWithValue(errorMessages);
        }
      } catch {
        return rejectWithValue('singUp1 error');
      }

      return rejectWithValue('singUp2 error');
    }
  },
);

export const logoutUser = createAsyncThunk('auth/logoutUser', async () => {
  try {
    localStorage.removeItem('token');
    localStorage.removeItem('user');

    return null;
  } catch (error: any) {
    console.log('Error in logoutUser:', error.message);
  }
});

export const changePasswordUser = createAsyncThunk(
  'auth/changePasswordUser',
  async (passwordData: ChangePassword, { rejectWithValue }) => {
    try {
      const response = await changePassword(passwordData);
      return response;
    } catch (error: any) {
      console.error('Error in changePasswordUser:', error.message);

      try {
        const parsedError = JSON.parse(error.message);
        if (Array.isArray(parsedError)) {
          const errorMessages = parsedError.map((err) => err.description).join('\n');
          return rejectWithValue(errorMessages);
        }
      } catch {
        return rejectWithValue('changePassword1 error');
      }

      return rejectWithValue('changePassword2 error');
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

        if (action.payload.token) {
          state.token = action.payload.token;
          if (action.payload.user) {
            state.user = action.payload.user;
          }
        }
      })
      .addCase(singUpUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(logoutUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(logoutUser.fulfilled, (state) => {
        state.loading = false;
        state.token = null;
        state.user = null;
      })
      .addCase(logoutUser.rejected, (state, action) => {
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
      });
  },
});

export const { clearError } = authSlice.actions;
export default authSlice.reducer;
