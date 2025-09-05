import { useMemo } from 'react';

import { useCustomTranslation } from '~modules-public/translations';

const useErrorMessage = ({ errorCode }) => {
  const { t } = useCustomTranslation('common');
  const errorMessages = useMemo(
    () => ({
      206: {
        title: t('affemity.payment.errorCodes.206.title'),
        subtitle: t('affemity.payment.errorCodes.206.subtitle'),
      },
      208: {
        title: t('affemity.payment.errorCodes.208.title'),
        subtitle: t('affemity.payment.errorCodes.208.subtitle'),
      },
      302: {
        title: t('affemity.payment.errorCodes.302.title'),
        subtitle: t('affemity.payment.errorCodes.302.subtitle'),
      },
      default: {
        title: t('affemity.payment.errorCodes.default.title'),
        subtitle: t('affemity.payment.errorCodes.default.subtitle'),
      },
    }),
    [t],
  );

  return errorMessages[errorCode] || errorMessages.default;
};

export default useErrorMessage;
