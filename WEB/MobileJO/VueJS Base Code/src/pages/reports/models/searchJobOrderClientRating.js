import moment from 'moment';
import constants from '../../../common/utils/constants';

export default {
  components: {
    constants
  },
  default() {  
    return {
      page: 1,
      page_size: 10,
      job_order_number: "",
      case_number: "",
      application_type: 0,
      reported_by: "",
      account_name: "",
      job_order_date_from: moment(new Date()).format(constants.dateFormat2),
      job_order_date_to: moment(new Date()).format(constants.dateFormat2)
    };
  },
};
  