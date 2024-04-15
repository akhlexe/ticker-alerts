import { TestBed } from '@angular/core/testing';

import { FinancialAssetsService } from './financial-assets.service';

describe('FinancialAssetsService', () => {
  let service: FinancialAssetsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FinancialAssetsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
