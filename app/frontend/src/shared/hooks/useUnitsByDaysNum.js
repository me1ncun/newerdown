import { useCustomTranslation } from '~modules-public/translations';

const DAYS_IN_MONTH = 30;
const DAYS_IN_WEEK = 7;

const UNIT_NAMES = {
  day: 'day',
  week: 'week',
  month: 'month',
};

const getUnitsFromNumber = (daysNum) => {
  const isMonths = daysNum >= DAYS_IN_MONTH && daysNum % DAYS_IN_MONTH === 0;
  const isWeeks = !isMonths && daysNum >= DAYS_IN_WEEK && daysNum % DAYS_IN_WEEK === 0;
  const isDays = !isMonths && !isWeeks;

  const unitsMap = [
    { name: [UNIT_NAMES.day], isItem: isDays, divider: 1 },
    { name: [UNIT_NAMES.week], isItem: isWeeks, divider: DAYS_IN_WEEK },
    { name: [UNIT_NAMES.month], isItem: isMonths, divider: DAYS_IN_MONTH },
  ];

  const { divider, ...rest } = unitsMap.find(({ isItem }) => !!isItem);

  const totalNum = daysNum / divider;
  const isMultiple = totalNum > 1;

  return {
    ...rest,
    totalNum,
    isMultiple,
  };
};

const useUnitsByDaysNum = ({ totalDaysNum, isSingle = false }) => {
  const { t } = useCustomTranslation('common');
  const { name, totalNum, isMultiple } = getUnitsFromNumber(totalDaysNum);
  const isMultipleNeeded = isMultiple && !isSingle;
  const single = t(
    `affemity.tariffPeriodUnits.${name}.${isMultipleNeeded ? 'multiple' : 'single'}`,
  );

  return { unitsNum: totalNum, unitsText: single };
};

export default useUnitsByDaysNum;
