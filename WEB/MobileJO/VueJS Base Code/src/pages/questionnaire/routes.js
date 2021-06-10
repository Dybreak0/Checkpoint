import TemplateList from './pages/template-list';
import TemplateForm from './pages/template-form';
import TemplateDetails from './pages/template-view';
import TemplateEdit from './pages/template-edit';
import ResponseView from './pages/response-view';
import ResponseList from './pages/response-list';

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
        name: 'templateEdit',
        path: '/questionnaire/template/edit/:id',
        component: TemplateEdit,
        props: true
    },
    {
        name: 'responseView',
        path: '/questionnaire/response-view/:id',
        component: ResponseView,
        props: true
    },
    {
        name: 'responseList',
        path: '/questionnaire/response-list',
        component: ResponseList
    },
];
