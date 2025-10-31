/* eslint-disable no-console */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { getUserInformation, deleteUserAccount, setUserInformation } from '../api/user';
import type { UserInformation } from '../shared/types/User';

interface UserState {
  token: string | null;
  loading: boolean;
  error: string | null;
  user: UserInformation | null;
}

const storedToken = localStorage.getItem('token') || null;

const initialState: UserState = {
  token: storedToken,
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

export const updateUserInformation = createAsyncThunk(
  'user/updateUserInformation',
  async (data: UserInformation, { rejectWithValue }) => {
    try {
      const response: UserInformation = await setUserInformation(data);
      return response;
    } catch (error: any) {
      console.error('Error in updateUserInformation:', error);

      if (error.response?.data?.error?.description) {
        return rejectWithValue(error.response.data.error.description);
      }

      return rejectWithValue(error.message || 'Unknown update user information');
    }
  },
);

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
        state.token = null;
        state.user = null;
        localStorage.removeItem('token');
      })
      .addCase(deleteUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateUserInformation.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateUserInformation.fulfilled, (state, action) => {
        state.loading = false;
        state.user = action.payload;
      })
      .addCase(updateUserInformation.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export default authSlice.reducer;
