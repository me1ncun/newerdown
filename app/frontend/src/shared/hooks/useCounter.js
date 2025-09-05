import { useEffect, useRef, useState } from 'react';

const useCounter = ({
  startNum = 0,
  endNum = 0,
  loadingIntervalMs = 0,
  onFinishCount = () => {},
}) => {
  const [counter, setCounter] = useState(startNum);
  const [isCounterStarted, setIsCounterStarted] = useState(false);
  const counterId = useRef(null);
  const endNumRef = useRef(endNum);
  const delayRef = useRef(loadingIntervalMs / (endNum - startNum));

  const clearCounterId = () => {
    clearInterval(counterId.current);
  };

  const startCounting = () => {
    setIsCounterStarted(true);
  };

  useEffect(() => {
    if (isCounterStarted) {
      counterId.current = setInterval(() => {
        setCounter((time) => time + 1);
      }, delayRef.current);
    }

    return () => clearCounterId();
  }, [isCounterStarted]);

  useEffect(() => {
    if (counter >= endNumRef.current) {
      clearCounterId();
      onFinishCount();
    }
  }, [counter, onFinishCount]);

  return { counter, startCounting };
};

export default useCounter;
