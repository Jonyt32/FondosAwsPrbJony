import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClientesServices } from '../../../services/core/clientes-services';
import { Cliente } from '../../../model/models';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-crear-cliente',
  standalone: false,
  templateUrl: './crear-cliente.html',
  styleUrl: './crear-cliente.scss',
})
export class CrearCliente {
  
  clienteForm: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder, private clientesServices: ClientesServices,private router: Router,
    private route: ActivatedRoute) {
    this.clienteForm = this.fb.group({
      nombre: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      preferenciaNotificacion: ['', Validators.required],
      saldoDisponible: [0, [Validators.required, Validators.min(500000)]],
    });
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.clienteForm.invalid) return;

    const cliente: Cliente = this.clienteForm.value;

    this.clientesServices.crearCliente(cliente).subscribe({
      next: () => {
        this.clienteForm.reset();
        //this.clienteCreado.emit();
        this.router.navigate(['../'], { relativeTo: this.route }); 
      },
      error: (err) => console.error('Error al crear cliente:', err)
    });
  }

  guardarCliente(): void {
    
    if (this.clienteForm.invalid) return;

    const nuevoCliente = this.clienteForm.value;
    console.log('Cliente guardado:', nuevoCliente);
    
  }

  volver(): void {
    this.router.navigate(['../'], { relativeTo: this.route });
  }


}
