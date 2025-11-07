import { useState, useEffect } from 'react';

type TranslationSource = 'local' | 'locize';

const STORAGE_KEY = 'translation-source-override';

/**
 * Hook to manage translation source switching at runtime
 *
 * This hook allows developers to override the default translation source
 * set in environment variables. The override is stored in localStorage.
 *
 * @returns {object} Translation source management utilities
 */
export const useTranslationSource = () => {
  const envSource = (import.meta.env.VITE_TRANSLATION_SOURCE || 'local') as TranslationSource;
  const [currentSource, setCurrentSource] = useState<TranslationSource>(envSource);
  const [isReloading, setIsReloading] = useState(false);

  useEffect(() => {
    // Check for override in localStorage
    const override = localStorage.getItem(STORAGE_KEY) as TranslationSource | null;
    if (override && (override === 'local' || override === 'locize')) {
      setCurrentSource(override);
    }
  }, []);

  const switchToLocal = async () => {
    if (currentSource === 'local') return;

    localStorage.setItem(STORAGE_KEY, 'local');
    setIsReloading(true);

    // Reload the page to reinitialize i18n with local backend
    window.location.reload();
  };

  const switchToLocize = async () => {
    if (currentSource === 'locize') return;

    localStorage.setItem(STORAGE_KEY, 'locize');
    setIsReloading(true);

    // Reload the page to reinitialize i18n with Locize backend
    window.location.reload();
  };

  const clearOverride = () => {
    localStorage.removeItem(STORAGE_KEY);
    setIsReloading(true);
    window.location.reload();
  };

  const isOverridden = localStorage.getItem(STORAGE_KEY) !== null;

  return {
    currentSource,
    envSource,
    isLocize: currentSource === 'locize',
    isLocal: currentSource === 'local',
    isOverridden,
    isReloading,
    switchToLocal,
    switchToLocize,
    clearOverride,
  };
};
