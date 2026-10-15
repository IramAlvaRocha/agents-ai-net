using primer_chatbot;
using primer_chatbot.ChatBots;

// Carga de las variables de entorno que se encuentran en el archivo .env
Utilities.CargarVariablesDeEntorno();

await ChatBotOpenAI.correr();