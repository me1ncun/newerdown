import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import Typography from '../Typography';

import raisedHandsIcon from '../../assets/raisedHands.png';

import styles from './Notification.module.scss';

const Notification = ({ notificationTitle, notificationSubtitle }) => {
  return (
    <div className={styles.notification} data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.NOTIFICATION_POPUP}>
      <div className={styles.notificationHead}>
        <Typography.Text
          type="body bold"
          className={styles.notificationText}
          dataE2e={QA_AUTOMATION_ATTRIBUTES_AF.POPUP_TITLE}
        >
          {notificationTitle}
        </Typography.Text>
        <img src={raisedHandsIcon} alt="icon" className={styles.notificationIcon} />
      </div>
      <Typography.Text
        type="small light"
        className={styles.notificationText}
        dataE2e={QA_AUTOMATION_ATTRIBUTES_AF.POPUP_SUBTITLE}
      >
        {notificationSubtitle}
      </Typography.Text>
    </div>
  );
};

export default Notification;
