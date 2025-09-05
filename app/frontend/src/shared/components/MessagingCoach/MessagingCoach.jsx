import { memo, useEffect, useRef, useState } from 'react';
import { CSSTransition, TransitionGroup } from 'react-transition-group';
import cx from 'classnames';

import useMessagingConfig from './hooks/useMessagingConfig';

import usePrintingText from '../../hooks/usePrintingText';

import IntersectionTopLine from '../IntersectionTopLine';
import Message from '../Message';
import MessageChat from '../MessageChat';

import chatAvatar from './assets/chatAvatar.webp';
import inputIcon from './assets/inputIcon.svg';

import styles from './MessagingCoach.module.scss';

const MessagingCoach = ({ header, inputText, inputTextTyping, className }) => {
  const messages = useMessagingConfig();
  const [isUserTyping, setIsUserTyping] = useState(false);
  const [isAnimationStarted, setIsAnimationStarted] = useState(false);
  const [shownMessages, setShownMessages] = useState([]);
  const timerTypingRef = useRef(null);

  const { printedText, startPrinting, resetPrinting, hasFinished } = usePrintingText({
    text: inputTextTyping,
  });

  const startAnimation = () => {
    setIsAnimationStarted(true);
  };

  const restartAnimation = () => {
    setTimeout(() => {
      setShownMessages([]);
      setIsAnimationStarted(false);
      resetPrinting();
    }, 7000);
  };

  useEffect(() => {
    let timeoutSum = 0;

    if (isAnimationStarted) {
      messages.forEach((msg, index) => {
        timeoutSum += msg.delay || 1000;
        setTimeout(() => {
          setShownMessages((prev) => [...prev, msg]);
          if (index === messages.length - 1) {
            restartAnimation();
          }
        }, timeoutSum);
      });
    }
  }, [isAnimationStarted]);

  useEffect(() => {
    if (isAnimationStarted) {
      timerTypingRef.current = setTimeout(() => {
        startPrinting();
        setIsUserTyping(true);
      }, 3000);
    }

    return () => {
      clearTimeout(timerTypingRef.current);
    };
  }, [isAnimationStarted, startPrinting]);

  useEffect(() => {
    if (hasFinished) {
      setIsUserTyping(false);
    }
  }, [hasFinished]);

  return (
    <div className={cx(styles.wrapper, className)}>
      <div className={styles.messagingHeader}>
        <img src={chatAvatar} alt="logo" className={styles.messagingHeaderIcon} />
        <h2 className={styles.messagingHeaderTitle}>{header}</h2>
      </div>
      <div className={styles.messagingMain}>
        <div className={styles.messagingContainer}>
          <TransitionGroup component="ul" className={styles.messagingList}>
            {shownMessages.map((message) => (
              <CSSTransition
                key={message.id}
                timeout={0}
                classNames={{
                  enter: styles.messageAnimationEnter,
                  enterActive: styles.messageAnimationEnterActive,
                  exit: styles.messageAnimationExit,
                  exitActive: styles.messageAnimationExitActive,
                }}
              >
                {message.author === 'user' ? (
                  <Message message={message} />
                ) : (
                  <MessageChat message={message} />
                )}
              </CSSTransition>
            ))}
          </TransitionGroup>
        </div>
        <IntersectionTopLine withoutAnimation={true} onIntersect={startAnimation}>
          <div
            className={cx(styles.messagingInput, {
              [styles.messagingInputPointer]: isAnimationStarted,
            })}
          >
            {isUserTyping ? (
              <p className={cx(styles.messagingInputText, styles.messagingInputTextTyping)}>
                {printedText}
              </p>
            ) : (
              <p className={styles.messagingInputText}>{inputText}</p>
            )}

            <img src={inputIcon} alt="InputIconSend" className={styles.messagingInputIcon} />
          </div>
        </IntersectionTopLine>
      </div>
    </div>
  );
};

export default memo(MessagingCoach);
