-- Conéctate a la base creada:
-- \c tienda_btg

-- 1) Schema
CREATE SCHEMA IF NOT EXISTS core;

-- 2) Tablas maestras
CREATE TABLE IF NOT EXISTS core.cliente (
  id        BIGINT PRIMARY KEY,
  nombre    VARCHAR(50) NOT NULL,
  apellidos VARCHAR(50) UNIQUE,
  ciudad    VARCHAR(20)
);

CREATE TABLE IF NOT EXISTS core.sucursal (
  id      BIGINT PRIMARY KEY,
  nombre  VARCHAR(100) NOT NULL,
  ciudad  VARCHAR(20)  NOT NULL
);

CREATE TABLE IF NOT EXISTS core.producto (
  id           BIGINT PRIMARY KEY,
  nombre       VARCHAR(150) NOT NULL,
  tipoProducto VARCHAR(30)  NOT NULL
);

-- 3) Tablas de relación
-- inscripcion: PK compuesta (idProducto, idCliente)
CREATE TABLE IF NOT EXISTS core.inscripcion (
  idProducto BIGINT NOT NULL,
  idCliente  BIGINT NOT NULL,
  CONSTRAINT pk_inscripcion PRIMARY KEY (idProducto, idCliente),
  CONSTRAINT fk_inscripcion_producto
    FOREIGN KEY (idProducto) REFERENCES core.producto(id)
      ON UPDATE CASCADE ON DELETE RESTRICT,
  CONSTRAINT fk_inscripcion_cliente
    FOREIGN KEY (idCliente) REFERENCES core.cliente(id)
      ON UPDATE CASCADE ON DELETE RESTRICT
);

-- disponibilidad: PK compuesta (idSucursal, idProducto)
CREATE TABLE IF NOT EXISTS core.disponibilidad (
  idSucursal BIGINT NOT NULL,
  idProducto BIGINT NOT NULL,
  CONSTRAINT pk_disponibilidad PRIMARY KEY (idSucursal, idProducto),
  CONSTRAINT fk_disponibilidad_sucursal
    FOREIGN KEY (idSucursal) REFERENCES core.sucursal(id)
      ON UPDATE CASCADE ON DELETE CASCADE,
  CONSTRAINT fk_disponibilidad_producto
    FOREIGN KEY (idProducto) REFERENCES core.producto(id)
      ON UPDATE CASCADE ON DELETE CASCADE
);

-- visitan: PK compuesta (idCliente, idSucursal)
CREATE TABLE IF NOT EXISTS core.visitan (
  idCliente    BIGINT NOT NULL,
  idSucursal   BIGINT NOT NULL,
  fecha_visita DATE,
  CONSTRAINT pk_visitan PRIMARY KEY (idCliente, idSucursal),
  CONSTRAINT fk_visitan_cliente
    FOREIGN KEY (idCliente) REFERENCES core.cliente(id)
      ON UPDATE CASCADE ON DELETE CASCADE,
  CONSTRAINT fk_visitan_sucursal
    FOREIGN KEY (idSucursal) REFERENCES core.sucursal(id)
      ON UPDATE CASCADE ON DELETE CASCADE
);

-- 4) Índices útiles
CREATE INDEX IF NOT EXISTS ix_inscripcion_cliente ON core.inscripcion (idCliente);
CREATE INDEX IF NOT EXISTS ix_inscripcion_producto ON core.inscripcion (idProducto);

CREATE INDEX IF NOT EXISTS ix_disp_producto ON core.disponibilidad (idProducto);
CREATE INDEX IF NOT EXISTS ix_disp_sucursal ON core.disponibilidad (idSucursal);

CREATE INDEX IF NOT EXISTS ix_visitan_cliente_sucursal ON core.visitan (idCliente, idSucursal);
