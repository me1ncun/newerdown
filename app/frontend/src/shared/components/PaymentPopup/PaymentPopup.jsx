import { useRef, useState } from 'react';
import cx from 'classnames';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import { canMakeApplePayPayments } from '~modules-public/paymentAffemity';
import { useCustomTranslation } from '~modules-public/translations';

import CloseIcon from '../CloseIcon';
import Loader from '../Loader';
import PaymentForm from '../PaymentForm';
import Typography from '../Typography';

import shieldIcon from './assets/shield.svg';

import styles from './PaymentPopup.module.scss';

const PaymentPopup = ({ merchantData, onSucess, onFail, paymentMethods, onClose, isAdaptive }) => {
  const { t } = useCustomTranslation('common');
  const isIOS = canMakeApplePayPayments();

  const appleContainerRef = useRef(null);
  const googleContainerRef = useRef(null);
  const paypalContainerRef = useRef(null);

  const [isLoading, setIsLoading] = useState(true);

  const handlePaymentReady = () => {
    setIsLoading(false);
  };

  return (
    <div className={styles.popupWrapper}>
      <div
        className={cx(styles.popupInner, {
          [styles.popupInnerAdaptive]: isAdaptive,
        })}
      >
        <>
          {isLoading && <Loader className={styles.popupLoader} />}
          <div
            className={cx(styles.popupContent, {
              [styles.popupContentLoading]: isLoading,
            })}
          >
            <div className={styles.popupHead}>
              <Typography.Text type="body light" className={styles.selectPopupTitle}>
                {t('affemity.payment.selectPayment')}
              </Typography.Text>
              <button
                className={styles.popupClose}
                type="button"
                aria-label="Close"
                onClick={onClose}
                data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.CLOSE_BUTTON}
              >
                <CloseIcon className={styles.closeIcon} />
              </button>
            </div>

            <div className={styles.formWrapper}>
              <div
                className={cx(styles.googlePay, {
                  [styles.hide]: isIOS,
                })}
              >
                <div
                  className={styles.googlePlayButtonContainer}
                  ref={googleContainerRef}
                  id="google-pay-container-sdk"
                />
              </div>
              <div
                className={cx(styles.applePay, {
                  [styles.hide]: !isIOS,
                })}
              >
                <div ref={appleContainerRef} className={styles.applePayButtonContainer} />
              </div>
              <div>
                <div ref={paypalContainerRef} className={styles.paypalButtonContainer} />
              </div>

              <div className={styles.paySafeBlockWrapper}>
                <div className={styles.paySafeBlockInner}>
                  <img src={shieldIcon} alt="pay safe" className={styles.paySafeIcon} />
                  <Typography.Text type="small bold" className={styles.paySafeText}>
                    {t('affemity.payment.paySafe')}
                  </Typography.Text>
                </div>
              </div>

              <PaymentForm
                onReady={handlePaymentReady}
                merchantData={merchantData}
                onSuccess={onSucess}
                onFail={onFail}
                googleContainerRef={googleContainerRef}
                appleContainerRef={appleContainerRef}
                paypalContainerRef={paypalContainerRef}
                paymentMethods={paymentMethods}
                isBrandColor={true}
              />
            </div>
          </div>
        </>
      </div>
    </div>
  );
};

export default PaymentPopup;
