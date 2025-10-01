import { NavLink, Outlet } from 'react-router-dom';
import classNames from 'classnames';
import { useAppDispatch, useAppSelector } from './shared/hooks/reduxHooks';
import { logout } from './features/authSlice';
import { useEffect } from 'react';
import './App.scss';

const getLinkActiveClass = ({ isActive }: { isActive: boolean }) =>
  classNames('topbar_main__link', {
    'topbar_main__link--active': isActive,
  });

export const App = () => {
  const dispatch = useAppDispatch();
  const { token } = useAppSelector((state) => state.auth);

  const handleLogout = () => {
    dispatch(logout());
  };

  useEffect(() => {
    const storedToken = localStorage.getItem('token');

    if (storedToken && !token) {
      dispatch({ type: 'auth/setToken', payload: storedToken });
    }
  }, [token, dispatch]);

  return (
    <div data-cy="app">
      <div className="topbar_main">
        <div className="container">
          <div className="topbar_main__content">
            <div className="topbar_main__auth">
              {token ? (
                <>
                  <NavLink className={getLinkActiveClass} to="/account">
                    Account
                  </NavLink>
                  <button className="topbar_main__logout" onClick={handleLogout}>
                    Logout
                  </button>
                </>
              ) : (
                <NavLink className={getLinkActiveClass} to="/login">
                  Login
                </NavLink>
              )}
            </div>

            <nav className="topbar_main__nav">
              <NavLink className={getLinkActiveClass} to="/">
                Home
              </NavLink>
              <NavLink className={getLinkActiveClass} to="/monitoring">
                Monitoring
              </NavLink>
            </nav>
          </div>
        </div>
      </div>

      <Outlet />
    </div>
  );
};
