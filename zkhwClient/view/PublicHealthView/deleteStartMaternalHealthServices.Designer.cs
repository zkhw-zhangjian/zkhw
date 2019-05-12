namespace zkhwClient.view.PublicHealthView
{
    partial class deleteStartMaternalHealthServices
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
            this.项目 = new System.Windows.Forms.GroupBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.取消 = new System.Windows.Forms.Button();
            this.确定 = new System.Windows.Forms.Button();
            this.项目.SuspendLayout();
            this.SuspendLayout();
            // 
            // 项目
            // 
            this.项目.Controls.Add(this.checkBox4);
            this.项目.Controls.Add(this.checkBox3);
            this.项目.Controls.Add(this.checkBox2);
            this.项目.Controls.Add(this.checkBox1);
            this.项目.Location = new System.Drawing.Point(38, 13);
            this.项目.Margin = new System.Windows.Forms.Padding(4);
            this.项目.Name = "项目";
            this.项目.Padding = new System.Windows.Forms.Padding(4);
            this.项目.Size = new System.Drawing.Size(258, 116);
            this.项目.TabIndex = 3;
            this.项目.TabStop = false;
            this.项目.Text = "项 目";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(147, 66);
            this.checkBox4.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(67, 19);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Tag = "5";
            this.checkBox4.Text = "第5次";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(32, 66);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(67, 19);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Tag = "4";
            this.checkBox3.Text = "第4次";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(147, 26);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(67, 19);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Tag = "3";
            this.checkBox2.Text = "第3次";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(32, 26);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(67, 19);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Tag = "2";
            this.checkBox1.Text = "第2次";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // 取消
            // 
            this.取消.Location = new System.Drawing.Point(196, 137);
            this.取消.Margin = new System.Windows.Forms.Padding(4);
            this.取消.Name = "取消";
            this.取消.Size = new System.Drawing.Size(100, 42);
            this.取消.TabIndex = 5;
            this.取消.Text = "取消";
            this.取消.UseVisualStyleBackColor = true;
            this.取消.Click += new System.EventHandler(this.取消_Click);
            // 
            // 确定
            // 
            this.确定.Location = new System.Drawing.Point(38, 137);
            this.确定.Margin = new System.Windows.Forms.Padding(4);
            this.确定.Name = "确定";
            this.确定.Size = new System.Drawing.Size(100, 42);
            this.确定.TabIndex = 4;
            this.确定.Text = "确定";
            this.确定.UseVisualStyleBackColor = true;
            this.确定.Click += new System.EventHandler(this.确定_Click);
            // 
            // deleteStartMaternalHealthServices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 185);
            this.Controls.Add(this.项目);
            this.Controls.Add(this.取消);
            this.Controls.Add(this.确定);
            this.Name = "deleteStartMaternalHealthServices";
            this.Text = "第2～5次产前随访删除";
            this.项目.ResumeLayout(false);
            this.项目.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox 项目;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button 取消;
        private System.Windows.Forms.Button 确定;
    }
}