import { useEffect, useState, useRef } from 'react';
import { useAppDispatch, useAppSelector } from '../../shared/hooks/reduxHooks';
import {
  getInformation,
  deleteUser,
  updateUserInformation,
  updateUserAvatar,
  removeUserAvatar,
} from '../../features/userAccountSlice';
import { useTranslation } from 'react-i18next';
import { changePasswordUser } from '../../features/authSlice';
import { Loader } from '../Loader';
import { ConfirmModal } from '../../shared/components/ConfirmModal';
import { AccountEditForm } from '../../shared/components/AccountEditForm';
import type { UserInformation } from '../../shared/types/User';
import type { ChangePassword } from '../../shared/types/Auth';
import defailtAvatar from '../../shared/assets/avatar-default.svg';
import styles from './AccountPage.module.scss';

export const AccountPage = () => {
  const dispatch = useAppDispatch();
  const { user, loading, error } = useAppSelector((state) => state.userAccount);
  const { t } = useTranslation();

  const [showConfirm, setShowConfirm] = useState(false);
  const [showEditForm, setShowEditForm] = useState(false);
  const [showRemoveAvatarModal, setShowRemoveAvatarModal] = useState(false);

  const fileInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    dispatch(getInformation());
  }, [dispatch]);

  const handleDeleteUser = () => {
    dispatch(deleteUser());
    setShowConfirm(false);
  };

  const handleChangePassword = async (data: ChangePassword) => {
    await dispatch(changePasswordUser(data));
  };

  const handleUpdateUserInformation = async (data: UserInformation) => {
    await dispatch(updateUserInformation(data));
    await dispatch(getInformation());
    setShowEditForm(false);
  };

  const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      await dispatch(updateUserAvatar(file));
      await dispatch(getInformation());
    }
    if (e.target) e.target.value = '';
  };

  const handleUploadClick = () => {
    fileInputRef.current?.click();
  };

  const handleRemoveAvatarConfirm = async () => {
    await dispatch(removeUserAvatar());
    await dispatch(getInformation());
    setShowRemoveAvatarModal(false);
  };

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
                  <input
                    type="file"
                    accept="image/*"
                    ref={fileInputRef}
                    style={{ display: 'none' }}
                    onChange={handleFileChange}
                  />

                  <div className={styles.avatarWrapper}>
                    <figure
                      className={`image is-64x64 ${styles.avatarFigure}`}
                      onClick={handleUploadClick}
                    >
                      <img
                        src={user.filePath || defailtAvatar}
                        alt={user.userName || 'User avatar'}
                      />
                      <div className={styles.avatarOverlay}>
                        <span>{t('accountPage.changeAvatar')}</span>
                      </div>
                    </figure>

                    {user.filePath && (
                      <button
                        className={`button is-danger is-small ${styles.avatarRemoveBtn}`}
                        onClick={() => setShowRemoveAvatarModal(true)}
                      >
                        {t('accountPage.removeAvatar')}
                      </button>
                    )}
                  </div>
                </div>

                <div className="media-content">
                  <p className="account__name title is-4">{user.displayName || user.userName}</p>
                  <p className="account__email subtitle is-6">{user.email}</p>
                </div>
              </div>

              {!showEditForm && (
                <>
                  <div className="account__details content">
                    <p>
                      <strong>{t('accountPage.organization')}:</strong>{' '}
                      {user.organizationName || '-'}
                    </p>
                    <p>
                      <strong>{t('accountPage.phone')}:</strong> {user.phoneNumber || '-'}
                    </p>
                    <p>
                      <strong>{t('accountPage.language')}:</strong> {user.language || '-'}
                    </p>
                  </div>

                  <div className="account__actions mt-4">
                    <button className="button is-link mr-3" onClick={() => setShowEditForm(true)}>
                      {t('accountPage.changeUserData')}
                    </button>

                    <button className="button is-danger" onClick={() => setShowConfirm(true)}>
                      {t('accountPage.deleteUser')}
                    </button>
                  </div>
                </>
              )}

              {showEditForm && (
                <AccountEditForm
                  user={user}
                  onUpdate={handleUpdateUserInformation}
                  onChangePassword={handleChangePassword}
                  onCancel={() => setShowEditForm(false)}
                />
              )}
            </div>
          </section>
        )}

        <ConfirmModal
          isOpen={showConfirm}
          title={t('accountPage.confirmDeleteTitle')}
          message={t('accountPage.confirmDeleteMessage')}
          onConfirm={handleDeleteUser}
          onCancel={() => setShowConfirm(false)}
          confirmText={t('accountPage.confirm')}
          cancelText={t('accountPage.cancel')}
        />

        <ConfirmModal
          isOpen={showRemoveAvatarModal}
          title={t('accountPage.confirmRemoveAvatarTitle')}
          message={t('accountPage.confirmRemoveAvatarMessage')}
          onConfirm={handleRemoveAvatarConfirm}
          onCancel={() => setShowRemoveAvatarModal(false)}
          confirmText={t('accountPage.confirm')}
          cancelText={t('accountPage.cancel')}
        />
      </div>
    </main>
  );
};
