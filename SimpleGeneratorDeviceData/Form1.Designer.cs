namespace SimpleGeneratorDeviceData
{
    partial class Form1
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
            this.checkBox_TempDev = new System.Windows.Forms.CheckBox();
            this.checkBox_PresDev = new System.Windows.Forms.CheckBox();
            this.button_sendData = new System.Windows.Forms.Button();
            this.label_title = new System.Windows.Forms.Label();
            this.label_resultmsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBox_TempDev
            // 
            this.checkBox_TempDev.AutoSize = true;
            this.checkBox_TempDev.Location = new System.Drawing.Point(44, 57);
            this.checkBox_TempDev.Name = "checkBox_TempDev";
            this.checkBox_TempDev.Size = new System.Drawing.Size(123, 17);
            this.checkBox_TempDev.TabIndex = 0;
            this.checkBox_TempDev.Text = "Temperature Device";
            this.checkBox_TempDev.UseVisualStyleBackColor = true;
            // 
            // checkBox_PresDev
            // 
            this.checkBox_PresDev.AutoSize = true;
            this.checkBox_PresDev.Location = new System.Drawing.Point(44, 92);
            this.checkBox_PresDev.Name = "checkBox_PresDev";
            this.checkBox_PresDev.Size = new System.Drawing.Size(104, 17);
            this.checkBox_PresDev.TabIndex = 0;
            this.checkBox_PresDev.Text = "Pressure Device";
            this.checkBox_PresDev.UseVisualStyleBackColor = true;
            // 
            // button_sendData
            // 
            this.button_sendData.Location = new System.Drawing.Point(33, 141);
            this.button_sendData.Name = "button_sendData";
            this.button_sendData.Size = new System.Drawing.Size(134, 23);
            this.button_sendData.TabIndex = 1;
            this.button_sendData.Text = "Send data";
            this.button_sendData.UseVisualStyleBackColor = true;
            this.button_sendData.Click += new System.EventHandler(this.button_sendData_Click);
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_title.Location = new System.Drawing.Point(30, 19);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(137, 13);
            this.label_title.TabIndex = 2;
            this.label_title.Text = "Сheck needed devices";
            // 
            // label_resultmsg
            // 
            this.label_resultmsg.AutoSize = true;
            this.label_resultmsg.ForeColor = System.Drawing.Color.Green;
            this.label_resultmsg.Location = new System.Drawing.Point(30, 180);
            this.label_resultmsg.Name = "label_resultmsg";
            this.label_resultmsg.Size = new System.Drawing.Size(0, 13);
            this.label_resultmsg.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 228);
            this.Controls.Add(this.label_resultmsg);
            this.Controls.Add(this.label_title);
            this.Controls.Add(this.button_sendData);
            this.Controls.Add(this.checkBox_PresDev);
            this.Controls.Add(this.checkBox_TempDev);
            this.Name = "Form1";
            this.Text = "Generator Data";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_TempDev;
        private System.Windows.Forms.CheckBox checkBox_PresDev;
        private System.Windows.Forms.Button button_sendData;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Label label_resultmsg;
    }
}

