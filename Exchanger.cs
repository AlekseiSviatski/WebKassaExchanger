using Microsoft.Extensions.Configuration;
using WebKassa;

namespace WebKassaExchanger
{
    public partial class Exchanger : Form
    {
        private WebKassaCore _webKassa;

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
                var sales = await _webKassa.GetSalesAsync(
                new DateTime(2024, 6, 4),
                Program.Configuration.GetSection("Account").GetSection("CashRegisterId").Get<int>());
                this.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                this.Enabled = true;
            }
        }

    }
}
