import { TestBed } from '@angular/core/testing';

import { FondosServices } from './fondos-services';

describe('FondosServices', () => {
  let service: FondosServices;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FondosServices);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
