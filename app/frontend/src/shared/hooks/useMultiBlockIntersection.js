import { useCallback, useEffect, useState } from 'react';

const checkMatchedInterval = (number, intervals) => {
  return intervals.some((interval) => number >= interval.start && number <= interval.end);
};

const getRefSizeData = (ref) => {
  return {
    start: ref?.current?.offsetTop || 0,
    end: ref?.current?.offsetHeight + ref?.current?.offsetTop || 0,
  };
};

const useMultiBlockIntersection = ({
  refs,
  offsetToTop = 0,
  initialIntersectedValueBeforeScroll = false,
}) => {
  const [intersected, setIntersected] = useState(initialIntersectedValueBeforeScroll);

  const handleBlocksIntersected = useCallback(
    (e = {}) => {
      const intervals = refs.map(getRefSizeData);
      const currentScrollPosition = (e?.target?.scrollTop || 0) + offsetToTop;
      const isIntervals = checkMatchedInterval(currentScrollPosition, intervals);

      setIntersected(isIntervals);
    },
    [offsetToTop, refs],
  );

  useEffect(() => {
    handleBlocksIntersected();
  }, []);

  useEffect(() => {
    document.body.addEventListener('scroll', handleBlocksIntersected, true);

    return () => {
      document.body.removeEventListener('scroll', handleBlocksIntersected);
    };
  }, [handleBlocksIntersected, refs]);

  return { intersected };
};

export default useMultiBlockIntersection;
