namespace InertialEstimator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonChooseFile = new System.Windows.Forms.Button();
            this.labelFilePath = new System.Windows.Forms.Label();
            this.textBoxOriginX = new System.Windows.Forms.TextBox();
            this.textBoxOriginY = new System.Windows.Forms.TextBox();
            this.textBoxOriginZ = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.textBoxDensity = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxDiameter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(30, 236);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(112, 34);
            this.buttonStart.TabIndex = 5;
            this.buttonStart.Text = "Analyze";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonChooseFile
            // 
            this.buttonChooseFile.Location = new System.Drawing.Point(30, 25);
            this.buttonChooseFile.Name = "buttonChooseFile";
            this.buttonChooseFile.Size = new System.Drawing.Size(189, 34);
            this.buttonChooseFile.TabIndex = 0;
            this.buttonChooseFile.Text = "Choose G-code file";
            this.buttonChooseFile.UseVisualStyleBackColor = true;
            this.buttonChooseFile.Click += new System.EventHandler(this.buttonChooseFile_Click);
            // 
            // labelFilePath
            // 
            this.labelFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFilePath.Location = new System.Drawing.Point(225, 30);
            this.labelFilePath.Name = "labelFilePath";
            this.labelFilePath.Size = new System.Drawing.Size(507, 52);
            this.labelFilePath.TabIndex = 2;
            this.labelFilePath.Text = "File not selected.";
            // 
            // textBoxOriginX
            // 
            this.textBoxOriginX.Location = new System.Drawing.Point(148, 99);
            this.textBoxOriginX.Name = "textBoxOriginX";
            this.textBoxOriginX.Size = new System.Drawing.Size(108, 31);
            this.textBoxOriginX.TabIndex = 1;
            this.textBoxOriginX.Text = "0";
            this.textBoxOriginX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxOriginY
            // 
            this.textBoxOriginY.Location = new System.Drawing.Point(148, 136);
            this.textBoxOriginY.Name = "textBoxOriginY";
            this.textBoxOriginY.Size = new System.Drawing.Size(108, 31);
            this.textBoxOriginY.TabIndex = 3;
            this.textBoxOriginY.Text = "0";
            this.textBoxOriginY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxOriginZ
            // 
            this.textBoxOriginZ.Location = new System.Drawing.Point(148, 173);
            this.textBoxOriginZ.Name = "textBoxOriginZ";
            this.textBoxOriginZ.Size = new System.Drawing.Size(108, 31);
            this.textBoxOriginZ.TabIndex = 4;
            this.textBoxOriginZ.Text = "0";
            this.textBoxOriginZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Reference X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Reference Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "Reference Z";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(163, 236);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(569, 34);
            this.progressBar1.TabIndex = 7;
            // 
            // textBoxResult
            // 
            this.textBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResult.Location = new System.Drawing.Point(30, 306);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxResult.Size = new System.Drawing.Size(702, 516);
            this.textBoxResult.TabIndex = 8;
            // 
            // textBoxDensity
            // 
            this.textBoxDensity.Location = new System.Drawing.Point(516, 99);
            this.textBoxDensity.Name = "textBoxDensity";
            this.textBoxDensity.Size = new System.Drawing.Size(121, 31);
            this.textBoxDensity.TabIndex = 9;
            this.textBoxDensity.Text = "1.24";
            this.textBoxDensity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(355, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 25);
            this.label4.TabIndex = 10;
            this.label4.Text = "Density [g/cm3]";
            // 
            // textBoxDiameter
            // 
            this.textBoxDiameter.Location = new System.Drawing.Point(516, 136);
            this.textBoxDiameter.Name = "textBoxDiameter";
            this.textBoxDiameter.Size = new System.Drawing.Size(121, 31);
            this.textBoxDiameter.TabIndex = 11;
            this.textBoxDiameter.Text = "1.75";
            this.textBoxDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(355, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 25);
            this.label5.TabIndex = 12;
            this.label5.Text = "Diameter [mm]";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 849);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxDiameter);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxDensity);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxOriginZ);
            this.Controls.Add(this.textBoxOriginY);
            this.Controls.Add(this.textBoxOriginX);
            this.Controls.Add(this.labelFilePath);
            this.Controls.Add(this.buttonChooseFile);
            this.Controls.Add(this.buttonStart);
            this.Name = "Form1";
            this.Text = "Moment of Inertia Estimator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button buttonStart;
        private Button buttonChooseFile;
        private Label labelFilePath;
        private TextBox textBoxOriginX;
        private TextBox textBoxOriginY;
        private TextBox textBoxOriginZ;
        private Label label1;
        private Label label2;
        private Label label3;
        private ProgressBar progressBar1;
        private TextBox textBoxResult;
        private TextBox textBoxDensity;
        private Label label4;
        private TextBox textBoxDiameter;
        private Label label5;
    }
}