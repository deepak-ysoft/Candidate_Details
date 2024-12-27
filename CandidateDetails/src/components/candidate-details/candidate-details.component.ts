import { Component, OnInit } from '@angular/core';
import { Candidate } from '../../class/candidate-model';
import { CommonModule } from '@angular/common';
import { CandidateDetailsService } from '../../Services/candidate-details.service';


@Component({
  selector: 'app-candidate-details',
  imports: [CommonModule],
  templateUrl: './candidate-details.component.html',
  styleUrl: './candidate-details.component.css'
})
export class CandidateDetailsComponent  implements OnInit{
  constructor(private candidateservice:CandidateDetailsService) { }
  candidates: Candidate[] = [];

  ngOnInit() {
    this.candidateList();
  }

 candidateList() {
   this.candidateservice.getCandidateDetails().subscribe((data: any) => {
    this.candidates = data;
   })
 }

}
