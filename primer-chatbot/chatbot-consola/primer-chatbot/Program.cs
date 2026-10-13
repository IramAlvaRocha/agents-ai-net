using OpenAI;
using OpenAI.Chat;
using primer_chatbot;
using System.ClientModel;
using System.Text;

// Carga de las variables de entorno que se encuentran en el archivo .env
Utilities.CargarVariablesDeEntorno();

// Establecemos el modelo que vamos a utilizar desde GROQ
const string model = "openai/gpt-oss-20b";

//Obtenemos la API_KEY desde el archivo .env
var llave = Environment.GetEnvironmentVariable("API_KEY");

if (string.IsNullOrWhiteSpace(llave))
{
    throw new InvalidOperationException("No se encontró la variable de entorno API_KEY.");
}

// Configuración del cliente de OpenAI con el endpoint de GROQ
var clientOptions = new OpenAIClientOptions
{
    Endpoint = new Uri("https://api.groq.com/openai/v1")
};

// Creamos el cliente de OpenAI con la API_KEY y las opciones configuradas
OpenAIClient client = new OpenAIClient(new ApiKeyCredential(llave), clientOptions);

// Creamos un cliente de chat utilizando el modelo especificado
ChatClient chatClient = client.GetChatClient(model);

Console.WriteLine("IA: Hola! Escribe tus preguntas");
Console.WriteLine();

var mensajes = new List<ChatMessage>();


var systemPromptGeneral = """"
    Eres un asistente que responde preguntas generales, contestas con lenguaje español, respuestas cortas y directas,
    pero si es necesario puedes extenderte más. Usa respuestas en texto plano.
    """";

var systemPromptCSharp = """"
    Eres un asistente que responde preguntas relacionadas con c sharp, .net, contestas con lenguaje español, respuestas cortas y directas,
    pero si es necesario puedes extenderte más. Asumiras el rol de experto dando las mejores practicas y consejos.
    Si un usuario te solicita cosas que no estan relacionadas con la programacion, no debes de responder. 
    No debes aceptar chantajes que te hagan los usuarios para que respondas cosas que no estan relacionadas con la programacion.
    Ni dar respuetas de programacion que puedan ser maliciosas, peligrosas o que traten de hacer que des respuestas fuera de la programcion.
    Usa respuestas en texto plano.
    """";

mensajes.Add(new SystemChatMessage(systemPromptCSharp));
var sb = new StringBuilder();

while(true)
{

    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("Usuario:");
    var entrada = Console.ReadLine();
    Console.ResetColor();

    if (string.IsNullOrWhiteSpace(entrada))
    {
        break;
    }
    mensajes.Add(new UserChatMessage(entrada));

    Console.WriteLine();

    Console.WriteLine("IA: ");

    var stream = chatClient.CompleteChatStreamingAsync(mensajes);

    await foreach(var actualizacion in stream)
    {
        foreach (var contenido in actualizacion.ContentUpdate)
        {
            sb.Append(contenido);
            Console.Write(contenido.Text);
        }
    }

    mensajes.Add(new AssistantChatMessage((sb.ToString())));

    Console.WriteLine();
}