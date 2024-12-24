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
            this.dgvFaultTypeStatistics = new System.Windows.Forms.DataGridView();
            this.btnGetStatistics = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFaultTypeStatistics)).BeginInit();
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
            // dgvFaultTypeStatistics
            // 
            this.dgvFaultTypeStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFaultTypeStatistics.Location = new System.Drawing.Point(330, 36);
            this.dgvFaultTypeStatistics.Name = "dgvFaultTypeStatistics";
            this.dgvFaultTypeStatistics.Size = new System.Drawing.Size(458, 402);
            this.dgvFaultTypeStatistics.TabIndex = 2;
            // 
            // btnGetStatistics
            // 
            this.btnGetStatistics.Location = new System.Drawing.Point(41, 134);
            this.btnGetStatistics.Name = "btnGetStatistics";
            this.btnGetStatistics.Size = new System.Drawing.Size(75, 23);
            this.btnGetStatistics.TabIndex = 3;
            this.btnGetStatistics.Text = "button1";
            this.btnGetStatistics.UseVisualStyleBackColor = true;
            this.btnGetStatistics.Click += new System.EventHandler(this.btnGetStatistics_Click);
            // 
            // StatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnGetStatistics);
            this.Controls.Add(this.dgvFaultTypeStatistics);
            this.Controls.Add(this.lblAverageCompletionTime);
            this.Controls.Add(this.lblCompletedRequests);
            this.Name = "StatisticsForm";
            this.Text = "StatisticsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFaultTypeStatistics)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCompletedRequests;
        private System.Windows.Forms.Label lblAverageCompletionTime;
        private System.Windows.Forms.DataGridView dgvFaultTypeStatistics;
        private System.Windows.Forms.Button btnGetStatistics;
    }
}