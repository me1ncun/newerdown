import cx from 'classnames';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import Typography from '../Typography';

import styles from './QuestionTitle.module.scss';

const QuestionTitle = ({ children, isAnimated = false, isDynamic = false, className, ...rest }) => {
  const dataAttrTitle = isDynamic
    ? `${QA_AUTOMATION_ATTRIBUTES_AF.STEP_TITLE} ${QA_AUTOMATION_ATTRIBUTES_AF.DYNAMIC_TEXT}`
    : QA_AUTOMATION_ATTRIBUTES_AF.STEP_TITLE;

  return (
    <Typography.Title
      level={1}
      center
      className={cx(styles.title, className, {
        [styles.titleAnimated]: isAnimated,
      })}
      dataE2e={dataAttrTitle}
      {...rest}
    >
      {children}
    </Typography.Title>
  );
};

export default QuestionTitle;
