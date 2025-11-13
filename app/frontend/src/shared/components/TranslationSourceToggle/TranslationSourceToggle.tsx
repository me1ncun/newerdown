import React from 'react';
import { useTranslationSource } from '../../hooks/useTranslationSource';
import './TranslationSourceToggle.scss';

export const TranslationSourceToggle: React.FC = () => {
  const {
    currentSource,
    envSource,
    isLocize,
    isLocal,
    isOverridden,
    isReloading,
    switchToLocal,
    switchToLocize,
    clearOverride,
  } = useTranslationSource();

  if (!import.meta.env.DEV) {
    return null;
  }

  return (
    <div className="translation-source-toggle">
      <div className="toggle-header">
        <h4>Translation Source</h4>
        {isOverridden && <span className="override-badge">Overridden</span>}
      </div>

      <div className="toggle-info">
        <p>
          <strong>Environment:</strong> {envSource}
        </p>
        <p>
          <strong>Active:</strong> {currentSource}
        </p>
      </div>

      <div className="toggle-buttons">
        <button
          className={`toggle-btn ${isLocal ? 'active' : ''}`}
          onClick={switchToLocal}
          disabled={isLocal || isReloading}
        >
          Local Files
        </button>
        <button
          className={`toggle-btn ${isLocize ? 'active' : ''}`}
          onClick={switchToLocize}
          disabled={isLocize || isReloading}
        >
          Locize
        </button>
      </div>

      {isOverridden && (
        <button className="reset-btn" onClick={clearOverride} disabled={isReloading}>
          Reset to Environment Default
        </button>
      )}

      {isReloading && <div className="reload-notice">Reloading application...</div>}
    </div>
  );
};
