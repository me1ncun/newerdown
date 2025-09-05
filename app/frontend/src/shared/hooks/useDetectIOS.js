import { useEffect, useRef } from 'react';

const useDetectIOS = () => {
  const isIOS = useRef();

  useEffect(() => {
    isIOS.current = /(iPod|iPhone|iPad)/i.test(navigator.userAgent) || false;
  }, []);

  return isIOS.current;
};

export default useDetectIOS;
