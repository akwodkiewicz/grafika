﻿namespace PolygonApp
{
    partial class PolygonFillApp
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Wymagana metoda obsługi projektanta — nie należy modyfikować 
        /// zawartość tej metody z edytorem kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fillGroupBox = new System.Windows.Forms.GroupBox();
            this.fillSolidButton = new System.Windows.Forms.Button();
            this.fillTextureButton = new System.Windows.Forms.Button();
            this.fillSolidPic = new System.Windows.Forms.PictureBox();
            this.fillTextureRadio = new System.Windows.Forms.RadioButton();
            this.fillSolidRadio = new System.Windows.Forms.RadioButton();
            this.fillTexturePic = new System.Windows.Forms.PictureBox();
            this.lightGroupBox = new System.Windows.Forms.GroupBox();
            this.lightPosGroupBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.animatedSphereRadiusNumeric = new System.Windows.Forms.NumericUpDown();
            this.lightPosPointRadio = new System.Windows.Forms.RadioButton();
            this.lightPosLabel = new System.Windows.Forms.Label();
            this.lightPosInfinity = new System.Windows.Forms.RadioButton();
            this.lightPosAuto = new System.Windows.Forms.RadioButton();
            this.lightColorLabel = new System.Windows.Forms.Label();
            this.lightColorButton = new System.Windows.Forms.Button();
            this.lightColorPic = new System.Windows.Forms.PictureBox();
            this.normalMapGroupBox = new System.Windows.Forms.GroupBox();
            this.normalMapButton = new System.Windows.Forms.Button();
            this.normalMapNoneRadio = new System.Windows.Forms.RadioButton();
            this.normalMapImageRadio = new System.Windows.Forms.RadioButton();
            this.normalMapPic = new System.Windows.Forms.PictureBox();
            this.heightMapGroupBox = new System.Windows.Forms.GroupBox();
            this.heightMapButton = new System.Windows.Forms.Button();
            this.heightMapNoneRadio = new System.Windows.Forms.RadioButton();
            this.heightMapImageRadio = new System.Windows.Forms.RadioButton();
            this.heightMapPic = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addVertexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lightAnimationTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.fillGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fillSolidPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fillTexturePic)).BeginInit();
            this.lightGroupBox.SuspendLayout();
            this.lightPosGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animatedSphereRadiusNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lightColorPic)).BeginInit();
            this.normalMapGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalMapPic)).BeginInit();
            this.heightMapGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.heightMapPic)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fillGroupBox);
            this.splitContainer1.Panel1.Controls.Add(this.lightGroupBox);
            this.splitContainer1.Panel1.Controls.Add(this.normalMapGroupBox);
            this.splitContainer1.Panel1.Controls.Add(this.heightMapGroupBox);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Size = new System.Drawing.Size(1026, 602);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.TabIndex = 0;
            // 
            // fillGroupBox
            // 
            this.fillGroupBox.Controls.Add(this.fillSolidButton);
            this.fillGroupBox.Controls.Add(this.fillTextureButton);
            this.fillGroupBox.Controls.Add(this.fillSolidPic);
            this.fillGroupBox.Controls.Add(this.fillTextureRadio);
            this.fillGroupBox.Controls.Add(this.fillSolidRadio);
            this.fillGroupBox.Controls.Add(this.fillTexturePic);
            this.fillGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fillGroupBox.Location = new System.Drawing.Point(5, 9);
            this.fillGroupBox.Name = "fillGroupBox";
            this.fillGroupBox.Size = new System.Drawing.Size(207, 105);
            this.fillGroupBox.TabIndex = 22;
            this.fillGroupBox.TabStop = false;
            this.fillGroupBox.Text = "Fill";
            // 
            // fillSolidButton
            // 
            this.fillSolidButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fillSolidButton.Location = new System.Drawing.Point(148, 21);
            this.fillSolidButton.Margin = new System.Windows.Forms.Padding(0);
            this.fillSolidButton.Name = "fillSolidButton";
            this.fillSolidButton.Size = new System.Drawing.Size(53, 30);
            this.fillSolidButton.TabIndex = 24;
            this.fillSolidButton.Text = "Pick";
            this.fillSolidButton.UseVisualStyleBackColor = true;
            this.fillSolidButton.Click += new System.EventHandler(this.FillSolidButton_Click);
            // 
            // fillTextureButton
            // 
            this.fillTextureButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fillTextureButton.Location = new System.Drawing.Point(148, 62);
            this.fillTextureButton.Margin = new System.Windows.Forms.Padding(0);
            this.fillTextureButton.Name = "fillTextureButton";
            this.fillTextureButton.Size = new System.Drawing.Size(53, 30);
            this.fillTextureButton.TabIndex = 23;
            this.fillTextureButton.Text = "Open";
            this.fillTextureButton.UseVisualStyleBackColor = true;
            this.fillTextureButton.Click += new System.EventHandler(this.FillTextureButton_Click);
            // 
            // fillSolidPic
            // 
            this.fillSolidPic.BackColor = System.Drawing.SystemColors.Window;
            this.fillSolidPic.Location = new System.Drawing.Point(97, 19);
            this.fillSolidPic.Name = "fillSolidPic";
            this.fillSolidPic.Size = new System.Drawing.Size(44, 35);
            this.fillSolidPic.TabIndex = 22;
            this.fillSolidPic.TabStop = false;
            // 
            // fillTextureRadio
            // 
            this.fillTextureRadio.AutoSize = true;
            this.fillTextureRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fillTextureRadio.Location = new System.Drawing.Point(12, 69);
            this.fillTextureRadio.Name = "fillTextureRadio";
            this.fillTextureRadio.Size = new System.Drawing.Size(61, 17);
            this.fillTextureRadio.TabIndex = 20;
            this.fillTextureRadio.Text = "Texture";
            this.fillTextureRadio.UseVisualStyleBackColor = true;
            this.fillTextureRadio.CheckedChanged += new System.EventHandler(this.FillTextureRadio_CheckedChanged);
            // 
            // fillSolidRadio
            // 
            this.fillSolidRadio.AutoSize = true;
            this.fillSolidRadio.Checked = true;
            this.fillSolidRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fillSolidRadio.Location = new System.Drawing.Point(12, 28);
            this.fillSolidRadio.Name = "fillSolidRadio";
            this.fillSolidRadio.Size = new System.Drawing.Size(75, 17);
            this.fillSolidRadio.TabIndex = 19;
            this.fillSolidRadio.TabStop = true;
            this.fillSolidRadio.Text = "Solid Color";
            this.fillSolidRadio.UseVisualStyleBackColor = true;
            this.fillSolidRadio.CheckedChanged += new System.EventHandler(this.FillSolidRadio_CheckedChanged);
            // 
            // fillTexturePic
            // 
            this.fillTexturePic.BackColor = System.Drawing.SystemColors.Window;
            this.fillTexturePic.Location = new System.Drawing.Point(97, 60);
            this.fillTexturePic.Name = "fillTexturePic";
            this.fillTexturePic.Size = new System.Drawing.Size(44, 35);
            this.fillTexturePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.fillTexturePic.TabIndex = 21;
            this.fillTexturePic.TabStop = false;
            // 
            // lightGroupBox
            // 
            this.lightGroupBox.Controls.Add(this.lightPosGroupBox);
            this.lightGroupBox.Controls.Add(this.lightColorLabel);
            this.lightGroupBox.Controls.Add(this.lightColorButton);
            this.lightGroupBox.Controls.Add(this.lightColorPic);
            this.lightGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lightGroupBox.Location = new System.Drawing.Point(5, 119);
            this.lightGroupBox.Name = "lightGroupBox";
            this.lightGroupBox.Size = new System.Drawing.Size(207, 181);
            this.lightGroupBox.TabIndex = 25;
            this.lightGroupBox.TabStop = false;
            this.lightGroupBox.Text = "Light";
            // 
            // lightPosGroupBox
            // 
            this.lightPosGroupBox.Controls.Add(this.label5);
            this.lightPosGroupBox.Controls.Add(this.animatedSphereRadiusNumeric);
            this.lightPosGroupBox.Controls.Add(this.lightPosPointRadio);
            this.lightPosGroupBox.Controls.Add(this.lightPosLabel);
            this.lightPosGroupBox.Controls.Add(this.lightPosInfinity);
            this.lightPosGroupBox.Controls.Add(this.lightPosAuto);
            this.lightPosGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lightPosGroupBox.Location = new System.Drawing.Point(5, 55);
            this.lightPosGroupBox.Name = "lightPosGroupBox";
            this.lightPosGroupBox.Size = new System.Drawing.Size(196, 120);
            this.lightPosGroupBox.TabIndex = 30;
            this.lightPosGroupBox.TabStop = false;
            this.lightPosGroupBox.Text = "Position";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.Location = new System.Drawing.Point(33, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "Sphere Radius:";
            // 
            // animatedSphereRadiusNumeric
            // 
            this.animatedSphereRadiusNumeric.Location = new System.Drawing.Point(128, 85);
            this.animatedSphereRadiusNumeric.Name = "animatedSphereRadiusNumeric";
            this.animatedSphereRadiusNumeric.Size = new System.Drawing.Size(46, 22);
            this.animatedSphereRadiusNumeric.TabIndex = 31;
            // 
            // lightPosPointRadio
            // 
            this.lightPosPointRadio.AutoSize = true;
            this.lightPosPointRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lightPosPointRadio.Location = new System.Drawing.Point(18, 46);
            this.lightPosPointRadio.Name = "lightPosPointRadio";
            this.lightPosPointRadio.Size = new System.Drawing.Size(49, 17);
            this.lightPosPointRadio.TabIndex = 30;
            this.lightPosPointRadio.Text = "Point";
            this.lightPosPointRadio.UseVisualStyleBackColor = true;
            this.lightPosPointRadio.CheckedChanged += new System.EventHandler(this.LightPosPointRadio_CheckedChanged);
            // 
            // lightPosLabel
            // 
            this.lightPosLabel.AutoSize = true;
            this.lightPosLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lightPosLabel.Location = new System.Drawing.Point(93, 48);
            this.lightPosLabel.Name = "lightPosLabel";
            this.lightPosLabel.Size = new System.Drawing.Size(40, 13);
            this.lightPosLabel.TabIndex = 26;
            this.lightPosLabel.Text = "(x, y, z)";
            // 
            // lightPosInfinity
            // 
            this.lightPosInfinity.AutoSize = true;
            this.lightPosInfinity.Checked = true;
            this.lightPosInfinity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lightPosInfinity.Location = new System.Drawing.Point(18, 23);
            this.lightPosInfinity.Name = "lightPosInfinity";
            this.lightPosInfinity.Size = new System.Drawing.Size(140, 17);
            this.lightPosInfinity.TabIndex = 27;
            this.lightPosInfinity.TabStop = true;
            this.lightPosInfinity.Text = "Infinity (Directional Light)";
            this.lightPosInfinity.UseVisualStyleBackColor = true;
            this.lightPosInfinity.CheckedChanged += new System.EventHandler(this.LightPosInfinity_CheckedChanged);
            // 
            // lightPosAuto
            // 
            this.lightPosAuto.AutoSize = true;
            this.lightPosAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lightPosAuto.Location = new System.Drawing.Point(18, 69);
            this.lightPosAuto.Name = "lightPosAuto";
            this.lightPosAuto.Size = new System.Drawing.Size(69, 17);
            this.lightPosAuto.TabIndex = 29;
            this.lightPosAuto.Text = "Animated";
            this.lightPosAuto.UseVisualStyleBackColor = true;
            this.lightPosAuto.CheckedChanged += new System.EventHandler(this.LightPosAuto_CheckedChanged);
            // 
            // lightColorLabel
            // 
            this.lightColorLabel.AutoSize = true;
            this.lightColorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lightColorLabel.Location = new System.Drawing.Point(9, 28);
            this.lightColorLabel.Name = "lightColorLabel";
            this.lightColorLabel.Size = new System.Drawing.Size(40, 16);
            this.lightColorLabel.TabIndex = 25;
            this.lightColorLabel.Text = "Color";
            // 
            // lightColorButton
            // 
            this.lightColorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lightColorButton.Location = new System.Drawing.Point(148, 21);
            this.lightColorButton.Margin = new System.Windows.Forms.Padding(0);
            this.lightColorButton.Name = "lightColorButton";
            this.lightColorButton.Size = new System.Drawing.Size(53, 30);
            this.lightColorButton.TabIndex = 24;
            this.lightColorButton.Text = "Pick";
            this.lightColorButton.UseVisualStyleBackColor = true;
            this.lightColorButton.Click += new System.EventHandler(this.LightColorButton_Click);
            // 
            // lightColorPic
            // 
            this.lightColorPic.BackColor = System.Drawing.SystemColors.Window;
            this.lightColorPic.Location = new System.Drawing.Point(97, 18);
            this.lightColorPic.Name = "lightColorPic";
            this.lightColorPic.Size = new System.Drawing.Size(44, 35);
            this.lightColorPic.TabIndex = 22;
            this.lightColorPic.TabStop = false;
            // 
            // normalMapGroupBox
            // 
            this.normalMapGroupBox.Controls.Add(this.normalMapButton);
            this.normalMapGroupBox.Controls.Add(this.normalMapNoneRadio);
            this.normalMapGroupBox.Controls.Add(this.normalMapImageRadio);
            this.normalMapGroupBox.Controls.Add(this.normalMapPic);
            this.normalMapGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.normalMapGroupBox.Location = new System.Drawing.Point(5, 306);
            this.normalMapGroupBox.Name = "normalMapGroupBox";
            this.normalMapGroupBox.Size = new System.Drawing.Size(207, 98);
            this.normalMapGroupBox.TabIndex = 3;
            this.normalMapGroupBox.TabStop = false;
            this.normalMapGroupBox.Text = "NormalMap";
            // 
            // normalMapButton
            // 
            this.normalMapButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.normalMapButton.Location = new System.Drawing.Point(148, 51);
            this.normalMapButton.Margin = new System.Windows.Forms.Padding(0);
            this.normalMapButton.Name = "normalMapButton";
            this.normalMapButton.Size = new System.Drawing.Size(53, 30);
            this.normalMapButton.TabIndex = 26;
            this.normalMapButton.Text = "Open";
            this.normalMapButton.UseVisualStyleBackColor = true;
            this.normalMapButton.Click += new System.EventHandler(this.NormalMapButton_Click);
            // 
            // normalMapNoneRadio
            // 
            this.normalMapNoneRadio.AutoSize = true;
            this.normalMapNoneRadio.Checked = true;
            this.normalMapNoneRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.normalMapNoneRadio.Location = new System.Drawing.Point(12, 25);
            this.normalMapNoneRadio.Name = "normalMapNoneRadio";
            this.normalMapNoneRadio.Size = new System.Drawing.Size(51, 17);
            this.normalMapNoneRadio.TabIndex = 19;
            this.normalMapNoneRadio.TabStop = true;
            this.normalMapNoneRadio.Text = "None";
            this.normalMapNoneRadio.UseVisualStyleBackColor = true;
            // 
            // normalMapImageRadio
            // 
            this.normalMapImageRadio.AutoSize = true;
            this.normalMapImageRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.normalMapImageRadio.Location = new System.Drawing.Point(12, 58);
            this.normalMapImageRadio.Name = "normalMapImageRadio";
            this.normalMapImageRadio.Size = new System.Drawing.Size(54, 17);
            this.normalMapImageRadio.TabIndex = 20;
            this.normalMapImageRadio.Text = "Image";
            this.normalMapImageRadio.UseVisualStyleBackColor = true;
            this.normalMapImageRadio.CheckedChanged += new System.EventHandler(this.NormalMapImageRadio_CheckedChanged);
            // 
            // normalMapPic
            // 
            this.normalMapPic.BackColor = System.Drawing.SystemColors.Window;
            this.normalMapPic.Location = new System.Drawing.Point(97, 49);
            this.normalMapPic.Name = "normalMapPic";
            this.normalMapPic.Size = new System.Drawing.Size(44, 35);
            this.normalMapPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.normalMapPic.TabIndex = 25;
            this.normalMapPic.TabStop = false;
            // 
            // heightMapGroupBox
            // 
            this.heightMapGroupBox.Controls.Add(this.heightMapButton);
            this.heightMapGroupBox.Controls.Add(this.heightMapNoneRadio);
            this.heightMapGroupBox.Controls.Add(this.heightMapImageRadio);
            this.heightMapGroupBox.Controls.Add(this.heightMapPic);
            this.heightMapGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.heightMapGroupBox.Location = new System.Drawing.Point(5, 410);
            this.heightMapGroupBox.Name = "heightMapGroupBox";
            this.heightMapGroupBox.Size = new System.Drawing.Size(207, 98);
            this.heightMapGroupBox.TabIndex = 27;
            this.heightMapGroupBox.TabStop = false;
            this.heightMapGroupBox.Text = "HeightMap";
            // 
            // heightMapButton
            // 
            this.heightMapButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.heightMapButton.Location = new System.Drawing.Point(148, 51);
            this.heightMapButton.Margin = new System.Windows.Forms.Padding(0);
            this.heightMapButton.Name = "heightMapButton";
            this.heightMapButton.Size = new System.Drawing.Size(53, 30);
            this.heightMapButton.TabIndex = 26;
            this.heightMapButton.Text = "Open";
            this.heightMapButton.UseVisualStyleBackColor = true;
            this.heightMapButton.Click += new System.EventHandler(this.HeightMapButton_Click);
            // 
            // heightMapNoneRadio
            // 
            this.heightMapNoneRadio.AutoSize = true;
            this.heightMapNoneRadio.Checked = true;
            this.heightMapNoneRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.heightMapNoneRadio.Location = new System.Drawing.Point(12, 25);
            this.heightMapNoneRadio.Name = "heightMapNoneRadio";
            this.heightMapNoneRadio.Size = new System.Drawing.Size(51, 17);
            this.heightMapNoneRadio.TabIndex = 19;
            this.heightMapNoneRadio.TabStop = true;
            this.heightMapNoneRadio.Text = "None";
            this.heightMapNoneRadio.UseVisualStyleBackColor = true;
            // 
            // heightMapImageRadio
            // 
            this.heightMapImageRadio.AutoSize = true;
            this.heightMapImageRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.heightMapImageRadio.Location = new System.Drawing.Point(12, 58);
            this.heightMapImageRadio.Name = "heightMapImageRadio";
            this.heightMapImageRadio.Size = new System.Drawing.Size(54, 17);
            this.heightMapImageRadio.TabIndex = 20;
            this.heightMapImageRadio.Text = "Image";
            this.heightMapImageRadio.UseVisualStyleBackColor = true;
            this.heightMapImageRadio.CheckedChanged += new System.EventHandler(this.HeightMapImageRadio_CheckedChanged);
            // 
            // heightMapPic
            // 
            this.heightMapPic.BackColor = System.Drawing.SystemColors.Window;
            this.heightMapPic.Location = new System.Drawing.Point(97, 49);
            this.heightMapPic.Name = "heightMapPic";
            this.heightMapPic.Size = new System.Drawing.Size(44, 35);
            this.heightMapPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.heightMapPic.TabIndex = 25;
            this.heightMapPic.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(5, 595);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(181, 71);
            this.panel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Vertex Size";
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(6, 23);
            this.trackBar1.Maximum = 31;
            this.trackBar1.Minimum = 3;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(173, 45);
            this.trackBar1.SmallChange = 2;
            this.trackBar1.TabIndex = 2;
            this.trackBar1.Value = 16;
            this.trackBar1.ValueChanged += new System.EventHandler(this.TrackBar1_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(47, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 54);
            this.label2.TabIndex = 6;
            this.label2.Text = "Create Mode:\r\n[LMB]      Add Vertex\r\n[Return]  Close Polygon";
            this.label2.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Window;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.ForeColor = System.Drawing.Color.Firebrick;
            this.label4.Location = new System.Drawing.Point(3, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(383, 42);
            this.label4.TabIndex = 2;
            this.label4.Text = "[SELECT ALL MODE]";
            this.label4.Visible = false;
            // 
            // pictureBox
            // 
            this.pictureBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(800, 600);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox_Paint);
            this.pictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseClick);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(147, 430);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 90);
            this.label3.TabIndex = 4;
            this.label3.Text = "Edit Mode:\r\n[RMB]   Context Menu\r\n[A]        Move Polygon\r\n[C]        Create Mode" +
    "\r\n[R]        Reset Canvas";
            this.label3.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addVertexToolStripMenuItem,
            this.fillToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(132, 48);
            // 
            // addVertexToolStripMenuItem
            // 
            this.addVertexToolStripMenuItem.Name = "addVertexToolStripMenuItem";
            this.addVertexToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.addVertexToolStripMenuItem.Text = "Add Vertex";
            this.addVertexToolStripMenuItem.Click += new System.EventHandler(this.AddVertexToolStripMenuItem_Click);
            // 
            // fillToolStripMenuItem
            // 
            this.fillToolStripMenuItem.Name = "fillToolStripMenuItem";
            this.fillToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.fillToolStripMenuItem.Text = "Fill";
            this.fillToolStripMenuItem.Click += new System.EventHandler(this.FillToolStripMenuItem_Click);
            // 
            // lightAnimationTimer
            // 
            this.lightAnimationTimer.Tick += new System.EventHandler(this.LightAnimationTimer_Tick);
            // 
            // PolygonFillApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 602);
            this.Controls.Add(this.splitContainer1);
            this.KeyPreview = true;
            this.Name = "PolygonFillApp";
            this.Text = "Polygon Editor [Create Mode]";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PolygonApp_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PolygonApp_KeyPress);
            this.Resize += new System.EventHandler(this.PolygonFillApp_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.fillGroupBox.ResumeLayout(false);
            this.fillGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fillSolidPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fillTexturePic)).EndInit();
            this.lightGroupBox.ResumeLayout(false);
            this.lightGroupBox.PerformLayout();
            this.lightPosGroupBox.ResumeLayout(false);
            this.lightPosGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animatedSphereRadiusNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lightColorPic)).EndInit();
            this.normalMapGroupBox.ResumeLayout(false);
            this.normalMapGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalMapPic)).EndInit();
            this.heightMapGroupBox.ResumeLayout(false);
            this.heightMapGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.heightMapPic)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem addVertexToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem fillToolStripMenuItem;
        private System.Windows.Forms.RadioButton normalMapNoneRadio;
        private System.Windows.Forms.RadioButton normalMapImageRadio;
        private System.Windows.Forms.GroupBox fillGroupBox;
        private System.Windows.Forms.Button fillSolidButton;
        private System.Windows.Forms.Button fillTextureButton;
        private System.Windows.Forms.PictureBox fillSolidPic;
        private System.Windows.Forms.RadioButton fillTextureRadio;
        private System.Windows.Forms.RadioButton fillSolidRadio;
        private System.Windows.Forms.PictureBox fillTexturePic;
        private System.Windows.Forms.GroupBox lightGroupBox;
        private System.Windows.Forms.RadioButton lightPosAuto;
        private System.Windows.Forms.RadioButton lightPosInfinity;
        private System.Windows.Forms.Label lightColorLabel;
        private System.Windows.Forms.Button lightColorButton;
        private System.Windows.Forms.PictureBox lightColorPic;
        private System.Windows.Forms.GroupBox lightPosGroupBox;
        private System.Windows.Forms.RadioButton lightPosPointRadio;
        private System.Windows.Forms.Label lightPosLabel;
        private System.Windows.Forms.GroupBox normalMapGroupBox;
        private System.Windows.Forms.Button normalMapButton;
        private System.Windows.Forms.PictureBox normalMapPic;
        private System.Windows.Forms.Timer lightAnimationTimer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown animatedSphereRadiusNumeric;
        private System.Windows.Forms.GroupBox heightMapGroupBox;
        private System.Windows.Forms.Button heightMapButton;
        private System.Windows.Forms.RadioButton heightMapNoneRadio;
        private System.Windows.Forms.RadioButton heightMapImageRadio;
        private System.Windows.Forms.PictureBox heightMapPic;
    }
}

