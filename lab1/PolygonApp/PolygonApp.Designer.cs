namespace PolygonApp
{
    partial class PolygonApp
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.panelRadio = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.radioPolygon = new System.Windows.Forms.RadioButton();
            this.radioVertices = new System.Windows.Forms.RadioButton();
            this.buttonReset = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.angleConstraintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearConstraintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addVertexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lockHorizontallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lockVerticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panelRadio.SuspendLayout();
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
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.panelRadio);
            this.splitContainer1.Panel1.Controls.Add(this.buttonReset);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox);
            this.splitContainer1.Size = new System.Drawing.Size(1022, 679);
            this.splitContainer1.SplitterDistance = 111;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Location = new System.Drawing.Point(24, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(182, 71);
            this.panel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Vertex size";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(542, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(423, 54);
            this.label3.TabIndex = 4;
            this.label3.Text = "Press [Return] to close the polygon\r\nPress [RMB] on a line to see available actio" +
    "ns\r\nPress [RMB] on a vertex to delete it or add a constraint\r\n";
            // 
            // panelRadio
            // 
            this.panelRadio.Controls.Add(this.label2);
            this.panelRadio.Controls.Add(this.radioPolygon);
            this.panelRadio.Controls.Add(this.radioVertices);
            this.panelRadio.Location = new System.Drawing.Point(238, 23);
            this.panelRadio.Name = "panelRadio";
            this.panelRadio.Size = new System.Drawing.Size(104, 71);
            this.panelRadio.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Move";
            // 
            // radioPolygon
            // 
            this.radioPolygon.AutoSize = true;
            this.radioPolygon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioPolygon.Location = new System.Drawing.Point(8, 41);
            this.radioPolygon.Name = "radioPolygon";
            this.radioPolygon.Size = new System.Drawing.Size(75, 20);
            this.radioPolygon.TabIndex = 1;
            this.radioPolygon.Text = "polygon";
            this.radioPolygon.UseVisualStyleBackColor = true;
            // 
            // radioVertices
            // 
            this.radioVertices.AutoSize = true;
            this.radioVertices.Checked = true;
            this.radioVertices.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioVertices.Location = new System.Drawing.Point(8, 23);
            this.radioVertices.Name = "radioVertices";
            this.radioVertices.Size = new System.Drawing.Size(73, 20);
            this.radioVertices.TabIndex = 0;
            this.radioVertices.TabStop = true;
            this.radioVertices.Text = "vertices";
            this.radioVertices.UseVisualStyleBackColor = true;
            // 
            // buttonReset
            // 
            this.buttonReset.CausesValidation = false;
            this.buttonReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonReset.Location = new System.Drawing.Point(385, 30);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(122, 54);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.TabStop = false;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(1020, 562);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox_Paint);
            this.pictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseClick);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.angleConstraintToolStripMenuItem,
            this.clearConstraintToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(194, 70);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // angleConstraintToolStripMenuItem
            // 
            this.angleConstraintToolStripMenuItem.Name = "angleConstraintToolStripMenuItem";
            this.angleConstraintToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.angleConstraintToolStripMenuItem.Text = "Angle Constraint";
            this.angleConstraintToolStripMenuItem.Click += new System.EventHandler(this.AngleConstraintToolStripMenuItem_Click);
            // 
            // clearConstraintToolStripMenuItem
            // 
            this.clearConstraintToolStripMenuItem.Name = "clearConstraintToolStripMenuItem";
            this.clearConstraintToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.clearConstraintToolStripMenuItem.Text = "Clear Angle Constraint";
            this.clearConstraintToolStripMenuItem.Click += new System.EventHandler(this.ClearConstraintToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addVertexToolStripMenuItem,
            this.lockHorizontallyToolStripMenuItem,
            this.lockVerticallyToolStripMenuItem,
            this.clearLockToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(167, 92);
            // 
            // addVertexToolStripMenuItem
            // 
            this.addVertexToolStripMenuItem.Name = "addVertexToolStripMenuItem";
            this.addVertexToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.addVertexToolStripMenuItem.Text = "Add Vertex";
            this.addVertexToolStripMenuItem.Click += new System.EventHandler(this.AddVertexToolStripMenuItem_Click);
            // 
            // lockHorizontallyToolStripMenuItem
            // 
            this.lockHorizontallyToolStripMenuItem.Name = "lockHorizontallyToolStripMenuItem";
            this.lockHorizontallyToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.lockHorizontallyToolStripMenuItem.Text = "Lock Horizontally";
            this.lockHorizontallyToolStripMenuItem.Click += new System.EventHandler(this.LockHorizontallyToolStripMenuItem_Click);
            // 
            // lockVerticallyToolStripMenuItem
            // 
            this.lockVerticallyToolStripMenuItem.Name = "lockVerticallyToolStripMenuItem";
            this.lockVerticallyToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.lockVerticallyToolStripMenuItem.Text = "Lock Vertically";
            this.lockVerticallyToolStripMenuItem.Click += new System.EventHandler(this.LockVerticallyToolStripMenuItem_Click);
            // 
            // clearLockToolStripMenuItem
            // 
            this.clearLockToolStripMenuItem.Name = "clearLockToolStripMenuItem";
            this.clearLockToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.clearLockToolStripMenuItem.Text = "Clear Lock";
            this.clearLockToolStripMenuItem.Click += new System.EventHandler(this.ClearLockToolStripMenuItem_Click);
            // 
            // PolygonApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 679);
            this.Controls.Add(this.splitContainer1);
            this.KeyPreview = true;
            this.Name = "PolygonApp";
            this.Text = "Polygon Editor [Create Mode]";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PolygonApp_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panelRadio.ResumeLayout(false);
            this.panelRadio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Panel panelRadio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioPolygon;
        private System.Windows.Forms.RadioButton radioVertices;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem angleConstraintToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem addVertexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lockHorizontallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lockVerticallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearLockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearConstraintToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
    }
}

