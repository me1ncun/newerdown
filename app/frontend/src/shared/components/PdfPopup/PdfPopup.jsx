import { useEffect, useState } from 'react';
import cx from 'classnames';

import { useCustomTranslation } from '~modules-public/translations';

import closeIcon from '../../assets/closeDark.svg';
import mailSentIcon from '../../assets/mailSent.svg';

import styles from './PdfPopup.module.scss';

const PdfPopup = ({ shouldShowPopup = false, onClose = () => {}, icon = mailSentIcon }) => {
  const { t } = useCustomTranslation();

  const [isPopupVisible, setIsPopupVisible] = useState(false);

  const handleClosePopup = () => {
    setIsPopupVisible(false);

    onClose();
  };

  useEffect(() => {
    setIsPopupVisible(shouldShowPopup);

    if (shouldShowPopup) {
      const timer = setTimeout(() => {
        handleClosePopup();
      }, 3000);

      return () => clearTimeout(timer);
    }
  }, [shouldShowPopup]);

  return (
    <div className={cx(styles.wrapper, { [styles.visible]: isPopupVisible })}>
      <img src={icon} alt="mail icon" className={styles.icon} />

      <p className={styles.text}>{t('pdf_popup.text')}</p>

      <button className={styles.iconClose} onClick={handleClosePopup} aria-label="Close">
        <img src={closeIcon} alt="Close" />
      </button>
    </div>
  );
};

export default PdfPopup;
