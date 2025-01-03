import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, OnInit } from '@angular/core';
import { Candidate } from '../Models/candidate.model';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CandidateService {
  private baseUrl = environment.apiURL;

  isCandidateAddOrUpdate = false;
  private candidateListSubject = new BehaviorSubject<Candidate[]>([]);
  private totalCandidatesSubject = new BehaviorSubject<number>(0);
  private totalPagesSubject = new BehaviorSubject<number>(0);

  candidateList$ = this.candidateListSubject.asObservable();
  totalCandidates$ = this.totalCandidatesSubject.asObservable();
  totalPages$ = this.totalPagesSubject.asObservable();

  constructor(private http: HttpClient) {}

  getCandidates(
    page: number = 1,
    pageSize: number = 10,
    sortColumn: string = 'id',
    sortDirection: string = 'asc',
    SearchField: string = '',
    SearchValue: string = ''
  ): void {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize)
      .set('sortColumn', sortColumn)
      .set('sortDirection', sortDirection)
      .set('SearchField', SearchField)
      .set('SearchValue', SearchValue);

    this.http.get(`${this.baseUrl}GetCandidates`, { params }).subscribe({
      next: (data: any) => {
        this.candidateListSubject.next(data.data);
        this.totalCandidatesSubject.next(data.totalCount);
        this.totalPagesSubject.next(Math.ceil(data.totalCount / pageSize));
        debugger;
      },
      error: (error) => {
        console.error('Error fetching candidates:', error);
      },
    });
  }

  UploadExcel(excel: FormData) {
    return this.http.post(`AddCandidatesFromExcel`, excel);
  }

  AddEditCandidate(data: FormData) {
    this.isCandidateAddOrUpdate = true;
    return this.http.post(`${this.baseUrl}AddEditCandidate`, data);
  }

  deleteCandidate(id: number) {
    return this.http.delete(`${this.baseUrl}DeleteCandidate/${id}`);
  }

  downloadCV(candidateId: number) {
    const url = `${this.baseUrl}DownloadCV/${candidateId}`;
    return this.http.get(url, { responseType: 'blob' });
  }
}
