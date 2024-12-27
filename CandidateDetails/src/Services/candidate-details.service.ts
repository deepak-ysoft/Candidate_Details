import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Candidate } from '../class/candidate-model';

@Injectable({
  providedIn: 'root'
})
export class CandidateDetailsService {

  apiGetCandidates:string = `https://localhost:7115/api/Candidate/GetCandidates`;
  constructor(private http: HttpClient) { }

 

  getCandidateDetails():any {
    return this.http.get(this.apiGetCandidates)
  }
  
}
