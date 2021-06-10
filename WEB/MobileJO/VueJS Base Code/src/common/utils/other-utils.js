import { app } from '../../main';

export default {
  name: 'otherUtils',

  listToOptions(list) {
    if (!list) {
      return [];
    }

    return list.map(i => this.modelToOption(i));
  },

  modelToOption(model) {
    if (!model) {
      return null;
    }

    return {
      id: model.id,
      label: model.name,
    };
  },

  // gets message from lang file
  // this is currently used in dialog boxes
  getMessage(key) {
    if (!key) {
      return '';
    }

    const lang = app.$i18n.locale;
    let message = app.$i18n.messages.en;

    if (lang === 'ja') {
      message = app.$i18n.messages.ja;
    }

    return message[key];
  },

};
