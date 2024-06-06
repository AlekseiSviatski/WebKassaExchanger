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
        }

        private async void getSalesBtn_ClickAsync(object sender, EventArgs e)
        {
            this.Enabled = false;
            try
            {
                var sales = (await _webKassa.GetSalesAsync(
                new DateTime(2024, 6, 4),
                Program.Configuration.GetSection("Account").GetSection("CashRegisterId").Get<int>())).ToList();
                MessageBox.Show("Done!");
                this.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var timeOfDay = DateTime.Now.TimeOfDay;
            var configTime = TimeSpan.Parse("12:02:00");
            if ((timeOfDay >= TimeSpan.Parse("12:11:30")) & (timeOfDay <= TimeSpan.Parse("12:11:31")))
            {
                MessageBox.Show("Ты лох");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;
                button1.Text = "Enable";
            }
            else
            {
                timer.Enabled = true;
                button1.Text = "Disable";
            }
        }

        private async void exportBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if(fd.ShowDialog() == DialogResult.OK)
            {
                _filePath = fd.SelectedPath;
            }
            await _webKassa.CreateImportFile(_filePath);
        }
    }
}
