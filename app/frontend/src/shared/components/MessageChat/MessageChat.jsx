import { useEffect, useState } from 'react';
import cx from 'classnames';

import Typing from '../Typing';

import likeIcon from './assets/likeIcon.webp';
import messagePartChat from './assets/messagePartChat.svg';

import styles from './MessageChat.module.scss';

const MessageChat = ({ message }) => {
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const timeout = setTimeout(() => {
      setIsLoading(false);
    }, message.typingDelay);

    return () => clearTimeout(timeout);
  }, [message.typingDelay]);

  return (
    <li
      className={cx(styles.message, {
        [styles.messageFirst]: !message.withLike,
      })}
    >
      <img src={message.avatar} alt="Avatar" className={styles.messageAvatar} />
      {isLoading ? (
        <Typing />
      ) : (
        <>
          <div className={styles.messageContainer}>
            <div className={cx(styles.messageContent)}>
              <p
                className={styles.messageContentText}
                dangerouslySetInnerHTML={{ __html: message.content }}
              />
            </div>

            <img
              src={messagePartChat}
              alt="MessageChat Part"
              className={styles.messageContentPart}
            />
          </div>
          {message.withLike && (
            <img src={likeIcon} alt="MessageChat like icon" className={styles.messageLike} />
          )}
        </>
      )}
    </li>
  );
};

export default MessageChat;
