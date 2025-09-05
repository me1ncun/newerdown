import { cloneElement, memo, useEffect, useRef, useState } from 'react';
import cx from 'classnames';
import useIntersection from '@react-hook/intersection-observer';

import { useCustomTracking } from '../../hooks/useCustomTracking';

import styles from './IntersectionTopLine.module.css';

const IntersectionTopLine = ({
  children: child,
  thresholdValue = 0.01,
  rootMarginValue = '0px',
  withoutAnimation,
  onIntersect = () => {},
  trackActionValue,
}) => {
  const intersectionRef = useRef(null);
  const [intersected, setIntersected] = useState(false);

  const { trackAction } = useCustomTracking();

  const { intersectionRatio } = useIntersection(intersectionRef, {
    threshold: thresholdValue,
    rootMargin: rootMarginValue,
  });

  useEffect(() => {
    if (intersectionRatio >= thresholdValue) {
      setIntersected(true);
    }
  }, [intersectionRatio]);

  useEffect(() => {
    if (intersected) {
      onIntersect();
    }
  }, [onIntersect, intersected]);

  useEffect(() => {
    if (intersected && trackActionValue) {
      trackAction({ action: trackActionValue });
    }
  }, [intersected, trackActionValue]);

  return cloneElement(child, {
    ref: intersectionRef,
    className: cx(
      {
        [styles.moveLinesTop]: !intersected,
        [styles.noTransition]: withoutAnimation,
      },
      child.props.className,
      styles.moveLinesTransition,
    ),
  });
};

export default memo(IntersectionTopLine);
