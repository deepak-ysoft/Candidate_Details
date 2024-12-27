import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Candidate } from '../class/candidate-model';

@Injectable({
  providedIn: 'root'
})
export class CandidateDetailsService {

  apiGetCandidates:string = 'http://localhost:3000/candidates';
  constructor(private http: HttpClient) { }

 

  getCandidateDetails():any {
    return this.http.get(this.apiGetCandidates)
  }
  
}
