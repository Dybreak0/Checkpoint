import moment from 'moment';
import constants from '../../../common/utils/constants';
import { mapGetters } from "vuex";
export default {
  default() {
    return {
      branch_id: null,
      role: null,
      application_no: "",
      client_name: "",
      date_from: moment().subtract(7, 'days').format(constants.dateFormat2),
      date_to: moment(new Date()).format(constants.dateFormat2),
      created_by_name: "",
      page: 1,
      page_size: 10,
    };
  },
};
