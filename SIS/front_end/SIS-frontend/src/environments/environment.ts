// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { HttpHeaders } from "@angular/common/http";

var myAPI = "unknown";

switch (window.location.hostname) {
  case "localhost": myAPI = "https://localhost:44389/"; break;
  case "leerlingvolgsysteem-test.azurewebsites.net": myAPI = "https://musthavecaffeine-test.azurewebsites.net/"; break;
  case "leerlingvolgsysteem.azurewebsites.net": myAPI = "https://musthavecaffeine.azurewebsites.net/"; break;
}

export const environment = {
  production: false,
  myAPI: myAPI,
  pagedEndpoint: myAPI,
  groupsEndpoint: myAPI + "groups",
  studentsEndpoint: myAPI + "students",
  gradesEndpoint: myAPI + "grades",
  teachersEndpoint: myAPI + "teachers",
  assignStudentsToGroupsEndPoint: myAPI + "groups/addstudentstogroups",
  unassignStudentsFromGroupsEndPoint: myAPI + "groups/removestudentsfromgroups",
  registrationEndpoint: myAPI + "authenticate/register",
  loginEndpoint: myAPI + "authenticate/login",
  externalLoginEndpoint: myAPI + "authenticate/externallogin",
  userDetailsEndpoint: myAPI + "authenticate/details",

  httpOptions: {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
      //Authorization: 'my-auth-token'
    })
  },

  jwtInterceptorConfig: {
    allowedDomains: ["localhost:44389", "localhost:5001", "musthavecaffeine-test.azurewebsites.net"],
    // disallowedRoutes: ["http://example.com/examplebadroute/"]
  },

  Authentication: {
    Google: {
      ClientId: "311001800198-oq8570gqn7mu28arrq9eb5hh43bg8178.apps.googleusercontent.com"
    },
    Microsoft: {
      clientId: '2626695a-e45d-4379-895a-6f759b5051df',
    }
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
