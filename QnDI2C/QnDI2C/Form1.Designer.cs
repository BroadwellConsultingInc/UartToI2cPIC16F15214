namespace SerialI2CMaster
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bOpenSerial = new System.Windows.Forms.Button();
            this.tbSend0 = new System.Windows.Forms.TextBox();
            this.bSend0 = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.tbSend1 = new System.Windows.Forms.TextBox();
            this.bSend1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bOpenSerial
            // 
            this.bOpenSerial.Location = new System.Drawing.Point(573, 23);
            this.bOpenSerial.Name = "bOpenSerial";
            this.bOpenSerial.Size = new System.Drawing.Size(75, 23);
            this.bOpenSerial.TabIndex = 0;
            this.bOpenSerial.Text = "Open Serial";
            this.bOpenSerial.UseVisualStyleBackColor = true;
            this.bOpenSerial.Click += new System.EventHandler(this.bOpenSerial_Click);
            // 
            // tbSend0
            // 
            this.tbSend0.Location = new System.Drawing.Point(23, 24);
            this.tbSend0.Name = "tbSend0";
            this.tbSend0.Size = new System.Drawing.Size(383, 23);
            this.tbSend0.TabIndex = 1;
            // 
            // bSend0
            // 
            this.bSend0.Enabled = false;
            this.bSend0.Location = new System.Drawing.Point(451, 22);
            this.bSend0.Name = "bSend0";
            this.bSend0.Size = new System.Drawing.Size(75, 23);
            this.bSend0.TabIndex = 2;
            this.bSend0.Text = "Send";
            this.bSend0.UseVisualStyleBackColor = true;
            this.bSend0.Click += new System.EventHandler(this.bSend0_Click);
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(35, 205);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(687, 233);
            this.tbLog.TabIndex = 3;
            // 
            // tbSend1
            // 
            this.tbSend1.Location = new System.Drawing.Point(23, 66);
            this.tbSend1.Name = "tbSend1";
            this.tbSend1.Size = new System.Drawing.Size(383, 23);
            this.tbSend1.TabIndex = 1;
            // 
            // bSend1
            // 
            this.bSend1.Enabled = false;
            this.bSend1.Location = new System.Drawing.Point(451, 64);
            this.bSend1.Name = "bSend1";
            this.bSend1.Size = new System.Drawing.Size(75, 23);
            this.bSend1.TabIndex = 2;
            this.bSend1.Text = "Send";
            this.bSend1.UseVisualStyleBackColor = true;
            this.bSend1.Click += new System.EventHandler(this.bSend1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.bSend1);
            this.Controls.Add(this.bSend0);
            this.Controls.Add(this.tbSend1);
            this.Controls.Add(this.tbSend0);
            this.Controls.Add(this.bOpenSerial);
            this.Name = "Form1";
            this.Text = "QnDI2C";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bOpenSerial;
        private System.Windows.Forms.TextBox tbSend0;
        private System.Windows.Forms.Button bSend0;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.TextBox tbSend1;
        private System.Windows.Forms.Button bSend1;
    }
}

