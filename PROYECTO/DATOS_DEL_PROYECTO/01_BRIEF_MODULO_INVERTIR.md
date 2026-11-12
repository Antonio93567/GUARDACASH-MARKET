# Módulo "Invertir" — Brief técnico

> **Importante:** El módulo **"Invertir"** de GUARDACASH MARKET es una **reinversión de las ganancias del propio negocio** (montos ficticios / simulados). **NO es cripto, no es bolsa, no son valores reales.** Es una simulación formativa para que el emprendedor joven aprenda a reinvertir sus utilidades.

## Propósito

Enseñar de forma interactiva al emprendedor a reinvertir las ganancias generadas por su negocio (fotografía, diseño, edición de video, manualidades, etc.) para hacer crecer su operación, en lugar de retirar todo el efectivo.

## Flujo simulado

1. El usuario parte de un **saldo de ganancias ficticio** (proveniente de ganancias simuladas del negocio).
2. Elige reinvertir un monto C$ (simulado).
3. La inversión se agrega a un portafolio simulado con monto y fecha.
4. La app proyecta un **rendimiento / crecimiento simulado** de la inversión.
5. La lógica es 100 % local y de demostración (sin dinero real, sin rede).

## Modelo de datos

`Investment` (ver `..\GuardacashMarket\Models\Financial.cs`):

- `Id`
- `Amount` (monto invertido, C$)
- `Date` (fecha de la operación)
- `ProjectedYield` / campos de proyección simulada

## Pantalla

- `Views\InvestPage.xaml(.cs)` — registro y listado de inversiones simuladas.

---

## Decisiones de diseño

- El módulo es **educativo/simulado**: no conecta con ningún servicio financiero externo.
- Los montos son ficticios, coherentes con el enfoque "del hobby al negocio".
- Persiste localmente en JSON (`GuardacashData\investments.json`).
