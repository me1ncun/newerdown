import { useEffect } from 'react';

function useBodyScrollLock(isOpen) {
  useEffect(() => {
    const preventMotion = (event) => {
      event.preventDefault();
      event.stopPropagation();
    };

    if (isOpen) {
      document.body.style.overflow = 'hidden';

      window.addEventListener('scroll', preventMotion, { passive: false });
      window.addEventListener('touchmove', preventMotion, { passive: false });
    } else {
      document.body.style.overflow = '';

      window.removeEventListener('scroll', preventMotion);
      window.removeEventListener('touchmove', preventMotion);
    }

    return () => {
      document.body.style.overflow = '';

      window.removeEventListener('scroll', preventMotion);
      window.removeEventListener('touchmove', preventMotion);
    };
  }, [isOpen]);
}

export default useBodyScrollLock;
