namespace RepairService
{
    partial class StatisticsForm
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
            this.lblCompletedRequests = new System.Windows.Forms.Label();
            this.lblAverageCompletionTime = new System.Windows.Forms.Label();
            this.btnGetStatistics = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCompletedRequests
            // 
            this.lblCompletedRequests.AutoSize = true;
            this.lblCompletedRequests.Location = new System.Drawing.Point(61, 36);
            this.lblCompletedRequests.Name = "lblCompletedRequests";
            this.lblCompletedRequests.Size = new System.Drawing.Size(35, 13);
            this.lblCompletedRequests.TabIndex = 0;
            this.lblCompletedRequests.Text = "label1";
            // 
            // lblAverageCompletionTime
            // 
            this.lblAverageCompletionTime.AutoSize = true;
            this.lblAverageCompletionTime.Location = new System.Drawing.Point(61, 91);
            this.lblAverageCompletionTime.Name = "lblAverageCompletionTime";
            this.lblAverageCompletionTime.Size = new System.Drawing.Size(35, 13);
            this.lblAverageCompletionTime.TabIndex = 1;
            this.lblAverageCompletionTime.Text = "label2";
            // 
            // btnGetStatistics
            // 
            this.btnGetStatistics.Location = new System.Drawing.Point(41, 134);
            this.btnGetStatistics.Name = "btnGetStatistics";
            this.btnGetStatistics.Size = new System.Drawing.Size(145, 23);
            this.btnGetStatistics.TabIndex = 3;
            this.btnGetStatistics.Text = "Показать статистику";
            this.btnGetStatistics.UseVisualStyleBackColor = true;
            this.btnGetStatistics.Click += new System.EventHandler(this.btnGetStatistics_Click);
            // 
            // StatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 170);
            this.Controls.Add(this.btnGetStatistics);
            this.Controls.Add(this.lblAverageCompletionTime);
            this.Controls.Add(this.lblCompletedRequests);
            this.Name = "StatisticsForm";
            this.Text = "StatisticsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCompletedRequests;
        private System.Windows.Forms.Label lblAverageCompletionTime;
        private System.Windows.Forms.Button btnGetStatistics;
    }
}