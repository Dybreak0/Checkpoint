import Vue from 'vue';
import VueI18n from 'vue-i18n';

import messages from '@/i18n';

Vue.use(VueI18n);

// declare a locale for i18n
const locale = '';

// declare a fallbackLocale for i18n
const fallbackLocale = 'en';

// store messages and locale in your VueI18n instance
export const i18n = new VueI18n({
  locale,
  fallbackLocale,
  messages, // lang json files
});

// array for loaded language files
const loadedLanguages = [];

// set selected lang to i18n.locale
function setI18nLanguage(lang) {
  i18n.locale = lang;
  return lang;
}

// loads the lang file of the selected language if it has not been loaded yet
export function loadLanguageAsync(lang) {
  if (i18n.locale !== lang) {
    if (loadedLanguages.indexOf(lang) < 0) {
      return import(/* webpackChunkName: "lang-[request]" */ `@/i18n/${lang}`).then((msgs) => {
        i18n.setLocaleMessage(lang, msgs);
        loadedLanguages.push(lang);
        return setI18nLanguage(lang);
      });
    }
    return Promise.resolve(setI18nLanguage(lang));
  }
  return Promise.resolve(lang);
}
