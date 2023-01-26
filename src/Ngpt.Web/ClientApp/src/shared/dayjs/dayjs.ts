import * as _dayjs from 'dayjs';
import * as dayjsUtcPlugin from 'dayjs/plugin/utc';
import * as durationPlugin from 'dayjs/plugin/duration';

_dayjs.extend(dayjsUtcPlugin);
_dayjs.extend(durationPlugin);

export const dayjs = _dayjs;