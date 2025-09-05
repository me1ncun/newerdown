import { useContext } from 'react';

import { ScrollContext } from '~modules-public/scroll/providers/ScrollProvider.tsx';

const useScroll = () => {
  const { scrollElementRef, scrollContainerRef, scrollElementsRefs } = useContext(ScrollContext);

  const scrollToElement = (delay = 0, block = 'end') => {
    setTimeout(() => {
      scrollElementRef.current.scrollIntoView({
        block,
        behavior: 'smooth',
      });
    }, delay);
  };

  const scrollToTop = () => {
    scrollContainerRef.current.scrollIntoView({ behavior: 'smooth' });
  };

  const scrollToTopDelayed = (delay = 150) => {
    // to scroll view after keyboard on mobile shows up
    setTimeout(() => scrollContainerRef.current.scrollIntoView({ behavior: 'smooth' }), delay);
  };

  const scrollToMultipleElement = (name, delay = 0, block = 'start') => {
    setTimeout(() => {
      scrollElementsRefs.current[name].current.scrollIntoView({
        block,
        behavior: 'smooth',
      });
    }, delay);
  };

  const addItemsToScrollElementsRefs = (newRefs = {}) => {
    scrollElementsRefs.current = {
      ...scrollElementsRefs.current,
      ...newRefs,
    };

    return scrollElementsRefs.current;
  };

  const scrollToNearestBlock = () => {
    const { block1, block2 } = scrollElementsRefs.current;
    const hasBlock1 = block1 && block1.current;
    const hasBlock2 = block2 && block2.current;
    if (!hasBlock1 && !hasBlock2) return;
    if (hasBlock1 && !hasBlock2) {
      block1.current.scrollIntoView({ behavior: 'smooth', block: 'start' });
      return;
    }
    if (!hasBlock1 && hasBlock2) {
      block2.current.scrollIntoView({ behavior: 'smooth', block: 'start' });
      return;
    }

    const block1Top = block1.current.getBoundingClientRect().top;
    const block2Top = block2.current.getBoundingClientRect().top;
    const distanceToBlock1 = Math.abs(block1Top);
    const distanceToBlock2 = Math.abs(block2Top);
    const nearest = distanceToBlock1 < distanceToBlock2 ? block1 : block2;
    nearest.current.scrollIntoView({ behavior: 'smooth', block: 'start' });
  };

  const scrollToNearestBlockWithCallback = (onBeforeScroll) => {
    const { block1, block2 } = scrollElementsRefs.current;
    const hasBlock1 = block1 && block1.current;
    const hasBlock2 = block2 && block2.current;
    if (!hasBlock1 && !hasBlock2) return;
    if (hasBlock1 && !hasBlock2) {
      onBeforeScroll?.('block1');
      block1.current.scrollIntoView({ behavior: 'smooth', block: 'start' });
      return;
    }
    if (!hasBlock1 && hasBlock2) {
      onBeforeScroll?.('block2');
      block2.current.scrollIntoView({ behavior: 'smooth', block: 'start' });
      return;
    }

    const block1Top = block1.current.getBoundingClientRect().top;
    const block2Top = block2.current.getBoundingClientRect().top;
    const distanceToBlock1 = Math.abs(block1Top);
    const distanceToBlock2 = Math.abs(block2Top);
    const nearest = distanceToBlock1 < distanceToBlock2 ? block1 : block2;
    const blockName = nearest === block1 ? 'block1' : 'block2';

    onBeforeScroll?.(blockName);
    nearest.current.scrollIntoView({ behavior: 'smooth', block: 'start' });
  };

  return {
    scrollElementRef,
    scrollContainerRef,
    scrollElementsRefs,
    scrollToElement,
    scrollToMultipleElement,
    scrollToTopDelayed,
    scrollToTop,
    addItemsToScrollElementsRefs,
    scrollToNearestBlock,
    scrollToNearestBlockWithCallback,
  };
};

export default useScroll;
