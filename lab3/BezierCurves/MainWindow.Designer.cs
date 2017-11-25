namespace BezierCurves
{
    partial class MainWindow
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
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this._loadImageBtn = new System.Windows.Forms.Button();
            this._startMovementBtn = new System.Windows.Forms.Button();
            this.rotateBtn = new System.Windows.Forms.Button();
            this.rotateCheckbox = new System.Windows.Forms.CheckBox();
            this.matrixRadio = new System.Windows.Forms.RadioButton();
            this.shearRadio = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _pictureBox
            // 
            this._pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pictureBox.Location = new System.Drawing.Point(0, 0);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(800, 600);
            this._pictureBox.TabIndex = 0;
            this._pictureBox.TabStop = false;
            this._pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox_Paint);
            this._pictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseClick);
            this._pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseDown);
            this._pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseMove);
            this._pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseUp);
            // 
            // _loadImageBtn
            // 
            this._loadImageBtn.Location = new System.Drawing.Point(0, 0);
            this._loadImageBtn.Name = "_loadImageBtn";
            this._loadImageBtn.Size = new System.Drawing.Size(75, 23);
            this._loadImageBtn.TabIndex = 1;
            this._loadImageBtn.Text = "Load Image";
            this._loadImageBtn.UseVisualStyleBackColor = true;
            this._loadImageBtn.Click += new System.EventHandler(this.LoadImageBtn_Click);
            // 
            // _startMovementBtn
            // 
            this._startMovementBtn.Location = new System.Drawing.Point(0, 29);
            this._startMovementBtn.Name = "_startMovementBtn";
            this._startMovementBtn.Size = new System.Drawing.Size(75, 23);
            this._startMovementBtn.TabIndex = 2;
            this._startMovementBtn.Text = "Go";
            this._startMovementBtn.UseVisualStyleBackColor = true;
            this._startMovementBtn.Click += new System.EventHandler(this.StartMovementBtn_Click);
            // 
            // rotateBtn
            // 
            this.rotateBtn.Location = new System.Drawing.Point(0, 58);
            this.rotateBtn.Name = "rotateBtn";
            this.rotateBtn.Size = new System.Drawing.Size(75, 23);
            this.rotateBtn.TabIndex = 3;
            this.rotateBtn.Text = "Rotate";
            this.rotateBtn.UseVisualStyleBackColor = true;
            this.rotateBtn.Click += new System.EventHandler(this.RotateBtn_Click);
            // 
            // rotateCheckbox
            // 
            this.rotateCheckbox.AutoSize = true;
            this.rotateCheckbox.Location = new System.Drawing.Point(13, 99);
            this.rotateCheckbox.Name = "rotateCheckbox";
            this.rotateCheckbox.Size = new System.Drawing.Size(111, 17);
            this.rotateCheckbox.TabIndex = 4;
            this.rotateCheckbox.Text = "Rotate along path";
            this.rotateCheckbox.UseVisualStyleBackColor = true;
            // 
            // matrixRadio
            // 
            this.matrixRadio.AutoSize = true;
            this.matrixRadio.Checked = true;
            this.matrixRadio.Location = new System.Drawing.Point(22, 173);
            this.matrixRadio.Name = "matrixRadio";
            this.matrixRadio.Size = new System.Drawing.Size(174, 17);
            this.matrixRadio.TabIndex = 5;
            this.matrixRadio.TabStop = true;
            this.matrixRadio.Text = "Rotate using matrix calculations";
            this.matrixRadio.UseVisualStyleBackColor = true;
            // 
            // shearRadio
            // 
            this.shearRadio.AutoSize = true;
            this.shearRadio.Location = new System.Drawing.Point(22, 196);
            this.shearRadio.Name = "shearRadio";
            this.shearRadio.Size = new System.Drawing.Size(128, 17);
            this.shearRadio.TabIndex = 6;
            this.shearRadio.Text = "Rotate using shearing";
            this.shearRadio.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.shearRadio);
            this.Controls.Add(this.matrixRadio);
            this.Controls.Add(this.rotateCheckbox);
            this.Controls.Add(this.rotateBtn);
            this.Controls.Add(this._startMovementBtn);
            this.Controls.Add(this._loadImageBtn);
            this.Controls.Add(this._pictureBox);
            this.MaximumSize = new System.Drawing.Size(816, 639);
            this.MinimumSize = new System.Drawing.Size(816, 639);
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "Bezier Curves";
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _pictureBox;
        private System.Windows.Forms.Button _loadImageBtn;
        private System.Windows.Forms.Button _startMovementBtn;
        private System.Windows.Forms.Button rotateBtn;
        private System.Windows.Forms.CheckBox rotateCheckbox;
        private System.Windows.Forms.RadioButton matrixRadio;
        private System.Windows.Forms.RadioButton shearRadio;
    }
}

