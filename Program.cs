using System;
using System.Text.RegularExpressions;

public class Programa
{
    public static void Main()
    {
        // Observer: suscribimos quienes reaccionan al agendar y cancelar
        var gestor = new GestorCitas();
        gestor.Suscribir(new NotificadorObservador(new AdaptadorSms()));
        gestor.Suscribir(new AuditoriaObservador());

        var especialidades = CatalogoEspecialidades.ObtenerTodas();
        int opcion;

        do
        {
            Console.WriteLine("\n===== CITAS MÉDICAS =====");
            Console.WriteLine("1. Agendar cita");
            Console.WriteLine("2. Cancelar cita");
            Console.WriteLine("0. Salir");
            Console.Write("Opción: ");
            int.TryParse(Console.ReadLine(), out opcion);

            if (opcion == 1)
            {
                for (int i = 0; i < especialidades.Count; i++)
                    Console.WriteLine($"{i + 1}. {especialidades[i].Nombre}");

                Console.Write("\nElija especialidad (número): ");
                int numeroEspecialidad = int.Parse(Console.ReadLine() ?? "1");
                string nombre = Leer("Nombre del paciente: ", @"^\p{L}+( \p{L}+)*$", "Solo letras, sin números.");
                string ci = Leer("CI (ej: 1234567 o 1234567-1A): ", @"^\d+(-\d[A-Z])?$", "Solo números; el complemento es guion, número y letra mayúscula (ej: -1A).");
                Console.Write("Teléfono: ");
                string telefono = Console.ReadLine() ?? "";

                // Builder: la cita se arma paso a paso
                Cita cita = new ConstructorCita()
                    .ConPaciente(new Paciente(nombre, ci, telefono))
                    .ConEspecialidad(especialidades[numeroEspecialidad - 1])
                    .Construir();

                gestor.Agendar(cita);
            }
            else if (opcion == 2)
            {
                Console.Write("N° de cita a cancelar: ");
                int.TryParse(Console.ReadLine(), out int numero);
                gestor.Cancelar(numero);
            }
        } while (opcion != 0);

        Console.WriteLine("Hasta luego.");
    }

    // Pide un dato y repite la pregunta hasta que cumpla el formato
    private static string Leer(string mensaje, string patron, string error)
    {
        while (true)
        {
            Console.Write(mensaje);
            string valor = (Console.ReadLine() ?? "").Trim();
            if (Regex.IsMatch(valor, patron)) return valor;
            Console.WriteLine(error);
        }
    }
}