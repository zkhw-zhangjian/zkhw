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
    public partial class operation_record : Form
    {

        public string operation_name = "";
        public string operation_time = "";
        public operation_record()
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
            operation_name = this.textBox1.Text;
            operation_time = this.dateTimePicker1.Text;
            this.DialogResult = DialogResult.OK;

        }
    }
}
