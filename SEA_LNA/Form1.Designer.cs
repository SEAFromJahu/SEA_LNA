namespace SEA_LNA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.OpenFileDialog = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.rBSHA1 = new System.Windows.Forms.RadioButton();
            this.rBSHA256 = new System.Windows.Forms.RadioButton();
            this.rBSHA384 = new System.Windows.Forms.RadioButton();
            this.rB512 = new System.Windows.Forms.RadioButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.CancelaListagem = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkBCalcularHash = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OpenFileDialog.Location = new System.Drawing.Point(157, 8);
            this.OpenFileDialog.Name = "OpenFileDialog";
            this.OpenFileDialog.Size = new System.Drawing.Size(100, 25);
            this.OpenFileDialog.TabIndex = 0;
            this.OpenFileDialog.Text = "Iniciar Processo";
            this.OpenFileDialog.UseVisualStyleBackColor = true;
            this.OpenFileDialog.Click += new System.EventHandler(this.OpenFileDialog_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.AutoWordSelection = true;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(803, 202);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(34, 13);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(117, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Listar Sub Pastas ?";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // rBSHA1
            // 
            this.rBSHA1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rBSHA1.AutoSize = true;
            this.rBSHA1.Checked = true;
            this.rBSHA1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rBSHA1.Location = new System.Drawing.Point(109, 13);
            this.rBSHA1.Name = "rBSHA1";
            this.rBSHA1.Size = new System.Drawing.Size(53, 17);
            this.rBSHA1.TabIndex = 3;
            this.rBSHA1.TabStop = true;
            this.rBSHA1.Text = "SHA1";
            this.rBSHA1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.rBSHA1.UseVisualStyleBackColor = true;
            this.rBSHA1.CheckedChanged += new System.EventHandler(this.RBSHA1_CheckedChanged_1);
            // 
            // rBSHA256
            // 
            this.rBSHA256.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rBSHA256.AutoSize = true;
            this.rBSHA256.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rBSHA256.Location = new System.Drawing.Point(168, 12);
            this.rBSHA256.Name = "rBSHA256";
            this.rBSHA256.Size = new System.Drawing.Size(65, 17);
            this.rBSHA256.TabIndex = 3;
            this.rBSHA256.Text = "SHA256";
            this.rBSHA256.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.rBSHA256.UseVisualStyleBackColor = true;
            this.rBSHA256.CheckedChanged += new System.EventHandler(this.RBSHA256_CheckedChanged_1);
            // 
            // rBSHA384
            // 
            this.rBSHA384.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rBSHA384.AutoSize = true;
            this.rBSHA384.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rBSHA384.Location = new System.Drawing.Point(239, 13);
            this.rBSHA384.Name = "rBSHA384";
            this.rBSHA384.Size = new System.Drawing.Size(65, 17);
            this.rBSHA384.TabIndex = 3;
            this.rBSHA384.Text = "SHA384";
            this.rBSHA384.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.rBSHA384.UseVisualStyleBackColor = true;
            this.rBSHA384.CheckedChanged += new System.EventHandler(this.RBSHA384_CheckedChanged_1);
            // 
            // rB512
            // 
            this.rB512.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rB512.AutoSize = true;
            this.rB512.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rB512.Location = new System.Drawing.Point(310, 14);
            this.rB512.Name = "rB512";
            this.rB512.Size = new System.Drawing.Size(65, 17);
            this.rB512.TabIndex = 3;
            this.rB512.Text = "SHA512";
            this.rB512.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.rB512.UseVisualStyleBackColor = true;
            this.rB512.CheckedChanged += new System.EventHandler(this.RB512_CheckedChanged);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker1_RunWorkerCompleted_1);
            // 
            // CancelaListagem
            // 
            this.CancelaListagem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelaListagem.Location = new System.Drawing.Point(679, 263);
            this.CancelaListagem.Name = "CancelaListagem";
            this.CancelaListagem.Size = new System.Drawing.Size(150, 23);
            this.CancelaListagem.TabIndex = 4;
            this.CancelaListagem.Text = "Cancelar Operação";
            this.CancelaListagem.UseVisualStyleBackColor = true;
            this.CancelaListagem.Click += new System.EventHandler(this.CancelaListagem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.chkBCalcularHash);
            this.groupBox1.Controls.Add(this.rBSHA1);
            this.groupBox1.Controls.Add(this.rB512);
            this.groupBox1.Controls.Add(this.rBSHA256);
            this.groupBox1.Controls.Add(this.rBSHA384);
            this.groupBox1.Location = new System.Drawing.Point(12, 252);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(381, 36);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hash";
            // 
            // chkBCalcularHash
            // 
            this.chkBCalcularHash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBCalcularHash.AutoSize = true;
            this.chkBCalcularHash.Location = new System.Drawing.Point(31, 13);
            this.chkBCalcularHash.Name = "chkBCalcularHash";
            this.chkBCalcularHash.Size = new System.Drawing.Size(73, 17);
            this.chkBCalcularHash.TabIndex = 5;
            this.chkBCalcularHash.Text = "Calcular ?";
            this.chkBCalcularHash.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.OpenFileDialog);
            this.groupBox2.Location = new System.Drawing.Point(405, 252);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(263, 36);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Arquivos";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(817, 234);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(809, 208);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Lista total dos Arquivos";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(809, 208);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Lista de Arquivos Iguais";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox2.Location = new System.Drawing.Point(3, 3);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(803, 202);
            this.richTextBox2.TabIndex = 0;
            this.richTextBox2.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 295);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CancelaListagem);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Lista Arquivos";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OpenFileDialog;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.RadioButton rBSHA1;
        private System.Windows.Forms.RadioButton rBSHA256;
        private System.Windows.Forms.RadioButton rBSHA384;
        private System.Windows.Forms.RadioButton rB512;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button CancelaListagem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkBCalcularHash;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox richTextBox2;
    }
}

