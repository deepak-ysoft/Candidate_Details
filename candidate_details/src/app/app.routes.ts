import { Routes } from '@angular/router';
import { CandidateListComponent } from './Components/candidate-list/candidate-list.component';
import { RolesComponent } from './Components/roles/roles.component';
import { LayoutComponent } from './Components/layout/layout/layout.component';
import { CalendarComponent } from './Components/calendar/calendar.component';
import { EmployeesComponent } from './Components/Employee/employees/employees.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', component: CandidateListComponent },
      { path: 'candidateList', component: CandidateListComponent },
      { path: 'roles', component: RolesComponent },
      { path: 'calendar', component: CalendarComponent },
      { path: 'employees', component: EmployeesComponent },
    ],
  },
];
