import { environment } from 'src/environments/environment';

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './modules/app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatRadioModule } from '@angular/material/radio';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { AppJwtModule } from './modules/app-jwt.module';
import { MatMenuModule } from '@angular/material/menu';
import { GoogleLoginProvider, MicrosoftLoginProvider, SocialAuthServiceConfig, SocialLoginModule } from 'angularx-social-login';

import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { MenuComponent } from './components/menu/menu.component';
import { GroupsOverviewComponent } from './components/groups/groups-overview/groups-overview.component';
import { EditGroupDialogComponent } from './components/groups/dialogs/edit-group-dialog/edit-group-dialog.component';
import { GroupDetailsDialogComponent } from './components/groups/dialogs/group-details-dialog/group-details-dialog.component';
import { StudentsListComponent } from './components/students/shared/students-list/students-list.component';
import { StudentsOverviewComponent } from './components/students/students-overview/students-overview.component';
import { StudentDetailsDialogComponent } from './components/students/dialogs/student-details-dialog/student-details-dialog.component';
import { EditStudentDialogComponent } from './components/students/dialogs/edit-student-dialog/edit-student-dialog.component';
import { TeachersOverviewComponent } from './components/teachers/teachers-overview/teachers-overview.component';
import { AssignGroupDialogComponent } from './components/students/dialogs/assign-group-dialog/assign-group-dialog.component';
import { EditTeacherDialogComponent } from './components/teachers/edit-teacher-dialog/edit-teacher-dialog.component';
import { GroupsListComponent } from './components/groups/shared/groups-list/groups-list.component';
import { GroupsTableComponent } from './components/groups/shared/groups-table/groups-table.component';
import { GradesOverviewComponent } from './components/grades/grades-overview/grades-overview.component';
import { AssignStudentDialogComponent } from './components/groups/dialogs/assign-student-dialog/assign-student-dialog.component';
import { StudentsTableComponent } from './components/students/shared/students-table/students-table.component';
import { SelectModelComponent } from './components/shared/select-model/select-model.component';
import { SelectModelListComponent } from './components/shared/select-model-list/select-model-list.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { UserProfileComponent } from './components/authentication/user-profile/user-profile.component';

import { LoggerService } from './services/logger.service';
import { HttpErrorInterceptorService } from './interceptors/http-error-interceptor.service';
import {
  apiServiceFactory, dataSourceFactory, GENERIC_API_TOKEN, GENERIC_DATA_TOKEN, GRADES_API_TOKEN, GRADES_DATA_TOKEN,
  groupsApiServiceFactory, GROUPS_API_TOKEN, GROUPS_DATA_TOKEN, STUDENTS_API_TOKEN, STUDENTS_DATA_TOKEN, TEACHERS_DATA_TOKEN,
  TEACHERS_API_TOKEN,
  USER_DATA_TOKEN,
  USER_API_TOKEN,
  GENERICSUB_API_TOKEN,
  GENERICSUB_DATA_TOKEN
} from "./services/dependency-injection/di-config";
import { UsersOverviewComponent } from './components/users/users-overview/users-overview.component';
import { GenericTableConfig, GenericTableServiceComponent } from './components/shared/generic-table/generic-table-service.component';
import { GenericDetailsDialogComponent } from './components/shared/generic-details-dialog/generic-details-dialog.component';
import { RolesOverviewComponent } from './components/roles/roles-overview/roles-overview.component';
import { GenericEditDialogComponent } from './components/shared/generic-edit-dialog/generic-edit-dialog.component';
import { SelectModelsListComponent } from './components/shared/select-models-list/select-models-list.component';
import { AssignModelsDialogComponent } from './components/shared/assign-models-dialog/assign-models-dialog.component';
import { StudentDetailsComponent } from './components/students/student-details/student-details.component';



@NgModule({
  declarations: [
    AppComponent,
    GroupsOverviewComponent,
    HeaderComponent,
    EditGroupDialogComponent,
    GroupDetailsDialogComponent,
    StudentsListComponent,
    MenuComponent,
    StudentsOverviewComponent,
    StudentDetailsDialogComponent,
    EditStudentDialogComponent,
    TeachersOverviewComponent,
    AssignGroupDialogComponent,
    GroupsListComponent,
    EditTeacherDialogComponent,
    GroupsTableComponent,
    GradesOverviewComponent,
    AssignStudentDialogComponent,
    StudentsTableComponent,
    SelectModelComponent,
    SelectModelListComponent,
    NotFoundComponent,
    UserProfileComponent,
    UsersOverviewComponent,
    GenericTableServiceComponent,
    GenericDetailsDialogComponent,
    RolesOverviewComponent,
    GenericEditDialogComponent,
    SelectModelsListComponent,
    AssignModelsDialogComponent,
    StudentDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatSidenavModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    FormsModule,
    MatInputModule,
    MatToolbarModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule,
    MatSortModule,
    HttpClientModule,
    MatListModule,
    MatPaginatorModule,
    MatSelectModule,
    MatCheckboxModule,
    MatGridListModule,
    FlexLayoutModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatSnackBarModule,
    MatMenuModule,
    AppJwtModule,
    SocialLoginModule,
  ],
  providers: [
    MatDatepickerModule,
    { provide: GROUPS_DATA_TOKEN, useFactory: dataSourceFactory, deps: [GROUPS_API_TOKEN, LoggerService] },
    { provide: STUDENTS_DATA_TOKEN, useFactory: dataSourceFactory, deps: [STUDENTS_API_TOKEN, LoggerService] },
    { provide: GRADES_DATA_TOKEN, useFactory: dataSourceFactory, deps: [GRADES_API_TOKEN, LoggerService] },
    { provide: TEACHERS_DATA_TOKEN, useFactory: dataSourceFactory, deps: [TEACHERS_API_TOKEN, LoggerService] },
    { provide: GENERIC_DATA_TOKEN, useFactory: dataSourceFactory, deps: [GENERIC_API_TOKEN, LoggerService] },
    { provide: USER_DATA_TOKEN, useFactory: dataSourceFactory, deps: [USER_API_TOKEN, LoggerService] },

    { provide: GROUPS_API_TOKEN, useFactory: groupsApiServiceFactory, deps: [HttpClient, LoggerService] },
    { provide: STUDENTS_API_TOKEN, useFactory: apiServiceFactory, deps: [HttpClient, LoggerService] },
    { provide: GRADES_API_TOKEN, useFactory: apiServiceFactory, deps: [HttpClient, LoggerService] },
    { provide: TEACHERS_API_TOKEN, useFactory: apiServiceFactory, deps: [HttpClient, LoggerService] },
    { provide: GENERIC_API_TOKEN, useFactory: apiServiceFactory, deps: [HttpClient, LoggerService] },

    { provide: USER_API_TOKEN, useFactory: apiServiceFactory, deps: [HttpClient, LoggerService] },

    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptorService, multi: true },
    {
      provide: 'SocialAuthServiceConfig', useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(environment.Authentication.Google.ClientId)
          },
          {
            id: MicrosoftLoginProvider.PROVIDER_ID,
            provider: new MicrosoftLoginProvider(environment.Authentication.Microsoft.clientId)
          }
        ]
      } as SocialAuthServiceConfig
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }


