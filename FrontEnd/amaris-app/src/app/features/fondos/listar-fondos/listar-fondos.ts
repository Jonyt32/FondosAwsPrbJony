import { Component } from '@angular/core';
import { Fondo } from '../../../model/models';
import { FondosServices } from '../../../services/core/fondos-services';

@Component({
  selector: 'app-listar-fondos',
  standalone: false,
  templateUrl: './listar-fondos.html',
  styleUrl: './listar-fondos.scss',
})
export class ListarFondos {
  fondos: Fondo[] = [];

  constructor(private fondosServices: FondosServices) {}

  ngOnInit(): void {
    this.cargarFondos();
  }

  cargarFondos(): void {
    this.fondosServices.obtenerFondos().subscribe({
      next: (data) => this.fondos = data,
      error: (err) => console.error('Error al obtener fondos:', err)
    });
  }

}
