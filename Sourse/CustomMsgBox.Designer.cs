namespace MailNotifier
{
    partial class CustomMsgBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomMsgBox));
            this.pictureBoxQuest = new System.Windows.Forms.PictureBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnCansel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxAllert = new System.Windows.Forms.PictureBox();
            this.colorLabel1 = new MailNotifier.ColorLabel(this.components);
            this.colorLabel2 = new MailNotifier.ColorLabel(this.components);
            this.labelAllert = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQuest)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAllert)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxQuest
            // 
            this.pictureBoxQuest.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxQuest.Image")));
            this.pictureBoxQuest.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxQuest.Name = "pictureBoxQuest";
            this.pictureBoxQuest.Size = new System.Drawing.Size(48, 46);
            this.pictureBoxQuest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxQuest.TabIndex = 4;
            this.pictureBoxQuest.TabStop = false;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.IndianRed;
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOk.Location = new System.Drawing.Point(28, 76);
            this.btnOk.Margin = new System.Windows.Forms.Padding(0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "ДА";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.Transparent;
            this.btnNo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNo.Location = new System.Drawing.Point(131, 76);
            this.btnNo.Margin = new System.Windows.Forms.Padding(0);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(90, 25);
            this.btnNo.TabIndex = 8;
            this.btnNo.Text = "НЕТ";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnCansel
            // 
            this.btnCansel.BackColor = System.Drawing.Color.Transparent;
            this.btnCansel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCansel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCansel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCansel.Location = new System.Drawing.Point(234, 76);
            this.btnCansel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCansel.Name = "btnCansel";
            this.btnCansel.Size = new System.Drawing.Size(90, 25);
            this.btnCansel.TabIndex = 9;
            this.btnCansel.Text = "ОТМЕНА";
            this.btnCansel.UseVisualStyleBackColor = false;
            this.btnCansel.Click += new System.EventHandler(this.btnCansel_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(40)))));
            this.panel1.Controls.Add(this.pictureBoxQuest);
            this.panel1.Controls.Add(this.pictureBoxAllert);
            this.panel1.Controls.Add(this.colorLabel1);
            this.panel1.Controls.Add(this.colorLabel2);
            this.panel1.Controls.Add(this.labelAllert);
            this.panel1.Controls.Add(this.btnCansel);
            this.panel1.Controls.Add(this.btnNo);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 108);
            this.panel1.TabIndex = 10;
            // 
            // pictureBoxAllert
            // 
            this.pictureBoxAllert.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxAllert.Image")));
            this.pictureBoxAllert.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxAllert.Name = "pictureBoxAllert";
            this.pictureBoxAllert.Size = new System.Drawing.Size(48, 46);
            this.pictureBoxAllert.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxAllert.TabIndex = 10;
            this.pictureBoxAllert.TabStop = false;
            this.pictureBoxAllert.Visible = false;
            // 
            // colorLabel1
            // 
            this.colorLabel1.AutoSize = true;
            this.colorLabel1.BackColor = System.Drawing.Color.Transparent;
            this.colorLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.colorLabel1.Location = new System.Drawing.Point(71, 14);
            this.colorLabel1.Name = "colorLabel1";
            this.colorLabel1.Size = new System.Drawing.Size(262, 16);
            this.colorLabel1.TabIndex = 5;
            this.colorLabel1.Text = "Значения параметров были изменены.";
            // 
            // colorLabel2
            // 
            this.colorLabel2.AutoSize = true;
            this.colorLabel2.BackColor = System.Drawing.Color.Transparent;
            this.colorLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.colorLabel2.Location = new System.Drawing.Point(71, 40);
            this.colorLabel2.Name = "colorLabel2";
            this.colorLabel2.Size = new System.Drawing.Size(214, 18);
            this.colorLabel2.TabIndex = 6;
            this.colorLabel2.Text = "Сохранить новые настройки?";
            // 
            // labelAllert
            // 
            this.labelAllert.BackColor = System.Drawing.Color.Transparent;
            this.labelAllert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelAllert.Location = new System.Drawing.Point(71, 8);
            this.labelAllert.MaximumSize = new System.Drawing.Size(276, 60);
            this.labelAllert.Name = "labelAllert";
            this.labelAllert.Size = new System.Drawing.Size(276, 60);
            this.labelAllert.TabIndex = 11;
            this.labelAllert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAllert.Visible = false;
            // 
            // CustomMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(360, 110);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomMsgBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CustomMsgBox";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQuest)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAllert)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBoxQuest;
        private ColorLabel colorLabel1;
        private ColorLabel colorLabel2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnCansel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBoxAllert;
        private System.Windows.Forms.Label labelAllert;
    }
}