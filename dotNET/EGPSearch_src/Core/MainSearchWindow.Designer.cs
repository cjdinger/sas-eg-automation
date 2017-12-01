namespace EGPSearch
{
    partial class MainSearchWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainSearchWindow));
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtSearchFor = new System.Windows.Forms.TextBox();
            this.txtFilespec = new System.Windows.Forms.TextBox();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSearchResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutProjectSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lvMatches = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chProcessFlow = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chProjectName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFolder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chMatch = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openProjectFile = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.selectProjectFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.menu.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 486);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(624, 22);
            this.statusBar.TabIndex = 1;
            this.statusBar.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.btnGo);
            this.panelSearch.Controls.Add(this.txtSearchFor);
            this.panelSearch.Controls.Add(this.txtFilespec);
            this.panelSearch.Controls.Add(this.menu);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(624, 90);
            this.panelSearch.TabIndex = 0;
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(569, 53);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(51, 23);
            this.btnGo.TabIndex = 6;
            this.btnGo.Text = "Go!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtSearchFor
            // 
            this.txtSearchFor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchFor.Location = new System.Drawing.Point(441, 27);
            this.txtSearchFor.Name = "txtSearchFor";
            this.txtSearchFor.Size = new System.Drawing.Size(179, 20);
            this.txtSearchFor.TabIndex = 1;
            // 
            // txtFilespec
            // 
            this.txtFilespec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilespec.Location = new System.Drawing.Point(3, 27);
            this.txtFilespec.Name = "txtFilespec";
            this.txtFilespec.Size = new System.Drawing.Size(431, 20);
            this.txtFilespec.TabIndex = 0;
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(624, 24);
            this.menu.TabIndex = 7;
            this.menu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectProjectFileToolStripMenuItem,
            this.saveSearchResultsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveSearchResultsToolStripMenuItem
            // 
            this.saveSearchResultsToolStripMenuItem.Name = "saveSearchResultsToolStripMenuItem";
            this.saveSearchResultsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.saveSearchResultsToolStripMenuItem.Text = "Save search results...";
            this.saveSearchResultsToolStripMenuItem.Click += new System.EventHandler(this.saveSearchResultsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutProjectSearchToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutProjectSearchToolStripMenuItem
            // 
            this.aboutProjectSearchToolStripMenuItem.Name = "aboutProjectSearchToolStripMenuItem";
            this.aboutProjectSearchToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.aboutProjectSearchToolStripMenuItem.Text = "About Project Search";
            this.aboutProjectSearchToolStripMenuItem.Click += new System.EventHandler(this.aboutProjectSearchToolStripMenuItem_Click);
            // 
            // lvMatches
            // 
            this.lvMatches.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chType,
            this.chProcessFlow,
            this.chLocation,
            this.chProjectName,
            this.chFolder,
            this.chMatch});
            this.lvMatches.ContextMenuStrip = this.contextMenu;
            this.lvMatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMatches.FullRowSelect = true;
            this.lvMatches.Location = new System.Drawing.Point(0, 0);
            this.lvMatches.MultiSelect = false;
            this.lvMatches.Name = "lvMatches";
            this.lvMatches.Size = new System.Drawing.Size(624, 320);
            this.lvMatches.TabIndex = 0;
            this.lvMatches.UseCompatibleStateImageBehavior = false;
            this.lvMatches.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 88;
            // 
            // chType
            // 
            this.chType.Text = "Type";
            this.chType.Width = 82;
            // 
            // chProcessFlow
            // 
            this.chProcessFlow.Text = "Flow";
            this.chProcessFlow.Width = 79;
            // 
            // chLocation
            // 
            this.chLocation.Text = "Location";
            this.chLocation.Width = 90;
            // 
            // chProjectName
            // 
            this.chProjectName.Text = "Project";
            this.chProjectName.Width = 79;
            // 
            // chFolder
            // 
            this.chFolder.Text = "Folder";
            this.chFolder.Width = 103;
            // 
            // chMatch
            // 
            this.chMatch.Text = "Match";
            // 
            // contextMenu
            // 
            this.contextMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openProjectFile});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(301, 26);
            this.contextMenu.Opened += new System.EventHandler(this.onContextMenuOpened);
            // 
            // openProjectFile
            // 
            this.openProjectFile.Name = "openProjectFile";
            this.openProjectFile.Size = new System.Drawing.Size(300, 22);
            this.openProjectFile.Text = "Open project file with SAS Enterprise Guide";
            this.openProjectFile.Click += new System.EventHandler(this.onOpenProjectFile);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 90);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.lvMatches);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.txtMessages);
            this.splitContainer.Size = new System.Drawing.Size(624, 396);
            this.splitContainer.SplitterDistance = 320;
            this.splitContainer.TabIndex = 3;
            // 
            // txtMessages
            // 
            this.txtMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessages.Location = new System.Drawing.Point(0, 0);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ReadOnly = true;
            this.txtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessages.Size = new System.Drawing.Size(624, 72);
            this.txtMessages.TabIndex = 0;
            // 
            // selectProjectFileToolStripMenuItem
            // 
            this.selectProjectFileToolStripMenuItem.Name = "selectProjectFileToolStripMenuItem";
            this.selectProjectFileToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.selectProjectFileToolStripMenuItem.Text = "Select project file...";
            this.selectProjectFileToolStripMenuItem.Click += new System.EventHandler(this.selectProjectFileToolStripMenuItem_Click);
            // 
            // MainSearchWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 508);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "MainSearchWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search SAS Enterprise Guide projects";
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtSearchFor;
        private System.Windows.Forms.TextBox txtFilespec;
        private System.Windows.Forms.ListView lvMatches;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.ColumnHeader chProcessFlow;
        private System.Windows.Forms.ColumnHeader chLocation;
        private System.Windows.Forms.ColumnHeader chProjectName;
        private System.Windows.Forms.ColumnHeader chFolder;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.ColumnHeader chMatch;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSearchResultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutProjectSearchToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem openProjectFile;
        private System.Windows.Forms.ToolStripMenuItem selectProjectFileToolStripMenuItem;
    }
}

