🧪 Prueba Técnica – Administración de Fondos y Clientes
Este proyecto es una prueba técnica fullstack que demuestra habilidades en desarrollo, arquitectura orientada a servicios, despliegue en AWS y resolución de problemas en entornos productivos. Incluye un frontend en Angular, un backend en .NET con FastEndpoints, y servicios desplegados en AWS.

📦 Contenido del proyecto
🖥️ Frontend – Angular
- SPA modular con rutas protegidas por roles (Admin, User)
- Módulos para:
- Gestión de clientes
- Administración de fondos
- Control de usuarios
- Integración con backend mediante HttpClient y configuración dinámica de basePath
- Desplegado en AWS S3 como sitio web estático
- URL pública:
http://fondo-amr-jony.s3-website-us-east-1.amazonaws.com
⚙️ Backend – .NET + FastEndpoints
- API RESTful construida con FastEndpoints, un framework minimalista y performante para .NET
- Arquitectura orientada a servicios: cada funcionalidad está encapsulada en su propio endpoint
- Autenticación basada en token JWT
- Persistencia en AWS DynamoDB
- Servicio de correo integrado con AWS SES (Simple Email Service)
⚠️ Importante: AWS SES está en modo sandbox por defecto. Para enviar correos a direcciones reales, es necesario verificar los destinatarios o solicitar acceso a producción. De lo contrario, los correos no se entregan.

- Desplegado en AWS App Runner desde Visual Studio 2022 usando AWS Toolkit
- URL pública del backend:
https://amaris-backend.us-east-1.awsapprunner.com (ejemplo)

🧠 Arquitectura
La solución está diseñada con enfoque orientado a servicios, donde cada módulo (clientes, fondos, usuarios) expone sus propios endpoints desacoplados. Esto permite:
- Escalabilidad por dominio funcional
- Separación clara de responsabilidades
- Fácil mantenimiento y evolución
| Componente | Tecnología | Justificación | 
| Frontend | Angular + S3 | SPA rápida, desplegable como sitio estático | 
| Backend | FastEndpoints + App Runner | API modular, sin necesidad de administrar servidores | 
| Base de datos | DynamoDB | NoSQL flexible, ideal para datos de clientes y fondos | 
| Autenticación | Token JWT | Seguridad sin estado, compatible con frontend | 
| Correo | AWS SES | Servicio confiable para notificaciones y validaciones | 



🚀 Despliegue en AWS
Frontend
- ng build --configuration production
- Subida de archivos desde dist/<app>/browser directamente a S3
- Renombrado manual de index.<hash>.html a index.html
- Activación de Static Website Hosting en S3
- Configuración de política pública en el bucket
Backend
- Publicado desde Visual Studio 2022 con AWS Toolkit
- Selección de App Runner como destino
- CORS habilitado para origen del frontend
- Validación de integración entre frontend y backend

🧪 Pruebas y validaciones
- Se resolvieron errores de prerendering en Angular relacionados con rutas dinámicas (:id)
- Se ajustó el AppRoutingModule para evitar redirecciones inesperadas al hacer F5
- Se configuró environment.prod.ts para apuntar al backend en AWS
- Se aplicó política de bucket en S3 para permitir acceso público a los archivos
- Se validó la comunicación entre frontend y backend en producción
- Se probó el envío de correos en entorno sandbox de SES

🧰 Pasos para correr localmente
Requisitos
- Node.js y Angular CLI
- .NET 7 o superior
- AWS CLI (opcional para pruebas con DynamoDB local)
- Visual Studio 2022
Frontend
cd FrontEnd/amaris-app
npm install
ng serve


Accede a http://localhost:4200
Backend
- Abre el proyecto en Visual Studio 2022
- Configura el entorno como Development
- Verifica que el archivo appsettings.Development.json tenga la configuración local de DynamoDB y SES (mock o sandbox)
- Ejecuta el proyecto
El backend estará disponible en https://localhost:7280 (o el puerto configurado)

🧠 Autor
Jhony
Desarrollador fullstack con experiencia en Angular, .NET, AWS, Docker y DevOps.
Especialista en despliegues reproducibles, configuración explícita y resolución de errores en tiempo real.
