import { useMemo, useRef } from 'react';
import type { ClientSdkInstance, MessageType, SdkMessage } from '@solidgate/react-sdk';
import Payment from '@solidgate/react-sdk';

import { PAYMENT_METHODS } from '~modules-public/paymentAffemity';
import { useCustomTranslation } from '~modules-public/translations';

import { pdfPaymentCardFormStyles } from '../../styles/pdfPaymentCardFormStyles';

import styles from './PaymentForm.module.scss';

const PaymentForm = ({
  merchantData,
  onSuccess,
  paymentMethods = [],
  onFail = (code) => {
    console.log(code);
  },
}) => {
  const { t } = useCustomTranslation('common');

  const appleContainerRef = useRef(null);
  const googleContainerRef = useRef(null);
  const paypalContainerRef = useRef(null);

  const handleOnError = (e: SdkMessage[MessageType.Error]) => {
    console.log({ e });
  };

  const handleOnFail = (e: SdkMessage[MessageType.Fail]) => {
    console.log({ e });
    onFail(e.code);
  };

  const handleOnMounted = (e: SdkMessage[MessageType.Mounted]) => {
    console.log({ e });
  };

  const handleOrderStatus = (e: SdkMessage[MessageType.OrderStatus]) => {
    console.log({ e });
  };

  const handleOnResize = (e: SdkMessage[MessageType.Resize]) => {
    console.log({ e });
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
    console.log({ e });
  };

  const handleCard = (e: SdkMessage[MessageType.Card]) => {
    console.log({ e });
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

  if (!merchantData) return <p className={styles.text}>No tariff selected</p>;

  return (
    <div className={styles.form}>
      <Payment
        formParams={{
          cardBrands: ['visa', 'mastercard', 'maestro'],
          secureBrands: ['visa-secure', 'mcc-id-check', 'pci-dss'],
          submitButtonText: t('affemity.continue') || 'Continue',
        }}
        styles={pdfPaymentCardFormStyles}
        googlePayContainerRef={googleContainerRef}
        applePayContainerRef={appleContainerRef}
        paypalContainerRef={paypalContainerRef}
        paypalButtonParams={{
          enabled: isPaymentMethodEnabled(PAYMENT_METHODS.payPal),
          shape: 'rect',
          label: 'paypal',
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

      <div ref={appleContainerRef} className={styles.button} />
      <div ref={googleContainerRef} className={styles.button} />
      <div ref={paypalContainerRef} className={styles.button} />
    </div>
  );
};

export default PaymentForm;
