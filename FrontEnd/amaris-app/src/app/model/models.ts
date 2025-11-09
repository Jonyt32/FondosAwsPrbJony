export interface Fondo { 
    fondoID?: string;
    nombreFondo?: string;
    montoMinimo?: number;
    categoria?: string;
}

export interface Cliente { 
    saldoDisponible?: number;
    fondosActivos?: Array<string>;
    preferenciaNotificacion?: string;
    nombre?: string;
}

export interface Transaccion { 
    tipo?: string;
    fondoID?: string;
    monto?: number;
    fecha?: string;
    notificacion?: string;
}

export interface Usuario { 
    usuarioID?: string;
    nombreUsuario?: string;
    email?: string;
    rol?: string;
    password?: string;
}
