using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WebKassa;
using WebKassa.Models;

namespace WebKassaExchanger
{
	public partial class Exchanger : Form
	{
		private WebKassaCore _webKassa;
		private string _filePath = string.Empty;
		private AccountConfig? _accountConfig;
		private PPSConfig? _ppsConfig;
		private readonly Logger? _logger;

		public Exchanger(WebKassaCore webKassa)
		{
			_logger = LogManager.GetCurrentClassLogger();
			_accountConfig = Program.Configuration?.GetSection("Account")?.Get<AccountConfig>();
			_ppsConfig = Program.Configuration?.GetSection("PPS")?.Get<PPSConfig>();
			_webKassa = webKassa;

			InitializeComponent();

			switch (timer.Enabled)
			{
				case true:
					autoImportBtn.Text = "��������� ����������";
					break;
				case false:
					autoImportBtn.Text = "��������� ����������";
					break;
			}

			cashboxComboBox.DataSource = _accountConfig?.CashboxPairs;
			cashboxComboBox.DisplayMember = "ProgramCashboxId";
		}

		private async void timer_Tick(object sender, EventArgs e)
		{
			try
			{
				var timeOfDay = TimeSpan.FromSeconds(Math.Round(DateTime.Now.TimeOfDay.TotalSeconds));

				if (
					(timeOfDay >= _ppsConfig!.AutoimportTime) &
					(timeOfDay < _ppsConfig!.AutoimportTime?.Add(TimeSpan.FromSeconds(1)))
				   )
				{
					if (importCheckBox.Checked)
					{
						await _webKassa.ImportFromAccountAsync(
						DateTime.Now,
						_accountConfig!.CashboxPairs);
					}
					else
					{
						await _webKassa.ImportFromAccountAsync(
						dateTimePicker1.Value,
						new List<CashboxPair> { (CashboxPair)cashboxComboBox.SelectedItem });
					}
				}
			}
			catch (Exception ex)
			{
				_logger?.Error(ex);
				MessageBox.Show($"��������� ������ �� ����� ������������, {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void importBtn_Click(object sender, EventArgs e)
		{
			try
			{
				if (importCheckBox.Checked)
				{
					var res = MessageBox.Show(
					$"�� �������, ��� ������ ������� ������ ������ �� ���� ����������� ���� �� {dateTimePicker1.Value.Date.ToString("dd.MM.yyyy")} �����?",
					"Attention",
					MessageBoxButtons.OKCancel,
					MessageBoxIcon.Warning);

					if (res == DialogResult.OK)
					{
						this.Enabled = false;
						await _webKassa.ImportFromAccountAsync(
						dateTimePicker1.Value,
						_accountConfig!.CashboxPairs);
						MessageBox.Show("���������!");
						this.Enabled = true;
					}
				}
				else
				{
					var res = MessageBox.Show(
					$"�� �������, ��� ������ ������� ������ ������ �� ����������� ����� � {((CashboxPair)cashboxComboBox.SelectedItem).ProgramCashboxId} �� {dateTimePicker1.Value.Date.ToString("dd.MM.yyyy")} �����?",
					"Attention",
					MessageBoxButtons.OKCancel,
					MessageBoxIcon.Warning);
					if (res == DialogResult.OK)
					{
						this.Enabled = false;
						await _webKassa.ImportFromAccountAsync(dateTimePicker1.Value,
							new List<CashboxPair> { (CashboxPair)cashboxComboBox.SelectedItem }
							);
						MessageBox.Show("���������!");
						this.Enabled = true;
					}
				}
			}
			catch (Exception ex)
			{
				_logger?.Error(ex);
				MessageBox.Show($"{ex.Message}. ������ �� ��������!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Enabled = true;
			}
		}

		private void autoImportBtn_Click(object sender, EventArgs e)
		{
			if (timer.Enabled)
			{
				timer.Enabled = false;
				importBtn.Enabled = true;
				autoImportBtn.Text = "��������� ����������";
				_ppsConfig!.AutoimportTime = TimeSpan.FromSeconds(Math.Round(dateTimePicker2.Value.TimeOfDay.TotalSeconds));
			}
			else
			{
				timer.Enabled = true;
				importBtn.Enabled = false;
				autoImportBtn.Text = "��������� ����������";
				_ppsConfig!.AutoimportTime = TimeSpan.FromSeconds(Math.Round(dateTimePicker2.Value.TimeOfDay.TotalSeconds));
			}
		}

		private void importCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (importCheckBox.Checked)
			{
				cashboxComboBox.Enabled = false;
				cashboxComboBox.SelectedItem = null;
			}
			else
			{
				cashboxComboBox.Enabled = true;
				cashboxComboBox.SelectedItem = ((List<CashboxPair>)cashboxComboBox.DataSource).FirstOrDefault();
			}
		}

		private async void exportBtn_Click_1(object sender, EventArgs e)
		{
			try
			{
				FolderBrowserDialog fd = new FolderBrowserDialog();
				if (fd.ShowDialog() == DialogResult.OK)
				{
					_filePath = fd.SelectedPath;
				}
				await _webKassa.CreateImportFileAsync(_filePath);
				MessageBox.Show("���������!");
			}
			catch (Exception ex)
			{
				_logger?.Error(ex);
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}