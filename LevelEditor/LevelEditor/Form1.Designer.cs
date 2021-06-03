namespace LevelEditor
{
    partial class Form1
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
            this.loadButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.createButton = new System.Windows.Forms.Button();
            this.heightBox = new System.Windows.Forms.TextBox();
            this.widthBox = new System.Windows.Forms.TextBox();
            this.heightLabel = new System.Windows.Forms.Label();
            this.widthLabel = new System.Windows.Forms.Label();
            this.exportButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadButton
            // 
            this.loadButton.BackColor = System.Drawing.Color.Beige;
            this.loadButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loadButton.Font = new System.Drawing.Font("Sitka Small", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadButton.Location = new System.Drawing.Point(290, 156);
            this.loadButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(268, 81);
            this.loadButton.TabIndex = 0;
            this.loadButton.Text = "Load Map";
            this.loadButton.UseVisualStyleBackColor = false;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.createButton);
            this.groupBox1.Controls.Add(this.heightBox);
            this.groupBox1.Controls.Add(this.widthBox);
            this.groupBox1.Controls.Add(this.heightLabel);
            this.groupBox1.Controls.Add(this.widthLabel);
            this.groupBox1.Font = new System.Drawing.Font("Palatino Linotype", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 146);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(268, 210);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create New Map";
            // 
            // createButton
            // 
            this.createButton.BackColor = System.Drawing.Color.Beige;
            this.createButton.Location = new System.Drawing.Point(4, 139);
            this.createButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(259, 66);
            this.createButton.TabIndex = 4;
            this.createButton.Text = "Create Map!";
            this.createButton.UseVisualStyleBackColor = false;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // heightBox
            // 
            this.heightBox.BackColor = System.Drawing.Color.Ivory;
            this.heightBox.Location = new System.Drawing.Point(103, 105);
            this.heightBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.heightBox.Name = "heightBox";
            this.heightBox.Size = new System.Drawing.Size(91, 22);
            this.heightBox.TabIndex = 3;
            // 
            // widthBox
            // 
            this.widthBox.BackColor = System.Drawing.Color.Ivory;
            this.widthBox.Location = new System.Drawing.Point(103, 47);
            this.widthBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.widthBox.Name = "widthBox";
            this.widthBox.Size = new System.Drawing.Size(91, 22);
            this.widthBox.TabIndex = 2;
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(16, 107);
            this.heightLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(86, 16);
            this.heightLabel.TabIndex = 1;
            this.heightLabel.Text = "Height (in tiles)";
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(16, 50);
            this.widthLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(82, 16);
            this.widthLabel.TabIndex = 0;
            this.widthLabel.Text = "Width (in tiles)";
            // 
            // exportButton
            // 
            this.exportButton.BackColor = System.Drawing.Color.Beige;
            this.exportButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exportButton.Font = new System.Drawing.Font("Sitka Small", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportButton.Location = new System.Drawing.Point(290, 33);
            this.exportButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(268, 81);
            this.exportButton.TabIndex = 2;
            this.exportButton.Text = "Export File to Game\r\n(Beta Test)";
            this.exportButton.UseVisualStyleBackColor = false;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Beige;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("Sitka Small", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(290, 275);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(268, 81);
            this.button1.TabIndex = 3;
            this.button1.Text = "Load Old Map";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Cornsilk;
            this.ClientSize = new System.Drawing.Size(566, 366);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.loadButton);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Level Editor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.TextBox heightBox;
        private System.Windows.Forms.TextBox widthBox;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Button button1;
    }
}

