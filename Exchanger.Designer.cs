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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Exchanger));
            importBtn = new Button();
            panel1 = new Panel();
            dateTimePicker2 = new DateTimePicker();
            dateTimePicker1 = new DateTimePicker();
            autoImportBtn = new Button();
            exportBtn = new Button();
            timer = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // importBtn
            // 
            importBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            importBtn.Location = new Point(121, 12);
            importBtn.Name = "importBtn";
            importBtn.Size = new Size(193, 23);
            importBtn.TabIndex = 0;
            importBtn.Text = "Импорт из личного кабинета";
            importBtn.UseVisualStyleBackColor = true;
            importBtn.Click += getSalesBtn_ClickAsync;
            // 
            // panel1
            // 
            panel1.Controls.Add(dateTimePicker2);
            panel1.Controls.Add(dateTimePicker1);
            panel1.Controls.Add(autoImportBtn);
            panel1.Controls.Add(exportBtn);
            panel1.Controls.Add(importBtn);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(324, 128);
            panel1.TabIndex = 1;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.CustomFormat = "HH:mm:ss";
            dateTimePicker2.Format = DateTimePickerFormat.Time;
            dateTimePicker2.Location = new Point(12, 39);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(103, 23);
            dateTimePicker2.TabIndex = 4;
            dateTimePicker2.TabStop = false;
            dateTimePicker2.Value = new DateTime(2024, 6, 6, 23, 59, 30, 0);
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.CustomFormat = "dd.MM.yyyy";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Location = new Point(12, 12);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(103, 23);
            dateTimePicker1.TabIndex = 3;
            dateTimePicker1.TabStop = false;
            // 
            // autoImportBtn
            // 
            autoImportBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            autoImportBtn.Location = new Point(121, 41);
            autoImportBtn.Name = "autoImportBtn";
            autoImportBtn.Size = new Size(193, 22);
            autoImportBtn.TabIndex = 2;
            autoImportBtn.Text = "Разрешить автоимпорт";
            autoImportBtn.UseVisualStyleBackColor = true;
            autoImportBtn.Click += button1_Click;
            // 
            // exportBtn
            // 
            exportBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            exportBtn.Location = new Point(56, 68);
            exportBtn.Name = "exportBtn";
            exportBtn.Size = new Size(220, 51);
            exportBtn.TabIndex = 1;
            exportBtn.Text = "Создать файл экспорта из ППС";
            exportBtn.UseVisualStyleBackColor = true;
            exportBtn.Click += exportBtn_Click;
            // 
            // timer
            // 
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
            // 
            // Exchanger
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(324, 128);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Exchanger";
            Text = "WebKassa Exchanger";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button importBtn;
        private Panel panel1;
        private Button exportBtn;
        private System.Windows.Forms.Timer timer;
        private Button autoImportBtn;
        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;
    }
}
