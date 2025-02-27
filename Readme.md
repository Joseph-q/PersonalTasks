# Aplicación de Tareas con Autenticación JWT

## Requisitos
Para ejecutar esta aplicación, asegúrate de tener instalados los siguientes requisitos:
- Docker

## Clonar el repositorio
```sh
git clone https://github.com/Joseph-q/PersonalTasks
```

## Navegar al repositorio
```sh
cd PersonalTasks
```

## Ejecutar la aplicación
Levanta los contenedores de Docker con el siguiente comando:
```sh
docker compose up
```

Ahora, el backend estará corriendo correctamente.

## Autenticación con JWT
Esta aplicación implementa autenticación mediante JSON Web Tokens (JWT). Para acceder a los recursos protegidos, sigue estos pasos:

### 1. Registro de usuario
Envía una solicitud `POST` a `/api/auth/register` con los siguientes datos en formato JSON:
```json
{
  "username": "tu_usuario",
  "password": "tu_contraseña"
}
```

### 2. Inicio de sesión
Envía una solicitud `POST` a `/api/auth/login` con las credenciales de usuario:
```json
{
  "username": "tu_usuario",
  "password": "tu_contraseña"
}
```
Si las credenciales son correctas, recibirás un token JWT en la respuesta.

### 3. Acceder a recursos protegidos
Para realizar solicitudes a rutas protegidas, agrega el token JWT en el encabezado `Authorization`:
```sh
Authorization: Bearer <tu_token>
```

Con esto, ya puedes gestionar tus tareas de manera segura en la aplicación.

