import { Component } from '@angular/core';
import { CandidateService } from '../../Services/candidate.service';
import { Candidate } from '../../Models/candidate.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-candidate-list',
  imports: [CommonModule],
  templateUrl: './candidate-list.component.html',
  styleUrl: './candidate-list.component.css',
})
export class CandidateListComponent {
  candidateList: Candidate[] = [];

  isCVAvailable = false; // Set to true if CV exists for the candidate
  constructor(private candidateService: CandidateService) {}

  ngOnInit(): void {
    this.candidateService
      .getCandidates(1, 10, 'id', 'asc')
      .subscribe((data: any) => {
        this.candidateList = data.data;
      });
  }

  downloadCV(candidateId: number) {
    this.candidateService.downloadCV(candidateId).subscribe({
      next: (response: Blob) => {
        const url = window.URL.createObjectURL(response);
        const link = document.createElement('a');
        link.href = url;
        link.download = `Candidate_${candidateId}_CV.pdf`; // Change file name if needed
        link.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error(err);
        alert('Error downloading CV.');
      },
    });
  }
}
