import JobOrderReport from "./pages/jobOrder-list";
import JobOrderDetail from "./pages/jobOrder-detail";
import AssignedCaseReport from "./pages/assignedCase-list";
import AssignedCaseDetail from "./pages/assignedCase-detail";
import JobOrderClientRatingReport from "./pages/jobOrderClientRating-list";

export default [
  {
    name: "jobOrderReport",
    path: "/report/job-order",
    component: JobOrderReport
  },
  {
    name: "jobOrderDetail",
    path: "/report/job-order/detail/:id",
    component: JobOrderDetail,
    props: true
  },
  {
    name: "assignedCaseReport",
    path: "/report/assigned-case",
    component: AssignedCaseReport
  },
  {
    name: "assignedCaseDetail",
    path: "/report/assigned-case/detail/:id",
    component: AssignedCaseDetail,
    props: true
  },
  {
    name: "jobOrderClientRatingReport",
    path: "/report/job-order-client-rating",
    component: JobOrderClientRatingReport
  }
];
