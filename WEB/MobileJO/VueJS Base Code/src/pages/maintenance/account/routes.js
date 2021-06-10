import AccountList from './account-list';
import AccountForm from './account-form';
import AccountView from './account-view';
import AccountEdit from './account-edit';


export default [
    { name: 'accountList', path: '/account-list', component: AccountList },
    { name: 'accountNew', path: '/account-form', component: AccountForm },
    { name: 'accountEdit', path: '/account-edit/:id', component: AccountEdit, props: true },
    { name: 'accountView', path: '/account-view/:id', component: AccountView, props: true  },
];
