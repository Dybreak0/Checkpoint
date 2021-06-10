import NoAuthorization from "./NoAuthorization";
import PageNotFound from "./PageNotFound";

export default [
  { name: "NoAuthorization", path: "/error/401", component: NoAuthorization },
  { name: "PageNotFound", path: "/error/404", component: PageNotFound }
];
