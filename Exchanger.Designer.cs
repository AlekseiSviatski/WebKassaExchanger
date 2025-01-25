using System;
using System.Drawing;
using System.Windows.Forms;

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
            timer = new System.Windows.Forms.Timer(components);
            importCheckBox = new CheckBox();
            label1 = new Label();
            cashboxComboBox = new ComboBox();
            dateTimePicker2 = new DateTimePicker();
            dateTimePicker1 = new DateTimePicker();
            autoImportBtn = new Button();
            exportBtn = new Button();
            importBtn = new Button();
            SuspendLayout();
            // 
            // timer
            // 
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
            // 
            // importCheckBox
            // 
            importCheckBox.AutoSize = true;
            importCheckBox.Location = new Point(436, 14);
            importCheckBox.Name = "importCheckBox";
            importCheckBox.Size = new Size(110, 19);
            importCheckBox.TabIndex = 15;
            importCheckBox.Text = "общий импорт";
            importCheckBox.UseVisualStyleBackColor = true;
            importCheckBox.CheckedChanged += importCheckBox_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 15);
            label1.Name = "label1";
            label1.Size = new Size(184, 15);
            label1.TabIndex = 14;
            label1.Text = "Рег. номер программной кассы";
            // 
            // cashboxComboBox
            // 
            cashboxComboBox.FormattingEnabled = true;
            cashboxComboBox.Location = new Point(204, 12);
            cashboxComboBox.Name = "cashboxComboBox";
            cashboxComboBox.Size = new Size(226, 23);
            cashboxComboBox.TabIndex = 13;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.CustomFormat = "HH:mm:ss";
            dateTimePicker2.Format = DateTimePickerFormat.Time;
            dateTimePicker2.Location = new Point(14, 84);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(103, 23);
            dateTimePicker2.TabIndex = 12;
            dateTimePicker2.TabStop = false;
            dateTimePicker2.Value = new DateTime(2024, 6, 6, 23, 59, 30, 0);
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.CustomFormat = "dd.MM.yyyy";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Location = new Point(14, 55);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(103, 23);
            dateTimePicker1.TabIndex = 11;
            dateTimePicker1.TabStop = false;
            // 
            // autoImportBtn
            // 
            autoImportBtn.Location = new Point(125, 84);
            autoImportBtn.Name = "autoImportBtn";
            autoImportBtn.Size = new Size(423, 22);
            autoImportBtn.TabIndex = 10;
            autoImportBtn.Text = "Разрешить автоимпорт";
            autoImportBtn.UseVisualStyleBackColor = true;
            autoImportBtn.Click += autoImportBtn_Click;
            // 
            // exportBtn
            // 
            exportBtn.Location = new Point(14, 112);
            exportBtn.Name = "exportBtn";
            exportBtn.Size = new Size(534, 36);
            exportBtn.TabIndex = 9;
            exportBtn.Text = "Создать файл экспорта из ППС";
            exportBtn.UseVisualStyleBackColor = true;
            exportBtn.Click += exportBtn_Click_1;
            // 
            // importBtn
            // 
            importBtn.Location = new Point(125, 55);
            importBtn.Name = "importBtn";
            importBtn.Size = new Size(423, 23);
            importBtn.TabIndex = 8;
            importBtn.Text = "Импорт из личного кабинета";
            importBtn.UseVisualStyleBackColor = true;
            importBtn.Click += importBtn_Click;
            // 
            // Exchanger
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(559, 160);
            Controls.Add(importCheckBox);
            Controls.Add(label1);
            Controls.Add(cashboxComboBox);
            Controls.Add(dateTimePicker2);
            Controls.Add(dateTimePicker1);
            Controls.Add(autoImportBtn);
            Controls.Add(exportBtn);
            Controls.Add(importBtn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Exchanger";
            Text = "WebKassa Exchanger";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private CheckBox importCheckBox;
        private Label label1;
        private ComboBox cashboxComboBox;
        private DateTimePicker dateTimePicker2;
        private DateTimePicker dateTimePicker1;
        private Button autoImportBtn;
        private Button exportBtn;
        private Button importBtn;
    }
}
