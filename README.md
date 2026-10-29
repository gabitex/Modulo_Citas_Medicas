# Modulo_Citas_Medicas
P10-Arquitectura de Software
Este sistema permitirá registrar pacientes, agendar citas en distintas especialidades, asignar un doctor y confirmar la ficha de atención.

Es una aplicación de consola en C# que demuestra tres patrones de diseño (uno creacional, uno estructural y uno de comportamiento) y los cinco principios SOLID sobre el caso de uso de reserva de citas médicas.

**Tecnologías**
Lenguaje: C#
Plataforma: .NET (net10.0), aplicación de consola
Editor recomendado: Visual Studio Code
Requisitos
-.NET SDK instalado (versión compatible con net10.0)
-Git (para clonar el repositorio)

**Para comprobar que .NET está instalado:**
bash
dotnet --version

**Cómo ejecutar el proyecto** en la terminal 
1.-Clonar el repositorio:
git clone https://github.com/gabitex/Modulo_Citas_Medicas.git
2.-Entrar a la carpeta del proyecto:
cd Modulo_Citas_Medicas
3.-Ejecutar:
**dotnet run** en la terminal

**Si falla algo, se puede limpiar y compilar de nuevo con:**
dotnet clean
dotnet build
**Cómo se usa**
Al ejecutar aparece este menú:

===== CITAS MÉDICAS =====
1. Agendar cita
2. Cancelar cita
0. Salir

1. Agendar cita

Elegir la especialidad escribiendo su número (cada especialidad tiene un doctor asignado).
Escribir el nombre del paciente. Solo se permiten letras (con tildes y espacios), no números.
Escribir el CI. Solo números, con complemento opcional: guion, número y letra mayúscula. Ejemplos válidos: 1234567 y 1234567-1A.
Escribir el teléfono.
El sistema asigna automáticamente la fecha y hora, registra la cita con un número y muestra la ficha.
Se envía un SMS simulado al paciente y se guarda un registro de auditoría.

2. Cancelar cita: escribir el número de cita que apareció en la ficha. El paciente recibe un SMS de cancelación y se registra en la auditoría.

0. Salir: termina el programa.

Ejemplo de salida
===== CITA REGISTRADA =====
Cita N°      : 1
Especialidad : Pediatría
Doctor       : Dr. Carlos
Paciente     : María Pérez (CI 1234567-1A)
Fecha        : 29/10/2026 15:30
===========================

[SMS] A: 70000000 | Prioridad: 1 | Hola María Pérez, su cita de Pediatría con Dr. Carlos es el 29/10/2026 15:30.
[Auditoría] 15:30:12 - Cita N° 1 registrada para CI 1234567-1A
