namespace WebKassaExchanger
{
    partial class Exchanger
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
            getSalesBtn = new Button();
            SuspendLayout();
            // 
            // getSalesBtn
            // 
            getSalesBtn.Location = new Point(78, 152);
            getSalesBtn.Name = "getSalesBtn";
            getSalesBtn.Size = new Size(75, 23);
            getSalesBtn.TabIndex = 0;
            getSalesBtn.Text = "Тык!";
            getSalesBtn.UseVisualStyleBackColor = true;
            getSalesBtn.Click += getSalesBtn_ClickAsync;
            // 
            // Exchanger
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(230, 209);
            Controls.Add(getSalesBtn);
            Name = "Exchanger";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button getSalesBtn;
    }
}
