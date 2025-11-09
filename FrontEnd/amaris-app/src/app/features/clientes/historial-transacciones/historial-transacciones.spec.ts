import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistorialTransacciones } from './historial-transacciones';

describe('HistorialTransacciones', () => {
  let component: HistorialTransacciones;
  let fixture: ComponentFixture<HistorialTransacciones>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HistorialTransacciones]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HistorialTransacciones);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
