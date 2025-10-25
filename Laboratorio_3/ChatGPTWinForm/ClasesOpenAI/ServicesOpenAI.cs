
namespace ChatGPTWinForm.ClasesOpenAI
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    namespace ChatGPTWinForms.Services
    {
        /// Clase para conectarse a la API de OpenAI y obtener respuestas del modelo de chat.
        public class OpenAiService
        {
            private readonly HttpClient _httpClient; //Cliente de HTTP para hacer la solictudes
            private readonly string _endpoint; // URL del endpont de OpenAI para chat completions
            private readonly string _model;  //Modelo de IA que se usara para las consultas 
            private readonly List<(string role, string content)> _historial = new(); // Memoria corta
            private readonly int _maxMensajes = 10; // Guardar últimos N turnos

            // Constructor qye configura la Api y el modelo q utlizas
            public OpenAiService(string? apiKey = null, string? baseUrl = null, string? model = null)
            {
                // Leer API key desde variable de entorno
                apiKey ??= Environment.GetEnvironmentVariable("OPENAI_API_KEY");

                if (string.IsNullOrWhiteSpace(apiKey))
                    throw new InvalidOperationException("API key inválida o ausente. Configura OPENAI_API_KEY en tu sistema.");

                // Configurar cliente HTTP
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Endpoint y modelo por defecto (OpenAI)
                _endpoint = baseUrl ?? "https://api.openai.com/v1/chat/completions";
                _model = model ?? "gpt-4o-mini";
            }

            // Metodo asincrono para enviar un pregunta para obtener repuesta 
            public async Task<string> GetCompletionAsync(string prompt)
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(prompt))
                    throw new ArgumentException("El mensaje no puede estar vacío.");
                if (prompt.Length > 1000)
                    prompt = prompt.Substring(0, 1000);

                // Normalizar saltos de línea
                prompt = prompt.Replace("\r\n", "\n");

                // Agregar mensaje del usuario al historial
                _historial.Add(("user", prompt));

                // Limitar cantidad de mensajes guardados
                if (_historial.Count > _maxMensajes * 2)
                    _historial.RemoveRange(0, _historial.Count - _maxMensajes * 2);

                // Armar JSON con el historial reciente
                var messages = _historial.Select(h => new { role = h.role, content = h.content }).ToArray();
                var requestBody = new { model = _model, messages = messages };

                string json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response = await _httpClient.PostAsync(_endpoint, content);

                    // Manejo de errores HTTP comunes
                    if (!response.IsSuccessStatusCode)
                    {
                        switch ((int)response.StatusCode)
                        {
                            case 401: throw new HttpRequestException("API key inválida o ausente."); //Indica que la clave es invalida
                            case 429: throw new HttpRequestException("Límite alcanzado, espera unos segundos."); // Indica que se alzando el limite de solictud configurado
                            case >= 500: throw new HttpRequestException("Servicio ocupado. Reintentar."); // error de servios con el servidor 
                            default:
                                string err = await response.Content.ReadAsStringAsync();
                                throw new HttpRequestException($"Error API ({response.StatusCode}): {err}");
                        }
                    }

                    // Leer y extraer la respuesta del asistente
                    var responseJson = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseJson);
                    string textoRespuesta = doc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString() ?? "(sin respuesta)";

                    textoRespuesta = textoRespuesta.Trim();

                    // Guardar respuesta en el historial
                    _historial.Add(("assistant", textoRespuesta));

                    return textoRespuesta; //Devuelve el texto que ofrece el asistebte de IA
                }
                catch (TaskCanceledException){throw new HttpRequestException("No se pudo conectar. Intenta de nuevo.");}
                catch (HttpRequestException)
                {
                    throw; // Propagar error HTTP específico
                }
                catch
                {
                    throw new Exception("Error inesperado al comunicarse con la API.");
                }
            }

            // Limpia la conversación actual
            public void LimpiarHistorial() {  _historial.Clear();}

            // Configuración alternativa para usar Groq en lugar de OpenAI
            public static OpenAiService CreateGroq(string apiKey, string model = "llama-3.1-8b-instant")
            {
                return new OpenAiService(apiKey, "https://api.groq.com/openai/v1/chat/completions", model);
            }
        }
    }
}
