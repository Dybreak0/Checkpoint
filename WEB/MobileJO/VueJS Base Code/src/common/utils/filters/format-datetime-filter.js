import moment from 'moment';

export default {
  name: 'formatDatetime',
  implementation(value) {
    if (value) {
      return moment(value).format('MM/DD/YYYY HH:mm');
    }

    return null;
  },
};
