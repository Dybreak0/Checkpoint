import ForgotPassword from './forgot-password';
import ChangePassword from './change-password';


export default [
  { name: 'forgotPassword', path: '/forgotPassword', component: ForgotPassword },
  { name: 'changePassword', path: '/changePassword/:userId/:token', component: ChangePassword, props: true }
];
