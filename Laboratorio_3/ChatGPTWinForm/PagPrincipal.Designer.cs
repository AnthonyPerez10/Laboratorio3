namespace ChatGPTWinForm
{
    partial class PagPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LblTitle = new Label();
            TextChat = new TextBox();
            TextPrompt = new TextBox();
            BtnEnviar = new Button();
            BtnLimpiar = new Button();
            LblEstadoEnvio = new Label();
            SuspendLayout();
            // 
            // LblTitle
            // 
            LblTitle.AutoSize = true;
            LblTitle.Font = new Font("Showcard Gothic", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblTitle.Location = new Point(199, 40);
            LblTitle.Name = "LblTitle";
            LblTitle.Size = new Size(289, 23);
            LblTitle.TabIndex = 0;
            LblTitle.Text = "CHATGPT - Client WinForms ";
            LblTitle.Click += LblTitle_Click;
            // 
            // TextChat
            // 
            TextChat.BackColor = Color.White;
            TextChat.CausesValidation = false;
            TextChat.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TextChat.Location = new Point(47, 86);
            TextChat.Multiline = true;
            TextChat.Name = "TextChat";
            TextChat.ReadOnly = true;
            TextChat.ScrollBars = ScrollBars.Vertical;
            TextChat.Size = new Size(572, 317);
            TextChat.TabIndex = 1;
            TextChat.TextChanged += TextChat_TextChanged;
            // 
            // TextPrompt
            // 
            TextPrompt.BackColor = SystemColors.Window;
            TextPrompt.BorderStyle = BorderStyle.FixedSingle;
            TextPrompt.Cursor = Cursors.IBeam;
            TextPrompt.Font = new Font("Arial Narrow", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TextPrompt.Location = new Point(47, 441);
            TextPrompt.Multiline = true;
            TextPrompt.Name = "TextPrompt";
            TextPrompt.ScrollBars = ScrollBars.Vertical;
            TextPrompt.Size = new Size(413, 31);
            TextPrompt.TabIndex = 2;
            TextPrompt.TextChanged += TextPrompt_TextChanged;
            // 
            // BtnEnviar
            // 
            BtnEnviar.BackColor = SystemColors.ActiveCaption;
            BtnEnviar.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnEnviar.ForeColor = SystemColors.ActiveCaptionText;
            BtnEnviar.Location = new Point(475, 440);
            BtnEnviar.Name = "BtnEnviar";
            BtnEnviar.Size = new Size(75, 33);
            BtnEnviar.TabIndex = 3;
            BtnEnviar.Text = "Enviar";
            BtnEnviar.UseVisualStyleBackColor = false;
            BtnEnviar.Click += BtnEnviar_Click;
            // 
            // BtnLimpiar
            // 
            BtnLimpiar.BackColor = SystemColors.ActiveCaption;
            BtnLimpiar.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnLimpiar.ForeColor = SystemColors.ActiveCaptionText;
            BtnLimpiar.Location = new Point(556, 440);
            BtnLimpiar.Name = "BtnLimpiar";
            BtnLimpiar.Size = new Size(75, 33);
            BtnLimpiar.TabIndex = 4;
            BtnLimpiar.Text = "Limpiar";
            BtnLimpiar.UseVisualStyleBackColor = false;
            BtnLimpiar.Click += BtnLimpiar_Click;
            // 
            // LblEstadoEnvio
            // 
            LblEstadoEnvio.AutoSize = true;
            LblEstadoEnvio.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblEstadoEnvio.Location = new Point(54, 494);
            LblEstadoEnvio.Name = "LblEstadoEnvio";
            LblEstadoEnvio.Size = new Size(58, 18);
            LblEstadoEnvio.TabIndex = 5;
            LblEstadoEnvio.Text = "Estado";
            LblEstadoEnvio.Click += LblEstadoEnvio_Click;
            // 
            // PagPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(679, 566);
            Controls.Add(LblEstadoEnvio);
            Controls.Add(BtnLimpiar);
            Controls.Add(BtnEnviar);
            Controls.Add(TextPrompt);
            Controls.Add(TextChat);
            Controls.Add(LblTitle);
            Font = new Font("Segoe UI Historic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "PagPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GPT 4 - Client API";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label LblTitle;
        private TextBox TextChat;
        private TextBox TextPrompt;
        private Button BtnEnviar;
        private Button BtnLimpiar;
        private Label LblEstadoEnvio;
    }
}
