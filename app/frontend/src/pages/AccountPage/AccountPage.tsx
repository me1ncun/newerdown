import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../shared/hooks/reduxHooks';
import { getInformation } from '../../features/userAccountSlice';
import { Loader } from '../Loader';
import defailtAvatar from '../../shared/assets/avatar-default.svg';
import { useTranslation } from 'react-i18next';
import './AccountPage.module.scss';

export const AccountPage = () => {
  const dispatch = useAppDispatch();
  const { user, loading, error } = useAppSelector((state) => state.userAccount);
  const { t } = useTranslation();

  useEffect(() => {
    dispatch(getInformation());
  }, [dispatch]);

  return (
    <main className="account">
      <div className="container">
        <h1 className="account__title title">{t('accountPage.account')}</h1>

        {loading && <Loader />}
        {error && (
          <p data-cy="accountPageError" className="account__error has-text-danger">
            Something went wrong: {error}
          </p>
        )}
        {user && !loading && !error && (
          <section className="account__card card">
            <div className="card-content">
              <div className="account__header media">
                <div className="media-left">
                  <figure className="image is-64x64 account__avatar">
                    <img src={defailtAvatar} alt={user.userName || 'User avatar'} />
                  </figure>
                </div>
                <div className="media-content">
                  <p className="account__name title is-4">{user.displayName || user.userName}</p>
                  <p className="account__email subtitle is-6">{user.email}</p>
                </div>
              </div>

              <div className="account__details content">
                <p>
                  <strong>{t('accountPage.organization')}:</strong> {user.organizationName || '-'}
                </p>
                <p>
                  <strong>{t('accountPage.phone')}:</strong> {user.phoneNumber || '-'}
                </p>
                <p>
                  <strong>{t('accountPage.language')}</strong> {user.language || '-'}
                </p>
                <p>
                  <strong>{t('accountPage.timeZone')}</strong> {user.timeZone || '-'}
                </p>
              </div>
            </div>
          </section>
        )}
      </div>
    </main>
  );
};
