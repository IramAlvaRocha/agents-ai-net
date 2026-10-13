using System;
using System.Collections.Generic;
using System.Text;

namespace primer_chatbot
{
    internal class Utilities
    {
        internal static void CargarVariablesDeEntorno()
        {
            var ruta = Path.Combine(AppContext.BaseDirectory, ".env");

            foreach (var linea in File.ReadAllLines(ruta))
            {
                var partes = linea.Split('=', 2);
                if (partes.Length == 2)
                {
                    var nombre = partes[0].Trim();
                    var valor = partes[1].Trim().Trim('"', '\'');

                    Environment.SetEnvironmentVariable(nombre, valor);
                }
            }
        }
    }
}
