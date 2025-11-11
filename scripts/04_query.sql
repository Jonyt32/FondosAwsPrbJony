-- “Nombres de clientes que tienen inscrito algún producto
--  disponible SOLO en las sucursales que visitan”
SELECT DISTINCT c.nombre
FROM core.inscripcion i
JOIN core.cliente c ON c.id = i.idCliente
WHERE NOT EXISTS (
  SELECT 1
  FROM core.disponibilidad d
  WHERE d.idProducto = i.idProducto
    AND NOT EXISTS (
      SELECT 1
      FROM core.visitan v
      WHERE v.idCliente  = i.idCliente
        AND v.idSucursal = d.idSucursal
    )
);
