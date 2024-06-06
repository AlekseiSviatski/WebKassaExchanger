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
            components = new System.ComponentModel.Container();
            getSalesBtn = new Button();
            panel1 = new Panel();
            dateTimePicker1 = new DateTimePicker();
            button1 = new Button();
            exportBtn = new Button();
            timer = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // getSalesBtn
            // 
            getSalesBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            getSalesBtn.Location = new Point(3, 190);
            getSalesBtn.Name = "getSalesBtn";
            getSalesBtn.Size = new Size(193, 35);
            getSalesBtn.TabIndex = 0;
            getSalesBtn.Text = "Импорт из личного кабинета";
            getSalesBtn.UseVisualStyleBackColor = true;
            getSalesBtn.Click += getSalesBtn_ClickAsync;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(dateTimePicker1);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(exportBtn);
            panel1.Controls.Add(getSalesBtn);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(363, 228);
            panel1.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.CustomFormat = "dd.MM.yyyy";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Location = new Point(12, 12);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(103, 23);
            dateTimePicker1.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(28, 119);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Disable";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // exportBtn
            // 
            exportBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            exportBtn.Location = new Point(213, 190);
            exportBtn.Name = "exportBtn";
            exportBtn.Size = new Size(147, 35);
            exportBtn.TabIndex = 1;
            exportBtn.Text = "Создать файл экспорта";
            exportBtn.UseVisualStyleBackColor = true;
            exportBtn.Click += exportBtn_Click;
            // 
            // timer
            // 
            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
            // 
            // Exchanger
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(363, 228);
            Controls.Add(panel1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Exchanger";
            Text = "Form1";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button getSalesBtn;
        private Panel panel1;
        private Button exportBtn;
        private System.Windows.Forms.Timer timer;
        private Button button1;
        private DateTimePicker dateTimePicker1;
    }
}
