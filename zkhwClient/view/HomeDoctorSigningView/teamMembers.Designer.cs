﻿namespace zkhwClient.view.HomeDoctorSigningView
{
    partial class teamMembers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.新建团队 = new System.Windows.Forms.Button();
            this.新建成员 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(73, 23);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "团队:";
            // 
            // 新建团队
            // 
            this.新建团队.Location = new System.Drawing.Point(226, 21);
            this.新建团队.Name = "新建团队";
            this.新建团队.Size = new System.Drawing.Size(75, 23);
            this.新建团队.TabIndex = 2;
            this.新建团队.Text = "新建团队";
            this.新建团队.UseVisualStyleBackColor = true;
            this.新建团队.Click += new System.EventHandler(this.新建团队_Click);
            // 
            // 新建成员
            // 
            this.新建成员.Location = new System.Drawing.Point(327, 21);
            this.新建成员.Name = "新建成员";
            this.新建成员.Size = new System.Drawing.Size(75, 23);
            this.新建成员.TabIndex = 4;
            this.新建成员.Text = "新建成员";
            this.新建成员.UseVisualStyleBackColor = true;
            this.新建成员.Click += new System.EventHandler(this.新建成员_Click_1);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Location = new System.Drawing.Point(24, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1030, 575);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // teamMembers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1079, 635);
            this.Controls.Add(this.新建成员);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.新建团队);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Name = "teamMembers";
            this.Text = "团队成员";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button 新建团队;
        private System.Windows.Forms.Button 新建成员;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}