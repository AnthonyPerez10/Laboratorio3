using ChatGPTWinForm.ClasesOpenAI.ChatGPTWinForms.Services;

namespace ChatGPTWinForm
{
    public partial class PagPrincipal : Form
    {
        public PagPrincipal()
        {
            InitializeComponent();
        }

        //Lbl De titulo 
        private void LblTitle_Click(object sender, EventArgs e)
        {

        }

        //TextChat de recibir los resultados 
        /*Controlar resultados derecha, consultas a la izquierda*/
        private void TextChat_TextChanged(object sender, EventArgs e)
        {

        }

        //TextPrompt de enviar preguntas a la API
        private void TextPrompt_TextChanged(object sender, EventArgs e)
        {

        }

        //Boton de enviar la consultas a API 
        private void BtnEnviar_Click(object sender, EventArgs e)
        {

        }

        //Boton de limpiar resultados. 
        private void BtnLimpiar_Click(object sender, EventArgs e)
        {

        }

        /*Labels de estado de envio que se actualizar dinamicamente: 
         “Listo”, “Enviando…”, “API key inválida o ausente.”, "Límite alcanzado, espera unos segundos.”
        "Error de envio"*/
        private void LblEstadoEnvio_Click(object sender, EventArgs e)
        {

        }
    }
}
