import { NavLink, Outlet } from 'react-router-dom';
import classNames from 'classnames';
import { useAppDispatch, useAppSelector } from './shared/hooks/reduxHooks';
import { logout } from './features/authSlice';
import { useEffect, Suspense } from 'react';
import { useTranslation } from 'react-i18next';
import './App.scss';

const getLinkActiveClass = ({ isActive }: { isActive: boolean }) =>
  classNames('topbar_main__link', {
    'topbar_main__link--active': isActive,
  });

export const App = () => {
  const dispatch = useAppDispatch();
  const { token } = useAppSelector((state) => state.auth);
  const { t, i18n } = useTranslation();

  const handleLogout = () => {
    dispatch(logout());
  };

  const toggleLanguage = () => {
    const newLang = i18n.language === 'en' ? 'ua' : 'en';
    i18n.changeLanguage(newLang);
  };

  useEffect(() => {
    const storedToken = localStorage.getItem('token');

    if (storedToken && !token) {
      dispatch({ type: 'auth/setToken', payload: storedToken });
    }
  }, [token, dispatch]);

  return (
    <Suspense fallback="Loading...">
      <div data-cy="app">
        <div className="topbar_main">
          <div className="container">
            <div className="topbar_main__content">
              <div className="topbar_main__auth">
                {token ? (
                  <>
                    <NavLink className={getLinkActiveClass} to="/account">
                      {t('main.account')}
                    </NavLink>
                    <button className="topbar_main__logout" onClick={handleLogout}>
                      {t('main.logout', 'Logout')}
                    </button>
                  </>
                ) : (
                  <NavLink className={getLinkActiveClass} to="/login">
                    {t('main.login', 'Login')}
                  </NavLink>
                )}

                <button className="topbar_main__lang" onClick={toggleLanguage}>
                  {i18n.language === 'en' ? 'UA' : 'EN'}
                </button>
              </div>

              <nav className="topbar_main__nav">
                <NavLink className={getLinkActiveClass} to="/">
                  {t('main.home', 'Home')}
                </NavLink>
                <NavLink className={getLinkActiveClass} to="/monitoring">
                  {t('main.monitoring', 'Monitoring')}
                </NavLink>
              </nav>
            </div>
          </div>
        </div>

        <Outlet />
      </div>
    </Suspense>
  );
};
