import cx from 'classnames';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import { useCustomTranslation } from '~modules-public/translations';

import styles from './SkipButton.module.scss';

const SkipButton = ({ onClick, className }) => {
  const { t } = useCustomTranslation('common');
  return (
    <button
      className={cx(styles.skip, className)}
      onClick={onClick}
      data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.SKIP_BUTTON}
    >
      <span className={styles.skipText}>{t('affemity.skip') || 'Skip'}</span>
    </button>
  );
};

export default SkipButton;
