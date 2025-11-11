import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { Cliente, Fondo } from '../../../model/models';
import { FondosServices } from '../../../services/core/fondos-services';
import { ClientesServices } from '../../../services/core/clientes-services';
import { ActivatedRoute } from '@angular/router';
import { HistorialTransacciones } from '../historial-transacciones/historial-transacciones';
declare var bootstrap: any;

@Component({
  selector: 'app-detalle-cliente',
  standalone: false,
  templateUrl: './detalle-cliente.html',
  styleUrl: './detalle-cliente.scss',
})
export class DetalleCliente {
  cliente: Cliente = {};
  fondos: Fondo[] = [];
  mostrarHistorial = false;
  mensajeSubscripcion: string = '';
  procesandoSuscripcion: { [fondoID: string]: boolean } = {};
  @ViewChild('historialModalRef') historialModalRef!: ElementRef;
  @ViewChild(HistorialTransacciones) historialComponent!: HistorialTransacciones;



  constructor(private fondosServices: FondosServices, private clientesService: ClientesServices,
    private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.cargarFondos();
    const id = this.route.snapshot.paramMap.get('id')!;

    this.clientesService.consultarClientePorId(id).subscribe({
    next: (data) => { this.cliente = data},
    error: (err) => console.error('Error al cargar cliente:', err)
  });

  }

  cargarFondos(): void {
    this.fondosServices.obtenerFondos().subscribe({
      next: (data) => this.fondos = data,
      error: (err) => console.error('Error al obtener fondos:', err)
    });
  }

  estaSuscrito(fondoID: string): boolean {
    return this.cliente.fondosActivos?.includes(fondoID) ?? false;
  }

  toggleSuscripcion(fondoID: string): void {
    const suscrito = this.estaSuscrito(fondoID);

    if (suscrito) {
      this.cliente.fondosActivos = this.cliente.fondosActivos?.filter(id => id !== fondoID);
    } else {
      this.cliente.fondosActivos = [...(this.cliente.fondosActivos ?? []), fondoID];
    }
  }

  suscribirseAFondo(fondo: Fondo): void {
    const id = fondo.fondoID!;
    if (this.procesandoSuscripcion[id]) return;
  
    this.procesandoSuscripcion[id] = true;
  
    this.clientesService.subscribirCliente(this.cliente.clienteID!, id).subscribe({
      next: (resp) => {
        this.cliente.saldoDisponible! -= fondo.montoMinimo!;
        this.cliente.fondosActivos!.push(id);
        this.mensajeSubscripcion = resp.mensajeNotificacion ?? `Suscripción exitosa al fondo ${fondo.nombreFondo}.`;
      },
      error: (err) => {
        this.mensajeSubscripcion = `Error al suscribirse: ${err.message}`;
      },
      complete: () => {
        this.procesandoSuscripcion[id] = false;
      }
    });
  }

  cancelarSuscripcion(fondo: Fondo): void {
      this.clientesService.cancelarSuscripcion(this.cliente.clienteID!, fondo.fondoID!).subscribe({
        next: (resp) => {
          this.cliente.saldoDisponible! += fondo.montoMinimo!;
          this.cliente.fondosActivos = this.cliente.fondosActivos!.filter(id => id !== fondo.fondoID);
        },
        error: (err) => {
          this.mensajeSubscripcion = `Error al cancelar suscripción: ${err.message}`;
        }
      });
  }

    abrirHistorial(): void {
    const modal = new bootstrap.Modal(this.historialModalRef.nativeElement);
    modal.show();
    this.historialComponent.cargarHistorial();
  }

  
}
