import { TestBed } from '@angular/core/testing';

import { GlobalTickerSearchService } from './global-ticker-search.service';

describe('GlobalTickerSearchService', () => {
  let service: GlobalTickerSearchService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GlobalTickerSearchService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
