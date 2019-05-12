namespace zkhwClient.view.PublicHealthView
{
    partial class deleteEndMaternalHealthServices
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
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.取消 = new System.Windows.Forms.Button();
            this.确定 = new System.Windows.Forms.Button();
            this.项目.SuspendLayout();
            this.SuspendLayout();
            // 
            // 项目
            // 
            this.项目.Controls.Add(this.checkBox2);
            this.项目.Controls.Add(this.checkBox1);
            this.项目.Location = new System.Drawing.Point(13, 13);
            this.项目.Margin = new System.Windows.Forms.Padding(4);
            this.项目.Name = "项目";
            this.项目.Padding = new System.Windows.Forms.Padding(4);
            this.项目.Size = new System.Drawing.Size(258, 72);
            this.项目.TabIndex = 6;
            this.项目.TabStop = false;
            this.项目.Text = "项 目";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(147, 26);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(90, 19);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Tag = "42";
            this.checkBox2.Text = "产后42天";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(32, 26);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(82, 19);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Tag = "7";
            this.checkBox1.Text = "产后7天";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // 取消
            // 
            this.取消.Location = new System.Drawing.Point(171, 93);
            this.取消.Margin = new System.Windows.Forms.Padding(4);
            this.取消.Name = "取消";
            this.取消.Size = new System.Drawing.Size(100, 42);
            this.取消.TabIndex = 8;
            this.取消.Text = "取消";
            this.取消.UseVisualStyleBackColor = true;
            this.取消.Click += new System.EventHandler(this.取消_Click);
            // 
            // 确定
            // 
            this.确定.Location = new System.Drawing.Point(13, 93);
            this.确定.Margin = new System.Windows.Forms.Padding(4);
            this.确定.Name = "确定";
            this.确定.Size = new System.Drawing.Size(100, 42);
            this.确定.TabIndex = 7;
            this.确定.Text = "确定";
            this.确定.UseVisualStyleBackColor = true;
            this.确定.Click += new System.EventHandler(this.确定_Click);
            // 
            // deleteEndMaternalHealthServices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 146);
            this.Controls.Add(this.项目);
            this.Controls.Add(this.取消);
            this.Controls.Add(this.确定);
            this.Name = "deleteEndMaternalHealthServices";
            this.Text = "产后访视删除";
            this.项目.ResumeLayout(false);
            this.项目.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox 项目;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button 取消;
        private System.Windows.Forms.Button 确定;
    }
}