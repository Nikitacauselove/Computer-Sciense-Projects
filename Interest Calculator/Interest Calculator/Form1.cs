using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Interest_Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static bool IsValid(TextBox input)
        {
            Regex pattern = null;
            string caption = null;
            string message = null;

            switch (input.Name)
            {
                case "sumTextBox":
                    pattern = new Regex(@"^[0-9]+(?:[,][0-9]+)?\r?$");
                    caption = "Сумма вклада введена некорректно";
                    message = "Пожалуйста, введите сумму в рублях без дополнительных символов";
                    break;
                case "percentTextBox":
                    pattern = new Regex(@"^[0-9]+\r?$");
                    caption = "Процентная ставка введена некорректно";
                    message = "Пожалуйста, введите численное значение без дополнительных символов";
                    break;
                case "intervalTextBox":
                    pattern = new Regex(@"^[0-9]+\r?$");
                    caption = "Срок размещения введен некорректно";
                    message = "Пожалуйста, введите количество месяцев";
                    break;

            }
            if (pattern.IsMatch(input.Text) && (double.Parse(input.Text) != 0))
            {
                return true;
            }
            else
            {
                MessageBox.Show(message, caption);
                return false;
            }
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (IsValid(sumTextBox) && IsValid(percentTextBox) && IsValid(intervalTextBox))
            {
                var sum = double.Parse(sumTextBox.Text);
                var percent = double.Parse(percentTextBox.Text) / 100;
                var interval = Math.Floor(double.Parse(intervalTextBox.Text) / 12);

                var income = sum * percent * interval;
                var effectiveRate = Math.Round(Math.Pow(1 + percent / 12, 12 * interval) - 1, 4) * 100;

                finalSunTextBox.Text = (sum + income).ToString() + " рублей";
                incomeTextBox.Text = (income).ToString() + " рублей";
                effectiveRateTextBox.Text = (effectiveRate).ToString() + "%";
            }
        }
    }
}
