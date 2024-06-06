using Microsoft.Extensions.Configuration;
using WebKassa;

namespace WebKassaExchanger
{
    public partial class Exchanger : Form
    {
        private WebKassaCore _webKassa;
        private string _filePath;

        public Exchanger(WebKassaCore webKassa)
        {
            _webKassa = webKassa;
            InitializeComponent();
            switch(timer.Enabled)
            {
                case true:
                    autoImportBtn.Text = "Запретить автоимпорт";
                    break;
                case false:
                    autoImportBtn.Text = "Разрешить автоимпорт";
                    break;
            }
        }

        private async void getSalesBtn_ClickAsync(object sender, EventArgs e)
        {
            var res = MessageBox.Show(
                $"Вы уверены, что хотите сделать импорт продаж за {dateTimePicker1.Value.Date.ToString("dd.MM.yyyy")} число?",
                "Attention",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);
            if (res == DialogResult.OK)
            {
                this.Enabled = false;
                try
                {
                    await _webKassa.ImportFromAccountAsync(
                        dateTimePicker1.Value, 
                        Program.Configuration.GetSection("Account").GetSection("CashRegisterId").Get<int>(),
                        Program.Configuration.GetSection("PPS").GetSection("CashierID").Get<int>(),
                        Program.Configuration.GetSection("PPS").GetSection("CashboxID").Get<int>(),
                        Program.Configuration.GetSection("PPS").GetSection("EmployeeID").Get<int>());
                    MessageBox.Show("Done!");
                    this.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = true;
                }
            }
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                var timeOfDay = TimeSpan.FromSeconds(Math.Round(DateTime.Now.TimeOfDay.TotalSeconds));
                var configTime = TimeSpan.Parse(Program.Configuration.GetSection("PPS").GetSection("AutoimportTime").Get<string>());
                if (
                    (timeOfDay >= TimeSpan.Parse(Program.Configuration.GetSection("PPS").GetSection("AutoimportTime").Get<string>()) &
                    (timeOfDay < TimeSpan.Parse(Program.Configuration.GetSection("PPS").GetSection("AutoimportTime").Get<string>()).Add(TimeSpan.FromSeconds(1)))))
                {
                    await _webKassa.ImportFromAccountAsync(
                        DateTime.Now,
                        Program.Configuration.GetSection("Account").GetSection("CashRegisterId").Get<int>(),
                        Program.Configuration.GetSection("PPS").GetSection("CashierID").Get<int>(),
                        Program.Configuration.GetSection("PPS").GetSection("CashboxID").Get<int>(),
                        Program.Configuration.GetSection("PPS").GetSection("EmployeeID").Get<int>());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка во время автовыгрузки, {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;
                importBtn.Enabled = true;
                autoImportBtn.Text = "Разрешить автоимпорт";
                Program.Configuration.GetSection("PPS").GetSection("AutoimportTime").Value = $"{TimeSpan.FromSeconds(Math.Round(dateTimePicker2.Value.TimeOfDay.TotalSeconds))}";
            }
            else
            {
                timer.Enabled = true;
                importBtn.Enabled = false;
                autoImportBtn.Text = "Запретить автоимпорт";
                Program.Configuration.GetSection("PPS").GetSection("AutoimportTime").Value = $"{TimeSpan.FromSeconds(Math.Round(dateTimePicker2.Value.TimeOfDay.TotalSeconds))}";
            }
        }

        private async void exportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    _filePath = fd.SelectedPath;
                }
                await _webKassa.CreateImportFile(_filePath);
                MessageBox.Show("Success!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
