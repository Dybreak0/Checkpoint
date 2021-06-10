import formatDateFilter from './utils/filters/format-date-filter';
import formatDatetimeFilter from './utils/filters/format-datetime-filter';
import autofocusMixin from './utils/mixins/autofocus-mixin';
import otherUtils from './utils/other-utils';

export default {
  filters: [
    formatDateFilter,
    formatDatetimeFilter,
  ],

  mixins: [
    autofocusMixin,
  ],

  otherUtils,
};
