import moment from 'moment';
import constants from '../../../common/utils/constants';
export default {
  default() {
    return {
      application_no: "",
      client_name: "",
      created_by_name: "",
      application_type: "",
      status: "All",
      date_from: moment().subtract(7, 'days').format(constants.dateFormat2),
      date_to: moment(new Date()).format(constants.dateFormat2),
      page: 1,
      page_size: 10,
    };
  },
};
