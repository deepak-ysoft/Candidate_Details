import { Component, OnInit } from '@angular/core';
import { Candidate } from '../../class/candidate-model';
import { CommonModule } from '@angular/common';
import { CandidateDetailsService } from '../../Services/candidate-details.service';
import { DataTables } from 'angular-datatables';
@Component({
  selector: 'app-candidate-details',
  imports: [CommonModule,DataTables],
  templateUrl: './candidate-details.component.html',
  styleUrl: './candidate-details.component.css'
})
export class CandidateDetailsComponent  implements OnInit{
  constructor(private candidateservice:CandidateDetailsService) { }
  candidates: Candidate[] = [];
  data: Candidate[] = [];
  errorMessage: string = '';
  dtOptions: DataTables.Settings = {};

  candidateList() {
   this.candidateservice.getCandidateDetails().subscribe((data: any) => {
    this.candidates = data;
   })
 }




 ngOnInit(): void {
   // DataTables options can be customized here
   this.dtOptions = {
     pagingType: 'full_numbers',
     pageLength: 10,
     processing: true
   };

   // Load users
   this.getUsers();
 }

 ngAfterViewInit(): void {
   // Initialize DataTable after the DOM is rendered
   setTimeout(() => {
     const table = document.querySelector('#user-table') as HTMLTableElement;
     if (table) {
       new DataTables(table, this.dtOptions);
     }
   }, 0); // Delay ensures the table is rendered in the DOM
 }

 getUsers(): void {
   this.candidateservice.getCandidateDetails().subscribe({
     next: (users: Candidate[]) => {
       this.data = users; // Assign the fetched data to the table
     }
     
   });
 }

}
