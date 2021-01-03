using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NET_Wykład_Zadanie_dodatkowe_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void writeToTextbox(string address)
        {
            var numberOfCharacters = File.ReadAllText("test.txt").Count(c => c.Equals(" ") || (!Char.IsControl(c) && !Char.IsWhiteSpace(c)));
            var fileSize = new System.IO.FileInfo("test.txt").Length;

            textBox2.Text = "Address URL: " + address + Environment.NewLine + "Number of characters: " + numberOfCharacters + Environment.NewLine + "Size of file: " + fileSize;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string address;
            if (checkBox1.Checked && checkBox2.Checked)
            {
                MessageBox.Show("Check only one chceckBox!");
            }
            else if (checkBox1.Checked && !checkBox2.Checked)
            {
                address = addressURLtextBox.Text;
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadProgressChanged += (s, e2) => { progressBar1.Value = e2.ProgressPercentage; };
                    webClient.DownloadFile(address, "test.txt");
                }
                writeToTextbox(address);
            }
            else if (!checkBox1.Checked && checkBox2.Checked)
            {
                address = addressURLtextBox.Text;
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(delegate (object sender2, DownloadProgressChangedEventArgs e2)
                    {
                        progressBar1.Value = e2.ProgressPercentage;
                    });
                    webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(delegate (object sender3, System.ComponentModel.AsyncCompletedEventArgs e3)
                        {
                            if(e3.Error == null && !e3.Cancelled)
                            {
                                writeToTextbox(address);
                            }
                        });
                    webClient.DownloadFileAsync(new Uri(address), "test.txt");
                }
            }
            else MessageBox.Show("Check synchronously or asynchronously");
        }
    }
}
