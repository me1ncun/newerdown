import { useEffect, useState } from 'react';
import cx from 'classnames';
import * as SolidgateSdk from '@solidgate/react-sdk';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import CloseIcon from '../CloseIcon';
import Loader from '../Loader';

import styles from './ResignPopup.module.scss';

const ResignPopup = ({ resignMerchantData, onSuccess, onError, onFail, onClose, isAdaptive }) => {
  const { Resign } = SolidgateSdk;

  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    setIsLoading(true);

    const timer = setTimeout(() => {
      setIsLoading(false);
    }, 1000);

    return () => clearTimeout(timer);
  }, []);

  console.log(resignMerchantData);
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
            <button
              className={styles.popupClose}
              type="button"
              onClick={onClose}
              data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.CLOSE_BUTTON}
            >
              <CloseIcon className={styles.closeIcon} />
            </button>

            <Resign
              resignRequest={resignMerchantData}
              onSuccess={onSuccess}
              onError={onError}
              onFail={onFail}
            />
          </div>
        </>
      </div>
    </div>
  );
};

export default ResignPopup;
