-- Ejecuta este script conectado al clúster (por ejemplo, a la BD "postgres").
-- Crea la base de datos del ejercicio.

CREATE DATABASE tienda_btg
  WITH TEMPLATE = template1
       ENCODING = 'UTF8';

-- En psql, conéctate luego:
-- \c tienda_btg
