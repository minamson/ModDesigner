using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmModFile : Form
    {
        bool chkBak = false;
        string fileExt = string.Empty;
        List<string> files;
        string filePath = string.Empty;
        BackgroundWorker worker;
        public frmModFile(string path, string fileExt, List<string> fileList, bool chkBak)
        {
            InitializeComponent();
            this.files = fileList;
            this.chkBak = chkBak;
            this.filePath = path;
            this.filePath = fileExt;

            Init();

        }
        private void Init()
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            progressBar1.Maximum = 100;// files.Count;

        }
        private void frmModFile_Shown(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();
            btnOk.Enabled = false;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
           
            try
            {
                int i = 0;
                int cnt = files.Count;
                foreach (var file in this.files)
                {
                    modFile(file);
                    var p = (100 * (double) (i + 1) / files.Count);
                    worker.ReportProgress((int)p, files[i]);
                    i++;
                }
            }
            catch (Exception ex)
            {
                worker.CancelAsync();
                MessageBox.Show(ex.Message); 
                btnOk.Text = "닫 기(실패)";
            }
            finally
            {
                worker.Dispose();
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;

            lblMsg.Text = string.Format("처리중 : {0} ", e.UserState);
            labelCount.Text = string.Format("Progress : {0} / {1} ", e.ProgressPercentage, progressBar1.Maximum);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = 100;
            lblMsg.Text = "처리완료....";
            labelCount.Text = this.files.Count.ToString() + "/" + this.files.Count.ToString();
            btnOk.Enabled = true;
        }

        private void modFile(string file)
        {
            StringBuilder sbFile = new StringBuilder();
            string fileText = File.ReadAllText(file);
            
            if(fileText.Contains("+=") && fileText.Contains("EventHandler"))
            {

                List<string> fileContent = File.ReadLines(file).ToList();
                if (fileContent == null || fileContent.Count == 0) return;

                foreach (var line in fileContent)
                {
                    //+= EventHandler 없으면... 추가
                    if (line.IndexOf("+=") == -1 && line.IndexOf("EventHandler") == -1)
                        sbFile.Append(line + "\r\n");

                }

                if (chkBak == false) return;

                if (File.Exists(file + "_bak"))
                    File.Delete(file + "_bak");

                File.WriteAllLines(file + "_bak", fileContent);

                File.WriteAllText(file, sbFile.ToString());

                sbFile.Clear();

            }

        }


        private bool isDesignerFile(string fileName)
        {
            return fileName.IndexOf(this.fileExt) > -1 ? true : false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
