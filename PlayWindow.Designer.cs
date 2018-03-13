namespace Worms
{
    partial class PlayWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayWindow));
            this.Time = new System.Windows.Forms.Timer(this.components);
            this.timeMovePress = new System.Windows.Forms.Timer(this.components);
            this.ShowStructure = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Time
            // 
            this.Time.Interval = 10;
            this.Time.Tick += new System.EventHandler(this.Time_Tick);
            // 
            // ShowStructure
            // 
            this.ShowStructure.Location = new System.Drawing.Point(1263, 12);
            this.ShowStructure.Name = "ShowStructure";
            this.ShowStructure.Size = new System.Drawing.Size(75, 23);
            this.ShowStructure.TabIndex = 0;
            this.ShowStructure.Text = "Показать матрицу";
            this.ShowStructure.UseVisualStyleBackColor = true;
            this.ShowStructure.Click += new System.EventHandler(this.ShowStructure_Click);
            this.ShowStructure.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PlayWindow_KeyPress);
            // 
            // PlayWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.ShowStructure);
            this.Name = "PlayWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PlayWindow_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PlayWindow_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer Time;
        private System.Windows.Forms.Timer timeMovePress;
        private System.Windows.Forms.Button ShowStructure;
    }
}

