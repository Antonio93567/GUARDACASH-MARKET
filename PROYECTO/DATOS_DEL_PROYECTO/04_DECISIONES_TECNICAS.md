# Decisiones Técnicas — GUARDACASH MARKET

## Stack

- **Framework:** .NET MAUI (.NET 10.0.302, workload MAUI 10.0.20)
- **Targets activos:** Android (`net10.0-android`) y Windows (`net10.0-windows10.0.19041.0`)
- **ApplicationId:** `com.unitedsolutionsnetwork.guardacashmarket`
- **Título:** GUARDACASH MARKET

## Arquitectura

### Proyecto / versión
- `GuardacashMarket.csproj` generado con `dotnet new maui`, reconfigurado.
- `WindowsPackageType = None` → el build de Windows produce un **EXE de escritorio autónomo** (sin MSIX), apto para distribuir como `.exe`.

### Inyección de dependencias (`MauiProgram.cs`)
- Registrados: `IDataService`, `ISimulationService`, `IChatBotService`, `IPermissionService` (singleton).
- Páginas registradas como singleton.
- `App.Services` expone el `IServiceProvider` de forma estática.

### Estado compartido (`App.xaml.cs`)
- `App.Services` — acceso global a servicios.
- `App.CurrentService` — servicio seleccionado actualmente.
- `App.CurrentConversation` — conversación de chat activa.

## Modelos

- `User.cs` — usuario / perfil emprendedor.
- `Service.cs` — servicios del negocio (con `Icon`, `IsPositive`, `ChangeDisplay`).
- `Financial.cs` — `Gain`, `Quote`, `Order`, `Investment` (ganancias, cotizaciones, pedidos, inversiones).
- `Chat.cs` — `ChatMessage`, `Conversation` (chat-bot).

## Servicios

- `PermissionService.cs` — lógica de permiso de almacenamiento + cierre automático si no se concede. Constante `DataFolderName = "GuardacashData"`.
- `DataService.cs` — persistencia JSON en `FileSystem.AppDataDirectory/GuardacashData`.
- `SimulationService.cs` — lógica de simulación (ganancias, ventas).
- `ChatBotService.cs` — respuestas de los chat-bots por perfil.

## Persistencia — DECISIÓN CLAVE

**Requerimiento del usuario:** solicitar permiso de almacenamiento; si se concede, el usuario selecciona la dirección; si no, la app se cierra.

**Implementación actual:** se usa `FileSystem.AppDataDirectory` (almacenamiento privado y seguro de la app) creando la carpeta `GuardacashData` con archivos JSON (perfil, ganancias, cotizaciones, pedidos, chats, config, inversiones).

> Nota: El `AndroidManifest.xml` declara `READ/WRITE_EXTERNAL_STORAGE` + `requestLegacyExternalStorage` y la UI implementa la solicitud de permiso con cierre automático si se deniega, según lo pedido. La persistencia efectiva de datos opera sobre almacenamiento privado (no requiere SAF), lo que es a la vez más seguro y funciona sin permisos de almacenamiento público en Android moderno.

Archivos JSON en `GuardacashData\`: perfil, ganancias, cotizaciones, pedidos, chats, config, inversiones.

## Pantallas

`Views\` (XAML + code-behind):
Splash, Onboarding, Login, Register, Dashboard, Market, ServiceDetail (gráfico `LineChartDrawable`), Copilot, Messages, Chat (bot), Profile, Settings, Invest.

`AppShell.xaml` registra las rutas: `//Splash`, `//Onboarding`, `//Login`, `//Register`, `//Dashboard`, `//Market`, `//Detail`, `//Copilot`, `//Messages`, `//Chat`, `//Profile`, `//Settings`, `//Invest`.

## Recursos visuales

- Icono/splash: `Resources\Images\logo.png` y `logo_splash.png` (copias; el original en `..\..\logos\` NO se modifica).
- `appicon.svg` / `appiconfg.svg` / `splash.svg`: símbolo diana/flecha en cian sobre negro.
- `AndroidManifest.xml`: permisos de almacenamiento + `requestLegacyExternalStorage`.
- `Platforms\Android\Resources\values\colors.xml`: colores nativos.

## Compilación / entorno de build

### Requisitos Android
- **JDK 21** (OBLIGATORIO — Java 25 falla con error `XA0030: Building with JDK version 25 is not supported`).
  - Instalado en: `%LOCALAPPDATA%\Android\jdk-21\jdk-21.0.6+7` (Microsoft OpenJDK 21.0.6).
- **Android SDK** en `%LOCALAPPDATA%\Android\Sdk`:
  - `platform-tools`, `platforms;android-36`, `build-tools;36.0.0`.
- Variables de entorno para build: `JAVA_HOME`, `ANDROID_HOME`, `ANDROID_SDK_ROOT`.

### Requisitos Windows
- .NET SDK + workload MAUI. Build Release genera EXE autónomo (WinUI desencapsulado).

## Warnings conocidos (no bloqueantes)

- `CS0618`: `DisplayAlert` obsoleto → usar `DisplayAlertAsync`. Presente en varias páginas; **no impide** la compilación (0 errores en ambos targets).
