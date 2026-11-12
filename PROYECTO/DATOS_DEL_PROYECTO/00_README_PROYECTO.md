# GUARDACASH MARKET — Documentación del Proyecto

Solución híbrida fintech neón + emprendimiento juvenil, construida con **.NET MAUI (.NET 10)** para **Android (APK)** y **Windows (EXE)**.

> Esta carpeta (`DATOS_DEL_PROYECTO`) contiene la documentación técnica y del desafío del proyecto.

| Archivo | Contenido |
|---|---|
| `00_README_PROYECTO.md` | Este índice + resumen general |
| `01_BRIEF_MODULO_INVERTIR.md` | Especificación del módulo "Invertir" |
| `02_PALETA_COLARES.md` | Paleta de colores oficial |
| `03_DOCUMENTO_DESAFIO.md` | Extracto del documento pautado (NexoMarket / United Solutions Network) |
| `04_DECISIONES_TECNICAS.md` | Decisiones de arquitectura y persistencia |

---

## Resumen rápido

- **Código:** `..\GuardacashMarket\`
- **Código fuente C# + XAML** del proyecto MAUI.
- **Ejecutables:** `..\..\BUILD\EJECUTABLES\`
  - `GUARDACASH_v1.0.0.apk` (Android)
  - `GUARDACASH_v1.0.0_win\GUARDACASH_v1.0.0.exe` (Windows)
- **Recursos visuales (NO modificar):** `..\..\logos\` y `..\..\paleta de colores\`.

## Stack

- .NET SDK 10.0.302 + workload MAUI 10.0.20
- Compilación Android: Android SDK (API 36, build-tools 36.0.0) + Microsoft OpenJDK 21
- Compilación Windows: WinUI 3 desencapsulado (`WindowsPackageType = None` → EXE de escritorio autónomo)

## Cómo compilar

### Windows (solo build / debug)
```
dotnet build "..\GuardacashMarket\GuardacashMarket.csproj" -f net10.0-windows10.0.19041.0 -c Release
```

### Windows (publicar EXE autónomo)
```
dotnet publish "..\GuardacashMarket\GuardacashMarket.csproj" -f net10.0-windows10.0.19041.0 -c Release -o <salida>
```

### Android (APK)
Debe tener configurado `JAVA_HOME` apuntando a un **JDK 21** (Java 25 no es compatible con Xamarin.Android) y el Android SDK.

```
$env:JAVA_HOME = "C:\Users\<usuario>\AppData\Local\Android\jdk-21\<version>"
$env:ANDROID_HOME = "%LOCALAPPDATA%\Android\Sdk"
$env:ANDROID_SDK_ROOT = "%LOCALAPPDATA%\Android\Sdk"

dotnet build "..\GuardacashMarket\GuardacashMarket.csproj" -f net10.0-android -c Release
```

El APK firmado se genera en `bin\Release\net10.0-android\com.unitedsolutionsnetwork.guardacashmarket-Signed.apk`.

---

## Equipo del desafío (contexto)

- **Proyecto (desafío):** NexoMarket — "Del hobby al negocio"
- **Equipo:** United Solutions Network — UNI CUR Estelí
- **App de entrega:** GUARDACASH MARKET (este proyecto)

Ver `03_DOCUMENTO_DESAFIO.md` para el contexto completo del desafío.
