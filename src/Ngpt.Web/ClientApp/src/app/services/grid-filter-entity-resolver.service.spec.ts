import { TestBed } from '@angular/core/testing';

import { GridFilterEntityResolverService } from './grid-filter-entity-resolver.service';

describe('GridFilterEntityResolverService', () => {
  let service: GridFilterEntityResolverService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GridFilterEntityResolverService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
