import { TestBed } from '@angular/core/testing';

import { GridFilterEnumResolverService } from './grid-filter-enum-resolver.service';

describe('GridFilterEnumResolverService', () => {
  let service: GridFilterEnumResolverService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GridFilterEnumResolverService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
