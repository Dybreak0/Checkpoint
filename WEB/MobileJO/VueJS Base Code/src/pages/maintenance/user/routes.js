import UserList from './user-list';
import UserForm from './user-form';
import UserView from './user-view';
import UserEdit from './user-edit';


export default [{
        name: 'userList',
        path: '/user-list',
        component: UserList
    },
    {
        name: 'userNew',
        path: '/user-form',
        component: UserForm
    },
    {
        name: 'userEdit',
        path: '/user-edit/:id',
        component: UserEdit,
        props: true
    },
    {
        name: 'userView',
        path: '/user-view/:id',
        component: UserView,
        props: true
    },
];
