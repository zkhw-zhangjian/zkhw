﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.view
{
    public partial class metachysis_record : Form
    {

        public string metachysis_reasonn = "";
        public string metachysis_time = "";
        public metachysis_record()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void follow_medicine_record_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            metachysis_reasonn = this.textBox1.Text;
            metachysis_time = this.dateTimePicker1.Text;
            this.DialogResult = DialogResult.OK;

        }
    }
}
