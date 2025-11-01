using ChatGPTWinForm.ClasesOpenAI.ChatGPTWinForms.Services;
using System;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ChatGPTWinForm
{
    public partial class PagPrincipal : Form
    {
        //Instancia de consultas que se conecta a la API de OpenAI
        private readonly OpenAiService _openAiService;

        public PagPrincipal()
        {
            InitializeComponent();

            // Manejo de teclas dentro del TextPrompt
            TextPrompt.KeyDown += (s, e) =>
            {
                // Ctrl + Enter para salto de línea
                if (e.Control && e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    int pos = TextPrompt.SelectionStart;
                    TextPrompt.Text = TextPrompt.Text.Insert(pos, Environment.NewLine);
                    TextPrompt.SelectionStart = pos + Environment.NewLine.Length;
                    return;
                }

                // Enter solito para enviar mensaje
                if (e.KeyCode == Keys.Enter && !e.Control)
                {
                    e.SuppressKeyPress = true;   // evita que se agregue nueva línea
                    BtnEnviar.PerformClick();    // ejecuta el envío
                    return;
                }
            };


            //Estado inicial del Label
            LblEstadoEnvio.Text = "Listo";

            try
            {
                //crear un servicion usando la API key del entorno
                _openAiService = new OpenAiService();
            }
            catch (InvalidOperationException k)
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
            await Task.Yield();
            string prompt = TextPrompt.Text.Trim();

            //Validaciones de el mensaje vacio
            if (string.IsNullOrEmpty(prompt))
            {
                MessageBox.Show("Por favor escribe una pregunta antes del envio");
                return;
            }

            //Validaciones de mensajes demasiados largos
            if (prompt.Length > 500)
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
                AlMessagePrompt($"Asistente AI: {respuesta}", derecha: false);

                LblEstadoEnvio.Text = "Listo";

            }
            catch (HttpRequestException ex)
            {
                LblEstadoEnvio.Text = ex.Message;
            }
            catch (Exception ex)
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
            LblEstadoEnvio.Text = "Chat limpiado";
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

        private void BtnCopiar_Click(object sender, EventArgs e)
        {
            // Convertir el contenido del chat en líneas
            var lineas = TextChat.Lines.ToList();

            // Buscar el índice de la última línea que contiene "Asistente AI:"
            int index = lineas.FindLastIndex(l => l.TrimStart().StartsWith("Asistente AI:"));

            // Validar que haya alguna respuesta
            if (index == -1 || index + 1 >= lineas.Count)
            {
                MessageBox.Show("No hay respuesta del asistente para copiar aún.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Buscar todas las líneas siguientes hasta el separador o final
            StringBuilder sb = new StringBuilder();
            for (int i = index + 1; i < lineas.Count; i++)
            {
                string linea = lineas[i];
                if (linea.TrimStart().StartsWith("----")) break; // detener si llega al separador
                sb.AppendLine(linea);
            }

            string texto = sb.ToString().Trim();

            // Validar si está vacío
            if (string.IsNullOrWhiteSpace(texto))
            {
                MessageBox.Show("El texto está vacío, no se puede copiar.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Copiar texto válido al portapapeles
            Clipboard.SetText(texto);
            LblEstadoEnvio.Text = "Respuesta copiada";
        }


        private void BtnGuardar_Click(object sender, EventArgs e) //Boton para guardar el historial
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog()) // Crear y configurar el cuadro de diálogo "Guardar como"
            {
                saveFileDialog.Title = "Guardar historial de chat";
                saveFileDialog.Filter = "Archivo de texto (*.txt)|*.txt"; // Para permitir solamente guardar como archivo.txt
                saveFileDialog.FileName = "HistorialChatGPT.txt";      // Nombre sugerido por defecto

                if (saveFileDialog.ShowDialog() == DialogResult.OK)  // Mostrar el cuadro de dialogo y verificar si el usuario presiono Guardar
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, TextChat.Text, Encoding.UTF8); // Escribir el contenido del TextBox del chat en el archivo elegido

                        MessageBox.Show($"Historial guardado en:\n{saveFileDialog.FileName}", // Para confirmar que se guardó correctamente
                            "Guardado exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LblEstadoEnvio.Text = "Historial guardado"; // Mostrar mensaje en el lbl de estado
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar el archivo:\n{ex.Message}", //Mensaje si ocurre alguna excepcion
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    LblEstadoEnvio.Text = "Guardado cancelado"; //Mensaje si el usuario cancela 
                }
            }
        }


        // Atajos de teclado globales
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Ctrl + L para limpiar el chat
            if (keyData == (Keys.Control | Keys.L))
            {
                BtnLimpiar.PerformClick();
                return true; // evita que la tecla siga su curso normal
            }

            // Ctrl + S para guardar historial
            if (keyData == (Keys.Control | Keys.S))
            {
                BtnGuardar.PerformClick();
                return true;
            }

            // Ctrl + C para copiar última respuesta
            if (keyData == (Keys.Control | Keys.C))
            {
                BtnCopiar.PerformClick();
                return true;
            }

            // Si no coincide con ningún atajo, sigue normal
            return base.ProcessCmdKey(ref msg, keyData);
        }


    }
}
