import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import HttpBackend from 'i18next-http-backend';
import LocizeBackend from 'i18next-locize-backend';
import LanguageDetector from 'i18next-browser-languagedetector';
import LastUsed from 'locize-lastused';
import { locizePlugin } from 'locize';

const STORAGE_KEY = 'translation-source-override';
const overrideSource = typeof window !== 'undefined' ? localStorage.getItem(STORAGE_KEY) : null;
const translationSource = overrideSource || import.meta.env.VITE_TRANSLATION_SOURCE || 'local';
const isLocizeEnabled = translationSource === 'locize';

const languageMap = {
  uk: 'uk-UA',
  en: 'en',
};

const normalizeLanguage = (lng) => {
  if (!lng) return 'en';
  // For Locize, map 'uk' to 'uk-UA'
  if (isLocizeEnabled && languageMap[lng]) {
    return languageMap[lng];
  }
  if (!isLocizeEnabled) {
    if (lng === 'uk-UA') return 'uk';
  }
  return lng;
};

const locizeOptions = {
  projectId: import.meta.env.VITE_LOCIZE_PROJECT_ID,
  apiKey: import.meta.env.VITE_LOCIZE_API_KEY,
  version: import.meta.env.VITE_LOCIZE_VERSION || 'latest',
  referenceLng: import.meta.env.VITE_LOCIZE_REFERENCE_LANGUAGE || 'en',
};

const i18nConfig = {
  detection: {
    order: ['localStorage', 'navigator'],
    caches: ['localStorage'],
    lookupLocalStorage: 'i18nextLng',
  },
  fallbackLng: 'en',
  supportedLngs: isLocizeEnabled ? ['en', 'uk-UA'] : ['en', 'uk'],
  load: 'currentOnly',
  nonExplicitSupportedLngs: !isLocizeEnabled,
  debug: import.meta.env.DEV,
  ns: ['common', 'monitoring'],
  defaultNS: 'common',
  interpolation: {
    escapeValue: false,
  },
  react: {
    useSuspense: true,
  },
};

if (isLocizeEnabled) {
  i18n
    .use(LocizeBackend)
    .use(LastUsed)
    .use(locizePlugin)
    .use(LanguageDetector)
    .use(initReactI18next)
    .init({
      ...i18nConfig,
      backend: locizeOptions,
      locizeLastUsed: locizeOptions,
      saveMissing: true,
      updateMissing: true,
    });

  const originalChangeLanguage = i18n.changeLanguage.bind(i18n);
  i18n.changeLanguage = (lng, callback) => {
    const normalizedLng = normalizeLanguage(lng);
    console.log(`[i18n] Language change requested: ${lng} -> ${normalizedLng}`);
    return originalChangeLanguage(normalizedLng, callback);
  };
} else {
  i18n
    .use(HttpBackend)
    .use(LanguageDetector)
    .use(initReactI18next)
    .init({
      ...i18nConfig,
      backend: {
        loadPath: `${import.meta.env.BASE_URL}locales/{{lng}}/{{ns}}.json`,
      },
    });

  const originalChangeLanguage = i18n.changeLanguage.bind(i18n);
  i18n.changeLanguage = (lng, callback) => {
    const normalizedLng = normalizeLanguage(lng);
    if (normalizedLng !== lng) {
      console.log(`[i18n] Language change requested: ${lng} -> ${normalizedLng}`);
    }
    return originalChangeLanguage(normalizedLng, callback);
  };
}

console.log(`[i18n] Translation source: ${translationSource}`);
if (isLocizeEnabled) {
  console.log(`[i18n] Locize project: ${locizeOptions.projectId}`);
  console.log(`[i18n] Locize version: ${locizeOptions.version}`);
}

export default i18n;
