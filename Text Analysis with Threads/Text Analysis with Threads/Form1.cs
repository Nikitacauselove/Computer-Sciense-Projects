using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Threading;



namespace Text_Analysis_with_Threads
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string filePath = string.Empty;

        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = ".txt|*.txt";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
            }
        }

        private void buttonStartAnalysis_Click(object sender, EventArgs e)
        {
            buttonStartAnalysis.Enabled = false;
            textBoxOutput.Visible = false;
            textBoxOutput.Clear();

            if (filePath != string.Empty)
            {
                progressBar.Style = ProgressBarStyle.Marquee;
                progressBar.Visible = true;
                var analysingThread = new Thread(analysis);
                analysingThread.Start();
            }  
            else
                MessageBox.Show("Пожалуйста, выберите файл", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

            buttonStartAnalysis.Enabled = true;
        }




        private void analysis()
        {
            Dictionary<string, int> wordFrequency = new Dictionary<string, int>();
            string formattedText;
            string[] lineContent;
            char[] separators = {' ', '.', ',', '!', '?', ';', ':', '-', '(', ')', '{', '}', '[', ']'};

            using (StreamReader SR = new StreamReader(filePath))
            {
                while (!SR.EndOfStream)
                {
                    formattedText = SR.ReadLine().ToLower();
                    lineContent = formattedText.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < lineContent.Length; i++)
                        if (wordFrequency.ContainsKey(lineContent[i]))
                            wordFrequency[lineContent[i]]++;
                        else
                            wordFrequency.Add(lineContent[i], 1);
                }
            }
            output(wordFrequency);
        }

        private void output(Dictionary<string, int> wordFrequency)
        {
            ICollection<string> keys = wordFrequency.Keys;
            if (keys.Count == 0)
                this.textBoxOutput.BeginInvoke((MethodInvoker)(() => this.textBoxOutput.AppendText("В тексте нет слов" + Environment.NewLine)));
            else
                foreach (string key in keys)
                {
                    this.textBoxOutput.BeginInvoke((MethodInvoker)(() => this.textBoxOutput.AppendText("Слово " + key + " встречается " + wordFrequency[key] + " раз" + Environment.NewLine)));              
                }

            Thread.Sleep(1000);

            this.progressBar.BeginInvoke((MethodInvoker)(() => this.progressBar.Visible = false));
            this.progressBar.BeginInvoke((MethodInvoker)(() => this.progressBar.Style = ProgressBarStyle.Blocks));
            this.textBoxOutput.BeginInvoke((MethodInvoker)(() => this.textBoxOutput.Visible = true));
        }
    }
}
