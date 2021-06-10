import roles from '../person-enumerators/person-role';

export default {
  default() {
    return {
      role: roles.list()[0],
    };
  },
};
