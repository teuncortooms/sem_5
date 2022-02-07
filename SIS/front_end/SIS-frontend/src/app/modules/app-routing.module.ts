import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GradesOverviewComponent } from '../components/grades/grades-overview/grades-overview.component';
import { GroupsOverviewComponent } from '../components/groups/groups-overview/groups-overview.component';
import { NotFoundComponent } from '../components/not-found/not-found.component';
import { RolesOverviewComponent } from '../components/roles/roles-overview/roles-overview.component';
import { StudentDetailsComponent } from '../components/students/student-details/student-details.component';
import { StudentsOverviewComponent } from '../components/students/students-overview/students-overview.component';
import { TeachersOverviewComponent } from '../components/teachers/teachers-overview/teachers-overview.component';
import { UsersOverviewComponent } from '../components/users/users-overview/users-overview.component';
import { AuthGuard } from '../guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/authentication/details', pathMatch: 'full' },
  { path: 'studenten', component: StudentsOverviewComponent, canActivate: [AuthGuard] },
  { path: 'student', component: StudentDetailsComponent, canActivate: [AuthGuard] },
  { path: 'klassen', component: GroupsOverviewComponent, canActivate: [AuthGuard] },
  { path: 'cijfers', component: GradesOverviewComponent, canActivate: [AuthGuard] },
  { path: 'docenten', component: TeachersOverviewComponent, canActivate: [AuthGuard] },
  { path: 'gebruikers', component: UsersOverviewComponent, canActivate: [AuthGuard] },
  { path: 'rollen', component: RolesOverviewComponent, canActivate: [AuthGuard] },

  {
    path: 'authentication', loadChildren: () =>
      import('./authentication.module').then(m => m.AuthenticationModule)
  },
  { path: '404', component: NotFoundComponent },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
