//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Windows.Forms;
using Terrarium.Glass;

namespace Terrarium.Forms
{
	/// <summary>
	///  Base Terrarium Form for use with Terrarium based dialogs.
	///  Controls painting of form borders, automatic set-up of
	///  a basic font, dragging functionality, and Terrarium style
	///  control boxes.
	/// </summary>
    public class TerrariumForm : Form
	{
		/// <summary>
        /// TitleBar backing store
		/// </summary>
        protected   GlassTitleBar   _titleBar;
        /// <summary>
        /// BottomPanel backing store
        /// </summary>
        protected   GlassPanel      _bottomPanel;
        /// <summary>
        /// Description Backing Store
        /// </summary>
        private     GlassLabel      _dialogDescriptionLabel;

        /// <summary>
        ///  Creates a default TerrariumForm.  The TerrariumForm should be extended
        ///  and not created directly.
        /// </summary>
		public TerrariumForm() 
        {
			InitializeComponent();

            try
			{
				Assembly thisAssembly = typeof(TerrariumForm).Assembly;

                this.Text = "Terrarium v" + thisAssembly.GetName().Version.ToString(3);
			}
			catch{}

			this.BackColor = GlassStyleManager.Active.DialogColor;
        }

    #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerrariumForm));
            this._titleBar = new Terrarium.Forms.GlassTitleBar();
            this._dialogDescriptionLabel = new Terrarium.Glass.GlassLabel();
            this._bottomPanel = new Terrarium.Glass.GlassPanel();
            this.SuspendLayout();
            // 
            // titleBar
            // 
            this._titleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this._titleBar.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._titleBar.ForeColor = System.Drawing.Color.White;
            this._titleBar.Image = ((System.Drawing.Image)(resources.GetObject("titleBar.Image")));
            this._titleBar.Location = new System.Drawing.Point(0, 0);
            this._titleBar.Name = "titleBar";
            this._titleBar.Size = new System.Drawing.Size(261, 32);
            this._titleBar.TabIndex = 13;
            this._titleBar.Title = "Form Title";
            // 
            // dialogDescriptionLabel
            // 
            this._dialogDescriptionLabel.BackColor = System.Drawing.Color.Transparent;
            this._dialogDescriptionLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._dialogDescriptionLabel.ForeColor = System.Drawing.Color.White;
            this._dialogDescriptionLabel.Location = new System.Drawing.Point(12, 35);
            this._dialogDescriptionLabel.Name = "dialogDescriptionLabel";
            this._dialogDescriptionLabel.NoWrap = false;
            this._dialogDescriptionLabel.Size = new System.Drawing.Size(237, 32);
            this._dialogDescriptionLabel.TabIndex = 14;
            this._dialogDescriptionLabel.Text = "Form Title";
            // 
            // bottomPanel
            // 
            this._bottomPanel.Borders = ((Terrarium.Glass.GlassBorders)((((Terrarium.Glass.GlassBorders.Left | Terrarium.Glass.GlassBorders.Top)
                        | Terrarium.Glass.GlassBorders.Right)
                        | Terrarium.Glass.GlassBorders.Bottom)));
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._bottomPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this._bottomPanel.IsGlass = true;
            this._bottomPanel.IsSunk = false;
            this._bottomPanel.Location = new System.Drawing.Point(0, 302);
            this._bottomPanel.Name = "bottomPanel";
            this._bottomPanel.Size = new System.Drawing.Size(261, 40);
            this._bottomPanel.TabIndex = 15;
            this._bottomPanel.UseStyles = true;
            // 
            // TerrariumForm
            // 
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(261, 342);
            this.Controls.Add(this._bottomPanel);
            this.Controls.Add(this._dialogDescriptionLabel);
            this.Controls.Add(this._titleBar);
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TerrariumForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Terrarium";
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Represents the Title text displayed at the top of the dialog
		/// </summary>
		public string Title
		{
			get
			{
                return _titleBar.Text;
			}
			set
			{
                _titleBar.Text = value;
			}
		}

		/// <summary>
		/// Represents the brief description text displayed at the top of the dialog
		/// </summary>
		public string Description
		{
			get
			{
                return _dialogDescriptionLabel.Text;
			}
			set
			{
                _dialogDescriptionLabel.Text = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public GlassTitleBar TitleBar
		{
			get
			{
                return _titleBar;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public GlassPanel BottomBar
		{
			get
			{
                return _bottomPanel;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
            GlassHelper.DrawBorder( this.ClientRectangle, GlassBorders.All, e.Graphics );
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _dialogDescriptionLabel.Width = this.Width - 24;
        }
	}
}