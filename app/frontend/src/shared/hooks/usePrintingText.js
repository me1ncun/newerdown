import { useCallback, useEffect, useRef, useState } from 'react';

const usePrintingText = ({ text = '' }) => {
  const [currentSymbolIndex, setCurrentSymbolIndex] = useState(0);
  const [hasStarted, setHasStarted] = useState(false);
  const [hasFinished, setHasFinished] = useState(false);
  const timerRef = useRef(null);
  const timerEndRef = useRef(null);

  const startPrinting = useCallback(() => {
    if (!hasStarted) {
      setHasStarted(true);
    }
  }, [hasStarted]);

  const resetPrinting = useCallback(() => {
    clearTimeout(timerRef.current);
    clearTimeout(timerEndRef.current);
    timerRef.current = null;
    timerEndRef.current = null;
    setCurrentSymbolIndex(0);
    setHasStarted(false);
    setHasFinished(false);
  }, []);

  useEffect(() => {
    if (!hasStarted || hasFinished) return;

    if (currentSymbolIndex >= text.length) {
      if (!timerEndRef.current) {
        timerEndRef.current = setTimeout(() => {
          setHasFinished(true);
        }, 1500);
      }
      return;
    }

    timerRef.current = setTimeout(() => {
      setCurrentSymbolIndex((prev) => prev + 1);
    }, 50);

    return () => {
      clearTimeout(timerRef.current);
      clearTimeout(timerEndRef.current);
    };
  }, [currentSymbolIndex, text, hasStarted, hasFinished]);

  return {
    printedText: text.slice(0, currentSymbolIndex),
    startPrinting,
    resetPrinting,
    hasFinished,
  };
};

export default usePrintingText;
