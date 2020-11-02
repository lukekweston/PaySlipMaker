namespace MonthlyPaySlipMaker
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
            this.textBoxPeriodEnd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxHours = new System.Windows.Forms.TextBox();
            this.buttonLoadPerson = new System.Windows.Forms.Button();
            this.textBoxPerson = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCreatePaySlip = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBoxHolidayHoursUsed = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // textBoxPeriodEnd
            // 
            this.textBoxPeriodEnd.Location = new System.Drawing.Point(104, 135);
            this.textBoxPeriodEnd.Name = "textBoxPeriodEnd";
            this.textBoxPeriodEnd.Size = new System.Drawing.Size(100, 20);
            this.textBoxPeriodEnd.TabIndex = 0;
            this.textBoxPeriodEnd.Text = "dd/mm/yyyy";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Period Ending:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Hours Worked:";
            // 
            // textBoxHours
            // 
            this.textBoxHours.Location = new System.Drawing.Point(104, 86);
            this.textBoxHours.Name = "textBoxHours";
            this.textBoxHours.Size = new System.Drawing.Size(100, 20);
            this.textBoxHours.TabIndex = 3;
            // 
            // buttonLoadPerson
            // 
            this.buttonLoadPerson.Location = new System.Drawing.Point(67, 12);
            this.buttonLoadPerson.Name = "buttonLoadPerson";
            this.buttonLoadPerson.Size = new System.Drawing.Size(97, 33);
            this.buttonLoadPerson.TabIndex = 4;
            this.buttonLoadPerson.Text = "Load Person";
            this.buttonLoadPerson.UseVisualStyleBackColor = true;
            this.buttonLoadPerson.Click += new System.EventHandler(this.buttonLoadPerson_Click);
            // 
            // textBoxPerson
            // 
            this.textBoxPerson.Enabled = false;
            this.textBoxPerson.Location = new System.Drawing.Point(104, 60);
            this.textBoxPerson.Name = "textBoxPerson";
            this.textBoxPerson.Size = new System.Drawing.Size(100, 20);
            this.textBoxPerson.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Person:";
            // 
            // buttonCreatePaySlip
            // 
            this.buttonCreatePaySlip.Enabled = false;
            this.buttonCreatePaySlip.Location = new System.Drawing.Point(65, 166);
            this.buttonCreatePaySlip.Name = "buttonCreatePaySlip";
            this.buttonCreatePaySlip.Size = new System.Drawing.Size(99, 28);
            this.buttonCreatePaySlip.TabIndex = 7;
            this.buttonCreatePaySlip.Text = "Create Payslip";
            this.buttonCreatePaySlip.UseVisualStyleBackColor = true;
            this.buttonCreatePaySlip.Click += new System.EventHandler(this.buttonCreatePaySlip_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBoxHolidayHoursUsed
            // 
            this.textBoxHolidayHoursUsed.Location = new System.Drawing.Point(104, 112);
            this.textBoxHolidayHoursUsed.Name = "textBoxHolidayHoursUsed";
            this.textBoxHolidayHoursUsed.Size = new System.Drawing.Size(100, 20);
            this.textBoxHolidayHoursUsed.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-1, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Holiday Hours used:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 206);
            this.Controls.Add(this.textBoxHolidayHoursUsed);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonCreatePaySlip);
            this.Controls.Add(this.textBoxPerson);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonLoadPerson);
            this.Controls.Add(this.textBoxHours);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPeriodEnd);
            this.Name = "Form1";
            this.Text = "Payslip Maker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPeriodEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxHours;
        private System.Windows.Forms.Button buttonLoadPerson;
        private System.Windows.Forms.TextBox textBoxPerson;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCreatePaySlip;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBoxHolidayHoursUsed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

