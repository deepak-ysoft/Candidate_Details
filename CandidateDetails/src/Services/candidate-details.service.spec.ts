import { TestBed } from '@angular/core/testing';

import { CandidateDetailsService } from './candidate-details.service';

describe('CandidateDetailsService', () => {
  let service: CandidateDetailsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CandidateDetailsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
