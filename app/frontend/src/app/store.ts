import { configureStore, type ThunkAction, type Action } from '@reduxjs/toolkit';
import authReducer from '../features/authSlice';
import userAccountReducer from '../features/userAccountSlice';
import monitorsReducer from '../features/monitorsSlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    userAccount: userAccountReducer,
    monitors: monitorsReducer,
  },
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;

export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;
