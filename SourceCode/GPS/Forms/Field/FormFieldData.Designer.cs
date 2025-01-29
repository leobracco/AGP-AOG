﻿namespace AgOpenGPS
{
    partial class FormFieldData
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblTimeRemaining = new System.Windows.Forms.Label();
            this.labelRemain = new System.Windows.Forms.Label();
            this.lblAreaRemain = new System.Windows.Forms.Label();
            this.lblWorkRate = new System.Windows.Forms.Label();
            this.labelRate = new System.Windows.Forms.Label();
            this.lblTotalArea = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            this.lblApplied = new System.Windows.Forms.Label();
            this.labelApplied = new System.Windows.Forms.Label();
            this.lblRemainPercent = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblActualLessOverlap = new System.Windows.Forms.Label();
            this.labelApplied2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblOverlapPercent = new System.Windows.Forms.Label();
            this.labelOverlap = new System.Windows.Forms.Label();
            this.labelActual = new System.Windows.Forms.Label();
            this.labelWorked = new System.Windows.Forms.Label();
            this.lblActualRemain = new System.Windows.Forms.Label();
            this.labelRemain2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 6000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblTimeRemaining
            // 
            this.lblTimeRemaining.AutoSize = true;
            this.lblTimeRemaining.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeRemaining.ForeColor = System.Drawing.Color.White;
            this.lblTimeRemaining.Location = new System.Drawing.Point(87, 153);
            this.lblTimeRemaining.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(18, 23);
            this.lblTimeRemaining.TabIndex = 479;
            this.lblTimeRemaining.Text = "-";
            // 
            // labelRemain
            // 
            this.labelRemain.AutoSize = true;
            this.labelRemain.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRemain.ForeColor = System.Drawing.Color.White;
            this.labelRemain.Location = new System.Drawing.Point(12, 95);
            this.labelRemain.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelRemain.Name = "labelRemain";
            this.labelRemain.Size = new System.Drawing.Size(80, 23);
            this.labelRemain.TabIndex = 478;
            this.labelRemain.Text = "Remain:";
            // 
            // lblAreaRemain
            // 
            this.lblAreaRemain.AutoSize = true;
            this.lblAreaRemain.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAreaRemain.ForeColor = System.Drawing.Color.White;
            this.lblAreaRemain.Location = new System.Drawing.Point(87, 95);
            this.lblAreaRemain.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblAreaRemain.Name = "lblAreaRemain";
            this.lblAreaRemain.Size = new System.Drawing.Size(18, 23);
            this.lblAreaRemain.TabIndex = 480;
            this.lblAreaRemain.Text = "-";
            // 
            // lblWorkRate
            // 
            this.lblWorkRate.AutoSize = true;
            this.lblWorkRate.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkRate.ForeColor = System.Drawing.Color.White;
            this.lblWorkRate.Location = new System.Drawing.Point(58, 325);
            this.lblWorkRate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblWorkRate.Name = "lblWorkRate";
            this.lblWorkRate.Size = new System.Drawing.Size(18, 23);
            this.lblWorkRate.TabIndex = 482;
            this.lblWorkRate.Text = "-";
            // 
            // labelRate
            // 
            this.labelRate.AutoSize = true;
            this.labelRate.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRate.ForeColor = System.Drawing.Color.White;
            this.labelRate.Location = new System.Drawing.Point(10, 325);
            this.labelRate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelRate.Name = "labelRate";
            this.labelRate.Size = new System.Drawing.Size(55, 23);
            this.labelRate.TabIndex = 481;
            this.labelRate.Text = "Rate:";
            // 
            // lblTotalArea
            // 
            this.lblTotalArea.AutoSize = true;
            this.lblTotalArea.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalArea.ForeColor = System.Drawing.Color.White;
            this.lblTotalArea.Location = new System.Drawing.Point(76, 0);
            this.lblTotalArea.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTotalArea.Name = "lblTotalArea";
            this.lblTotalArea.Size = new System.Drawing.Size(18, 23);
            this.lblTotalArea.TabIndex = 484;
            this.lblTotalArea.Text = "-";
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotal.ForeColor = System.Drawing.Color.White;
            this.labelTotal.Location = new System.Drawing.Point(23, 0);
            this.labelTotal.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(58, 23);
            this.labelTotal.TabIndex = 483;
            this.labelTotal.Text = "Total:";
            // 
            // lblApplied
            // 
            this.lblApplied.AutoSize = true;
            this.lblApplied.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplied.ForeColor = System.Drawing.Color.White;
            this.lblApplied.Location = new System.Drawing.Point(87, 66);
            this.lblApplied.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblApplied.Name = "lblApplied";
            this.lblApplied.Size = new System.Drawing.Size(18, 23);
            this.lblApplied.TabIndex = 486;
            this.lblApplied.Text = "-";
            // 
            // labelApplied
            // 
            this.labelApplied.AutoSize = true;
            this.labelApplied.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApplied.ForeColor = System.Drawing.Color.White;
            this.labelApplied.Location = new System.Drawing.Point(13, 66);
            this.labelApplied.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelApplied.Name = "labelApplied";
            this.labelApplied.Size = new System.Drawing.Size(79, 23);
            this.labelApplied.TabIndex = 485;
            this.labelApplied.Text = "Applied:";
            // 
            // lblRemainPercent
            // 
            this.lblRemainPercent.AutoSize = true;
            this.lblRemainPercent.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemainPercent.ForeColor = System.Drawing.Color.White;
            this.lblRemainPercent.Location = new System.Drawing.Point(87, 123);
            this.lblRemainPercent.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblRemainPercent.Name = "lblRemainPercent";
            this.lblRemainPercent.Size = new System.Drawing.Size(18, 23);
            this.lblRemainPercent.TabIndex = 487;
            this.lblRemainPercent.Text = "-";
            this.lblRemainPercent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 19);
            this.label1.TabIndex = 488;
            this.label1.Text = "_________________";
            // 
            // lblActualLessOverlap
            // 
            this.lblActualLessOverlap.AutoSize = true;
            this.lblActualLessOverlap.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActualLessOverlap.ForeColor = System.Drawing.Color.White;
            this.lblActualLessOverlap.Location = new System.Drawing.Point(85, 229);
            this.lblActualLessOverlap.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblActualLessOverlap.Name = "lblActualLessOverlap";
            this.lblActualLessOverlap.Size = new System.Drawing.Size(18, 23);
            this.lblActualLessOverlap.TabIndex = 490;
            this.lblActualLessOverlap.Text = "-";
            // 
            // labelApplied2
            // 
            this.labelApplied2.AutoSize = true;
            this.labelApplied2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApplied2.ForeColor = System.Drawing.Color.White;
            this.labelApplied2.Location = new System.Drawing.Point(9, 229);
            this.labelApplied2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelApplied2.Name = "labelApplied2";
            this.labelApplied2.Size = new System.Drawing.Size(79, 23);
            this.labelApplied2.TabIndex = 489;
            this.labelApplied2.Text = "Applied:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(7, 203);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(162, 19);
            this.label7.TabIndex = 491;
            this.label7.Text = "_________________";
            // 
            // lblOverlapPercent
            // 
            this.lblOverlapPercent.AutoSize = true;
            this.lblOverlapPercent.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOverlapPercent.ForeColor = System.Drawing.Color.White;
            this.lblOverlapPercent.Location = new System.Drawing.Point(86, 287);
            this.lblOverlapPercent.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblOverlapPercent.Name = "lblOverlapPercent";
            this.lblOverlapPercent.Size = new System.Drawing.Size(18, 23);
            this.lblOverlapPercent.TabIndex = 493;
            this.lblOverlapPercent.Text = "-";
            // 
            // labelOverlap
            // 
            this.labelOverlap.AutoSize = true;
            this.labelOverlap.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOverlap.ForeColor = System.Drawing.Color.White;
            this.labelOverlap.Location = new System.Drawing.Point(9, 287);
            this.labelOverlap.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelOverlap.Name = "labelOverlap";
            this.labelOverlap.Size = new System.Drawing.Size(81, 23);
            this.labelOverlap.TabIndex = 492;
            this.labelOverlap.Text = "Overlap:";
            // 
            // labelActual
            // 
            this.labelActual.AutoSize = true;
            this.labelActual.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelActual.ForeColor = System.Drawing.Color.White;
            this.labelActual.Location = new System.Drawing.Point(51, 193);
            this.labelActual.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelActual.Name = "labelActual";
            this.labelActual.Size = new System.Drawing.Size(61, 23);
            this.labelActual.TabIndex = 494;
            this.labelActual.Text = "Actual";
            // 
            // labelWorked
            // 
            this.labelWorked.AutoSize = true;
            this.labelWorked.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWorked.ForeColor = System.Drawing.Color.White;
            this.labelWorked.Location = new System.Drawing.Point(45, 30);
            this.labelWorked.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelWorked.Name = "labelWorked";
            this.labelWorked.Size = new System.Drawing.Size(74, 23);
            this.labelWorked.TabIndex = 495;
            this.labelWorked.Text = "Worked";
            // 
            // lblActualRemain
            // 
            this.lblActualRemain.AutoSize = true;
            this.lblActualRemain.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActualRemain.ForeColor = System.Drawing.Color.White;
            this.lblActualRemain.Location = new System.Drawing.Point(86, 258);
            this.lblActualRemain.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblActualRemain.Name = "lblActualRemain";
            this.lblActualRemain.Size = new System.Drawing.Size(18, 23);
            this.lblActualRemain.TabIndex = 497;
            this.lblActualRemain.Text = "-";
            // 
            // labelRemain2
            // 
            this.labelRemain2.AutoSize = true;
            this.labelRemain2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRemain2.ForeColor = System.Drawing.Color.White;
            this.labelRemain2.Location = new System.Drawing.Point(10, 258);
            this.labelRemain2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelRemain2.Name = "labelRemain2";
            this.labelRemain2.Size = new System.Drawing.Size(80, 23);
            this.labelRemain2.TabIndex = 496;
            this.labelRemain2.Text = "Remain:";
            // 
            // FormFieldData
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(176, 356);
            this.Controls.Add(this.lblWorkRate);
            this.Controls.Add(this.lblActualRemain);
            this.Controls.Add(this.labelRemain2);
            this.Controls.Add(this.labelWorked);
            this.Controls.Add(this.labelActual);
            this.Controls.Add(this.lblOverlapPercent);
            this.Controls.Add(this.labelOverlap);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblActualLessOverlap);
            this.Controls.Add(this.labelApplied2);
            this.Controls.Add(this.lblRemainPercent);
            this.Controls.Add(this.lblApplied);
            this.Controls.Add(this.labelApplied);
            this.Controls.Add(this.lblTotalArea);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.labelRate);
            this.Controls.Add(this.lblAreaRemain);
            this.Controls.Add(this.lblTimeRemaining);
            this.Controls.Add(this.labelRemain);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFieldData";
            this.ShowInTaskbar = false;
            this.Text = "System Data";
            this.Load += new System.EventHandler(this.FormFieldData_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblTimeRemaining;
        private System.Windows.Forms.Label labelRemain;
        private System.Windows.Forms.Label lblAreaRemain;
        private System.Windows.Forms.Label lblWorkRate;
        private System.Windows.Forms.Label labelRate;
        private System.Windows.Forms.Label lblTotalArea;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.Label lblApplied;
        private System.Windows.Forms.Label labelApplied;
        private System.Windows.Forms.Label lblRemainPercent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblActualLessOverlap;
        private System.Windows.Forms.Label labelApplied2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblOverlapPercent;
        private System.Windows.Forms.Label labelOverlap;
        private System.Windows.Forms.Label labelActual;
        private System.Windows.Forms.Label labelWorked;
        private System.Windows.Forms.Label lblActualRemain;
        private System.Windows.Forms.Label labelRemain2;
    }
}