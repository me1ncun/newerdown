import cx from 'classnames';

import messagePartUser from './assets/messagePartUser.svg';

import styles from './Message.module.scss';

const Message = ({ message }) => {
  return (
    <li className={cx(styles.message, styles.messageRightSide)}>
      <img src={message.avatar} alt="Avatar" className={styles.messageAvatar} />
      <div className={styles.messageContainer}>
        <div className={cx(styles.messageContent, styles.messageContentRightSide)}>
          <p
            className={styles.messageContentText}
            dangerouslySetInnerHTML={{ __html: message.content }}
          />
        </div>

        <img
          src={messagePartUser}
          alt="MessageChat Part"
          className={cx(styles.messageContentPart, styles.messageContentPartUser)}
        />
      </div>
    </li>
  );
};

export default Message;
