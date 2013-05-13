//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using OrganismBase;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using Terrarium.Configuration;
using Terrarium.Forms;
using Terrarium.Game;
using Terrarium.Glass;
using Terrarium.Services.Species;

namespace Terrarium.Client 
{
    internal class ReintroduceSpecies : TerrariumForm
    {
        // For use in caching
        private static DataSet          _speciesDataSet     = null;
        private static DateTime         _cacheDate          = DateTime.Now;
        
        private static string           _refreshExplanation = "To introduce creatures from the Server please download the Server List.  You can only download a new list once every 30 minutes.\n\n";
        
        private DataGrid                _speciesDataGrid;
        private DataGridTableStyle      _dataGridTableStyle1;
        private DataGridTextBoxColumn   _dataGridTextBoxColumn1;
        private DataGridTextBoxColumn   _dataGridTextBoxColumn2;
        private DataGridTextBoxColumn   _dataGridTextBoxColumn3;
        private DataGridTextBoxColumn   _dataGridTextBoxColumn4;
        private DataGridTextBoxColumn   _dataGridTextBoxColumn5;

        private Container               _components         = null;
        private bool                    _reintroduce;
        private OpenFileDialog          _openFileDialog1;
        private WebClientAsyncResult    _pendingAsyncResult;
        private SpeciesService          _service;
        private GlassLabel              _retrievingData;
        private GlassLabel              _refreshTime;
        private GlassButton             _okButton;
        private GlassButton             _cancelButton;
        private GlassButton             _serverListButton;
        private GlassButton             _browseButton;
        private bool                    _connectionCancelled = false;

        internal ReintroduceSpecies(bool reintroduce)
        {
            _reintroduce = reintroduce;

            _openFileDialog1            = new OpenFileDialog();
            _openFileDialog1.Filter     = ".NET Terrarium Assemblies (*.dll)|*.dll|All Files (*.*)|*.*";
            _openFileDialog1.Title      = "Choose the Assembly where your animal is located";
            _openFileDialog1.DefaultExt = ".dll";

            InitializeComponent();

            _titleBar.ShowMaximizeButton = false;
            _titleBar.ShowMinimizeButton = false;

            this.Description            = _refreshExplanation;

            _service = new SpeciesService
            {
                Timeout = 10000,
                Url = GameConfig.WebRoot + "/Species/AddSpecies.asmx"
            };

            if (_speciesDataSet == null)
            {
                _serverListButton.Enabled = true;
                _refreshTime.Text = "";
            }
            else
            {
                _serverListButton.Enabled = false;
                
                if (_cacheDate > DateTime.Now.AddMinutes(-30))
                {
                    _serverListButton.Enabled = false;
                    _refreshTime.ForeColor = Color.Green;
                }
                else
                {
                    _serverListButton.Enabled = true;
                    _refreshTime.ForeColor = Color.Yellow;
                }
                
                _refreshTime.Text = "Cached for: " + ((int) (DateTime.Now - _cacheDate).TotalMinutes).ToString() + " mins";
            }

            if (reintroduce)
            {
                _browseButton.Visible = false;
            }
        }

        protected override void Dispose(bool dispose)
        {
            if (dispose)
            {
                if (_components != null)
                {
                    _components.Dispose();
                }
            }
            base.Dispose(dispose);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this._speciesDataGrid = new System.Windows.Forms.DataGrid();
            this._dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this._dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this._dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this._dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            this._dataGridTextBoxColumn4 = new System.Windows.Forms.DataGridTextBoxColumn();
            this._dataGridTextBoxColumn5 = new System.Windows.Forms.DataGridTextBoxColumn();
            this._retrievingData = new Terrarium.Glass.GlassLabel();
            this._refreshTime = new Terrarium.Glass.GlassLabel();
            this._okButton = new Terrarium.Glass.GlassButton();
            this._cancelButton = new Terrarium.Glass.GlassButton();
            this._serverListButton = new Terrarium.Glass.GlassButton();
            this._browseButton = new Terrarium.Glass.GlassButton();
            this._bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._speciesDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // titleBar
            // 
            this._titleBar.Size = new System.Drawing.Size(456, 32);
            this._titleBar.Title = "Introduce Creature";
            this._titleBar.CloseClicked += new System.EventHandler(this.Cancel_Click);
            // 
            // bottomPanel
            // 
            this._bottomPanel.Controls.Add(this._browseButton);
            this._bottomPanel.Controls.Add(this._serverListButton);
            this._bottomPanel.Controls.Add(this._cancelButton);
            this._bottomPanel.Controls.Add(this._okButton);
            this._bottomPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._bottomPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this._bottomPanel.Location = new System.Drawing.Point(0, 352);
            this._bottomPanel.Size = new System.Drawing.Size(456, 40);
            // 
            // dataGrid1
            // 
            this._speciesDataGrid.AlternatingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._speciesDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._speciesDataGrid.BackgroundColor = System.Drawing.Color.Gray;
            this._speciesDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._speciesDataGrid.CaptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._speciesDataGrid.CaptionForeColor = System.Drawing.Color.White;
            this._speciesDataGrid.CaptionVisible = false;
            this._speciesDataGrid.DataMember = "";
            this._speciesDataGrid.FlatMode = true;
            this._speciesDataGrid.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._speciesDataGrid.ForeColor = System.Drawing.Color.Black;
            this._speciesDataGrid.GridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._speciesDataGrid.HeaderBackColor = System.Drawing.Color.Gray;
            this._speciesDataGrid.HeaderForeColor = System.Drawing.Color.White;
            this._speciesDataGrid.Location = new System.Drawing.Point(8, 71);
            this._speciesDataGrid.Name = "dataGrid1";
            this._speciesDataGrid.ReadOnly = true;
            this._speciesDataGrid.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._speciesDataGrid.Size = new System.Drawing.Size(440, 272);
            this._speciesDataGrid.TabIndex = 8;
            this._speciesDataGrid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this._dataGridTableStyle1});
            // 
            // dataGridTableStyle1
            // 
            this._dataGridTableStyle1.AlternatingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this._dataGridTableStyle1.BackColor = System.Drawing.Color.White;
            this._dataGridTableStyle1.DataGrid = this._speciesDataGrid;
            this._dataGridTableStyle1.ForeColor = System.Drawing.Color.Black;
            this._dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this._dataGridTextBoxColumn1,
            this._dataGridTextBoxColumn2,
            this._dataGridTextBoxColumn3,
            this._dataGridTextBoxColumn4,
            this._dataGridTextBoxColumn5});
            this._dataGridTableStyle1.HeaderBackColor = System.Drawing.Color.Gray;
            this._dataGridTableStyle1.HeaderForeColor = System.Drawing.Color.White;
            this._dataGridTableStyle1.MappingName = "Table";
            // 
            // dataGridTextBoxColumn1
            // 
            this._dataGridTextBoxColumn1.Format = "";
            this._dataGridTextBoxColumn1.FormatInfo = null;
            this._dataGridTextBoxColumn1.HeaderText = "Species Name";
            this._dataGridTextBoxColumn1.MappingName = "Name";
            this._dataGridTextBoxColumn1.ReadOnly = true;
            this._dataGridTextBoxColumn1.Width = 150;
            // 
            // dataGridTextBoxColumn2
            // 
            this._dataGridTextBoxColumn2.Format = "";
            this._dataGridTextBoxColumn2.FormatInfo = null;
            this._dataGridTextBoxColumn2.HeaderText = "Type";
            this._dataGridTextBoxColumn2.MappingName = "Type";
            this._dataGridTextBoxColumn2.ReadOnly = true;
            this._dataGridTextBoxColumn2.Width = 75;
            // 
            // dataGridTextBoxColumn3
            // 
            this._dataGridTextBoxColumn3.Format = "";
            this._dataGridTextBoxColumn3.FormatInfo = null;
            this._dataGridTextBoxColumn3.HeaderText = "Author";
            this._dataGridTextBoxColumn3.MappingName = "Author";
            this._dataGridTextBoxColumn3.ReadOnly = true;
            this._dataGridTextBoxColumn3.Width = 75;
            // 
            // dataGridTextBoxColumn4
            // 
            this._dataGridTextBoxColumn4.Format = "d";
            this._dataGridTextBoxColumn4.FormatInfo = null;
            this._dataGridTextBoxColumn4.HeaderText = "Date Introduced";
            this._dataGridTextBoxColumn4.MappingName = "DateAdded";
            this._dataGridTextBoxColumn4.ReadOnly = true;
            this._dataGridTextBoxColumn4.Width = 90;
            // 
            // dataGridTextBoxColumn5
            // 
            this._dataGridTextBoxColumn5.Format = "d";
            this._dataGridTextBoxColumn5.FormatInfo = null;
            this._dataGridTextBoxColumn5.HeaderText = "Last Reintroduction";
            this._dataGridTextBoxColumn5.MappingName = "LastReintroduction";
            this._dataGridTextBoxColumn5.ReadOnly = true;
            this._dataGridTextBoxColumn5.Width = 125;
            // 
            // retrievingData
            // 
            this._retrievingData.BackColor = System.Drawing.Color.Transparent;
            this._retrievingData.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._retrievingData.ForeColor = System.Drawing.Color.White;
            this._retrievingData.Location = new System.Drawing.Point(72, 195);
            this._retrievingData.Name = "retrievingData";
            this._retrievingData.NoWrap = false;
            this._retrievingData.Size = new System.Drawing.Size(321, 19);
            this._retrievingData.TabIndex = 5;
            this._retrievingData.Text = "retrieving Species List From Server...";
            this._retrievingData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._retrievingData.Visible = false;
            // 
            // RefreshTime
            // 
            this._refreshTime.BackColor = System.Drawing.Color.Transparent;
            this._refreshTime.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._refreshTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._refreshTime.Location = new System.Drawing.Point(456, 152);
            this._refreshTime.Name = "RefreshTime";
            this._refreshTime.NoWrap = false;
            this._refreshTime.Size = new System.Drawing.Size(88, 32);
            this._refreshTime.TabIndex = 10;
            this._refreshTime.Text = "label1";
            this._refreshTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // okButton
            // 
            this._okButton.BackColor = System.Drawing.Color.Transparent;
            this._okButton.BorderColor = System.Drawing.Color.Black;
            this._okButton.Depth = 3;
            this._okButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._okButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._okButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._okButton.ForeColor = System.Drawing.Color.White;
            this._okButton.Highlight = false;
            this._okButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this._okButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this._okButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this._okButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this._okButton.IsGlass = true;
            this._okButton.Location = new System.Drawing.Point(288, 2);
            this._okButton.Name = "okButton";
            this._okButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._okButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this._okButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this._okButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this._okButton.Size = new System.Drawing.Size(75, 36);
            this._okButton.TabIndex = 16;
            this._okButton.TabStop = false;
            this._okButton.Text = "OK";
            this._okButton.UseStyles = true;
            this._okButton.UseVisualStyleBackColor = false;
            this._okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // cancelButton
            // 
            this._cancelButton.BackColor = System.Drawing.Color.Transparent;
            this._cancelButton.BorderColor = System.Drawing.Color.Black;
            this._cancelButton.Depth = 3;
            this._cancelButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._cancelButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._cancelButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._cancelButton.ForeColor = System.Drawing.Color.White;
            this._cancelButton.Highlight = false;
            this._cancelButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this._cancelButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this._cancelButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this._cancelButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this._cancelButton.IsGlass = true;
            this._cancelButton.Location = new System.Drawing.Point(369, 2);
            this._cancelButton.Name = "cancelButton";
            this._cancelButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._cancelButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this._cancelButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this._cancelButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this._cancelButton.Size = new System.Drawing.Size(75, 36);
            this._cancelButton.TabIndex = 17;
            this._cancelButton.TabStop = false;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseStyles = true;
            this._cancelButton.UseVisualStyleBackColor = false;
            this._cancelButton.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // serverListButton
            // 
            this._serverListButton.BackColor = System.Drawing.Color.Transparent;
            this._serverListButton.BorderColor = System.Drawing.Color.Black;
            this._serverListButton.Depth = 3;
            this._serverListButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._serverListButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._serverListButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._serverListButton.ForeColor = System.Drawing.Color.White;
            this._serverListButton.Highlight = false;
            this._serverListButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this._serverListButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this._serverListButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this._serverListButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this._serverListButton.IsGlass = true;
            this._serverListButton.Location = new System.Drawing.Point(12, 2);
            this._serverListButton.Name = "serverListButton";
            this._serverListButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._serverListButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this._serverListButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this._serverListButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this._serverListButton.Size = new System.Drawing.Size(100, 36);
            this._serverListButton.TabIndex = 18;
            this._serverListButton.TabStop = false;
            this._serverListButton.Text = "Server List";
            this._serverListButton.UseStyles = true;
            this._serverListButton.UseVisualStyleBackColor = false;
            this._serverListButton.Click += new System.EventHandler(this.ServerList_Click);
            // 
            // browseButton
            // 
            this._browseButton.BackColor = System.Drawing.Color.Transparent;
            this._browseButton.BorderColor = System.Drawing.Color.Black;
            this._browseButton.Depth = 3;
            this._browseButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._browseButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._browseButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._browseButton.ForeColor = System.Drawing.Color.White;
            this._browseButton.Highlight = false;
            this._browseButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this._browseButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this._browseButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this._browseButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this._browseButton.IsGlass = true;
            this._browseButton.Location = new System.Drawing.Point(118, 2);
            this._browseButton.Name = "browseButton";
            this._browseButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._browseButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this._browseButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this._browseButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this._browseButton.Size = new System.Drawing.Size(80, 36);
            this._browseButton.TabIndex = 0;
            this._browseButton.TabStop = false;
            this._browseButton.Text = "Browse...";
            this._browseButton.UseStyles = true;
            this._browseButton.UseVisualStyleBackColor = false;
            this._browseButton.Click += new System.EventHandler(this.Browse_Click);
            // 
            // ReintroduceSpecies
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(456, 392);
            this.Controls.Add(this._refreshTime);
            this.Controls.Add(this._retrievingData);
            this.Controls.Add(this._speciesDataGrid);
            this.Name = "ReintroduceSpecies";
            this.Title = "Reintroduce Species";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ReintroduceSpecies_Paint);
            this.Controls.SetChildIndex(this._bottomPanel, 0);
            this.Controls.SetChildIndex(this._titleBar, 0);
            this.Controls.SetChildIndex(this._speciesDataGrid, 0);
            this.Controls.SetChildIndex(this._retrievingData, 0);
            this.Controls.SetChildIndex(this._refreshTime, 0);
            this._bottomPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._speciesDataGrid)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void OK_Click(object sender, System.EventArgs e)
        {
            if (_pendingAsyncResult != null)
            {
                _connectionCancelled = true;
                _pendingAsyncResult.Abort();
                _pendingAsyncResult = null;
            }

            if (GameEngine.Current == null || _speciesDataGrid.DataSource == null ||
                this.BindingContext[_speciesDataGrid.DataSource, "Table"] == null ||
                this.BindingContext[_speciesDataGrid.DataSource, "Table"].Count == 0)
            {
                this.Hide();
                return;
            }

            DataRowView     drv     = this.BindingContext[_speciesDataGrid.DataSource, "Table"].Current as DataRowView;
            
            byte[] speciesAssemblyBytes = null;

            try
            {
                string versionString    = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string dataRowName      = (string)drv["Name"];

                if (_reintroduce)
                {
                    speciesAssemblyBytes = _service.ReintroduceSpecies(dataRowName, versionString, GameEngine.Current.CurrentVector.State.StateGuid);
                }
                else
                {
                    speciesAssemblyBytes = _service.GetSpeciesAssembly(dataRowName, versionString);
                }
            }
            catch (WebException)
            {
                MessageBox.Show(this, "The connection to the server timed out.  Please try again later.");
            }

            if (speciesAssemblyBytes == null)
            {
                MessageBox.Show("Error retrieving species from server.");
            }
            else
            {
                // Save it to a temp file
                string tempFile = PrivateAssemblyCache.GetSafeTempFileName();
                try
                {
                    _speciesDataSet.Tables["Table"].Rows.Remove(drv.Row);

                    using (Stream fileStream = File.OpenWrite(tempFile))
                    {
                        fileStream.Write(speciesAssemblyBytes, 0, (int)speciesAssemblyBytes.Length);
                    }

                    GameEngine.Current.AddNewOrganism(tempFile, Point.Empty, _reintroduce);
                }
                catch (TargetInvocationException exception)
                {
                    Exception innerException = exception;
                    while (innerException.InnerException != null)
                    {
                        innerException = innerException.InnerException;
                    }

                    MessageBox.Show(innerException.Message, "Error Loading Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (GameEngineException exception)
                {
                    MessageBox.Show(exception.Message, "Error Loading Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (Exception exception) {
                    MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                finally
                {
                    if (File.Exists(tempFile))
                        File.Delete(tempFile);
                }

                this.Hide();
            }
        }

        private void Cancel_Click(object sender, System.EventArgs e)
        {
            if (_pendingAsyncResult != null)
            {
                _connectionCancelled = true;
                _pendingAsyncResult.Abort();
                _pendingAsyncResult = null;
            }

            this.Hide();
        }

        private void ServerList_Click(object sender, EventArgs e)
        {
            _retrievingData.Visible = true;

            try
            {
                String assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                if (_reintroduce)
                {
                    _pendingAsyncResult = (WebClientAsyncResult)_service.BeginGetExtinctSpecies(assemblyVersion, "", new AsyncCallback(ExtinctSpeciesCallback), null);
                }
                else
                {
                    _pendingAsyncResult = (WebClientAsyncResult)_service.BeginGetAllSpecies(assemblyVersion, "", new AsyncCallback(AllSpeciesCallback), null);
                }
            }
            catch (WebException)
            {
                MessageBox.Show(this, "The connection to the server timed out.  Please try again later.");
            }
        }

        private void ExtinctSpeciesCallback(IAsyncResult asyncResult)
        {
            try
            {
                _pendingAsyncResult     = null;
                _speciesDataSet         = _service.EndGetExtinctSpecies(asyncResult);
                _cacheDate              = DateTime.Now;
                _retrievingData.Visible = false;
            }
            catch
            {
                if (!_connectionCancelled)
                {
                    _retrievingData.Text = "There was a problem getting species list from server.";
                }
                return;
            }

            this.Invalidate();
        }

        private void AllSpeciesCallback(IAsyncResult asyncResult)
        {
            try
            {
                _pendingAsyncResult = null;
                _speciesDataSet = _service.EndGetAllSpecies(asyncResult);
                _cacheDate = DateTime.Now;
                _retrievingData.Visible = false;
            }
            catch
            {
                if (!_connectionCancelled)
                {
                    _retrievingData.Text = "There was a problem getting species list from server.";
                }
                return;
            }

            this.Invalidate();
        }

        private void Browse_Click(object sender, System.EventArgs e)
        {
            String assemblyName;

            if (_openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                assemblyName = _openFileDialog1.FileName;
                Species newSpecies = null;
                try
                {
                    if (GameEngine.Current != null)
                    {
                        newSpecies = GameEngine.Current.AddNewOrganism(assemblyName, Point.Empty, false);
                        if (newSpecies != null)
                        {
                            string warnings = newSpecies.GetAttributeWarnings();
                            if (warnings.Length != 0)
                            {
                                MessageBox.Show("Your organism was introduced, but there were some warnings:\r\n" + warnings, "Organism Assembly Warnings");
                            }
                        }
                    }
                }
                catch (TargetInvocationException exception)
                {
                    Exception innerException = exception;
                    while (innerException.InnerException != null)
                    {
                        innerException = innerException.InnerException;
                    }

                    MessageBox.Show(innerException.Message, "Error Loading Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (GameEngineException exception)
                {
                    MessageBox.Show(exception.Message, "Error Loading Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (IOException exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                this.Hide();
            }
        }

        private void ReintroduceSpecies_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (_speciesDataSet != null)
            {
                _speciesDataSet.Tables[0].DefaultView.AllowNew = false;
                _speciesDataSet.Tables[0].DefaultView.AllowEdit = false;
                _speciesDataGrid.DataSource = _speciesDataSet;
                _speciesDataGrid.DataMember = "Table";
            }
        }
    }
}