using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
namespace ProjectIO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string sourceFolderPath = @"T:\COMP";
        string targetFolderPath = @"U:\TEST";

        public void RefreshListBox(ListBox lb, string[] sa)
        {
            lb.Items.Clear();
            lb.Items.AddRange(sa);//AddRange is to add the whole array
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog diaglog = new FolderBrowserDialog();
            diaglog.RootFolder = Environment.SpecialFolder.MyComputer;
            diaglog.SelectedPath = sourceFolderPath;
            diaglog.Description = "Please select a folder.";
            diaglog.ShowNewFolderButton = true; //allows creating a new folder
            DialogResult result = diaglog.ShowDialog();

            if (result == DialogResult.OK)
            {
                sourceFolderPath = diaglog.SelectedPath;
                lblDescription.Text = "Folder: " + sourceFolderPath;
                rtbNoteDisplay.Text = "You have selected " + sourceFolderPath +
                    " as source folder.";
            }
            else
            {
                MessageBox.Show("operation canceled by user");
                return;
            }

        }

        private void btnListSub_Click(object sender, EventArgs e)
        {
            lstDisplay.Items.Clear(); //clear the listbox
            //all subfolder paths will be stored in subfolderPaths array
            String[] subFolderPaths = Directory.GetDirectories(sourceFolderPath);
            //Now display all subfolder paths in ListBox
            if (subFolderPaths.Length > 0)
            {
                RefreshListBox(lstDisplay, subFolderPaths);
                rtbNoteDisplay.Text = "Subfolders of " + sourceFolderPath + " are listed now.";
            }
            else
            {
                rtbNoteDisplay.Text = sourceFolderPath + " doesn't have subfolders.";
            }
        }

        private void btnListFiles_Click(object sender, EventArgs e)
        {
            lstDisplay.Items.Clear(); //clear the listbox
            //all file paths will be stored in filePaths array
            String[] filePaths = Directory.GetFiles(sourceFolderPath);
            //now display all paths in ListBox lstDisplay
            if (filePaths.Length > 0)
            {
                RefreshListBox(lstDisplay, filePaths);
                rtbNoteDisplay.Text = "Files in " + sourceFolderPath + " are listed now.";
            } 
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if ((lstDisplay.SelectedIndex != -1))
            {
                string filePath = lstDisplay.SelectedItem.ToString();
                //target file path should be a full path, not just the folder path
                string targetFilePath = filePath.Replace(sourceFolderPath, targetFolderPath);
                try
                {
                    File.Copy(filePath, targetFilePath, true);
                    rtbNoteDisplay.Text = filePath + " is copied to " + targetFolderPath;
                }
                catch (Exception ex)
                {
                    rtbNoteDisplay.Text = ex.Message;
                }
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            if ((lstDisplay.SelectedIndex != -1))
            {
                string filePath = lstDisplay.SelectedItem.ToString();
                string newFilePath = filePath.Replace(targetFolderPath, targetFolderPath);
                try
                {
                    File.Move(filePath, newFilePath);
                    //refresh the ListBox is required as one file has been moved out
                    btnListFiles.PerformClick();
                    rtbNoteDisplay.Text = filePath + " is moved to " + targetFolderPath;
                }
                catch (Exception ex)
                {
                    rtbNoteDisplay.Text = ex.Message;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ((lstDisplay.SelectedIndex != -1))
            {
                string filePath = lstDisplay.SelectedItem.ToString();
                try
                {
                    File.Delete(filePath);
                    rtbNoteDisplay.Text = filePath + " is deleted.";
                    btnListFiles.PerformClick();
                }
                catch (Exception ex)
                {
                    rtbNoteDisplay.Text = ex.Message;
                }
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if ((lstDisplay.SelectedIndex != -1))
            {
                string filePath = lstDisplay.SelectedItem.ToString();
                //get the index of the last \
                int lastSlash = filePath.LastIndexOf('\\');
                string currentFileName = filePath.Substring(lastSlash + 1);
                //add MS VisualBasic reference for InputBox
                string newFileName = Interaction.InputBox(
                    "Current file name is " + currentFileName + @"
Please type new file name.");
                string newFilePath = Path.Combine(sourceFolderPath, newFileName);
                try
                {
                    File.Move(filePath, newFilePath);
                    btnListFiles.PerformClick();
                    rtbNoteDisplay.Text = currentFileName + " is renamed " + newFileName;
                }
                catch (Exception ex)
                {
                    rtbNoteDisplay.Text = ex.Message;
                }
            }
        }
    }
}
