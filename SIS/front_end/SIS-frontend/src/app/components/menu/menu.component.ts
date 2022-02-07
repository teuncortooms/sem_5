import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';

class MenuConfig {

  home = false;
  student = false;
  students = false;
  groups = false;
  grades = false;
  teachers = false;
  users = false;
  roles = false;
}

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  isUserAuthenticated: boolean = false;
  menuConfig: MenuConfig = new MenuConfig();


  constructor(private authService: AuthenticationService) { }

  ngOnInit(): void {
    this.authService.authChanged.subscribe(isAuth => {
      this.isUserAuthenticated = isAuth;
      this.menuConfig = this.getStaffConfig();
    });
  }

  getStaffConfig(): MenuConfig {
    return {
      ...new MenuConfig(),
      students: this.authService.hasClaimPart('p_student_read_all') || this.authService.hasClaimPart('p_all'),
      student: this.authService.hasClaimPart('p_student_read_own'),
      groups: this.authService.hasClaimPart('p_group_') || this.authService.hasClaimPart('p_all'),
      grades: this.authService.hasClaimPart('p_grade_') || this.authService.hasClaimPart('p_all'),
      teachers: this.authService.hasClaimPart('p_teacher_') || this.authService.hasClaimPart('p_all'),
      users: this.authService.hasClaimPart('p_admin_user_') || this.authService.hasClaimPart('p_all'),
      roles: this.authService.hasClaimPart('p_admin_role_') || this.authService.hasClaimPart('p_all'),
    }
  }
}
