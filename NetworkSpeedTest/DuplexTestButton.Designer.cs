namespace NetworkSpeedTest
{
	partial class DuplexTestButton
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnDuplexTest = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnDuplexTest
			// 
			this.btnDuplexTest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDuplexTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDuplexTest.Location = new System.Drawing.Point(3, 3);
			this.btnDuplexTest.Name = "btnDuplexTest";
			this.btnDuplexTest.Size = new System.Drawing.Size(145, 62);
			this.btnDuplexTest.TabIndex = 0;
			this.btnDuplexTest.Text = "Server-to-Server Duplex Test";
			this.btnDuplexTest.UseVisualStyleBackColor = true;
			this.btnDuplexTest.Click += new System.EventHandler(this.btnDuplexTest_Click);
			// 
			// DuplexTestButton
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnDuplexTest);
			this.Name = "DuplexTestButton";
			this.Size = new System.Drawing.Size(151, 68);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnDuplexTest;
	}
}
