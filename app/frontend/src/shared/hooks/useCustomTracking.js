import { useCallback } from 'react';

import useCustomTrackingInfo from '../../../../../system-host/modules-private/tracking/hooks/useCustomTrackingInfo';

import { useStepData } from '../../../../../system-host/modules-public/step';

export const useCustomTracking = () => {
  const { sendCustomTrackingInfo } = useCustomTrackingInfo();
  const { stepName } = useStepData();

  const trackAction = useCallback(
    ({ action }) => {
      sendCustomTrackingInfo({
        step: stepName,
        action,
      });
    },
    [sendCustomTrackingInfo, stepName],
  );

  return { trackAction };
};
