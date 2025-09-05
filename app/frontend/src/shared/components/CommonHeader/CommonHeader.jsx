import cx from 'classnames';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import Logo from '../Logo';
import ProgressBar from '../ProgressBar';

import styles from './CommonHeader.module.scss';

const CommonHeader = ({
  currentProgressNum = 0,
  progressBlocks = [11, 11, 11],
  className = '',
  handleBackStep = () => window.history.go(-1),
  canGoBack = true,
}) => {
  return (
    <div className={cx(styles.wrapper, className)}>
      <div className={styles.inner}>
        <button
          type="button"
          className={cx(styles.backButton, { [styles.hidden]: !canGoBack })}
          onClick={handleBackStep}
          data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.BACK_BUTTON}
        >
          <svg
            width="24"
            height="24"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
            className={styles.backButtonIcon}
          >
            <path
              d="M15.5996 5L7 11.7334L15.5996 18.4668"
              stroke="currentColor"
              strokeWidth="3"
              strokeMiterlimit="10"
              strokeLinecap="round"
              strokeLinejoin="round"
            />
          </svg>
        </button>

        <Logo className={styles.logo} />
      </div>
      {currentProgressNum !== 0 && (
        <ProgressBar.Line currentProgressNum={currentProgressNum} progressBlocks={progressBlocks} />
      )}
    </div>
  );
};

export default CommonHeader;
