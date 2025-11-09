import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListarFondos } from './listar-fondos';

describe('ListarFondos', () => {
  let component: ListarFondos;
  let fixture: ComponentFixture<ListarFondos>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ListarFondos]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListarFondos);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
