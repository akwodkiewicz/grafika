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
            this.timerTrackBar = new System.Windows.Forms.TrackBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.image3Btn = new System.Windows.Forms.Button();
            this.image2Btn = new System.Windows.Forms.Button();
            this.image1Btn = new System.Windows.Forms.Button();
            this.rotationGroupBox = new System.Windows.Forms.GroupBox();
            this.bezierGroupBox = new System.Windows.Forms.GroupBox();
            this.segmentsLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.segmentsTrackBar = new System.Windows.Forms.TrackBar();
            this.bezierMovementGroupBox = new System.Windows.Forms.GroupBox();
            this.drawTangentCheckBox = new System.Windows.Forms.CheckBox();
            this.animationGroupBox = new System.Windows.Forms.GroupBox();
            this.algoGroupBox = new System.Windows.Forms.GroupBox();
            this.monoCheckbox = new System.Windows.Forms.CheckBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.rotationGroupBox.SuspendLayout();
            this.bezierGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.segmentsTrackBar)).BeginInit();
            this.bezierMovementGroupBox.SuspendLayout();
            this.animationGroupBox.SuspendLayout();
            this.algoGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pictureBox
            // 
            this._pictureBox.BackColor = System.Drawing.Color.White;
            this._pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pictureBox.Location = new System.Drawing.Point(0, 0);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(818, 759);
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
            this._loadImageBtn.Location = new System.Drawing.Point(13, 11);
            this._loadImageBtn.Name = "_loadImageBtn";
            this._loadImageBtn.Size = new System.Drawing.Size(94, 75);
            this._loadImageBtn.TabIndex = 1;
            this._loadImageBtn.Text = "Load Image";
            this._loadImageBtn.UseVisualStyleBackColor = true;
            this._loadImageBtn.Click += new System.EventHandler(this.LoadImageBtn_Click);
            // 
            // _startMovementBtn
            // 
            this._startMovementBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._startMovementBtn.Location = new System.Drawing.Point(8, 22);
            this._startMovementBtn.Name = "_startMovementBtn";
            this._startMovementBtn.Size = new System.Drawing.Size(107, 31);
            this._startMovementBtn.TabIndex = 2;
            this._startMovementBtn.Text = "Start / Stop";
            this._startMovementBtn.UseVisualStyleBackColor = true;
            this._startMovementBtn.Click += new System.EventHandler(this.StartMovementBtn_Click);
            // 
            // rotateBtn
            // 
            this.rotateBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rotateBtn.Location = new System.Drawing.Point(8, 19);
            this.rotateBtn.Name = "rotateBtn";
            this.rotateBtn.Size = new System.Drawing.Size(148, 50);
            this.rotateBtn.TabIndex = 3;
            this.rotateBtn.Text = "Start / Stop";
            this.rotateBtn.UseVisualStyleBackColor = true;
            this.rotateBtn.Click += new System.EventHandler(this.RotateBtn_Click);
            // 
            // rotateCheckbox
            // 
            this.rotateCheckbox.AutoSize = true;
            this.rotateCheckbox.Checked = true;
            this.rotateCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rotateCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rotateCheckbox.Location = new System.Drawing.Point(8, 59);
            this.rotateCheckbox.Name = "rotateCheckbox";
            this.rotateCheckbox.Size = new System.Drawing.Size(115, 17);
            this.rotateCheckbox.TabIndex = 4;
            this.rotateCheckbox.Text = "Rotate accordingly";
            this.rotateCheckbox.UseVisualStyleBackColor = true;
            // 
            // matrixRadio
            // 
            this.matrixRadio.AutoSize = true;
            this.matrixRadio.Checked = true;
            this.matrixRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.matrixRadio.Location = new System.Drawing.Point(10, 25);
            this.matrixRadio.Name = "matrixRadio";
            this.matrixRadio.Size = new System.Drawing.Size(153, 17);
            this.matrixRadio.TabIndex = 5;
            this.matrixRadio.TabStop = true;
            this.matrixRadio.Text = "Matrix calculations (SLOW)";
            this.matrixRadio.UseVisualStyleBackColor = true;
            this.matrixRadio.CheckedChanged += new System.EventHandler(this.RotationAlgorithm_CheckedChanged);
            // 
            // shearRadio
            // 
            this.shearRadio.AutoSize = true;
            this.shearRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.shearRadio.Location = new System.Drawing.Point(10, 48);
            this.shearRadio.Name = "shearRadio";
            this.shearRadio.Size = new System.Drawing.Size(67, 17);
            this.shearRadio.TabIndex = 6;
            this.shearRadio.Text = "Shearing";
            this.shearRadio.UseVisualStyleBackColor = true;
            this.shearRadio.CheckedChanged += new System.EventHandler(this.RotationAlgorithm_CheckedChanged);
            // 
            // timerTrackBar
            // 
            this.timerTrackBar.Location = new System.Drawing.Point(10, 18);
            this.timerTrackBar.Maximum = 200;
            this.timerTrackBar.Minimum = 5;
            this.timerTrackBar.Name = "timerTrackBar";
            this.timerTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.timerTrackBar.Size = new System.Drawing.Size(148, 45);
            this.timerTrackBar.TabIndex = 7;
            this.timerTrackBar.TickFrequency = 10;
            this.timerTrackBar.Value = 80;
            this.timerTrackBar.ValueChanged += new System.EventHandler(this.TimerTrackBar_ValueChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.image3Btn);
            this.splitContainer1.Panel1.Controls.Add(this.image2Btn);
            this.splitContainer1.Panel1.Controls.Add(this.image1Btn);
            this.splitContainer1.Panel1.Controls.Add(this.rotationGroupBox);
            this.splitContainer1.Panel1.Controls.Add(this.bezierGroupBox);
            this.splitContainer1.Panel1.Controls.Add(this.animationGroupBox);
            this.splitContainer1.Panel1.Controls.Add(this.algoGroupBox);
            this.splitContainer1.Panel1.Controls.Add(this._loadImageBtn);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._pictureBox);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 761);
            this.splitContainer1.SplitterDistance = 184;
            this.splitContainer1.TabIndex = 8;
            // 
            // image3Btn
            // 
            this.image3Btn.Location = new System.Drawing.Point(113, 63);
            this.image3Btn.Name = "image3Btn";
            this.image3Btn.Size = new System.Drawing.Size(54, 23);
            this.image3Btn.TabIndex = 13;
            this.image3Btn.Text = "Image3";
            this.image3Btn.UseVisualStyleBackColor = true;
            this.image3Btn.Click += new System.EventHandler(this.Image3Btn_Click);
            // 
            // image2Btn
            // 
            this.image2Btn.Location = new System.Drawing.Point(113, 37);
            this.image2Btn.Name = "image2Btn";
            this.image2Btn.Size = new System.Drawing.Size(54, 23);
            this.image2Btn.TabIndex = 12;
            this.image2Btn.Text = "Image2";
            this.image2Btn.UseVisualStyleBackColor = true;
            this.image2Btn.Click += new System.EventHandler(this.Image2Btn_Click);
            // 
            // image1Btn
            // 
            this.image1Btn.Location = new System.Drawing.Point(113, 11);
            this.image1Btn.Name = "image1Btn";
            this.image1Btn.Size = new System.Drawing.Size(54, 23);
            this.image1Btn.TabIndex = 11;
            this.image1Btn.Text = "Image1";
            this.image1Btn.UseVisualStyleBackColor = true;
            this.image1Btn.Click += new System.EventHandler(this.Image1Btn_Click);
            // 
            // rotationGroupBox
            // 
            this.rotationGroupBox.Controls.Add(this.rotateBtn);
            this.rotationGroupBox.Enabled = false;
            this.rotationGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rotationGroupBox.Location = new System.Drawing.Point(5, 500);
            this.rotationGroupBox.Name = "rotationGroupBox";
            this.rotationGroupBox.Size = new System.Drawing.Size(164, 81);
            this.rotationGroupBox.TabIndex = 4;
            this.rotationGroupBox.TabStop = false;
            this.rotationGroupBox.Text = "Rotation";
            // 
            // bezierGroupBox
            // 
            this.bezierGroupBox.Controls.Add(this.segmentsLabel);
            this.bezierGroupBox.Controls.Add(this.label1);
            this.bezierGroupBox.Controls.Add(this.segmentsTrackBar);
            this.bezierGroupBox.Controls.Add(this.bezierMovementGroupBox);
            this.bezierGroupBox.Enabled = false;
            this.bezierGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.bezierGroupBox.Location = new System.Drawing.Point(5, 92);
            this.bezierGroupBox.Name = "bezierGroupBox";
            this.bezierGroupBox.Size = new System.Drawing.Size(164, 203);
            this.bezierGroupBox.TabIndex = 10;
            this.bezierGroupBox.TabStop = false;
            this.bezierGroupBox.Text = "Bezier";
            // 
            // segmentsLabel
            // 
            this.segmentsLabel.AutoSize = true;
            this.segmentsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.segmentsLabel.Location = new System.Drawing.Point(120, 30);
            this.segmentsLabel.Name = "segmentsLabel";
            this.segmentsLabel.Size = new System.Drawing.Size(0, 13);
            this.segmentsLabel.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(9, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Number of segments";
            // 
            // segmentsTrackBar
            // 
            this.segmentsTrackBar.Location = new System.Drawing.Point(4, 46);
            this.segmentsTrackBar.Maximum = 350;
            this.segmentsTrackBar.Minimum = 3;
            this.segmentsTrackBar.Name = "segmentsTrackBar";
            this.segmentsTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.segmentsTrackBar.Size = new System.Drawing.Size(152, 45);
            this.segmentsTrackBar.TabIndex = 8;
            this.segmentsTrackBar.TickFrequency = 25;
            this.segmentsTrackBar.Value = 200;
            this.segmentsTrackBar.ValueChanged += new System.EventHandler(this.SegmentsTrackBar_ValueChanged);
            // 
            // bezierMovementGroupBox
            // 
            this.bezierMovementGroupBox.Controls.Add(this.rotateCheckbox);
            this.bezierMovementGroupBox.Controls.Add(this._startMovementBtn);
            this.bezierMovementGroupBox.Controls.Add(this.drawTangentCheckBox);
            this.bezierMovementGroupBox.Enabled = false;
            this.bezierMovementGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.bezierMovementGroupBox.Location = new System.Drawing.Point(4, 97);
            this.bezierMovementGroupBox.Name = "bezierMovementGroupBox";
            this.bezierMovementGroupBox.Size = new System.Drawing.Size(152, 100);
            this.bezierMovementGroupBox.TabIndex = 1;
            this.bezierMovementGroupBox.TabStop = false;
            this.bezierMovementGroupBox.Text = "Movement";
            // 
            // drawTangentCheckBox
            // 
            this.drawTangentCheckBox.AutoSize = true;
            this.drawTangentCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.drawTangentCheckBox.Location = new System.Drawing.Point(8, 77);
            this.drawTangentCheckBox.Name = "drawTangentCheckBox";
            this.drawTangentCheckBox.Size = new System.Drawing.Size(90, 17);
            this.drawTangentCheckBox.TabIndex = 5;
            this.drawTangentCheckBox.Text = "Draw tangent";
            this.drawTangentCheckBox.UseVisualStyleBackColor = true;
            // 
            // animationGroupBox
            // 
            this.animationGroupBox.Controls.Add(this.timerTrackBar);
            this.animationGroupBox.Enabled = false;
            this.animationGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.animationGroupBox.Location = new System.Drawing.Point(5, 425);
            this.animationGroupBox.Name = "animationGroupBox";
            this.animationGroupBox.Size = new System.Drawing.Size(164, 69);
            this.animationGroupBox.TabIndex = 9;
            this.animationGroupBox.TabStop = false;
            this.animationGroupBox.Text = "Animation speed";
            // 
            // algoGroupBox
            // 
            this.algoGroupBox.Controls.Add(this.monoCheckbox);
            this.algoGroupBox.Controls.Add(this.radioButton1);
            this.algoGroupBox.Controls.Add(this.shearRadio);
            this.algoGroupBox.Controls.Add(this.matrixRadio);
            this.algoGroupBox.Enabled = false;
            this.algoGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.algoGroupBox.Location = new System.Drawing.Point(5, 301);
            this.algoGroupBox.Name = "algoGroupBox";
            this.algoGroupBox.Size = new System.Drawing.Size(164, 118);
            this.algoGroupBox.TabIndex = 8;
            this.algoGroupBox.TabStop = false;
            this.algoGroupBox.Text = "Rotation algorithm";
            // 
            // monoCheckbox
            // 
            this.monoCheckbox.AutoSize = true;
            this.monoCheckbox.Enabled = false;
            this.monoCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.monoCheckbox.Location = new System.Drawing.Point(30, 93);
            this.monoCheckbox.Name = "monoCheckbox";
            this.monoCheckbox.Size = new System.Drawing.Size(53, 17);
            this.monoCheckbox.TabIndex = 10;
            this.monoCheckbox.Text = "Mono";
            this.monoCheckbox.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.radioButton1.Location = new System.Drawing.Point(10, 71);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(144, 17);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.Text = "Shearing with antialiasing";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 761);
            this.Controls.Add(this.splitContainer1);
            this.MaximumSize = new System.Drawing.Size(1024, 800);
            this.MinimumSize = new System.Drawing.Size(800, 639);
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "Bezier Curves";
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerTrackBar)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.rotationGroupBox.ResumeLayout(false);
            this.bezierGroupBox.ResumeLayout(false);
            this.bezierGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.segmentsTrackBar)).EndInit();
            this.bezierMovementGroupBox.ResumeLayout(false);
            this.bezierMovementGroupBox.PerformLayout();
            this.animationGroupBox.ResumeLayout(false);
            this.animationGroupBox.PerformLayout();
            this.algoGroupBox.ResumeLayout(false);
            this.algoGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _pictureBox;
        private System.Windows.Forms.Button _loadImageBtn;
        private System.Windows.Forms.Button _startMovementBtn;
        private System.Windows.Forms.Button rotateBtn;
        private System.Windows.Forms.CheckBox rotateCheckbox;
        private System.Windows.Forms.RadioButton matrixRadio;
        private System.Windows.Forms.RadioButton shearRadio;
        private System.Windows.Forms.TrackBar timerTrackBar;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox animationGroupBox;
        private System.Windows.Forms.GroupBox algoGroupBox;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.CheckBox monoCheckbox;
        private System.Windows.Forms.GroupBox rotationGroupBox;
        private System.Windows.Forms.GroupBox bezierGroupBox;
        private System.Windows.Forms.CheckBox drawTangentCheckBox;
        private System.Windows.Forms.TrackBar segmentsTrackBar;
        private System.Windows.Forms.GroupBox bezierMovementGroupBox;
        private System.Windows.Forms.Label segmentsLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button image3Btn;
        private System.Windows.Forms.Button image2Btn;
        private System.Windows.Forms.Button image1Btn;
    }
}

