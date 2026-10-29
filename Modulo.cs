using System;
using System.Collections.Generic;

// SRP: Paciente, Especialidad y Cita solo almacenan datos
public record Paciente(string Nombre, string Ci, string Telefono);
public record Especialidad(string Nombre, string Doctor);
public record Cita(Paciente Paciente, Especialidad Especialidad, DateTime Fecha, int Numero = 0);

// OCP: agregar una especialidad = agregar una línea aquí
public static class CatalogoEspecialidades
{
    public static List<Especialidad> ObtenerTodas() => new()
    {
        new("Medicina General",     "Dr. René"),
        new("Ginecología",          "Dra. María"),
        new("Pediatría",            "Dr. Carlos"),
        new("Otorrinolaringología", "Dra. Ana"),
        new("Medicina Interna",     "Dr. José"),
        new("Dermatología",         "Dra. Lucía"),
        new("Emergencia",           "Dr. Urgencias"),
    };
}

// PATRÓN CREACIONAL (Builder): ConstructorCita arma la cita paso a paso y le pone la fecha y hora automáticamente
public class ConstructorCita
{
    private Paciente? _paciente;
    private Especialidad? _especialidad;

    public ConstructorCita ConPaciente(Paciente paciente) { _paciente = paciente; return this; }
    public ConstructorCita ConEspecialidad(Especialidad especialidad) { _especialidad = especialidad; return this; }

    public Cita Construir()
    {
        if (_paciente == null || _especialidad == null)
            throw new InvalidOperationException("Faltan paciente o especialidad.");
        return new Cita(_paciente, _especialidad, DateTime.Now);
    }
}

// ISP: interfaz mínima de notificación (un solo método)
public interface INotificador
{
    void Notificar(string destino, string mensaje);
}

// Servicio externo con interfaz incompatible (no se puede modificar)
public class ServicioSmsExterno
{
    public void EnviarSms(string numero, string texto, int prioridad) =>
        Console.WriteLine($"[SMS] A: {numero} | Prioridad: {prioridad} | {texto}");
}

// PATRÓN ESTRUCTURAL (Adapter): AdaptadorSms adapta ServicioSmsExterno a la interfaz INotificador
// LSP: AdaptadorSms es sustituible por cualquier otro INotificador (correo, WhatsApp)
public class AdaptadorSms : INotificador
{
    private readonly ServicioSmsExterno _servicio = new();

    public void Notificar(string destino, string mensaje) =>
        _servicio.EnviarSms(destino, mensaje, prioridad: 1);
}

// ISP: interfaz mínima de observador (dos métodos de aviso)
public interface IObservadorCitas
{
    void CitaAgendada(Cita cita);
    void CitaCancelada(Cita cita);
}

// DIP: NotificadorObservador depende de la abstracción INotificador, no del SMS concreto
public class NotificadorObservador : IObservadorCitas
{
    private readonly INotificador _notificador;
    public NotificadorObservador(INotificador notificador) => _notificador = notificador;

    public void CitaAgendada(Cita cita) => _notificador.Notificar(cita.Paciente.Telefono,
        $"Hola {cita.Paciente.Nombre}, su cita de {cita.Especialidad.Nombre} con {cita.Especialidad.Doctor} es el {cita.Fecha:dd/MM/yyyy HH:mm}.");

    public void CitaCancelada(Cita cita) => _notificador.Notificar(cita.Paciente.Telefono,
        $"Hola {cita.Paciente.Nombre}, su cita N° {cita.Numero} de {cita.Especialidad.Nombre} fue cancelada.");
}

// SRP: AuditoriaObservador solo deja el registro de auditoría
public class AuditoriaObservador : IObservadorCitas
{
    public void CitaAgendada(Cita cita) =>
        Console.WriteLine($"[Auditoría] {DateTime.Now:HH:mm:ss} - Cita N° {cita.Numero} registrada para CI {cita.Paciente.Ci}");

    public void CitaCancelada(Cita cita) =>
        Console.WriteLine($"[Auditoría] {DateTime.Now:HH:mm:ss} - Cita N° {cita.Numero} cancelada (CI {cita.Paciente.Ci})");
}

// PATRÓN DE COMPORTAMIENTO (Observer): GestorCitas avisa a los observadores suscritos cuando se agenda o cancela una cita
// OCP: se agregan observadores nuevos sin modificar GestorCitas
public class GestorCitas
{
    private readonly List<IObservadorCitas> _observadores = new();
    private readonly List<Cita> _citas = new();
    private int _siguienteNumero = 1;

    public void Suscribir(IObservadorCitas observador) => _observadores.Add(observador);

    public void Agendar(Cita cita)
    {
        cita = cita with { Numero = _siguienteNumero++ };
        _citas.Add(cita);

        Console.WriteLine("\n===== CITA REGISTRADA =====");
        Console.WriteLine($"Cita N°      : {cita.Numero}");
        Console.WriteLine($"Especialidad : {cita.Especialidad.Nombre}");
        Console.WriteLine($"Doctor       : {cita.Especialidad.Doctor}");
        Console.WriteLine($"Paciente     : {cita.Paciente.Nombre} (CI {cita.Paciente.Ci})");
        Console.WriteLine($"Fecha        : {cita.Fecha:dd/MM/yyyy HH:mm}");
        Console.WriteLine("===========================\n");

        foreach (var observador in _observadores) observador.CitaAgendada(cita);
    }

    public void Cancelar(int numero)
    {
        var cita = _citas.Find(c => c.Numero == numero);
        if (cita == null) { Console.WriteLine("\nNo existe una cita con ese número."); return; }

        _citas.Remove(cita);
        Console.WriteLine($"\nCITA N° {numero} CANCELADA\n");

        foreach (var observador in _observadores) observador.CitaCancelada(cita);
    }
}