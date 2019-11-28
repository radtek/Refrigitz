﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShizoImprove
{
    public partial class FormShizoImprove : Form
    {
        public String path = "D:\\";//'DVD'\\";
        ShizoImprove t = null;
        public FormShizoImprove()
        {
            InitializeComponent();
        }
   
        private void buttonSearchAndTree_Click(object sender, EventArgs e)
        {
            t = new ShizoImprove(path);
        }

        private void FormShizoImprove_Load(object sender, EventArgs e)
        {

        }

        private void buttonImproveCollection_Click(object sender, EventArgs e)
        {
            if (ShizoImprove.AllFiles.Count == 0)
                t = new ShizoImprove(path);
            progressBarWorking.Maximum = ShizoImprove.AllFiles.Count;
            progressBarWorking.Minimum = 0;
            t.FormShizoImprove(textBoxWorkingProject.Text, ref progressBarWorking);

        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            path = textBoxInput.Text;
        }

        private void buttonSetImprove_Click(object sender, EventArgs e)
        {
            textBoxInput.Text = "C:\\ShizoImprove\\" + textBoxWorkingProject.Text + "\\";
            textBoxOutput.Text = "C:\\ShizoImprove\\Improved\\";
            path = textBoxInput.Text;
            buttonImproved.Enabled = true;
        }

        private void textBoxOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ShizoImprove.AllFiles.Count > 0)
            {
                ShizoImprove.AllFiles.Clear();
                t = new ShizoImprove(path);

                progressBarWorking.Maximum = ShizoImprove.AllFiles.Count;
                progressBarWorking.Minimum = 0;
                t.FormImprove(textBoxWorkingProject.Text, ref progressBarWorking);
            }
            else
            {
                t = new ShizoImprove(path);

                progressBarWorking.Maximum = ShizoImprove.AllFiles.Count;
                progressBarWorking.Minimum = 0;
                t.FormImprove(textBoxWorkingProject.Text, ref progressBarWorking);
            }
        }
    }
}