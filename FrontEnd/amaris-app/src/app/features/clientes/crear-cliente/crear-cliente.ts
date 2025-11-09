import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClientesServices } from '../../../services/core/clientes-services';
import { Cliente } from '../../../model/models';

@Component({
  selector: 'app-crear-cliente',
  standalone: false,
  templateUrl: './crear-cliente.html',
  styleUrl: './crear-cliente.scss',
})
export class CrearCliente {
  @Output() clienteCreado = new EventEmitter<void>();
  clienteForm: FormGroup;

  constructor(private fb: FormBuilder, private clientesServices: ClientesServices) {
    this.clienteForm = this.fb.group({
      nombre: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      preferenciaNotificacion: ['', Validators.required],
      saldoDisponible: [0, [Validators.required, Validators.min(0)]],
      fondosActivos: [[]]
    });
  }

  onSubmit(): void {
    if (this.clienteForm.invalid) return;

    const cliente: Cliente = this.clienteForm.value;

    this.clientesServices.crearCliente(cliente).subscribe({
      next: () => {
        this.clienteForm.reset();
        this.clienteCreado.emit(); 
      },
      error: (err) => console.error('Error al crear cliente:', err)
    });
  }

}
