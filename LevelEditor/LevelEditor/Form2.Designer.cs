namespace LevelEditor
{
    partial class levelEditor
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
            this.mapBox = new System.Windows.Forms.GroupBox();
            this.textureBox = new System.Windows.Forms.GroupBox();
            this.texturePic = new System.Windows.Forms.PictureBox();
            this.backgroundButton = new System.Windows.Forms.Button();
            this.colorPick = new System.Windows.Forms.ColorDialog();
            this.collisionsButton = new System.Windows.Forms.Button();
            this.rotateTexture = new System.Windows.Forms.Button();
            this.texture1 = new System.Windows.Forms.PictureBox();
            this.texture2 = new System.Windows.Forms.PictureBox();
            this.texture4 = new System.Windows.Forms.PictureBox();
            this.texture3 = new System.Windows.Forms.PictureBox();
            this.texture8 = new System.Windows.Forms.PictureBox();
            this.texture7 = new System.Windows.Forms.PictureBox();
            this.texture6 = new System.Windows.Forms.PictureBox();
            this.texture5 = new System.Windows.Forms.PictureBox();
            this.textures = new System.Windows.Forms.GroupBox();
            this.paintButton = new System.Windows.Forms.Button();
            this.objButton = new System.Windows.Forms.Button();
            this.Metadata = new System.Windows.Forms.Button();
            this.oldLoadButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.textureBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.texturePic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture5)).BeginInit();
            this.textures.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadButton
            // 
            this.loadButton.BackColor = System.Drawing.Color.LavenderBlush;
            this.loadButton.Location = new System.Drawing.Point(223, 352);
            this.loadButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(91, 80);
            this.loadButton.TabIndex = 8;
            this.loadButton.Text = "Load File";
            this.loadButton.UseVisualStyleBackColor = false;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // mapBox
            // 
            this.mapBox.BackColor = System.Drawing.Color.Cornsilk;
            this.mapBox.Location = new System.Drawing.Point(433, 12);
            this.mapBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mapBox.Name = "mapBox";
            this.mapBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mapBox.Size = new System.Drawing.Size(749, 750);
            this.mapBox.TabIndex = 9;
            this.mapBox.TabStop = false;
            this.mapBox.Text = "Map";
            // 
            // textureBox
            // 
            this.textureBox.Controls.Add(this.texturePic);
            this.textureBox.Location = new System.Drawing.Point(17, 273);
            this.textureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textureBox.Name = "textureBox";
            this.textureBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textureBox.Size = new System.Drawing.Size(199, 206);
            this.textureBox.TabIndex = 11;
            this.textureBox.TabStop = false;
            this.textureBox.Text = "Current Texture";
            // 
            // texturePic
            // 
            this.texturePic.Location = new System.Drawing.Point(21, 37);
            this.texturePic.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.texturePic.Name = "texturePic";
            this.texturePic.Size = new System.Drawing.Size(151, 143);
            this.texturePic.TabIndex = 0;
            this.texturePic.TabStop = false;
            // 
            // backgroundButton
            // 
            this.backgroundButton.BackColor = System.Drawing.Color.LavenderBlush;
            this.backgroundButton.Location = new System.Drawing.Point(320, 273);
            this.backgroundButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.backgroundButton.Name = "backgroundButton";
            this.backgroundButton.Size = new System.Drawing.Size(107, 71);
            this.backgroundButton.TabIndex = 15;
            this.backgroundButton.Text = "Background";
            this.backgroundButton.UseVisualStyleBackColor = false;
            this.backgroundButton.Click += new System.EventHandler(this.backgroundButton_Click);
            // 
            // collisionsButton
            // 
            this.collisionsButton.BackColor = System.Drawing.Color.LavenderBlush;
            this.collisionsButton.Location = new System.Drawing.Point(320, 352);
            this.collisionsButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.collisionsButton.Name = "collisionsButton";
            this.collisionsButton.Size = new System.Drawing.Size(107, 75);
            this.collisionsButton.TabIndex = 18;
            this.collisionsButton.Text = "Collisions";
            this.collisionsButton.UseVisualStyleBackColor = false;
            this.collisionsButton.Click += new System.EventHandler(this.collisionsButton_Click);
            // 
            // rotateTexture
            // 
            this.rotateTexture.BackColor = System.Drawing.Color.LavenderBlush;
            this.rotateTexture.Location = new System.Drawing.Point(221, 273);
            this.rotateTexture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rotateTexture.Name = "rotateTexture";
            this.rotateTexture.Size = new System.Drawing.Size(91, 71);
            this.rotateTexture.TabIndex = 19;
            this.rotateTexture.Text = "Rotate \r\nTexture\r\n";
            this.rotateTexture.UseVisualStyleBackColor = false;
            this.rotateTexture.Click += new System.EventHandler(this.rotateTexture_Click);
            // 
            // texture1
            // 
            this.texture1.Location = new System.Drawing.Point(37, 21);
            this.texture1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.texture1.Name = "texture1";
            this.texture1.Size = new System.Drawing.Size(75, 73);
            this.texture1.TabIndex = 20;
            this.texture1.TabStop = false;
            this.texture1.Click += new System.EventHandler(this.ChangePath);
            // 
            // texture2
            // 
            this.texture2.Location = new System.Drawing.Point(117, 21);
            this.texture2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.texture2.Name = "texture2";
            this.texture2.Size = new System.Drawing.Size(75, 73);
            this.texture2.TabIndex = 21;
            this.texture2.TabStop = false;
            this.texture2.Click += new System.EventHandler(this.ChangePath);
            // 
            // texture4
            // 
            this.texture4.Location = new System.Drawing.Point(277, 21);
            this.texture4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.texture4.Name = "texture4";
            this.texture4.Size = new System.Drawing.Size(75, 73);
            this.texture4.TabIndex = 22;
            this.texture4.TabStop = false;
            this.texture4.Click += new System.EventHandler(this.ChangePath);
            // 
            // texture3
            // 
            this.texture3.Location = new System.Drawing.Point(197, 21);
            this.texture3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.texture3.Name = "texture3";
            this.texture3.Size = new System.Drawing.Size(75, 73);
            this.texture3.TabIndex = 23;
            this.texture3.TabStop = false;
            this.texture3.Click += new System.EventHandler(this.ChangePath);
            // 
            // texture8
            // 
            this.texture8.Location = new System.Drawing.Point(277, 100);
            this.texture8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.texture8.Name = "texture8";
            this.texture8.Size = new System.Drawing.Size(75, 73);
            this.texture8.TabIndex = 24;
            this.texture8.TabStop = false;
            this.texture8.Click += new System.EventHandler(this.ChangePath);
            // 
            // texture7
            // 
            this.texture7.Location = new System.Drawing.Point(197, 100);
            this.texture7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.texture7.Name = "texture7";
            this.texture7.Size = new System.Drawing.Size(75, 73);
            this.texture7.TabIndex = 25;
            this.texture7.TabStop = false;
            this.texture7.Click += new System.EventHandler(this.ChangePath);
            // 
            // texture6
            // 
            this.texture6.Location = new System.Drawing.Point(117, 100);
            this.texture6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.texture6.Name = "texture6";
            this.texture6.Size = new System.Drawing.Size(75, 73);
            this.texture6.TabIndex = 26;
            this.texture6.TabStop = false;
            this.texture6.Click += new System.EventHandler(this.ChangePath);
            // 
            // texture5
            // 
            this.texture5.Location = new System.Drawing.Point(37, 100);
            this.texture5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.texture5.Name = "texture5";
            this.texture5.Size = new System.Drawing.Size(75, 73);
            this.texture5.TabIndex = 27;
            this.texture5.TabStop = false;
            this.texture5.Click += new System.EventHandler(this.ChangePath);
            // 
            // textures
            // 
            this.textures.Controls.Add(this.texture5);
            this.textures.Controls.Add(this.texture1);
            this.textures.Controls.Add(this.texture6);
            this.textures.Controls.Add(this.texture3);
            this.textures.Controls.Add(this.texture4);
            this.textures.Controls.Add(this.texture7);
            this.textures.Controls.Add(this.texture8);
            this.textures.Controls.Add(this.texture2);
            this.textures.Location = new System.Drawing.Point(27, 30);
            this.textures.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textures.Name = "textures";
            this.textures.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textures.Size = new System.Drawing.Size(367, 183);
            this.textures.TabIndex = 28;
            this.textures.TabStop = false;
            this.textures.Text = "Textures";
            // 
            // paintButton
            // 
            this.paintButton.BackColor = System.Drawing.Color.LavenderBlush;
            this.paintButton.Location = new System.Drawing.Point(55, 218);
            this.paintButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.paintButton.Name = "paintButton";
            this.paintButton.Size = new System.Drawing.Size(315, 50);
            this.paintButton.TabIndex = 29;
            this.paintButton.Text = "Paint Background Layer";
            this.paintButton.UseVisualStyleBackColor = false;
            this.paintButton.Click += new System.EventHandler(this.PaintBox);
            // 
            // objButton
            // 
            this.objButton.BackColor = System.Drawing.Color.LavenderBlush;
            this.objButton.Location = new System.Drawing.Point(320, 437);
            this.objButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.objButton.Name = "objButton";
            this.objButton.Size = new System.Drawing.Size(107, 75);
            this.objButton.TabIndex = 30;
            this.objButton.Text = "Objects";
            this.objButton.UseVisualStyleBackColor = false;
            this.objButton.Click += new System.EventHandler(this.objButton_Click);
            // 
            // Metadata
            // 
            this.Metadata.BackColor = System.Drawing.Color.LavenderBlush;
            this.Metadata.Location = new System.Drawing.Point(221, 519);
            this.Metadata.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Metadata.Name = "Metadata";
            this.Metadata.Size = new System.Drawing.Size(205, 80);
            this.Metadata.TabIndex = 31;
            this.Metadata.Text = "Metadata: \"\"";
            this.Metadata.UseVisualStyleBackColor = false;
            this.Metadata.Click += new System.EventHandler(this.Metadata_Click);
            // 
            // oldLoadButton
            // 
            this.oldLoadButton.BackColor = System.Drawing.Color.LavenderBlush;
            this.oldLoadButton.Location = new System.Drawing.Point(125, 519);
            this.oldLoadButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.oldLoadButton.Name = "oldLoadButton";
            this.oldLoadButton.Size = new System.Drawing.Size(91, 80);
            this.oldLoadButton.TabIndex = 32;
            this.oldLoadButton.Text = "Load old File";
            this.oldLoadButton.UseVisualStyleBackColor = false;
            this.oldLoadButton.Click += new System.EventHandler(this.oldLoadButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.BackColor = System.Drawing.Color.LavenderBlush;
            this.nextButton.Location = new System.Drawing.Point(335, 603);
            this.nextButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(91, 80);
            this.nextButton.TabIndex = 33;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = false;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // prevButton
            // 
            this.prevButton.BackColor = System.Drawing.Color.LavenderBlush;
            this.prevButton.Location = new System.Drawing.Point(221, 603);
            this.prevButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size(91, 80);
            this.prevButton.TabIndex = 34;
            this.prevButton.Text = "Previous";
            this.prevButton.UseVisualStyleBackColor = false;
            this.prevButton.Click += new System.EventHandler(this.prevButton_Click);
            // 
            // levelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Cornsilk;
            this.ClientSize = new System.Drawing.Size(1252, 854);
            this.Controls.Add(this.prevButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.oldLoadButton);
            this.Controls.Add(this.Metadata);
            this.Controls.Add(this.objButton);
            this.Controls.Add(this.paintButton);
            this.Controls.Add(this.textures);
            this.Controls.Add(this.rotateTexture);
            this.Controls.Add(this.collisionsButton);
            this.Controls.Add(this.backgroundButton);
            this.Controls.Add(this.textureBox);
            this.Controls.Add(this.mapBox);
            this.Controls.Add(this.loadButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "levelEditor";
            this.Text = "Level Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LevelEditor_FormClosing);
            this.Load += new System.EventHandler(this.LevelEditor_Load);
            this.textureBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.texturePic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture5)).EndInit();
            this.textures.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.GroupBox mapBox;
        private System.Windows.Forms.GroupBox textureBox;
        private System.Windows.Forms.PictureBox texturePic;
        private System.Windows.Forms.Button backgroundButton;
        private System.Windows.Forms.ColorDialog colorPick;
        private System.Windows.Forms.Button collisionsButton;
        private System.Windows.Forms.Button rotateTexture;
        private System.Windows.Forms.PictureBox texture1;
        private System.Windows.Forms.PictureBox texture2;
        private System.Windows.Forms.PictureBox texture4;
        private System.Windows.Forms.PictureBox texture3;
        private System.Windows.Forms.PictureBox texture8;
        private System.Windows.Forms.PictureBox texture7;
        private System.Windows.Forms.PictureBox texture6;
        private System.Windows.Forms.PictureBox texture5;
        private System.Windows.Forms.GroupBox textures;
        private System.Windows.Forms.Button paintButton;
        private System.Windows.Forms.Button objButton;
        private System.Windows.Forms.Button Metadata;
        private System.Windows.Forms.Button oldLoadButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button prevButton;
    }
}