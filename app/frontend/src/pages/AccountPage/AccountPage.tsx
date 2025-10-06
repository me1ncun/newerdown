import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../shared/hooks/reduxHooks';
import { getInformation } from '../../features/userAccountSlice';
import { refreshAccessToken } from '../../features/authSlice';
import { Loader } from '../Loader';

export const AccountPage = () => {
  const dispatch = useAppDispatch();
  const { user, loading, error } = useAppSelector((state) => state.userAccount);

  useEffect(() => {
    dispatch(getInformation());
  }, [dispatch]);

  return (
    <main className="section">
      <div className="container">
        <button onClick={() => dispatch(refreshAccessToken())}>bt</button>
        <h1 className="title">Account Page</h1>
        {loading && <Loader />}
        {error && <p style={{ color: 'red' }}>{error}</p>}
        {user && (
          <div>
            <p>
              <strong>name:</strong> {user.userName || <span style={{ color: '#aaa' }}>...</span>}
            </p>
            <p>
              <strong>Email:</strong> {user.email || <span style={{ color: '#aaa' }}>.</span>}
            </p>
            <p>
              <strong>org:</strong>{' '}
              {user.organizationName || <span style={{ color: '#aaa' }}>...</span>}
            </p>
            <p>
              <strong>utc:</strong> {user.timeZone || <span style={{ color: '#aaa' }}>.</span>}
            </p>
            <p>
              <strong>lang:</strong> {user.language || <span style={{ color: '#aaa' }}>...</span>}
            </p>
            <p>
              <strong>visib name:</strong>{' '}
              {user.displayName || <span style={{ color: '#aaa' }}>...</span>}
            </p>
            <p>
              <strong>tell:</strong>{' '}
              {user.phoneNumber || <span style={{ color: '#aaa' }}>...</span>}
            </p>
          </div>
        )}
      </div>
    </main>
  );
};
