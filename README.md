# ProductChallenge

Se desarrolló una API REST en .NET 10 para el registro de productos.

## Funcionalidades

- Crear producto (`POST /api/products`)
- Actualizar producto (`PUT /api/products/{id}`)
- Obtener producto por id (`GET /api/products/{id}`)
- Persistencia local con SQLite
- Caché en memoria para estados del producto
- Consumo de API externa para descuento por `ProductId`
- Cálculo de `FinalPrice`
- Manejo global de excepciones
- Documentación con Swagger
- Pruebas unitarias y de integración

## Arquitectura

La solución está organizada en las siguientes capas:

- **ProductChallenge.Api**: controllers, middleware y configuración
- **ProductChallenge.Application**: casos de uso, validaciones, handlers y contratos
- **ProductChallenge.Domain**: entidad `Product` y reglas básicas
- **ProductChallenge.Infrastructure**: EF Core, SQLite, repositorios, caché y servicios externos
- **ProductChallenge.Tests**: pruebas unitarias e integración

## Patrones usados

- CQRS
- MediatR
- Repository Pattern
- Middleware para excepciones y logging

## Tecnologías

- .NET 10
- ASP.NET Core 10
- EF Core 10.0.5
- SQLite
- MediatR
- FluentValidation
- Swagger / OpenAPI
- xUnit
- Moq
- FluentAssertions

## Estructura de la solución

```text
ProductChallenge
├── src
│   ├── ProductChallenge.Api
│   ├── ProductChallenge.Application
│   ├── ProductChallenge.Domain
│   └── ProductChallenge.Infrastructure
└── tests
    └── ProductChallenge.Tests
```

## Requisitos previos

- Visual Studio 2026
- .NET 10 SDK
- Conexión a internet para consumir la API de descuentos

## Configuración

### Base de datos local

La aplicación usa SQLite y genera una base local:

```text
productchallenge.db
```

### API externa de descuentos

En `appsettings.json` se configura la URL base del servicio externo:

```json
"ExternalServices": {
  "DiscountApi": {
    "BaseUrl": "https://69ea4bd615c7e2d51269a255.mockapi.io/api/v1/"
  }
}
```

La consulta se hace con una ruta tipo:

```text
GET discounts/{productId}
```

Ejemplo de respuesta:

```json
{
  "id": "2",
  "discount": 15
}
```

## Cómo ejecutar el proyecto

### 1. Restaurar paquetes
Abrir la solución en Visual Studio y esperar que termine la restauración de NuGet.

### 2. Crear o actualizar la base de datos
Abrir **Package Manager Console** y ejecutar:

```powershell
Update-Database
```

Para este paso:
- **Establecer como proyecto de inicio**: `ProductChallenge.Api`
- **En consola poner como proyecto predeterminado**: `ProductChallenge.Infrastructure`

### 3. Ejecutar la API
Establecer `ProductChallenge.Api` como proyecto de inicio y ejecutar.

### 4. Probar en Swagger
Abrir en el navegador:

```text
https://localhost:{puerto}/swagger
```

## Endpoints

### POST `/api/products`

Ejemplo de body:

```json
{
  "productId": 1,
  "name": "Laptop Asus",
  "status": 1,
  "stock": 10,
  "description": "Equipo de prueba",
  "price": 2500.00
}
```

### PUT `/api/products/{id}`

Actualiza un producto existente.

### GET `/api/products/{id}`

Retorna:

- `ProductId`
- `Name`
- `StatusName`
- `Stock`
- `Description`
- `Price`
- `Discount`
- `FinalPrice`

## Caché

Se mantiene en memoria por 5 minutos el diccionario de estados:

- `1 => Active`
- `0 => Inactive`

## Logging

Cada request registra su tiempo de respuesta en:

```text
Logs/request-log.txt
```

Ejemplo:

```text
2026-04-21 22:19:02 | GET /api/products/1 | 200 | 37 ms
```

## Pruebas

Desde Visual Studio:

1. Abrir **Test Explorer**
2. Ejecutar **Run All Tests**

## Códigos HTTP usados

- `200 OK`
- `201 Created`
- `204 No Content`
- `400 Bad Request`
- `404 Not Found`
- `409 Conflict`
- `503 Service Unavailable`
- `500 Internal Server Error`