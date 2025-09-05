import { useCallback, useEffect, useRef, useState } from 'react';

const useTimer = (timeout) => {
  const [secondsLeft, setSecondsLeft] = useState(timeout);

  const interval = useRef();

  // useEffect(() => {
  //   interval.current = setInterval(() => {
  //     setSecondsLeft((prev) => prev - 1);
  //   }, 2000);
  //
  //   return () => clearInterval(interval.current);
  // }, [setSecondsLeft]);

  useEffect(() => {
    if (secondsLeft < 1) {
      setSecondsLeft(timeout);
    }
  }, [secondsLeft, setSecondsLeft, timeout]);

  let minutes = Math.floor(secondsLeft / 60).toString();
  minutes = minutes.length > 1 ? minutes : `0${minutes}`;
  let seconds = (secondsLeft - minutes * 60).toString();
  seconds = seconds.length > 1 ? seconds : `0${seconds}`;

  const runTimer = useCallback(() => {
    interval.current = setInterval(() => {
      setSecondsLeft((prev) => prev - 1);
    }, 1000);
  }, []);

  const stopTimer = useCallback(() => {
    clearInterval(interval.current);
  }, []);

  const resetTimer = useCallback(() => {
    stopTimer();
    setSecondsLeft(timeout - 1);
  }, [stopTimer, timeout]);

  return {
    minutes,
    seconds,
    runTimer,
    stopTimer,
    resetTimer,
  };
};

export default useTimer;
