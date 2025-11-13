import { Navigate, Route, BrowserRouter as Router, Routes } from 'react-router-dom';
import { App } from './App';
import { HomePage } from './pages/HomePage';
import { NotFoundPage } from './pages/NotFoundPage';
import { LoginPage } from './pages/LoginPage';
import { RegisterPage } from './pages/RegisterPage';
import { Provider } from 'react-redux';
import { store } from './app/store';
import { RequireAuth } from './pages/RequireAuth';
import { AccountPage } from './pages/AccountPage';
import { MonitoringPage } from './pages/MonitoringPage';
import { CreateMonitorPage } from './pages/CreateMonitorPage';
import { EditMonitorPage } from './pages/EditMonitorPage';

export const Root = () => (
  <Provider store={store}>
    <Router basename="/newerdown">
      <Routes>
        <Route path="/" element={<App />}>
          <Route index element={<HomePage />} />
          <Route element={<RequireAuth />}>
            <Route path="monitoring" element={<MonitoringPage />} />
            <Route path="monitoring/create" element={<CreateMonitorPage />} />
            <Route path="monitoring/:id/edit" element={<EditMonitorPage />} />
            <Route path="account" element={<AccountPage />} />
          </Route>
          <Route path="login" element={<LoginPage />} />
          <Route path="register" element={<RegisterPage />} />
        </Route>
        <Route path="*" element={<NotFoundPage />} />
        <Route path="home" element={<Navigate to="/" replace />} />
      </Routes>
    </Router>
  </Provider>
);
