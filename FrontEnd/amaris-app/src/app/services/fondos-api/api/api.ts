export * from './auth.service';
import { AuthService } from './auth.service';
export * from './clientes.service';
import { ClientesService } from './clientes.service';
export * from './fondos.service';
import { FondosService } from './fondos.service';
export * from './usuarios.service';
import { UsuariosService } from './usuarios.service';
export const APIS = [AuthService, ClientesService, FondosService, UsuariosService];
