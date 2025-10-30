import { useEffect, useState } from 'react';
import { useAppDispatch, useAppSelector } from '../../shared/hooks/reduxHooks';
import { clearError, singUpUser } from '../../features/authSlice';
import { Link, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

export const RegisterPage = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { loading, error } = useAppSelector((state) => state.auth);
  const { t } = useTranslation();

  const [userName, setUserName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  useEffect(() => {
    dispatch(clearError());
  }, [dispatch]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const resultAction = await dispatch(singUpUser({ userName, email, password }));

    if (singUpUser.fulfilled.match(resultAction)) {
      navigate('/login');
    }
  };

  return (
    <div
      className="container is-flex is-justify-content-center is-align-items-center"
      style={{ height: '100vh' }}
    >
      <div className="box" style={{ maxWidth: '400px', width: '100%' }}>
        <h1 className="title has-text-centered">{t('registerPage.register')}</h1>
        <form onSubmit={handleSubmit}>
          <div className="field">
            <label className="label">{t('registerPage.name')}</label>
            <div className="control has-icons-left">
              <input
                className="input"
                type="text"
                placeholder="Your name"
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
                required
              />
              <span className="icon is-small is-left">
                <i className="fas fa-user"></i>
              </span>
            </div>
          </div>

          <div className="field">
            <label className="label">{t('registerPage.email')}</label>
            <div className="control has-icons-left">
              <input
                className="input"
                type="email"
                placeholder="Your email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
              <span className="icon is-small is-left">
                <i className="fas fa-envelope"></i>
              </span>
            </div>
          </div>

          <div className="field">
            <label className="label">{t('registerPage.password')}</label>
            <div className="control has-icons-left">
              <input
                className="input"
                type="password"
                placeholder="Your password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
              <span className="icon is-small is-left">
                <i className="fas fa-lock"></i>
              </span>
            </div>
          </div>

          {error && <p className="has-text-danger has-text-centered">{error}</p>}

          <button className="button is-primary is-fullwidth mt-3" type="submit" disabled={loading}>
            {loading ? 'Registering...' : 'Register'}
          </button>
        </form>

        <p className="has-text-centered mt-4">
          {t('registerPage.have')}{' '}
          <Link to="/login" className="has-text-link">
            {t('registerPage.login')}
          </Link>
        </p>
      </div>
    </div>
  );
};
