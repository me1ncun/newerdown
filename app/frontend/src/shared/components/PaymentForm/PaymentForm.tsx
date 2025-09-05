import { useMemo } from 'react';
import type { ClientSdkInstance, MessageType, SdkMessage } from '@solidgate/react-sdk';
import Payment from '@solidgate/react-sdk';

import useIsDesktop from '~system-host/shared/hooks/useIsDesktop';

import { PAYMENT_METHODS } from '~modules-public/paymentAffemity';
import { useCustomTranslation } from '~modules-public/translations';

import { paymentCardFormStyles } from '../../styles/paymentCardFormStyles';
import { paymentCardFormStylesAdptv } from '../../styles/paymentCardFormStylesAdptv';
import { paymentCardFormStylesBrand } from '../../styles/paymentCardFormStylesBrand';

import styles from './PaymentForm.module.scss';

const PaymentForm = ({
  merchantData,
  onSuccess,
  onReady,
  appleContainerRef,
  googleContainerRef,
  paypalContainerRef,
  paymentMethods = [],
  isBrandColor = false,
}) => {
  const { t } = useCustomTranslation('common');

  const handleOnError = (e: SdkMessage[MessageType.Error]) => {
    console.log({ e });
  };

  const handleOnFail = (e: SdkMessage[MessageType.Fail]) => {
    console.log({ e });
  };

  const handleOnMounted = (e: SdkMessage[MessageType.Mounted]) => {
    console.log(`Mounted ${e.entity}`);
    onReady();
  };

  const handleOrderStatus = (e: SdkMessage[MessageType.OrderStatus]) => {
    console.log({ e });
  };

  const handleOnResize = (e: SdkMessage[MessageType.Resize]) => {
    console.log(e.type);
    onReady();
  };

  const handleOnSuccess = (e: SdkMessage[MessageType.Success]) => {
    console.log({ e });
    onSuccess();
  };

  const handleOnSubmit = (e: SdkMessage[MessageType.Submit]) => {
    console.log({ e });
  };

  const handleOnInteraction = (e: SdkMessage[MessageType.Interaction]) => {
    console.log({ e });
  };

  const handleOnVerify = (e: SdkMessage[MessageType.Verify]) => {
    console.log({ e });
  };

  const handleOnRedirectMessage = (e: SdkMessage[MessageType.Redirect]) => {
    console.log({ e });
  };

  const handleOnCustomStylesAppended = (e: SdkMessage[MessageType.CustomStylesAppended]) => {
    console.log(e.type);
  };

  const handleCard = (e: SdkMessage[MessageType.Card]) => {
    console.log(e.type);
  };

  const handleOnReadyPaymentInstance = (form: ClientSdkInstance) => {
    // eslint-disable-next-line no-console
    console.log('form', form);
  };

  const isPaymentMethodEnabled = useMemo(
    () => (paymentMethod) => {
      return paymentMethods.includes(paymentMethod);
    },
    [paymentMethods],
  );

  const isDesktop = useIsDesktop();

  const paymentCardBrandStyles = isDesktop
    ? paymentCardFormStylesAdptv
    : paymentCardFormStylesBrand;

  if (!merchantData) return <p className={styles.text}>No tariff selected</p>;

  return (
    <div className={styles.form}>
      <Payment
        formParams={{
          cardBrands: ['visa', 'mastercard', 'maestro'],
          submitButtonText: t('affemity.continue') || 'Continue',
        }}
        styles={isBrandColor ? paymentCardBrandStyles : paymentCardFormStyles}
        googlePayContainerRef={googleContainerRef}
        applePayContainerRef={appleContainerRef}
        paypalContainerRef={paypalContainerRef}
        paypalButtonParams={{
          enabled: isPaymentMethodEnabled(PAYMENT_METHODS.payPal),
          shape: 'rect',
          label: 'paypal',
          height: 40,
        }}
        merchantData={merchantData}
        onError={handleOnError}
        onFail={handleOnFail}
        onCard={handleCard}
        onMounted={handleOnMounted}
        onOrderStatus={handleOrderStatus}
        onResize={handleOnResize}
        onSuccess={handleOnSuccess}
        onSubmit={handleOnSubmit}
        onInteraction={handleOnInteraction}
        onVerify={handleOnVerify}
        onFormRedirect={handleOnRedirectMessage}
        onCustomStylesAppended={handleOnCustomStylesAppended}
        onReadyPaymentInstance={handleOnReadyPaymentInstance}
      />
    </div>
  );
};

export default PaymentForm;
