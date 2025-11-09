import { useState, useEffect } from 'react';

type TranslationSource = 'local' | 'locize';

const STORAGE_KEY = 'translation-source-override';

export const useTranslationSource = () => {
  const envSource = (import.meta.env.VITE_TRANSLATION_SOURCE || 'local') as TranslationSource;
  const [currentSource, setCurrentSource] = useState<TranslationSource>(envSource);
  const [isReloading, setIsReloading] = useState(false);

  useEffect(() => {
    const override = localStorage.getItem(STORAGE_KEY) as TranslationSource | null;
    if (override && (override === 'local' || override === 'locize')) {
      setCurrentSource(override);
    }
  }, []);

  const switchToLocal = async () => {
    if (currentSource === 'local') return;

    localStorage.setItem(STORAGE_KEY, 'local');
    setIsReloading(true);

    window.location.reload();
  };

  const switchToLocize = async () => {
    if (currentSource === 'locize') return;

    localStorage.setItem(STORAGE_KEY, 'locize');
    setIsReloading(true);

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
