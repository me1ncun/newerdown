import { useState } from 'react';
import cx from 'classnames';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import arrowTop from '../../assets/arrowTop.svg';

import styles from './Spoiler.module.scss';

const Spoiler = ({ className, titleText, spoilerText, open = false, handleClick = () => {} }) => {
  const [isOpen, setIsOpen] = useState(open);
  const [isTriggered, setIsTriggered] = useState(false);

  const handleTriggerClick = () => {
    if (!isTriggered) {
      handleClick();
      setIsTriggered(true);
    }
  };

  return (
    <div
      className={cx(className, styles.wrapper)}
      onClick={handleTriggerClick}
      data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.DROPDOWN}
    >
      <button
        type="button"
        onClick={() => setIsOpen((prev) => !prev)}
        className={cx(styles.titleButton, { [styles.titleButtonOpen]: isOpen })}
      >
        <div className={styles.titleText}>{titleText}</div>
        <img
          src={arrowTop}
          alt="arrow"
          className={cx(styles.arrowTop, { [styles.arrowBottom]: !isOpen })}
        />
      </button>
      {isOpen && <div className={styles.spoilerText}>{spoilerText}</div>}
    </div>
  );
};

export default Spoiler;
