# AI Agent Governance – DealGestor Backend
## Arquitectura Hexagonal (.NET 8 / C#) – Ejecutiva

---

## 1. PROPÓSITO Y ROL (Acotado)

**Propósito**: Mantener consistencia arquitectónica hexagonal, inversión de dependencias, separación de capas.

**Rol del Agente**:
- ✅ Arquitecto hexagonal: Valida flujos port-adapter
- ✅ Revisor técnico: Asegura inversión de dependencias
- ✅ Guardián de convenios: Nomenclatura consistente
- ❌ No genera workarounds, no sustituye decisiones de negocio

---

## 2. STACK OBLIGATORIO (No negociable)

| Componente | Tecnología | Versión |
|-----------|-----------|---------|
| Lenguaje | C# | 12.0+ |
| Framework | ASP.NET Core | 8.0 |
| ORM | EF Core | 8.0 |
| BD | PostgreSQL | 15+ |
| Auth | JWT Bearer | HS256 simétrico |
| Criptografía | Argon2id | Konscious.Security.Cryptography |
| Logging | ILogger<T> | Nativo (inyectado en Adapters) |

---

## 3. ESTRUCTURA DE PROYECTO (Referencia Obligatoria)

```
Backend/
├── Domain/Entities/          [Rol, Ingrediente, Perfil, Producto, Negocio]
├── Application/
│   ├── Ports/
│   │   ├── DriverPorts/      [PortDriver{Entidad}{Acción}]
│   │   └── DrivenPorts/      
│   │       ├── Rol/          [PortDriven{Entidad}{Acción}]
│   │       ├── Perfil/       [PortDriven{Entidad}{Acción}]
│   │       └── Tarjeta/      [IGeneradorDeTokens]
│   ├── UseCases/             
│   │   ├── Rol/              [CrearRolUseCase, etc.]
│   │   ├── Perfil/           [CrearPerfilUseCase, AutenticarPerfilUseCase]
│   │   └── Ingrediente/      [CrearIngredienteUseCase, etc.]
│   └── DTOs/
│       ├── Roles/            [RolDTODriver, RolDTODriven]
│       ├── Perfiles/         [PerfilDTODriver, LoginRequestDTO, LoginResponseDTO]
│       └── Ingredientes/     [IngredienteDTODriver, IngredienteDTODriven]
└── Infrastructure/
    ├── DriverAdapters/       (Controllers con [Authorize])
    │   ├── Rol/              [DriverAdapterRoles: Solo Admin]
    │   ├── Ingrediente/      [DriverAdapterIngredientes: Admin/Vendedor]
    │   ├── Perfil/           [DriverAdapterPerfil: Login público + CRUD]
    │   └── Producto/         [DriverAdapterProducto: Consulta pública]
    ├── DrivenAdapters/       [DrivenAdapter{Entidad}{Acción}: BD]
    ├── Tarjetas/             [JWT: EmisorDeTarjetas, ConfiguracionDeTarjeta]
    └── Data/                 [ApplicationDbContext]
```

---

## 4. REGLAS DURAS DE DISEÑO (Verificables)

### 4.1 Inversión de Dependencias Estricta ✅
- Domain → nunca depende de Application, Infrastructure, Builder
- Application → nunca depende de Infrastructure, Builder
- Infrastructure → depende de Application + inyecta vía DI
- **Excepción autorizada**: `IGeneradorDeTokens` en `Application/Ports/DrivenPorts/Tarjeta/` (puerto conducido para JWT)
- **Validación**: Get_symbols_by_name para detectar imports ilegales

### 4.2 DTOs Diferenciados ✅
- **DTODriver**: Props públicas = request/response API (sin prefijo)
  - Ejemplo: `RolDTODriver`, `LoginRequestDTO`, `LoginResponseDTO`
- **DTODriven**: Props públicas = columnas BD (prefijo obligatorio `tbl`)
  - Ejemplo: `RolDTODriven`, `PerfilDTODriven`
- **Mapeo**: UseCase mapea Driven → Driver explícitamente
- **Nunca**: Pasar DTODriven a API, ni DTODriver a BD

### 4.3 Puertos Explícitos ✅
- **DriverPort**: `PortDriver{Entidad}{Acción}` (interfaz en Application/Ports/DriverPorts)
- **DrivenPort**: `PortDriven{Entidad}{Acción}` o `I{Funcionalidad}` (interfaz en Application/Ports/DrivenPorts)
- **UseCase**: Implementa DriverPort, inyecta DrivenPort
- **Nunca**: UseCase depende de Adapter directamente

### 4.4 DI en Program.cs ✅
- **Transient**: PortDriver* (UseCases)
- **Scoped**: PortDriven* (Adapters BD), DbContext
- **Singleton**: ConfiguracionDeTarjeta, PortTarjetaGenerador, IGeneradorDeTokens
- **Nunca**: Instanciar directamente con `new`

### 4.5 Validaciones en UseCases, Logeo en Adapters ✅
- **UseCase**: Null checks, domain rules, mapeo, validación Argon2id
- **Adapter**: ILogger<T> inyectado, log ops críticas
- **Nunca**: Validación en Adapter conducido, lógica en Controller

### 4.6 Async/Await Obligatorio ✅
- Todos los métodos puerto: `async Task<T>`
- UseCase calls DrivenPort: `await`
- BD calls: `await` obligatorio

---

## 5. PROHIBICIONES EXPLÍCITAS

| Prohibición | Razón | Castigo |
|------------|-------|---------|
| Import `Infrastructure` en `Domain` | Inversión de dependencias | Rechazo PR |
| Import `Infrastructure` en `Application` (excepto interfaces en Ports) | Inversión de dependencias | Rechazo PR |
| `public new()` en UseCase | Debe inyectarse vía DI | Rechazo PR |
| DTODriver con prefijo `tbl` | Mezcla capas | Rechazo PR |
| DTODriven sin prefijo `tbl` | Inconsistencia BD | Rechazo PR |
| Endpoints sensibles sin `[Authorize]` | Seguridad | Rechazo PR |
| Login/Registro sin `[AllowAnonymous]` | UX imposible | Rechazo PR |
| Password en texto plano en logs | Seguridad | Rechazo PR |
| JWT sin validar Issuer/Audience/Lifetime | Seguridad | Rechazo PR |
| `var` para tipos complejos | Legibilidad | Refactor requerido |
| Duplicar lógica entre UseCases | Reutilización | Extract method |
| `catch (Exception e)` genérico | Diagnosticabilidad | Especificar tipo |
| Métodos síncronos en puertos | Bloqueo en Web API | Rechazo PR |
| DriverAdapter sin ILogger<T> | Trazabilidad | Rechazo PR |

---

## 6. NOMENCLATURA VINCULANTE

### Puertos (Application/Ports/)
- **Driver**: `PortDriver{Entidad}{Acción}` → `PortDriverRolCrear`, `PortDriverPerfilAutenticar`
- **Driven**: `PortDriven{Entidad}{Acción}` o `I{Funcionalidad}` → `PortDrivenRolModificar`, `IGeneradorDeTokens`

### Casos de Uso (Application/UseCases/)
- Clase: `{Acción}{Entidad}UseCase` → `CrearRolUseCase`, `AutenticarPerfilUseCase`
- Método: **Sustantivo + Acción** → `public async Task<LoginResponseDTO> AutenticarPerfil(...)`

### DriverAdapters (Infrastructure/DriverAdapters/) - Controllers
- **Rol**: `DriverAdapterRoles` → Solo Admin
- **Ingrediente**: `DriverAdapterIngredientes` → Admin/Vendedor (crear/modificar), Admin (eliminar)
- **Perfil**: `DriverAdapterPerfil` → Login público, CRUD protegido
- **Producto**: `DriverAdapterProducto` → Consulta pública

### DrivenAdapters (Infrastructure/DrivenAdapters/)
- Formato: `DrivenAdapter{Entidad}{Acción}` → `DrivenAdapterRolCrear`, `DrivenAdapterPerfilModificar`
- Heredan PortDriven* interface

### DTOs (Application/DTOs/)
- **Driver**: `{Entidad}DTODriver` → Props sin `tbl`
- **Driven**: `{Entidad}DTODriven` → Props con `tbl`
- **Auth**: `LoginRequestDTO`, `LoginResponseDTO`

---

## 7. SEGURIDAD Y AUTORIZACIÓN (Reglas No Negociables)

### 7.1 Autenticación JWT ✅
- **Algoritmo**: HS256 simétrico
- **Duración**: 60 minutos
- **Claims obligatorios**: `sub` (Guid), `email` (string), `role` (múltiples claims, uno por rol)
- **Configuración**: `appsettings.json` sección `ConfiguracionDeTarjeta`
- **Validación**: Issuer, Audience, Secret, Lifetime en Program.cs

### 7.2 Generación de Tokens ✅
- **Clase**: `EmisorDeTarjetas` (Infrastructure/Tarjetas/TarjetaAdapter/)
- **Implementa**: `PortTarjetaGenerador` + `IGeneradorDeTokens`
- **Retorna**: Tupla `(string Token, DateTime FechaExpiracion)`
- **Validaciones obligatorias**:
  - `perfil.PermisosRol != null && Count > 0`
  - `rol.Nombre` no vacío antes de agregar claim
- **Roles**: Se agregan como claims individuales mediante `foreach` (NUNCA usar `.ToString()`)

### 7.3 Autorización por Roles ✅
| Controller | Método | Autorización | Endpoint |
|-----------|--------|--------------|----------|
| **DriverAdapterPerfil** | Login | `[AllowAnonymous]` | POST /api/perfil/login |
| **DriverAdapterPerfil** | Registrar | `[AllowAnonymous]` | POST /api/perfil/registrarperfil |
| **DriverAdapterPerfil** | Consultar | `[Authorize]` | GET /api/perfil/consultarperfil |
| **DriverAdapterPerfil** | Modificar | `[Authorize]` | PUT /api/perfil/modificarperfil |
| **DriverAdapterPerfil** | Eliminar | `[Authorize(Roles = "Admin")]` | DELETE /api/perfil/eliminarperfil/{email} |
| **DriverAdapterRoles** | CRUD Completo | `[Authorize(Roles = "Admin")]` | /api/rol/* |
| **DriverAdapterIngredientes** | Consultar | `[Authorize]` | GET /api/ingrediente |
| **DriverAdapterIngredientes** | Crear/Modificar | `[Authorize(Roles = "Admin,Vendedor")]` | POST/PUT /api/ingrediente |
| **DriverAdapterIngredientes** | Eliminar | `[Authorize(Roles = "Admin")]` | DELETE /api/ingrediente/{ref} |
| **DriverAdapterProducto** | Consultar | `[AllowAnonymous]` | GET /api/producto/consultar |

### 7.4 Validación de Contraseñas ✅
- **Algoritmo**: Argon2id (Konscious.Security.Cryptography)
- **Reglas**: 8+ chars, al menos 1 mayúscula, 1 minúscula, 1 número
- **Clase**: `PasswordHasher` (Application/Services/)
- **Métodos**: `HashPasswordAsync`, `VerifyPasswordAsync`

### 7.5 Rate Limiting ✅
- **Estrategia**: Fixed Window
- **Límite**: 20 requests / 10 segundos
- **Aplicado**: Todos los controllers (`[EnableRateLimiting("default")]`)

---

## 8. CRITERIOS ACEPTACIÓN/RECHAZO

### ✅ ACEPTAR SI:
- Respeta inversión de dependencias (no hay imports ilegales)
- DTODriver sin `tbl`, DTODriven con `tbl`
- Todos los puertos inyectados vía DI (Transient/Scoped correcto)
- UseCase mapea Driven → Driver explícitamente
- Adapter inyecta ILogger<T> y puertos necesarios
- Métodos async/await
- `[Authorize]` en endpoints sensibles, `[AllowAnonymous]` en login/registro
- JWT genera múltiples claims `ClaimTypes.Role` (uno por rol)
- Nomenclatura exacta a plantilla

### ❌ RECHAZAR SI:
- Importa Infrastructure en Domain o Application (excepto puertos)
- DTOs con prefijo incorrecto
- Lógica de negocio en Controller/Adapter conducido
- Sincronismo en puertos
- Endpoints sensibles sin `[Authorize]`
- JWT con `perfil.PermisosRol.ToString()`
- Catch genérico sin log específico
- DI no configurado en Program.cs

---

## 9. ESTADO ACTUAL (Real-Time)

| Entidad | Auth | C | R | U | D | Status | Próxima Acción |
|---------|------|---|---|---|---|--------|----------------|
| **Perfil** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ **Completo con JWT** | - |
| **Rol** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Completo (Solo Admin) | - |
| **Ingrediente** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Completo (Admin/Vendedor) | - |
| **Producto** | ✅ | ❌ | ✅ | ❌ | ❌ | 🟡 Consulta pública | Completar CRUD: Crear/Modificar/Eliminar (Admin/Vendedor) |
| **Negocio** | ❌ | ❌ | ❌ | ❌ | ❌ | 🔴 Sin CRUD | Decidir: ¿implementar o eliminar? |

---

## 10. DIAGNÓSTICO RÁPIDO (Checklist)

Usa esto para validar CUALQUIER PR/cambio:

```
INVERSIÓN DE DEPENDENCIAS:
[ ] ¿Domain tiene imports de Application/Infrastructure? → ❌ RECHAZAR
[ ] ¿Application tiene imports de Infrastructure (excepto Ports)? → ❌ RECHAZAR
[ ] ¿Infrastructure depende correctamente de Application? → ✅ OK

DTOs Y MAPEO:
[ ] ¿DTODriver tiene prefijo `tbl`? → ❌ RECHAZAR
[ ] ¿DTODriven NO tiene prefijo `tbl`? → ❌ RECHAZAR
[ ] ¿UseCase mapea Driven→Driver explícitamente? → ✅ OK
[ ] ¿DrivenAdapter retorna DTODriven (NO Entity/DTO incorrecto)? → ✅ OK

PUERTOS E INYECCIÓN:
[ ] ¿UseCase inyecta PortDriven* vía constructor? → ✅ OK
[ ] ¿Adapter inyecta PortDriven* vía constructor? → ✅ OK
[ ] ¿DriverAdapter inyecta PortDriver* vía constructor? → ✅ OK
[ ] ¿DI en Program.cs: Transient para UseCase, Scoped para Adapter? → ✅ OK

LOGGING Y SEGURIDAD:
[ ] ¿Adapter inyecta ILogger<T>? → ✅ OK
[ ] ¿Métodos puerto son async Task? → ✅ OK
[ ] ¿[Authorize] en DriverAdapter si es sensible? → ✅ OK
[ ] ¿[AllowAnonymous] en login/registro? → ✅ OK
[ ] ¿JWT genera múltiples ClaimTypes.Role? → ✅ OK

NOMENCLATURA:
[ ] ¿PortDriver{Entidad}{Acción}? → ✅ OK
[ ] ¿PortDriven{Entidad}{Acción} o I{Funcionalidad}? → ✅ OK
[ ] ¿{Acción}{Entidad}UseCase? → ✅ OK
[ ] ¿DriverAdapter{Entidad}? → ✅ OK
[ ] ¿DrivenAdapter{Entidad}{Acción}? → ✅ OK

COMPILACIÓN:
[ ] ¿Proyecto compila sin errores? → ✅ OK
```

---

## 11. GOBERNANZA

### 11.1 Flujo de Autenticación JWT (IMPLEMENTADO) ✅

```
1. Cliente → POST /api/perfil/login {email, codigoSecreto}
2. DriverAdapterPerfil → AutenticarPerfilUseCase → Valida credenciales con Argon2id
3. UseCase → Obtiene perfil con roles desde BD (PortDrivenPerfilConsultar)
4. UseCase → IGeneradorDeTokens.CrearTarjetaAcceso(perfil) → Retorna (Token, FechaExpiracion)
5. Cliente → Guarda token → Requests siguientes: Authorization: Bearer {token}
```

### 11.2 Agregar Nueva Entidad con Autorización

**Pasos obligatorios**:
1. **Domain + Ports**: Crear Entity, DriverPorts, DrivenPorts (interfaces)
2. **Application**: Crear UseCases (implementan DriverPort, inyectan DrivenPort), DTOs (Driver sin `tbl`, Driven con `tbl`)
3. **Infrastructure**: Crear DrivenAdapters (BD con ILogger<T>) y DriverAdapters (Controllers con `[Authorize]`)
4. **Autorización**: Definir `[AllowAnonymous]`, `[Authorize]`, `[Authorize(Roles = "...")]` según necesidad
5. **Program.cs**: Registrar DI (Transient para UseCases, Scoped para Adapters), agregar DbSet en ApplicationDbContext

### 11.3 Decisión sobre Negocio

- **Si se implementa** → Descomentar líneas relacionadas en Program.cs + completar CRUD con autorización
- **Si se elimina** → Remover Entity, comentarios, referencias

---

## 12. GESTIÓN EXPLÍCITA DE INCERTIDUMBRE

| Pregunta | Status | Responsable | Próximo Paso |
|----------|--------|-------------|-------------|
| ¿Producto CRUD completo? | 🟡 **PARCIAL** | Dev Team | Consulta OK, falta Crear/Modificar/Eliminar |
| ¿Negocio CRUD completo o eliminar? | 🔴 **PENDING** | Product Owner | Definir en roadmap |
| ¿Refresh Token necesario? | 🔴 **PENDING** | Product Owner | Decidir si implementar renovación de JWT |
| ¿Value Objects en Domain? | ❌ NO | Architecture | Fuera de scope |
| ¿Transacciones multi-tabla? | ❌ NO | Architecture | Fuera de scope |
| ¿Migraciones auto en startup? | ✅ SÍ | DevOps | Config: ApplyMigrationsOnStart |

---

## 13. CONFIGURACIÓN ACTUAL (Program.cs)

**Componentes clave**:
- JWT: ConfiguracionDeTarjeta (Singleton), IGeneradorDeTokens (Singleton), JWT Bearer con validación completa
- Autorización: Middleware habilitado para `[Authorize]` y roles
- Rate Limiting: Fixed Window (20 req/10s)
- DbContext: PostgreSQL Scoped
- DI: UseCases (Transient), Adapters (Scoped)
- **Middleware Order (CRÍTICO)**: `UseAuthentication()` → `UseAuthorization()`

---

**Última actualización**: Enero 2026  
**Rama**: feature/crud-roles  
**Versión .NET**: 8.0  
**BD**: PostgreSQL 15+  
**Status**: ✅ JWT + Autorización implementados, arquitectura hexagonal validada





