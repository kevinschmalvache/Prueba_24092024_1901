# Microservicios  - Gestión de Citas

## Resumen
Este proyecto consiste en una aplicación para la gestión de Citas, que se compone de tres microservicios interconectados con bases de datos independientes:

1. **Microservicio de Personas**: Encargado de manejar la información de médicos y pacientes.
2. **Microservicio de Citas**: Responsable de la creación y manejo de las Citas.
3. **Microservicio de Rectas**: Gestiona las recetas médicas vinculadas a las citas, incluyendo los id de medicos y pacientes.

Todos de los microservicios permite realizar operaciones CRUD (Crear, Leer, Actualizar y Eliminar) para sus respectivas entidades.

## ¿Como se compone el proyecto?

1. **Microservicio de Personas (MicroServicioPersonas)**: 
   - Administra la información relacionada con médicos y pacientes.
   - Cada individuo es identificado por un atributo "tipo de usuario".
   - Ofrece una API Web para proporcionar datos al microservicio de Citas.
   
2. **Microservicio de Citas (MicroServicioCitas)**:
   - Facilita la programación de citas, almacenando información como fecha, lugar, paciente y médico.
   - Realiza el seguimiento del estado de cada cita: "Pendiente", "En proceso" y "Finalizada".
   - Solicita al médico que ingrese la receta al concluir una cita.
   - Proporciona una API Web para la actualización de estados de las citas.
   - Funciona como emisor para RabbitMQ, promoviendo la creación de recetas al finalizar las citas.
   
3. **Microservicio de Rectas (MicroServicioRecetas)**:
   - Administra y almacena las recetas médicas.
   - Monitorea el estado de cada receta: "Activa", "Vencida" o "Entregada".
   - Cada receta es identificada mediante un código único.
   - Asocia las recetas a pacientes citas espesificas.
   - Actúa como receptor para RabbitMQ, recibiendo información sobre recetas del microservicio de Citas.
   - Ofrece una API Web para consultar recetas por código único o por paciente, así como para actualizar su estado.

### ¿Que Estamos Usando?
- **Swagger**
- **RabbitMQ**
- **SQL Server**: Sistema de gestión de bases de datos relacional.
- **C# WEB API (.NET Framework 4.8)**
- **EF (Entity Framework V6)**: Para la interacción con la base de datos y el mapeo objeto-relacional (ORM).
- **Unity**: Inyección de dependencias.
- **AutoMapper**: Facilita la conversión entre entidades y DTOs.


### Requisitos Minimos Para Ejecutar

1. **MSSQL Server** para la gestión de datos.
2. **Visual Studio >= 2019 | Visual studio code** compatible con .NET Framework 4.8.


### ¿Como hacer la puesta en marcha?
    - Abre la solución.
    - Compila la solución para instalar todos los paquetes (nuguets).
    - Verifica que cada proyecto de microservicio esté configurado para iniciar adecuadamente.
    - Ejecuta la solución para iniciar los 3 microservicios Personas, Citas y Recetas.
    - Para ver detalladamente la lista de los EndPoints puedes escribir en tu navegador https://localhost:{puertoDelServicio}/swagger .


#### MicroServicioPersonas - https://localhost:44399

| HTTP | URL                          | Descripción                                            |
|--------|-------------------------------|--------------------------------------------------------|
| GET    | `/api/personas`               | Obtener todas las personas registradas                 |
| GET    | `/api/personas/{id}`          | Obtener una persona específica por su ID               |
| POST   | `/api/personas`               | Crear una nueva persona                                 |
| PUT    | `/api/personas/{id}`          | Actualizar una persona existente                        |
| DELETE | `/api/personas/{id}`          | Eliminar una persona existente por su ID               |
| GET    | `/api/personas/validate/{id:int}/{tipoPersona}` | Validar si una persona es válida según su ID y tipo de persona |


#### MicroServicioCitas - https://localhost:44389

| HTTP | URL                          | Descripción                                            |
|--------|-------------------------------|--------------------------------------------------------|
| GET    | `/api/citas`                  | Obtener todas las citas                                 |
| GET    | `/api/citas/{id}`             | Obtener una cita específica por su ID                  |
| POST   | `/api/citas`                  | Crear una nueva cita                                   |
| PUT    | `/api/citas/{id}`             | Actualizar una cita existente                           |
| DELETE | `/api/citas/{id}`             | Eliminar una cita existente por su ID                  |
| PUT    | `/api/citas/{id}/estado`      | Actualizar el estado de una cita existente              |


#### MicroServicioRecetas - https://localhost:44379

| HTTP | URL                          | Descripción                                              |
|--------|-------------------------------|----------------------------------------------------------|
| GET    | `/api/recetas`                | Obtener todas las recetas                                |
| GET    | `/api/recetas/{id}`           | Obtener receta por ID                                    |
| GET    | `/api/recetas/paciente/{id}`  | Obtener recetas por paciente                             |
| POST   | `/api/recetas`                | Crear una nueva receta                                   |
| PUT    | `/api/recetas/{id}`           | Actualizar una receta                                    |
| DELETE | `/api/recetas/{id}`           | Eliminar una receta                                      |
| PUT    | `/api/recetas/estado/{id}`    | Actualizar el estado de una receta                       |



