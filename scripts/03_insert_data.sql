-- Limpieza opcional (si re-ejecutas)
TRUNCATE core.inscripcion, core.disponibilidad, core.visitan, core.producto, core.sucursal, core.cliente RESTART IDENTITY CASCADE;

-- Clientes
INSERT INTO core.cliente (id, nombre, apellidos, ciudad) VALUES
  (1, 'Ana',   'Perez', 'Bogota'),
  (2, 'Luis',  'Gomez', 'Medellin'),
  (3, 'Marta', 'Diaz',  'Cali');

-- Sucursales
INSERT INTO core.sucursal (id, nombre, ciudad) VALUES
  (10, 'Bogota Centro',     'Bogota'),
  (20, 'Medellin Norte',    'Medellin'),
  (30, 'Cali Sur',          'Cali'),
  (40, 'Barranquilla Prado','Barranquilla');

-- Productos
INSERT INTO core.producto (id, nombre, tipoProducto) VALUES
  (100, 'Fondo A', 'FPV'),
  (200, 'Fondo B', 'FIC'),
  (300, 'Fondo C', 'FIC');

-- Disponibilidad por sucursal
-- Fondo A: Bogotá (10), Medellín (20)
INSERT INTO core.disponibilidad (idSucursal, idProducto) VALUES
  (10, 100), (20, 100);

-- Fondo B: Bogotá (10), Cali (30)
INSERT INTO core.disponibilidad (idSucursal, idProducto) VALUES
  (10, 200), (30, 200);

-- Fondo C: Medellín (20), Barranquilla (40)
INSERT INTO core.disponibilidad (idSucursal, idProducto) VALUES
  (20, 300), (40, 300);

-- Visitas de clientes
-- Ana visita Bogotá y Medellín
INSERT INTO core.visitan (idCliente, idSucursal, fecha_visita) VALUES
  (1, 10, CURRENT_DATE),
  (1, 20, CURRENT_DATE);

-- Luis visita solo Bogotá
INSERT INTO core.visitan (idCliente, idSucursal, fecha_visita) VALUES
  (2, 10, CURRENT_DATE);

-- Marta visita Medellín y Barranquilla
INSERT INTO core.visitan (idCliente, idSucursal, fecha_visita) VALUES
  (3, 20, CURRENT_DATE),
  (3, 40, CURRENT_DATE);

-- Inscripciones
-- Ana inscrita al Fondo A (cumple: visita todas las sucursales donde A está disponible: 10 y 20)
INSERT INTO core.inscripcion (idProducto, idCliente) VALUES (100, 1);

-- Luis inscrito al Fondo B (NO cumple: Fondo B está en 10 y 30, él no visita 30)
INSERT INTO core.inscripcion (idProducto, idCliente) VALUES (200, 2);

-- Marta inscrita al Fondo C (cumple: Fondo C está en 20 y 40, ella visita 20 y 40)
INSERT INTO core.inscripcion (idProducto, idCliente) VALUES (300, 3);
