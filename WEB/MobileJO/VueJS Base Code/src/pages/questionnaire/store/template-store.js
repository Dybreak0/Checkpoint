import serviceUtil from '../../../common/utils/main-service';
import moment from 'moment';
import constants from '../../../common/utils/constants';


export default {
    namespaced: true,

    state: {
        list: [],
        model: [],
        templateDetails: [],
        branches: [],
        questionTypes: [],
        questionModel: [],
        choiceModel: []
    },

    getters: {
        list: state => state.list,
        model: state => state.model,
        templateDetails: state=> state.templateDetails,
        branches: state => state.branches,
        questionTypes: state => state.questionTypes,
        questionModel: state => state.questionModel,
        choiceModel: state=> state.choiceModel
    },

    mutations: {
        LIST(state, payload) {
            state.list = payload || [];
        },

        MODEL(state, payload) {
            state.model = payload || [];
        },
        BRANCHES(state, payload) {
            state.branches = payload || [];
        },

        CLEAR(state) {
            state.list = [];
            state.model = [];
        },

        QUESTIONTYPES(state, payload){
            state.questionTypes = payload || [];
        },
        QUESTIONMODEL(state, payload){
            state.questionModel = payload || [];
        }

    },

    actions: {

        search({ commit }, searchValues) {
            var Title = encodeURIComponent(searchValues.Title);
            var Category = encodeURIComponent(searchValues.Category);
            var page = searchValues.Page;
            var pageSize = searchValues.PageSize;
            commit('LIST', []);

            return serviceUtil
                .list('api/QuestionnaireAPI/list?Title=' + Title + '&Category='+ Category + '&Page='+ page +'&PageSize='+ pageSize)
                .then((list) => {
                    commit('LIST', list);
                });
        },

        delete({ commit }, id) {

            return serviceUtil
                .delete(`api/QuestionnaireAPI/delete?id=${id}`).then((list) => {
                    commit('LIST', list);
                });
        },

        find({ commit }, id) {
            commit('MODEL', []);

            return serviceUtil
                .find(`api/QuestionnaireAPI/find?id=${id}`)
                .then((model) => {
                    if(null != model.data){
                        model.data.start_date = moment(model.data.start_date).format(constants.dateFormat2);
                        model.data.end_date = moment(model.data.end_date).format(constants.dateFormat2);
                        model.data.max_limit = model.data.max_limit.toString();
                    }
                    commit('MODEL', model);
                });
        },

        addQuestion({ commit , getters }) {
            commit('LIST', []);
            return serviceUtil
                .insert('api/QuestionnaireAPI/add_question', JSON.stringify(getters.questionModel[0]))
                .then((list) => {
                    commit('LIST', list);
                });
        },

        questionTypes({ commit }) {
            var data = {};
            var types = [];
            commit('QUESTIONTYPES', []);
            return serviceUtil
                .list('api/QuestionnaireAPI/get_question_types')
                .then((list) => {
                    list.data && list.data.map(data => {
                        var type = {'text': data.Type, 'value': data.QuestionTypeID};
                        types.push(type);
                    })
                    data.data = types;
                    data.message = list.message;
                    commit('QUESTIONTYPES', data);
                });
        },

        add({ commit , getters }) {
            commit('LIST', []);
            return serviceUtil
                .insert('api/QuestionnaireAPI/add', JSON.stringify(getters.model[0]))
                .then((list) => {
                    commit('LIST', list);
                });;
        },

        edit({ commit, getters }) {
            commit('LIST', []);
            return serviceUtil
                .update('api/QuestionnaireAPI/edit', JSON.stringify(getters.model.data))
                .then((list) => {
                    commit('LIST', list);
                });

        },

        addChoice({ commit, getters }){
            commit('LIST', []);
            return serviceUtil
                .insert('api/QuestionnaireAPI/add_choice', JSON.stringify(getters.choiceModel[0]))
                .then((list) => {
                    commit('LIST', list);
                });
        },

        branches({ commit }) {
            commit('BRANCHES', []);
            return serviceUtil
                .list('api/QuestionnaireAPI/get_branches')
                .then((list) => {
                    commit('BRANCHES', list);
                });
        },

        editQuestion({ commit, getters }){
            commit('LIST', []);
            return serviceUtil
                .update('api/QuestionnaireAPI/edit_question', JSON.stringify(getters.questionModel[0]))
                .then((list) => {
                    commit('LIST', list);
                });

        },
        deleteQuestion({ commit }, id) {
            return serviceUtil
                .delete(`api/QuestionnaireAPI/delete_question?id=${id}`).then((list) => {
                    commit('LIST', list);
                });
        },
        deleteChoice({ commit }, id) {
            return serviceUtil
                .delete(`api/QuestionnaireAPI/delete_choice?id=${id}`).then((list) => {
                    commit('LIST', list);
                });
        },
        editChoice({ commit, getters }){
            commit('LIST', []);
            return serviceUtil
                .update('api/QuestionnaireAPI/edit_choice', JSON.stringify(getters.choiceModel[0]))
                .then((list) => {
                    commit('LIST', list);
                });
        },
        clear({ commit }) {
            commit('CLEAR');
        },

    },
};
