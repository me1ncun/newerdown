import { NavLink, Outlet } from 'react-router-dom';
import classNames from 'classnames';
import { useAppDispatch, useAppSelector } from './shared/hooks/reduxHooks';
import { logout } from './features/authSlice';
import { useEffect, Suspense } from 'react';
import { useTranslation } from 'react-i18next';
import { Languages, LogOut } from 'lucide-react';
import styles from './App.module.scss';
import { Loader } from './pages/Loader';

const getLinkActiveClass = ({ isActive }: { isActive: boolean }) =>
  classNames(styles.navLink, {
    [styles.active]: isActive,
  });

export const App = () => {
  const dispatch = useAppDispatch();
  const { token } = useAppSelector((state) => state.auth);
  const { t, i18n } = useTranslation();

  const handleLogout = () => {
    dispatch(logout());
  };

  const toggleLanguage = () => {
    const newLang = i18n.language === 'en' ? 'uk' : 'en';
    i18n.changeLanguage(newLang);
  };

  useEffect(() => {
    const storedToken = localStorage.getItem('token');
    if (storedToken && !token) {
      dispatch({ type: 'auth/setToken', payload: storedToken });
    }
  }, [token, dispatch]);

  return (
    <Suspense fallback={<Loader />}>
      <div className={styles.appContainer} data-cy="app">
        <header className={styles.topbar}>
          <div className={styles.container}>
            <div className={styles.content}>
              <div className={styles.leftNav}>
                <NavLink to="/" className={styles.logo}>
                  NewerDown
                </NavLink>
                <nav className={styles.mainNav}>
                  <NavLink className={getLinkActiveClass} to="/">
                    {t('app.home')}
                  </NavLink>
                  {token && (
                    <NavLink className={getLinkActiveClass} to="/monitoring">
                      {t('app.monitoring')}
                    </NavLink>
                  )}
                </nav>
              </div>

              <div className={styles.rightNav}>
                <button
                  className={styles.iconButton}
                  onClick={toggleLanguage}
                  title={t('app.changeLanguage') || ''}
                >
                  <Languages size={18} />
                  <span>{i18n.language === 'en' ? 'UK' : 'EN'}</span>
                </button>

                {token ? (
                  <>
                    <NavLink className={getLinkActiveClass} to="/account">
                      {t('app.account')}
                    </NavLink>
                    <button
                      className={`${styles.iconButton} ${styles.logoutButton}`}
                      onClick={handleLogout}
                    >
                      <LogOut size={18} />
                      <span>{t('app.logout')}</span>
                    </button>
                  </>
                ) : (
                  <NavLink className={getLinkActiveClass} to="/login">
                    {t('app.login')}
                  </NavLink>
                )}
              </div>
            </div>
          </div>
        </header>

        <main className={styles.outletContainer}>
          <Outlet />
        </main>
      </div>
    </Suspense>
  );
};
