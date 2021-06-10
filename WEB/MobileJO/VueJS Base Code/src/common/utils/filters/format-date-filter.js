import moment from 'moment';

export default {
  name: 'formatDate',
  implementation(value) {
    if (value) {
      return moment(value).format('MM/DD/YYYY');
    }

    return null;
  },
};
