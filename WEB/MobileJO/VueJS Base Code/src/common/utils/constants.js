//messages
const noInternet = "An error was encountered while processing the request. Please refresh the page and try again. If the problem still occurs, contact your administrator.";
const fillRequireFieldsError = "Please fill up all required fields.";
const nameFormatError = "Must be greater than 4 characters.";
const passordNotMatchingError = "Passwords do not match.";
const invalidPhoneNoError = "Not a valid calling number.";
const invalidEmailError = "Invalid email address.";
const networkError = "An error was encountered while processing the request. Please refresh the page and try again. If the problem still occurs, contact your administrator.";
const fileNotFound = "File cannot be found."
const saveConfirm = "Are you sure you want to save?";
const deleteConfirm = "Are you sure you want to delete this record?";
const cancelConfirm = "Are you sure you want to cancel? This will discard all the changes on the form.";
const cancelLogout = "Are you sure you want to logout?";
const greaterThanZero = "Must be greater than 0";

const noResults = "No results found.";
const notAvailable = "Record is no longer available.";
const recordInvalid = "Record Invalid.";
const recordHasPendingJOs = "Record has pending JOs.";
const noRecords = "No records found.";
const noData = "Empty";
const maxCharsReached = "Maximum characters reached.";
const nameFormatErrorMax = "Must be lesser or equal than 20 characters";
const saved = "Record successfully saved.";
const recordNotExist = "Record does not exist.";
const notAdmin = "Cannot perform action. User is no longer an administrator.";
const deletedUser = "Your account has been deleted.";
const failedSave = "Failed to save record.";
const sessionTimeout = "Session expired. Please log in again to continue.";
const invalidUsername = "Invalid username. It must not contain spaces.";
const invalidInput = "Must not contain invalid characters.";
const maxEmail = "Cannot add more than 50 email addresses.";
const approveRevert = "Are you sure you want to approve this revert request?";
const denyRevert = "Are you sure you want to deny this revert request?";
const forgetPasswordSent = "Email has been successfully sent.";
const CannotDeleteActiveTemplate = "Cannot delete active template.";
const SuccesfullyDeleted= "Record successfully deleted.";

//common
const defaultSelect = "Please Select";
const defaultEmail = { "email": [] };
const yes = "Yes";
const no = "No";
const defaultTableAlign = "center";
const prevPage = "prevPage";
const xls = ".xls";
const excelApplicationType = "application/vnd.ms-excel";
const octetApplicationType = "application/octet-stream";
const dateFormat = "YYYYMMDD";
const dateFormat2 = "YYYY-MM-DD";
const jobOrderReportExcel = "TS JO Report_";
const jobOrderClientRatingReportExcel = "JO Client Rating Report_";
const assignedCaseReportExcel = "Assigned Cases Report_";
const all = "All";
const applicationType = "application_type";
const status = "status";
const success = "Success.";
const undefined = "undefined";
const token = "token";
const responseReportFileName = "Response Report_";

const toID = 1;
const ccID = 2;
const bccID = 3;
const minPassword = 4;

const questionText = 1;
const questionCheckbox = 2;
const questionRadio = 3;
const questionVideo = 5;
const questionImage = 6;
const questionSlider = 7;
const RangeTypeID = 7;


//response signalr
const getVideo = "GetVideo";
const getImage = "GetImage";
const receiveVideo = "ReceiveVideo";
const receiveImage = "ReceiveImage";
const videoURL = "http://checkpoint.alliance.com.ph:8011/video-hub";
const imageURL = "http://checkpoint.alliance.com.ph:8011/image-hub";


//modal
const message = "MESSAGE";
const confirm = "CONFIRM";
const warning = "WARNING";

//color
const modal_color = "red";
const error_color = "black";
const invalid_email_color = "red";
const valid_email_color = "gray";

//routes
const defaultRoute = "/";
const home = "/application/list";
const userList = "/user-list";
const userEdit = "/user-edit/";
const userView = "/user-view/";
const userForm = "/user-form";

const accountList = "/account-list";
const accountEdit = "/account-edit/";
const accountView = "/account-view/";
const accountForm = "/account-form";

const loginRoute = "/login";
const forgotPassword = "forgotPassword";
const templateForm = "templateForm";
const templateList = "/questionnaire/template";
const templateView = "/questionnaire/template/detail/";
const templateEdit = "/questionnaire/template/edit/";

const jobOrderDetail = "jobOrderDetail";
const jobOrderReport = "jobOrderReport";
const assignedCaseDetail = "assignedCaseDetail";
const assignedCaseReport = "assignedCaseReport";
const jobOrderClientRatingReport = "jobOrderClientRatingReport";
const revertJO = "revertJOList";

const responseView = "/questionnaire/response-view/"
const responseList = "/questionnaire/response-list/"

const clearLogin = `login/logout`;
const clearUsers = `user/user/clear`;
const clearAccounts = `account/account/clear`;
const clearEmails = `emailSetup/emailSetup/clear`;
const clearJobOrders = `reports/jobOrderStore/clear`;
const clearCases =  `reports/assignedCaseStore/clear`;
const clearRating = `reports/jobOrderClienRatingStore/clear`;
const clearResponse = `questionnaire/response/clear`;
const clearTemplate = `questionnaire/template/clear`;

//getters
const loginError = "login/error";
const login = "login/login";
const isLoggedIn = "login/isLoggedIn";
const loginRole = "login/role";
const loginClear = "login/clear"
const execRefreshToken = 'login/getAccessToken';
const refreshToken = "login/refreshToken";

const listEmail = "emailSetup/emailSetup/list";
const listMessage = "emailSetup/emailSetup/message";
const emailModel = "emailSetup/emailSetup/object";
const saveEmails = "emailSetup/emailSetup/save";

const user = "user/user/user";
const getUserRoles = "user/user/getRoles";
const userRoles = "user/user/roles";
const getUserTypes = "user/user/getUserTypes";
const userTypes = "user/user/types";
const getCompanies = "user/user/getCompanies";
const companies = "user/user/companies";
const getBranches = "user/user/getBranches";
const branches = "user/user/branches";
const AddUser = "user/user/add";
const listUsers = "user/user/list";
const userName = "login/userName";
const userID = "login/id";
const userModel = "user/user/model";
const searchUser = "user/user/search";
const destroyUser = "user/user/destroy";
const editUser = "user/user/edit";
const clearUser = "user/user/clear";

const account = "account/account/account";
const AddAccount = "account/account/add";
const listAccount = "account/account/list";
const accountModel = "account/account/model";
const searchAccount = "account/account/search";
const destroyAccount = "account/account/destroy";
const editAccount = "account/account/edit";
const clearAccount = "account/account/clear";

const jobOrderReportNew = "reports/jobOrderStore/new";
const jobOrderReportLoadOptions = "reports/jobOrderStore/loadOptions";
const jobOrderReportList = "reports/jobOrderStore/list";
const jobOrderReportClearFilters = "reports/jobOrderStore/clearFilters";
const jobOrderReportClear = "reports/jobOrderStore/clear";
const jobOrderReportDownload = "reports/jobOrderStore/download";
const jobOrderReportDownloadAttachment = "reports/jobOrderStore/downloadAttachment";
const jobOrderReportOptions = "reports/jobOrderStore/options";
const jobOrderReportSearchModel = "reports/jobOrderStore/searchModel";
const jobOrderReportViewModel = "reports/jobOrderStore/viewModel";
const jobOrderReportSuccessMessage = "reports/jobOrderStore/successMessage";
const jobOrderReportErrorMessage = "reports/jobOrderStore/errorMessage";
const jobOrderReportView = "reports/jobOrderStore/view";
const jobOrderReportGet = "reports/jobOrderStore/get";
const jobOrderReportRevertJO = "reports/jobOrderStore/revertJO";

const assignedCaseReportNew = "reports/assignedCaseStore/new";
const assignedCaseReportLoadOptions = "reports/assignedCaseStore/loadOptions";
const assignedCaseReportList = "reports/assignedCaseStore/list";
const assignedCaseReportClearFilters = "reports/assignedCaseStore/clearFilters";
const assignedCaseReportClear = "reports/assignedCaseStore/clear";
const assignedCaseReportDownload = "reports/assignedCaseStore/download";
const assignedCaseSyncData = "reports/assignedCaseStore/sync";
const assignedCaseReportOptions = "reports/assignedCaseStore/options";
const assignedCaseReportSearchModel = "reports/assignedCaseStore/searchModel";
const assignedCaseReportViewModel = "reports/assignedCaseStore/viewModel";
const assignedCaseReportErrorMessage = "reports/assignedCaseStore/errorMessage";
const assignedCaseReportGet = "reports/assignedCaseStore/get";

const jobOrderClientRatingReportNew = "reports/jobOrderClienRatingStore/new";
const jobOrderClientRatingReportLoadOptions = "reports/jobOrderClienRatingStore/loadOptions";
const jobOrderClientRatingReportList = "reports/jobOrderClienRatingStore/list";
const jobOrderClientRatingReportClearFilters = "reports/jobOrderClienRatingStore/clearFilters";
const jobOrderClientRatingReportDownload = "reports/jobOrderClienRatingStore/download";
const jobOrderClientRatingReportOptions = "reports/jobOrderClienRatingStore/options";
const jobOrderClientRatingReportSearchModel = "reports/jobOrderClienRatingStore/searchModel";
const jobOrderClientRatingReportErrorMessage = "reports/jobOrderClienRatingStore/errorMessage";

const revertJONew = "revertJO/revertJOStore/new";
const revertJOList = "revertJO/revertJOStore/list";
const revertJOClearFilters = "revertJO/revertJOStore/clearFilters";
const revertJORevert = "revertJO/revertJOStore/revertJO";
const revertJOSearchModel = "revertJO/revertJOStore/searchModel";
const revertJOSuccessMessage = "revertJO/revertJOStore/successMessage";
const revertJOErrorMessage = "revertJO/revertJOStore/errorMessage";

//response
const searchResponse = "questionnaire/response/search";
const responseLists = "questionnaire/response/list";
const responseDetail = "questionnaire/response/model";
const findResponseDetail = "questionnaire/response/find";
const editResponseDetail = "questionnaire/response/editResponse";
const responseDownloadExcel = "questionnaire/response/downloadExcel";
const responseDownloadPDF = "questionnaire/response/downloadPDF";

//forgot password
const sendRequest = "forgotPassword/sendRequest";
const sendRequestModel = "forgotPassword/model";
const sendList = "forgotPassword/list";
const checkValidity = "forgotPassword/checkValidity";
const resetPassword = "forgotPassword/resetPassword";

const searchTemplate = "questionnaire/template/search";
const listTemplate = "questionnaire/template/list";
const deleteTemplate = "questionnaire/template/delete";
const branchesList = "questionnaire/template/branches";
const templateModel = "questionnaire/template/model";
const addTemplate = "questionnaire/template/add";
const templateViewDetails = "questionnaire/template/find";
const editTemplate = "questionnaire/template/edit";
const questionTypes = "questionnaire/template/questionTypes";
const questionModel = "questionnaire/template/questionModel";
const addQuestion = "questionnaire/template/addQuestion";
const deleteQuestion = "questionnaire/template/deleteQuestion";
const editQuestion = "questionnaire/template/editQuestion";
const addChoice = "questionnaire/template/addChoice";
const choiceModel = "questionnaire/template/choiceModel";
const deleteChoice = "questionnaire/template/deleteChoice";
const editChoice = "questionnaire/template/editChoice";


//pages
const jobOrderReportPage = "app-jobOrder-list";
const assignedCaseReportPage = "app-assignedCase-list";
const jobOrderClientRatingReportPage = "app-assignedCase-list";
const revertJOPage = "app-revertJO-list";

//table headers
const jobOrderNumberHeader = "JO #";
const caseNumberHeader = "Case #";
const caseSubjectHeader = "Subject";
const detailsHeader = "Details";
const reportedByHeader = "Reported By";

const jobOrderDateStartHeader = "JO Start Date";
const jobOrderDateEndHeader = "JO End Date";
const applicationTypeHeader = "Application Type";
const accountNameHeader = "Account Name";
const statusHeader = "Status";
const clientRatingHeader = "Client Rating";
const actionHeader = "Action";
const jobOrderSubject = "JO Subject";

//mutations
const SEARCHMODEL = "SEARCHMODEL";
const VIEWMODEL = "VIEWMODEL";
const LIST = "LIST";
const CLEAR_FILTERS = "CLEAR_FILTERS";
const CLEAR = "CLEAR";
const ADD_OPTIONS = "ADD_OPTIONS";
const ERROR = "ERROR";
const SUCESSS_MESSAGE = "SUCESSS_MESSAGE";

//user role options
const roleAdmin = "Admin";
const roleUser = "User";
const roleAdminID = "1";
const roleUserID = "2";

const attachInfo = "Maximum file attachment is only 5, The exceeding files are not included.";

const loanModel = "application/installment/model";
const successMessage = "Installment Application Submitted Successfully!";
const loanApplicationExcel = "Loan Application_";
const success_color = "#077401";

const approveApplication = "Are you sure you want to approve this Installment Application ?";
const denyApplication = "Are you sure you want to deny this Installment Application ?";

export default {
    defaultRoute,
    noInternet,
    message,
    confirm,
    warning,
    modal_color,
    error_color,
    loginError,
    login,
    isLoggedIn,
    loginClear,
    home,
    prevPage,
    fillRequireFieldsError,
    nameFormatError,
    passordNotMatchingError,
    invalidPhoneNoError,
    invalidEmailError,
    saveConfirm,
    defaultSelect,
    getUserRoles,
    getCompanies,
    userRoles,
    getUserTypes,
    userTypes,
    getCompanies,
    companies,
    getBranches,
    branches,
    AddUser,
    listUsers,
    userName,
    userModel,
    userList,
    yes,
    no,
    userEdit,
    userView,
    userForm,
    searchUser,
    destroyUser,
    deleteConfirm,
    user,
    noResults,
    notAvailable,
    recordInvalid,
    editUser,
    cancelConfirm,
    accountList,
    accountEdit,
    accountView,
    accountForm,
    account,
    AddAccount,
    listAccount,
    accountModel,
    searchAccount,
    destroyAccount,
    editAccount,
    execRefreshToken,
    refreshToken,
    userID,
    cancelLogout,
    jobOrderReportPage,
    noRecords,
    noData,
    jobOrderNumberHeader,
    caseNumberHeader,
    detailsHeader,
    reportedByHeader,
    jobOrderDateStartHeader,
    jobOrderDateEndHeader,
    applicationTypeHeader,
    statusHeader,
    defaultTableAlign,
    jobOrderReportNew,
    jobOrderReportLoadOptions,
    jobOrderReportList,
    jobOrderReportClearFilters,
    jobOrderReportClear,
    jobOrderReportDownload,
    jobOrderReportDownloadAttachment,
    jobOrderDetail,
    jobOrderReport,
    jobOrderReportOptions,
    jobOrderReportSearchModel,
    jobOrderReportViewModel,
    jobOrderReportSuccessMessage,
    jobOrderReportErrorMessage,
    jobOrderReportView,
    jobOrderReportGet,
    jobOrderReportRevertJO,
    assignedCaseReportPage,
    caseSubjectHeader,
    accountNameHeader,
    assignedCaseReportNew,
    assignedCaseReportLoadOptions,
    assignedCaseReportList,
    assignedCaseReportClearFilters,
    assignedCaseReportClear,
    assignedCaseReportDownload,
    assignedCaseSyncData,
    assignedCaseReportOptions,
    assignedCaseReportSearchModel,
    assignedCaseReportViewModel,
    assignedCaseReportErrorMessage,
    assignedCaseReportGet,
    assignedCaseDetail,
    assignedCaseReport,
    jobOrderClientRatingReportPage,
    clientRatingHeader,
    jobOrderClientRatingReport,
    jobOrderClientRatingReportNew,
    jobOrderClientRatingReportLoadOptions,
    jobOrderClientRatingReportList,
    jobOrderClientRatingReportClearFilters,
    jobOrderClientRatingReportDownload,
    jobOrderClientRatingReportOptions,
    jobOrderClientRatingReportSearchModel,
    jobOrderClientRatingReportErrorMessage,
    approveRevert,
    denyRevert,
    jobOrderReportExcel,
    jobOrderClientRatingReportExcel,
    assignedCaseReportExcel,
    xls,
    all,
    excelApplicationType,
    octetApplicationType,
    dateFormat,
    status,
    applicationType,
    SEARCHMODEL,
    VIEWMODEL,
    LIST,
    CLEAR_FILTERS,
    CLEAR,
    ADD_OPTIONS,
    ERROR,
    SUCESSS_MESSAGE,
    revertJOPage,
    actionHeader,
    revertJO,
    revertJONew,
    revertJOList,
    revertJOClearFilters,
    revertJORevert,
    revertJOSearchModel,
    revertJOSuccessMessage,
    revertJOErrorMessage,
    maxCharsReached,
    nameFormatErrorMax,
    listEmail,
    emailModel,
    saveEmails,
    invalid_email_color,
    valid_email_color,
    success,
    defaultEmail,
    dateFormat2,
    networkError,
    fileNotFound,
    undefined,
    recordNotExist,
    recordHasPendingJOs,
    notAdmin,
    deletedUser,
    undefined,
    saved,
    toID,
    ccID,
    bccID,
    clearAccount,
    clearUser,
    failedSave,
    sessionTimeout,
    invalidUsername,
    invalidInput,
    minPassword,
    clearLogin,
    clearUsers,
    clearAccounts,
    clearEmails,
    clearJobOrders,
    clearCases,
    clearRating,
    jobOrderSubject,
    listMessage,
    maxEmail,
    responseLists,
    clearResponse,
    responseView,
    responseDetail,
    findResponseDetail,
    responseList,
    editResponseDetail,
    getImage,
    getVideo,
    receiveVideo,
    receiveImage,
    videoURL,
    imageURL,
    questionText,
    questionCheckbox,
    questionRadio,
    questionVideo,
    questionImage,
    questionSlider,
    searchResponse,
    responseDownloadExcel,
    responseDownloadPDF,
    responseReportFileName,
    sendRequest,
    sendRequestModel,
    sendList,
    forgetPasswordSent,
    checkValidity,
    resetPassword,
    loginRoute,
    forgotPassword,
    roleAdmin,
    roleUser,
    roleAdminID,
    roleUserID,
    searchTemplate,
    listTemplate,
    deleteTemplate,
    CannotDeleteActiveTemplate,
    templateForm,
    clearTemplate,
    branchesList,
    templateModel,
    addTemplate,
    greaterThanZero,
    templateList,
    templateView,
    templateViewDetails,
    templateEdit,
    editTemplate,
    questionTypes,
    questionModel,
    addQuestion,
    deleteQuestion,
    SuccesfullyDeleted,
    editQuestion,
    addChoice,
    choiceModel,
    deleteChoice,
    editChoice,

    attachInfo,
    loanModel,
    success_color,

    loanApplicationExcel,
    successMessage,
    
    approveApplication,
    denyApplication,
};
