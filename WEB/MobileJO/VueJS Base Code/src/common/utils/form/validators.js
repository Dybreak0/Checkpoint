const emailFormat = /^(([^<>()\[\]\\*.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,6}))$/;
const phoneNumberFormat = /^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$/;
const alphabetFormat = /[A-Z]/;
const numericFormat = /[0-9]/;
const textFormat = /^[a-zA-Z0-9@#$%&()_=.,\s-]+$/;
const accountTextFormat = /^[a-zA-Z0-9@#$%&()_=.,\s-/–]+$/;
const emailtextFormat = /^[a-zA-Z0-9@.\s-]+$/;
const usernameFormat = /.*([ \t]).*/;
import moment from 'moment';
import constants from '../../../common/utils/constants';
const emptyUser = '{"first_name":"","last_name":"","memo":"","telephone_number":"","mobile_number":"","email_address":"","address":"","user_name":"","password":"","c_password":"","allowed_to_login":true}';
const emptyAccount = '{"name":"","memo":"","email":"","contact_person":"","contact_number":"","address":"","created_by":"","created_date":""}';
const emptyTemplate = '{"title":"","description":"","respondents":[],"start_date":"' + moment(new Date()).format(constants.dateFormat2) + '","end_date":"' + moment(new Date()).format(constants.dateFormat2) + '","max_limit":""}'

const string = (label, required, min, max) => {
  const output = [];

  if (required) {
    output.push({ required: !!required, message: `${label} is required` });
  }

  if (min || min === 0) {
    output.push({ min, message: `${label} length must be at least ${min}` });
  }

  if (max || max === 0) {
    output.push({ max, message: `${label} length must be a maximum of ${max}` });
  }

  return output;
};

const decimal = (label, required, min, max) => {
  const output = [
    { type: 'number', message: `${label} should be a number` },
  ];

  if (required) {
    output.push({ type: 'number', required: !!required, message: `${label} is required` });
  }

  if (min || min === 0) {
    output.push({ type: 'number', min, message: `${label} must be at least ${min}` });
  }

  if (max || max === 0) {
    output.push({ type: 'number', max, message: `${label} must be a maximum of ${max}` });
  }

  return output;
};

const integer = (label, required, min, max) => {
  const integerFn = (rule, value, callback) => {
    if (!value) {
      callback();
      return;
    }

    if (Number.isInteger(value)) {
      callback();
      return;
    }

    callback(new Error('Value should be an integer'));
  };

  return [
    { validator: integerFn, message: `${label} should be an integer` },
    ...decimal(label, required, min, max),
  ];
};

const generic = (label, required) => {
  const output = [];

  if (required) {
    output.push({ required: true, message: `${label} is required` });
  }

  return output;
};

const hasOne = (label, required) => {
  const requiredFn = (rule, value, callback) => {
    if (!value) {
      callback(new Error('Value is required'));
      return;
    }

    callback();
  };

  if (!required) {
    return [];
  }

  return [
    { required: true, validator: requiredFn, message: `${label} is required` },
  ];
};

const hasMany = (label, required, min, max) => {
  const requiredFn = (rule, value, callback) => {
    if (!value || !value.length) {
      callback(new Error('Value is required'));
      return;
    }

    callback();
  };

  const minFn = (rule, value, callback) => {
    if (!value || !value.length) {
      callback();
      return;
    }

    if (value.length < min) {
      callback(new Error(`Value size must be at least ${min}`));
    }

    callback();
  };

  const maxFn = (rule, value, callback) => {
    if (!value || !value.length) {
      callback();
      return;
    }

    if (value.length > max) {
      callback(new Error(`Value size must be a maximum of ${max}`));
    }

    callback();
  };

  const output = [];

  if (required) {
    output.push({ required: true, validator: requiredFn, message: `${label} is required` });
  }

  if (min || min === 0) {
    output.push({ validator: minFn, message: `${label} size must be at least ${min}` });
  }

  if (max || max === 0) {
    output.push({ validator: maxFn, message: `${label} size must be a maximum of ${max}` });
  }

  return output;
};

export default {
    string,
    integer,
    decimal,
    generic,
    hasOne,
    hasMany,
    emailFormat,
    phoneNumberFormat,
    alphabetFormat,
    numericFormat,
    emptyUser,
    emptyAccount,
    textFormat,
    accountTextFormat,
    usernameFormat,
    emailtextFormat,
    emptyTemplate
};
