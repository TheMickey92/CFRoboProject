namespace ConnectFour
{
    partial class ChoosePlayer
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
            this.numPlayer = new System.Windows.Forms.NumericUpDown();
            this.btnPlay = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // numPlayer
            // 
            this.numPlayer.Location = new System.Drawing.Point(12, 12);
            this.numPlayer.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numPlayer.Name = "numPlayer";
            this.numPlayer.Size = new System.Drawing.Size(45, 20);
            this.numPlayer.TabIndex = 0;
            this.numPlayer.ValueChanged += new System.EventHandler(this.numPlayer_ValueChanged);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(63, 12);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(62, 22);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // ChoosePlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(138, 41);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.numPlayer);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChoosePlayer";
            this.Text = "ChoosePlayer";
            ((System.ComponentModel.ISupportInitialize)(this.numPlayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numPlayer;
        private System.Windows.Forms.Button btnPlay;
    }
}