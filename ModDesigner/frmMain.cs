using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog(this);
            txtPath.Text =  folderBrowserDialog1.SelectedPath;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (txtFileExt.Text.Length < 1) return;

            var files = findDesignerFile(txtPath.Text);
            if (files.Count == 0)
            {
                MessageBox.Show("해당 폴더에는 " + txtFileExt.Text + " 파일이 없습니다.", "확인",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("실행하시겠습니까?", "확인",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) 
            {
                return;
            }

            frmModFile f = new frmModFile(txtPath.Text, txtFileExt.Text,files,chkBak.Checked);
            f.ShowDialog();

            //try
            //{
            //    var files = findDesignerFile(txtPath.Text);
            //    foreach (var file in files)
            //    {
            //        modFile(file);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private List<string> findDesignerFile(string filePath)
        {
            var list = new List<string>();
            var dir = Directory.GetFiles(filePath,txtFileExt.Text.Trim(),SearchOption.AllDirectories);
            foreach (var file in dir)
            {
                //MessageBox.Show(file);
                list.Add(file);
            }

            return list;
        }

        //private void modFile(string file)
        //{
        //    StringBuilder sbFile = new StringBuilder();
        //    List<string> fileContent = File.ReadLines(file).ToList();
        //    if(fileContent == null || fileContent.Count == 0) return;

        //    foreach (var line in fileContent)
        //    {
        //        //+= EventHandler 없으면... 추가
        //        if (line.IndexOf("+=") == -1 && line.IndexOf("EventHandler") == -1) 
        //            sbFile.Append(line + "\r\n");

        //    }

        //    if(chkBak.Checked == false) return;

        //    if (File.Exists(file + "_bak"))
        //        File.Delete(file + "_bak");

        //    File.WriteAllLines(file + "_bak", fileContent);

        //    File.WriteAllText(file, sbFile.ToString());

        //    sbFile.Clear();

        //}


        //private bool isDesignerFile(string fileName)
        //{
        //    return fileName.IndexOf("Designer.cs") > -1 ? true : false;
        //}

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
