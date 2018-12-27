namespace Sample.CognitoSyncSettings.Windows
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
            this.components = new System.ComponentModel.Container();
            this.boolValue = new System.Windows.Forms.CheckBox();
            this.textValue = new System.Windows.Forms.TextBox();
            this.enumValue = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dynamicValue6 = new System.Windows.Forms.CheckBox();
            this.dynamicValue5 = new System.Windows.Forms.CheckBox();
            this.dynamicValue4 = new System.Windows.Forms.CheckBox();
            this.dynamicValue3 = new System.Windows.Forms.CheckBox();
            this.dynamicValue2 = new System.Windows.Forms.CheckBox();
            this.dynamicValue1 = new System.Windows.Forms.CheckBox();
            this.facebookLogin = new System.Windows.Forms.Button();
            this.googleLogin = new System.Windows.Forms.Button();
            this.logout = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.synchronize = new System.Windows.Forms.Button();
            this.cognitoSyncSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cognitoSyncSettingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // boolValue
            // 
            this.boolValue.AutoSize = true;
            this.boolValue.Location = new System.Drawing.Point(6, 19);
            this.boolValue.Name = "boolValue";
            this.boolValue.Size = new System.Drawing.Size(75, 17);
            this.boolValue.TabIndex = 0;
            this.boolValue.Text = "bool value";
            this.boolValue.UseVisualStyleBackColor = true;
            this.boolValue.CheckedChanged += new System.EventHandler(this.boolValue_CheckedChanged);
            // 
            // textValue
            // 
            this.textValue.Location = new System.Drawing.Point(6, 42);
            this.textValue.Name = "textValue";
            this.textValue.Size = new System.Drawing.Size(352, 20);
            this.textValue.TabIndex = 1;
            this.textValue.TextChanged += new System.EventHandler(this.textValue_TextChanged);
            // 
            // enumValue
            // 
            this.enumValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.enumValue.FormattingEnabled = true;
            this.enumValue.Location = new System.Drawing.Point(6, 68);
            this.enumValue.Name = "enumValue";
            this.enumValue.Size = new System.Drawing.Size(352, 21);
            this.enumValue.TabIndex = 2;
            this.enumValue.SelectedIndexChanged += new System.EventHandler(this.enumValue_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dynamicValue6);
            this.groupBox1.Controls.Add(this.dynamicValue5);
            this.groupBox1.Controls.Add(this.dynamicValue4);
            this.groupBox1.Controls.Add(this.dynamicValue3);
            this.groupBox1.Controls.Add(this.dynamicValue2);
            this.groupBox1.Controls.Add(this.dynamicValue1);
            this.groupBox1.Location = new System.Drawing.Point(13, 121);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 100);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dynamic settings";
            // 
            // dynamicValue6
            // 
            this.dynamicValue6.AutoSize = true;
            this.dynamicValue6.Location = new System.Drawing.Point(186, 68);
            this.dynamicValue6.Name = "dynamicValue6";
            this.dynamicValue6.Size = new System.Drawing.Size(103, 17);
            this.dynamicValue6.TabIndex = 5;
            this.dynamicValue6.Text = "dynamic value 6";
            this.dynamicValue6.UseVisualStyleBackColor = true;
            this.dynamicValue6.CheckedChanged += new System.EventHandler(this.dynamicValue6_CheckedChanged);
            // 
            // dynamicValue5
            // 
            this.dynamicValue5.AutoSize = true;
            this.dynamicValue5.Location = new System.Drawing.Point(186, 44);
            this.dynamicValue5.Name = "dynamicValue5";
            this.dynamicValue5.Size = new System.Drawing.Size(103, 17);
            this.dynamicValue5.TabIndex = 4;
            this.dynamicValue5.Text = "dynamic value 5";
            this.dynamicValue5.UseVisualStyleBackColor = true;
            this.dynamicValue5.CheckedChanged += new System.EventHandler(this.dynamicValue5_CheckedChanged);
            // 
            // dynamicValue4
            // 
            this.dynamicValue4.AutoSize = true;
            this.dynamicValue4.Location = new System.Drawing.Point(186, 19);
            this.dynamicValue4.Name = "dynamicValue4";
            this.dynamicValue4.Size = new System.Drawing.Size(103, 17);
            this.dynamicValue4.TabIndex = 3;
            this.dynamicValue4.Text = "dynamic value 4";
            this.dynamicValue4.UseVisualStyleBackColor = true;
            this.dynamicValue4.CheckedChanged += new System.EventHandler(this.dynamicValue4_CheckedChanged);
            // 
            // dynamicValue3
            // 
            this.dynamicValue3.AutoSize = true;
            this.dynamicValue3.Location = new System.Drawing.Point(7, 68);
            this.dynamicValue3.Name = "dynamicValue3";
            this.dynamicValue3.Size = new System.Drawing.Size(103, 17);
            this.dynamicValue3.TabIndex = 2;
            this.dynamicValue3.Text = "dynamic value 3";
            this.dynamicValue3.UseVisualStyleBackColor = true;
            this.dynamicValue3.CheckedChanged += new System.EventHandler(this.dynamicValue3_CheckedChanged);
            // 
            // dynamicValue2
            // 
            this.dynamicValue2.AutoSize = true;
            this.dynamicValue2.Location = new System.Drawing.Point(7, 44);
            this.dynamicValue2.Name = "dynamicValue2";
            this.dynamicValue2.Size = new System.Drawing.Size(103, 17);
            this.dynamicValue2.TabIndex = 1;
            this.dynamicValue2.Text = "dynamic value 2";
            this.dynamicValue2.UseVisualStyleBackColor = true;
            this.dynamicValue2.CheckedChanged += new System.EventHandler(this.dynamicValue2_CheckedChanged);
            // 
            // dynamicValue1
            // 
            this.dynamicValue1.AutoSize = true;
            this.dynamicValue1.Location = new System.Drawing.Point(7, 20);
            this.dynamicValue1.Name = "dynamicValue1";
            this.dynamicValue1.Size = new System.Drawing.Size(103, 17);
            this.dynamicValue1.TabIndex = 0;
            this.dynamicValue1.Text = "dynamic value 1";
            this.dynamicValue1.UseVisualStyleBackColor = true;
            this.dynamicValue1.CheckedChanged += new System.EventHandler(this.dynamicValue1_CheckedChanged);
            // 
            // facebookLogin
            // 
            this.facebookLogin.Location = new System.Drawing.Point(12, 268);
            this.facebookLogin.Name = "facebookLogin";
            this.facebookLogin.Size = new System.Drawing.Size(364, 23);
            this.facebookLogin.TabIndex = 4;
            this.facebookLogin.Text = "Login with Facebook";
            this.facebookLogin.UseVisualStyleBackColor = true;
            this.facebookLogin.Click += new System.EventHandler(this.facebookLogin_Click);
            // 
            // googleLogin
            // 
            this.googleLogin.Location = new System.Drawing.Point(12, 297);
            this.googleLogin.Name = "googleLogin";
            this.googleLogin.Size = new System.Drawing.Size(364, 23);
            this.googleLogin.TabIndex = 5;
            this.googleLogin.Text = "Login with Google";
            this.googleLogin.UseVisualStyleBackColor = true;
            this.googleLogin.Click += new System.EventHandler(this.googleLogin_Click);
            // 
            // logout
            // 
            this.logout.Location = new System.Drawing.Point(13, 326);
            this.logout.Name = "logout";
            this.logout.Size = new System.Drawing.Size(364, 23);
            this.logout.TabIndex = 6;
            this.logout.Text = "Logout";
            this.logout.UseVisualStyleBackColor = true;
            this.logout.Visible = false;
            this.logout.Click += new System.EventHandler(this.logout_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.boolValue);
            this.groupBox2.Controls.Add(this.textValue);
            this.groupBox2.Controls.Add(this.enumValue);
            this.groupBox2.Location = new System.Drawing.Point(13, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 100);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Static settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 385);
            this.label1.MaximumSize = new System.Drawing.Size(350, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 26);
            this.label1.TabIndex = 8;
            this.label1.Text = "See Sample.CognitoSyncSettings.iOS and Sample.CognitoSyncSettings.Android project" +
    "s for more detail";
            // 
            // synchronize
            // 
            this.synchronize.Location = new System.Drawing.Point(13, 228);
            this.synchronize.Name = "synchronize";
            this.synchronize.Size = new System.Drawing.Size(363, 34);
            this.synchronize.TabIndex = 9;
            this.synchronize.Text = "Synchronize dataset";
            this.synchronize.UseVisualStyleBackColor = true;
            this.synchronize.Visible = false;
            this.synchronize.Click += new System.EventHandler(this.SynchronizeDataset_Click);
            // 
            // cognitoSyncSettingsBindingSource
            // 
            this.cognitoSyncSettingsBindingSource.DataSource = typeof(Sample.CognitoSyncSettings.Windows.CognitoSyncSettings);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 421);
            this.Controls.Add(this.synchronize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.logout);
            this.Controls.Add(this.googleLogin);
            this.Controls.Add(this.facebookLogin);
            this.Controls.Add(this.groupBox1);
            this.MaximumSize = new System.Drawing.Size(405, 460);
            this.MinimumSize = new System.Drawing.Size(405, 460);
            this.Name = "Form1";
            this.Text = "Advexp.Settings + Amazon Cognito Sync Sample";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cognitoSyncSettingsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox boolValue;
        private System.Windows.Forms.TextBox textValue;
        private System.Windows.Forms.ComboBox enumValue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox dynamicValue6;
        private System.Windows.Forms.CheckBox dynamicValue5;
        private System.Windows.Forms.CheckBox dynamicValue4;
        private System.Windows.Forms.CheckBox dynamicValue3;
        private System.Windows.Forms.CheckBox dynamicValue2;
        private System.Windows.Forms.CheckBox dynamicValue1;
        private System.Windows.Forms.Button facebookLogin;
        private System.Windows.Forms.Button googleLogin;
        private System.Windows.Forms.Button logout;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.BindingSource cognitoSyncSettingsBindingSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button synchronize;
    }
}

