import cx from 'classnames';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import Typography from '../Typography';

import styles from './SummaryField.module.scss';

const SummaryField = ({ icon = '', text = '', className = '', ...rest }) => {
  return (
    <div
      className={cx(styles.infoDescription, className)}
      data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.DYNAMIC_TEXT}
      {...rest}
    >
      <img className={styles.infoIcon} src={icon} alt="icon" />
      <Typography.Text type="secondary light" className={styles.infoText}>
        {text}
      </Typography.Text>
    </div>
  );
};

export default SummaryField;
