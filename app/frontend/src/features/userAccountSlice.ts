/* eslint-disable no-console */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { getUserInformation, deleteUserAccount } from '../api/user';
import type { UserInformation } from '../shared/types/User';

interface AuthState {
  loading: boolean;
  error: string | null;
  user: UserInformation | null;
}

const initialState: AuthState = {
  loading: false,
  error: null,
  user: null,
};

export const getInformation = createAsyncThunk(
  'user/getInformation',
  async (_, { rejectWithValue }) => {
    try {
      const response: UserInformation = await getUserInformation();
      return response;
    } catch (error: any) {
      console.error('Error in getUserInformation:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown get user information');
    }
  },
);

export const deleteUser = createAsyncThunk('user/deleteUser', async (_, { rejectWithValue }) => {
  try {
    await deleteUserAccount();
    return;
  } catch (error: any) {
    console.error('Error in deleteUser:', error);

    if (error.response?.data?.error?.description) {
      return rejectWithValue(error.response.data.error.description);
    }

    return rejectWithValue(error.message || 'Unknown delete user acc');
  }
});

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(getInformation.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(getInformation.fulfilled, (state, action) => {
        state.loading = false;
        state.user = action.payload ?? null;
      })
      .addCase(getInformation.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteUser.fulfilled, (state) => {
        state.loading = false;
        state.user = null;
        localStorage.removeItem('token');
        localStorage.removeItem('user');
      })
      .addCase(deleteUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export default authSlice.reducer;
