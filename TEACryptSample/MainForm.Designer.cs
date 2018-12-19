namespace TEACrypt
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtTEAKey = new System.Windows.Forms.TextBox();
            this.txtPlainText = new System.Windows.Forms.TextBox();
            this.txtEncryptedText = new System.Windows.Forms.TextBox();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.btnGenKey = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtTEAKey
            // 
            this.txtTEAKey.Location = new System.Drawing.Point(86, 12);
            this.txtTEAKey.Name = "txtTEAKey";
            this.txtTEAKey.Size = new System.Drawing.Size(527, 21);
            this.txtTEAKey.TabIndex = 0;
            // 
            // txtPlainText
            // 
            this.txtPlainText.Location = new System.Drawing.Point(12, 55);
            this.txtPlainText.Multiline = true;
            this.txtPlainText.Name = "txtPlainText";
            this.txtPlainText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPlainText.Size = new System.Drawing.Size(744, 235);
            this.txtPlainText.TabIndex = 1;
            this.txtPlainText.WordWrap = false;
            // 
            // txtEncryptedText
            // 
            this.txtEncryptedText.Location = new System.Drawing.Point(12, 338);
            this.txtEncryptedText.Multiline = true;
            this.txtEncryptedText.Name = "txtEncryptedText";
            this.txtEncryptedText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEncryptedText.Size = new System.Drawing.Size(744, 198);
            this.txtEncryptedText.TabIndex = 2;
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(99, 301);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(105, 26);
            this.btnEncrypt.TabIndex = 3;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(536, 301);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(105, 26);
            this.btnDecrypt.TabIndex = 4;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // btnGenKey
            // 
            this.btnGenKey.Location = new System.Drawing.Point(619, 8);
            this.btnGenKey.Name = "btnGenKey";
            this.btnGenKey.Size = new System.Drawing.Size(135, 26);
            this.btnGenKey.TabIndex = 5;
            this.btnGenKey.Text = "Generate New Key";
            this.btnGenKey.UseVisualStyleBackColor = true;
            this.btnGenKey.Click += new System.EventHandler(this.btnGenKey_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "TEA Key :";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 548);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGenKey);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.txtEncryptedText);
            this.Controls.Add(this.txtPlainText);
            this.Controls.Add(this.txtTEAKey);
            this.Name = "MainForm";
            this.Text = "TEA Encrypt/Decrypt Sample";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTEAKey;
        private System.Windows.Forms.TextBox txtPlainText;
        private System.Windows.Forms.TextBox txtEncryptedText;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnGenKey;
        private System.Windows.Forms.Label label1;
    }
}

