import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CandidateService {
  private apiUrl = 'https://localhost:44319/api/Candidate/GetCandidates';

  constructor(private http: HttpClient) {}

  getCandidates(
    page: number,
    pageSize: number,
    sortColumn: string,
    sortDirection: string
  ) {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize)
      .set('sortColumn', sortColumn)
      .set('sortDirection', sortDirection);

    return this.http.get(this.apiUrl, { params });
  }
  // Handle CV download
  downloadCV(candidateId: number) {
    return this.http.get(`${this.apiUrl}/DownloadCV/${candidateId}`, {
      responseType: 'blob',
    });
  }
  AddCandidate() {}
}
