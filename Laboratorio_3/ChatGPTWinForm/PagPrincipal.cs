using ChatGPTWinForm.ClasesOpenAI.ChatGPTWinForms.Services;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTWinForm
{
    public partial class PagPrincipal : Form
    {
        //Instancia de consultas que se conecta a la API de OpenAI
        private readonly OpenAiService _openAiService; 

        public PagPrincipal()
        {
            InitializeComponent();

            //Estado inicial del Label
            LblEstadoEnvio.Text = "Listo";

            try
            {
                //crear un servicion usando la API key del entorno
                _openAiService = new OpenAiService();
            }catch(InvalidOperationException k)
            {
                //Si no hay API key valida, se desactiva el boton del enviar 
                LblEstadoEnvio.Text = k.Message;
                BtnEnviar.Enabled = false;
            }
        }

        //Lbl De titulo 
        private void LblTitle_Click(object sender, EventArgs e)
        {

        }

        //TextChat de recibir los resultados
        private void TextChat_TextChanged(object sender, EventArgs e)
        {

        }

        //TextPrompt de enviar preguntas a la API
        private void TextPrompt_TextChanged(object sender, EventArgs e)
        {

        }

        //Boton de enviar la consultas a API 
        private async void BtnEnviar_Click(object sender, EventArgs e)
        {
            string prompt = TextPrompt.Text.Trim();

            //Validaciones de el mensaje vacio
            if (string.IsNullOrEmpty(prompt))
            {
                MessageBox.Show("Por favor escribe una pregunta antes del envio");
                return;
            }

            //Validaciones de mensajes demasiados largos
            if(prompt.Length > 500)
            {
                MessageBox.Show("El mensaje pasa los 500 caracteres permitidos");
                return; 
            }

            //Validacion de bloquera el boton de envio cuando se realiza una consulta
            BtnEnviar.Enabled = false;
            LblEstadoEnvio.Text = "Enviando...";

            //Mostrar el mensaje de la consulta a la derecha
            AlMessagePrompt($"Usuario: {prompt}", derecha: true);

            try
            {
                //Obtener respuesta del gpt
                string respuesta = await _openAiService.GetCompletionAsync(prompt);

                //Limitar las repuesta 
                respuesta = LimitarPalabras(respuesta, 1000);

                //Mostra la respuesta del asistente gpt
                AlMessagePrompt($"Asistente AI: {respuesta}", derecha:false);

                LblEstadoEnvio.Text = "Listo";

            }catch(HttpRequestException ex)
            {
                LblEstadoEnvio.Text = ex.Message;
            }
            catch(Exception ex)
            {
                LblEstadoEnvio.Text = $"Error: {ex.Message}";
            }
            finally
            {
                //Reactivar los boton y limpiar promt
                BtnEnviar.Enabled = true;
                TextPrompt.Clear();
            }
        }

        // Metodo para alineacion de texto izquierda y derecha 
        private void AlMessagePrompt(string texto, bool derecha)
        {
            // Línea separadora entre mensajes
            string separador = new string('-', 67);

            // Si el mensaje es del usuario
            if (derecha)
            {
                // Simula alineación a la derecha con sangría
                TextChat.AppendText(Environment.NewLine);
                TextChat.AppendText($"Usuario: {texto.Replace("Usuario: ", "")}" + Environment.NewLine);
                TextChat.AppendText(Environment.NewLine);
            }
            else
            {
                // Mensaje del asistente
                TextChat.AppendText($"Asistente AI:" + Environment.NewLine);
                TextChat.AppendText(texto.Replace("Asistente AI:", "").Trim() + Environment.NewLine);
                TextChat.AppendText(Environment.NewLine + separador + Environment.NewLine);
            }

            // Scroll automático hacia el final
            TextChat.SelectionStart = TextChat.Text.Length;
            TextChat.ScrollToCaret();
        }

        //Limitar las respuesta a mil palabras
        private static string LimitarPalabras(string texto, int maxPalabras)
        {
            var palabras = texto.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (palabras.Length <= maxPalabras)
                return texto;

            // Truncar y agregar indicación de respuesta truncada
            return string.Join(' ', palabras.Take(maxPalabras)) + "... [respuesta finalizada]";
        }

        //Boton de limpiar resultados. 
        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            TextChat.Clear();             // Limpiar TextBox de chat
            TextPrompt.Clear();           // Limpiar TextBox de prompt
            _openAiService.LimpiarHistorial(); // Limpiar historial de la conversación
            LblEstadoEnvio.Text = "Listo";
        }


        /*Labels de estado de envio que se actualizar dinamicamente: 
         “Listo”, “Enviando…”, “API key inválida o ausente.”, "Límite alcanzado, espera unos segundos.”
        "Error de envio"*/
        private void LblEstadoEnvio_Click(object sender, EventArgs e)
        {
            // Mostrar un mensaje informativo al usuario cuando hace clic en el label
            MessageBox.Show("El estado actual del envío es: " + LblEstadoEnvio.Text,
                            "Estado de Envío",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }
    }
}
