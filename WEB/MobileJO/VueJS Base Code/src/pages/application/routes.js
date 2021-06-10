import TemplateList from './pages/template-list';
import TemplateForm from './pages/template-form';
import TemplateDetails from './pages/template-view';
import InstallmentCreate from './pages/installment-create';
import InstallmentView from './pages/installment-view';
import InstallmentList from './pages/installment-list';
import InstallmentApproval from './pages/installent-approval';
export default [{
        name: 'templateList',
        path: '/questionnaire/template',
        component: TemplateList
    },
    {
        name: 'templateForm',
        path: '/questionnaire/template-form',
        component: TemplateForm
    },
    {
        name: 'templateDetail',
        path: '/questionnaire/template/detail/:id',
        component: TemplateDetails,
        props: true
    },
    {
        name: 'applicationCreate',
        path: '/application/create',
        component: InstallmentCreate
    },
    {
        name: 'installmentList',
        path: '/application/list',
        component: InstallmentList
    },
    {
      name: 'installmentView',
      path: '/application/view/detail/:loan_id',
      component: InstallmentView,
      props: true
  },
    {
        name: 'installmentApproval',
        path: '/application/approval',
        component: InstallmentApproval
    },
];
