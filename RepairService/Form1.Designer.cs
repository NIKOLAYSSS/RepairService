namespace RepairService
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewRequests = new System.Windows.Forms.DataGridView();
            this.Обновить = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnGetStatistics = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRequests)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewRequests
            // 
            this.dataGridViewRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRequests.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewRequests.Name = "dataGridViewRequests";
            this.dataGridViewRequests.Size = new System.Drawing.Size(606, 426);
            this.dataGridViewRequests.TabIndex = 0;
            // 
            // Обновить
            // 
            this.Обновить.Location = new System.Drawing.Point(639, 64);
            this.Обновить.Name = "Обновить";
            this.Обновить.Size = new System.Drawing.Size(142, 23);
            this.Обновить.TabIndex = 1;
            this.Обновить.Text = "Обновить";
            this.Обновить.UseVisualStyleBackColor = true;
            this.Обновить.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(639, 93);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(142, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Добавить заявку";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnOpenAddRequest);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(639, 129);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(142, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Изменить заявку";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(681, 9);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(100, 20);
            this.txtSearch.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(636, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Поиск";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(639, 35);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(142, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "поиск";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnGetStatistics
            // 
            this.btnGetStatistics.Location = new System.Drawing.Point(639, 158);
            this.btnGetStatistics.Name = "btnGetStatistics";
            this.btnGetStatistics.Size = new System.Drawing.Size(142, 23);
            this.btnGetStatistics.TabIndex = 7;
            this.btnGetStatistics.Text = "Посмотреть статистику";
            this.btnGetStatistics.UseVisualStyleBackColor = true;
            this.btnGetStatistics.Click += new System.EventHandler(this.btnGetStatistics_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(639, 187);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Удалить заявку";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGetStatistics);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Обновить);
            this.Controls.Add(this.dataGridViewRequests);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRequests)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewRequests;
        private System.Windows.Forms.Button Обновить;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnGetStatistics;
        private System.Windows.Forms.Button button1;
    }
}

