# FondosAwsPrbJony

Solución backend para la gestión de fondos de inversión, diseñada con arquitectura serverless sobre AWS. Implementada en .NET 9 y preparada para despliegue mediante contenedor Docker. Cumple con buenas prácticas de seguridad, mantenibilidad y automatización.

---

## 🧱 Tecnologías utilizadas

- **.NET 9** (C#)
- **AWS Lambda** (con imagen personalizada)
- **API Gateway**
- **DynamoDB** (modelo NoSQL)
- **Terraform** (infraestructura como código)
- **Postman** (colección de pruebas)
- **GitHub Actions** (CI/CD opcional)

---

## 📦 Funcionalidades implementadas

- Crear cliente
- Crear fondo
- Suscribir cliente a fondo
- Cancelar suscripción
- Actualizar saldo de cliente
- Consultar fondos disponibles
- Consultar transacciones por cliente
- Consultar fondos asignados a cliente

---

## 🔐 Seguridad

- Autenticación con **AWS Cognito**
- Autorización basada en **roles JWT**
- Encriptación de datos sensibles (AES/KMS)
- Validación de entrada/salida vía DTOs

---

## 🗃️ Modelo de datos NoSQL (DynamoDB)

| Entidad       | PK                  | SK / SortKey             |
|---------------|---------------------|--------------------------|
| Clientes      | CLIENTE#<id>        | METADATA                 |
| Fondos        | FONDO#<id>          | METADATA                 |
| Transacciones | CLIENTE#<id>        | TRANSACCION#<timestamp>  |

---

## 🚀 Despliegue con Terraform

```bash
terraform init
terraform apply