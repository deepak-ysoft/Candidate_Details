import { Routes } from '@angular/router';
import { CandidateListComponent } from './Components/candidate-list/candidate-list.component';
import { RolesComponent } from './Components/roles/roles.component';

export const routes: Routes = [
  { path: '', component: CandidateListComponent },
  { path: 'candidateList', component: CandidateListComponent },
  { path: 'roles', component: RolesComponent },
];
