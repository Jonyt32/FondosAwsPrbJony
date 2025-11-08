import { TestBed } from '@angular/core/testing';

import { ClientesServices } from './clientes-services';

describe('ClientesServices', () => {
  let service: ClientesServices;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientesServices);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
