namespace Tonyo_sPizza.UI.Views.Crew
{
    partial class CrewQueueControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.topHeader = new System.Windows.Forms.Panel();
            this.row3 = new System.Windows.Forms.Panel();
            this._btnCancelSelected = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnArchiveSelected = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._lblSelectedCount = new System.Windows.Forms.Label();
            this._chkSelectAll = new System.Windows.Forms.CheckBox();
            this._pnlTabs = new System.Windows.Forms.FlowLayoutPanel();
            this._btnFilterAll = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnFilterPending = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnFilterPreparing = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnFilterReady = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnFilterCompleted = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnFilterCancelled = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnFilterArchived = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.row1 = new System.Windows.Forms.Panel();
            this.pnlRightRow1 = new System.Windows.Forms.Panel();
            this.pnlSearch = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this._btnRefresh = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.lblSub = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this._pnlArchiveNotice = new System.Windows.Forms.Panel();
            this.lblArchiveNotice = new System.Windows.Forms.Label();
            this._flpOrders = new System.Windows.Forms.FlowLayoutPanel();
            this.topHeader.SuspendLayout();
            this.row3.SuspendLayout();
            this._pnlTabs.SuspendLayout();
            this.row1.SuspendLayout();
            this.pnlRightRow1.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this._pnlArchiveNotice.SuspendLayout();
            this.SuspendLayout();
            // 
            // topHeader
            // 
            this.topHeader.Controls.Add(this.row3);
            this.topHeader.Controls.Add(this._pnlTabs);
            this.topHeader.Controls.Add(this.row1);
            this.topHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.topHeader.Location = new System.Drawing.Point(24, 18);
            this.topHeader.Name = "topHeader";
            this.topHeader.Size = new System.Drawing.Size(1477, 136);
            this.topHeader.TabIndex = 0;
            // 
            // row3
            // 
            this.row3.Controls.Add(this._btnCancelSelected);
            this.row3.Controls.Add(this._btnArchiveSelected);
            this.row3.Controls.Add(this._lblSelectedCount);
            this.row3.Controls.Add(this._chkSelectAll);
            this.row3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.row3.Location = new System.Drawing.Point(0, 100);
            this.row3.Name = "row3";
            this.row3.Size = new System.Drawing.Size(1477, 36);
            this.row3.TabIndex = 2;
            // 
            // _btnCancelSelected
            // 
            this._btnCancelSelected.BackColor = System.Drawing.Color.Transparent;
            this._btnCancelSelected.BorderRadius = 6;
            this._btnCancelSelected.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnCancelSelected.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnCancelSelected.CustomBorderWidth = 0;
            this._btnCancelSelected.Enabled = false;
            this._btnCancelSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnCancelSelected.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this._btnCancelSelected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this._btnCancelSelected.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this._btnCancelSelected.IconName = null;
            this._btnCancelSelected.Location = new System.Drawing.Point(400, 2);
            this._btnCancelSelected.Name = "_btnCancelSelected";
            this._btnCancelSelected.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this._btnCancelSelected.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnCancelSelected.Size = new System.Drawing.Size(140, 32);
            this._btnCancelSelected.TabIndex = 3;
            this._btnCancelSelected.Text = "Cancel Selected";
            this._btnCancelSelected.UseVisualStyleBackColor = false;
            // 
            // _btnArchiveSelected
            // 
            this._btnArchiveSelected.BackColor = System.Drawing.Color.Transparent;
            this._btnArchiveSelected.BorderRadius = 6;
            this._btnArchiveSelected.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnArchiveSelected.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnArchiveSelected.CustomBorderWidth = 0;
            this._btnArchiveSelected.Enabled = false;
            this._btnArchiveSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnArchiveSelected.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this._btnArchiveSelected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this._btnArchiveSelected.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this._btnArchiveSelected.IconName = "delete";
            this._btnArchiveSelected.Location = new System.Drawing.Point(240, 2);
            this._btnArchiveSelected.Name = "_btnArchiveSelected";
            this._btnArchiveSelected.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this._btnArchiveSelected.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnArchiveSelected.Size = new System.Drawing.Size(150, 32);
            this._btnArchiveSelected.TabIndex = 2;
            this._btnArchiveSelected.Text = "Archive Selected";
            this._btnArchiveSelected.UseVisualStyleBackColor = false;
            this._btnArchiveSelected.Click += new System.EventHandler(this._btnArchiveSelected_Click);
            // 
            // _lblSelectedCount
            // 
            this._lblSelectedCount.AutoSize = true;
            this._lblSelectedCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblSelectedCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this._lblSelectedCount.Location = new System.Drawing.Point(140, 9);
            this._lblSelectedCount.Name = "_lblSelectedCount";
            this._lblSelectedCount.Size = new System.Drawing.Size(59, 15);
            this._lblSelectedCount.TabIndex = 1;
            this._lblSelectedCount.Text = "0 selected";
            // 
            // _chkSelectAll
            // 
            this._chkSelectAll.AutoSize = true;
            this._chkSelectAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this._chkSelectAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._chkSelectAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this._chkSelectAll.Location = new System.Drawing.Point(2, 8);
            this._chkSelectAll.Name = "_chkSelectAll";
            this._chkSelectAll.Size = new System.Drawing.Size(113, 19);
            this._chkSelectAll.TabIndex = 0;
            this._chkSelectAll.Text = "Select All Orders";
            this._chkSelectAll.UseVisualStyleBackColor = true;
            // 
            // _pnlTabs
            // 
            this._pnlTabs.Controls.Add(this._btnFilterAll);
            this._pnlTabs.Controls.Add(this._btnFilterPending);
            this._pnlTabs.Controls.Add(this._btnFilterPreparing);
            this._pnlTabs.Controls.Add(this._btnFilterReady);
            this._pnlTabs.Controls.Add(this._btnFilterCompleted);
            this._pnlTabs.Controls.Add(this._btnFilterCancelled);
            this._pnlTabs.Controls.Add(this._btnFilterArchived);
            this._pnlTabs.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlTabs.Location = new System.Drawing.Point(0, 48);
            this._pnlTabs.Name = "_pnlTabs";
            this._pnlTabs.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this._pnlTabs.Size = new System.Drawing.Size(1477, 40);
            this._pnlTabs.TabIndex = 1;
            this._pnlTabs.WrapContents = false;
            // 
            // _btnFilterAll
            // 
            this._btnFilterAll.BackColor = System.Drawing.Color.Transparent;
            this._btnFilterAll.BorderRadius = 6;
            this._btnFilterAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnFilterAll.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnFilterAll.CustomBorderWidth = 0;
            this._btnFilterAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnFilterAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnFilterAll.ForeColor = System.Drawing.Color.White;
            this._btnFilterAll.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(63)))), ((int)(((byte)(94)))));
            this._btnFilterAll.IconName = null;
            this._btnFilterAll.Location = new System.Drawing.Point(0, 4);
            this._btnFilterAll.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnFilterAll.Name = "_btnFilterAll";
            this._btnFilterAll.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(24)))), ((int)(((byte)(93)))));
            this._btnFilterAll.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnFilterAll.Size = new System.Drawing.Size(95, 32);
            this._btnFilterAll.TabIndex = 0;
            this._btnFilterAll.Text = "All Orders";
            this._btnFilterAll.UseVisualStyleBackColor = false;
            // 
            // _btnFilterPending
            // 
            this._btnFilterPending.BackColor = System.Drawing.Color.Transparent;
            this._btnFilterPending.BorderRadius = 6;
            this._btnFilterPending.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnFilterPending.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnFilterPending.CustomBorderWidth = 0;
            this._btnFilterPending.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnFilterPending.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnFilterPending.ForeColor = System.Drawing.Color.White;
            this._btnFilterPending.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(63)))), ((int)(((byte)(94)))));
            this._btnFilterPending.IconName = null;
            this._btnFilterPending.Location = new System.Drawing.Point(103, 4);
            this._btnFilterPending.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnFilterPending.Name = "_btnFilterPending";
            this._btnFilterPending.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(24)))), ((int)(((byte)(93)))));
            this._btnFilterPending.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnFilterPending.Size = new System.Drawing.Size(95, 32);
            this._btnFilterPending.TabIndex = 1;
            this._btnFilterPending.Text = "Pending";
            this._btnFilterPending.UseVisualStyleBackColor = false;
            // 
            // _btnFilterPreparing
            // 
            this._btnFilterPreparing.BackColor = System.Drawing.Color.Transparent;
            this._btnFilterPreparing.BorderRadius = 6;
            this._btnFilterPreparing.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnFilterPreparing.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnFilterPreparing.CustomBorderWidth = 0;
            this._btnFilterPreparing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnFilterPreparing.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnFilterPreparing.ForeColor = System.Drawing.Color.White;
            this._btnFilterPreparing.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(63)))), ((int)(((byte)(94)))));
            this._btnFilterPreparing.IconName = null;
            this._btnFilterPreparing.Location = new System.Drawing.Point(206, 4);
            this._btnFilterPreparing.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnFilterPreparing.Name = "_btnFilterPreparing";
            this._btnFilterPreparing.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(24)))), ((int)(((byte)(93)))));
            this._btnFilterPreparing.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnFilterPreparing.Size = new System.Drawing.Size(95, 32);
            this._btnFilterPreparing.TabIndex = 2;
            this._btnFilterPreparing.Text = "Preparing";
            this._btnFilterPreparing.UseVisualStyleBackColor = false;
            // 
            // _btnFilterReady
            // 
            this._btnFilterReady.BackColor = System.Drawing.Color.Transparent;
            this._btnFilterReady.BorderRadius = 6;
            this._btnFilterReady.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnFilterReady.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnFilterReady.CustomBorderWidth = 0;
            this._btnFilterReady.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnFilterReady.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnFilterReady.ForeColor = System.Drawing.Color.White;
            this._btnFilterReady.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(63)))), ((int)(((byte)(94)))));
            this._btnFilterReady.IconName = null;
            this._btnFilterReady.Location = new System.Drawing.Point(309, 4);
            this._btnFilterReady.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnFilterReady.Name = "_btnFilterReady";
            this._btnFilterReady.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(24)))), ((int)(((byte)(93)))));
            this._btnFilterReady.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnFilterReady.Size = new System.Drawing.Size(130, 32);
            this._btnFilterReady.TabIndex = 3;
            this._btnFilterReady.Text = "Ready to Serve";
            this._btnFilterReady.UseVisualStyleBackColor = false;
            // 
            // _btnFilterCompleted
            // 
            this._btnFilterCompleted.BackColor = System.Drawing.Color.Transparent;
            this._btnFilterCompleted.BorderRadius = 6;
            this._btnFilterCompleted.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnFilterCompleted.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnFilterCompleted.CustomBorderWidth = 0;
            this._btnFilterCompleted.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnFilterCompleted.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnFilterCompleted.ForeColor = System.Drawing.Color.White;
            this._btnFilterCompleted.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(63)))), ((int)(((byte)(94)))));
            this._btnFilterCompleted.IconName = null;
            this._btnFilterCompleted.Location = new System.Drawing.Point(447, 4);
            this._btnFilterCompleted.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnFilterCompleted.Name = "_btnFilterCompleted";
            this._btnFilterCompleted.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(24)))), ((int)(((byte)(93)))));
            this._btnFilterCompleted.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnFilterCompleted.Size = new System.Drawing.Size(110, 32);
            this._btnFilterCompleted.TabIndex = 4;
            this._btnFilterCompleted.Text = "Completed";
            this._btnFilterCompleted.UseVisualStyleBackColor = false;
            // 
            // _btnFilterCancelled
            // 
            this._btnFilterCancelled.BackColor = System.Drawing.Color.Transparent;
            this._btnFilterCancelled.BorderRadius = 6;
            this._btnFilterCancelled.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnFilterCancelled.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnFilterCancelled.CustomBorderWidth = 0;
            this._btnFilterCancelled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnFilterCancelled.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnFilterCancelled.ForeColor = System.Drawing.Color.White;
            this._btnFilterCancelled.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(63)))), ((int)(((byte)(94)))));
            this._btnFilterCancelled.IconName = null;
            this._btnFilterCancelled.Location = new System.Drawing.Point(565, 4);
            this._btnFilterCancelled.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnFilterCancelled.Name = "_btnFilterCancelled";
            this._btnFilterCancelled.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(24)))), ((int)(((byte)(93)))));
            this._btnFilterCancelled.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnFilterCancelled.Size = new System.Drawing.Size(105, 32);
            this._btnFilterCancelled.TabIndex = 5;
            this._btnFilterCancelled.Text = "Cancelled";
            this._btnFilterCancelled.UseVisualStyleBackColor = false;
            // 
            // _btnFilterArchived
            // 
            this._btnFilterArchived.BackColor = System.Drawing.Color.Transparent;
            this._btnFilterArchived.BorderRadius = 6;
            this._btnFilterArchived.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnFilterArchived.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnFilterArchived.CustomBorderWidth = 0;
            this._btnFilterArchived.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnFilterArchived.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnFilterArchived.ForeColor = System.Drawing.Color.White;
            this._btnFilterArchived.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(63)))), ((int)(((byte)(94)))));
            this._btnFilterArchived.IconName = null;
            this._btnFilterArchived.Location = new System.Drawing.Point(678, 4);
            this._btnFilterArchived.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnFilterArchived.Name = "_btnFilterArchived";
            this._btnFilterArchived.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(24)))), ((int)(((byte)(93)))));
            this._btnFilterArchived.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnFilterArchived.Size = new System.Drawing.Size(155, 32);
            this._btnFilterArchived.TabIndex = 6;
            this._btnFilterArchived.Text = "Archived (30 Days)";
            this._btnFilterArchived.UseVisualStyleBackColor = false;
            // 
            // row1
            // 
            this.row1.Controls.Add(this.pnlRightRow1);
            this.row1.Controls.Add(this.lblSub);
            this.row1.Controls.Add(this.lblTitle);
            this.row1.Dock = System.Windows.Forms.DockStyle.Top;
            this.row1.Location = new System.Drawing.Point(0, 0);
            this.row1.Name = "row1";
            this.row1.Size = new System.Drawing.Size(1477, 48);
            this.row1.TabIndex = 0;
            // 
            // pnlRightRow1
            // 
            this.pnlRightRow1.Controls.Add(this.pnlSearch);
            this.pnlRightRow1.Controls.Add(this._btnRefresh);
            this.pnlRightRow1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRightRow1.Location = new System.Drawing.Point(1032, 0);
            this.pnlRightRow1.Name = "pnlRightRow1";
            this.pnlRightRow1.Size = new System.Drawing.Size(445, 48);
            this.pnlRightRow1.TabIndex = 2;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlSearch.BorderRadius = 8;
            this.pnlSearch.BorderWidth = 1;
            this.pnlSearch.Controls.Add(this._txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearchIcon);
            this.pnlSearch.DrawBorder = true;
            this.pnlSearch.GradientAngle = 90F;
            this.pnlSearch.GradientEndColor = System.Drawing.Color.Empty;
            this.pnlSearch.GradientStartColor = System.Drawing.Color.Empty;
            this.pnlSearch.Location = new System.Drawing.Point(10, 4);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(235, 36);
            this.pnlSearch.TabIndex = 0;
            // 
            // _txtSearch
            // 
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtSearch.Location = new System.Drawing.Point(32, 9);
            this._txtSearch.Name = "_txtSearch";
            this._txtSearch.Size = new System.Drawing.Size(195, 17);
            this._txtSearch.TabIndex = 1;
            // 
            // lblSearchIcon
            // 
            this.lblSearchIcon.Location = new System.Drawing.Point(8, 9);
            this.lblSearchIcon.Name = "lblSearchIcon";
            this.lblSearchIcon.Size = new System.Drawing.Size(18, 18);
            this.lblSearchIcon.TabIndex = 0;
            // 
            // _btnRefresh
            // 
            this._btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this._btnRefresh.BorderRadius = 8;
            this._btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnRefresh.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnRefresh.CustomBorderWidth = 0;
            this._btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnRefresh.ForeColor = System.Drawing.Color.White;
            this._btnRefresh.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this._btnRefresh.IconName = "refresh";
            this._btnRefresh.Location = new System.Drawing.Point(255, 4);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this._btnRefresh.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnRefresh.Size = new System.Drawing.Size(145, 36);
            this._btnRefresh.TabIndex = 1;
            this._btnRefresh.Text = "Refresh Queue";
            this._btnRefresh.UseVisualStyleBackColor = false;
            // 
            // lblSub
            // 
            this.lblSub.AutoSize = true;
            this.lblSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSub.Location = new System.Drawing.Point(0, 28);
            this.lblSub.Name = "lblSub";
            this.lblSub.Size = new System.Drawing.Size(341, 15);
            this.lblSub.TabIndex = 1;
            this.lblSub.Text = "Live order pipeline, preparation statuses & 30-day order archiving";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(211, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Kitchen Order Queue";
            // 
            // _pnlArchiveNotice
            // 
            this._pnlArchiveNotice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(243)))), ((int)(((byte)(199)))));
            this._pnlArchiveNotice.Controls.Add(this.lblArchiveNotice);
            this._pnlArchiveNotice.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlArchiveNotice.Location = new System.Drawing.Point(24, 154);
            this._pnlArchiveNotice.Name = "_pnlArchiveNotice";
            this._pnlArchiveNotice.Padding = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this._pnlArchiveNotice.Size = new System.Drawing.Size(1477, 32);
            this._pnlArchiveNotice.TabIndex = 1;
            this._pnlArchiveNotice.Visible = false;
            // 
            // lblArchiveNotice
            // 
            this.lblArchiveNotice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblArchiveNotice.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblArchiveNotice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(83)))), ((int)(((byte)(9)))));
            this.lblArchiveNotice.Location = new System.Drawing.Point(12, 6);
            this.lblArchiveNotice.Name = "lblArchiveNotice";
            this.lblArchiveNotice.Size = new System.Drawing.Size(1453, 20);
            this.lblArchiveNotice.TabIndex = 0;
            this.lblArchiveNotice.Text = "Orders in archive remain visible for 30 days before being automatically purged.";
            this.lblArchiveNotice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblArchiveNotice.Click += new System.EventHandler(this.lblArchiveNotice_Click);
            // 
            // _flpOrders
            // 
            this._flpOrders.AutoScroll = true;
            this._flpOrders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this._flpOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this._flpOrders.Location = new System.Drawing.Point(24, 186);
            this._flpOrders.Name = "_flpOrders";
            this._flpOrders.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this._flpOrders.Size = new System.Drawing.Size(1477, 505);
            this._flpOrders.TabIndex = 2;
            // 
            // CrewQueueControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.Controls.Add(this._flpOrders);
            this.Controls.Add(this._pnlArchiveNotice);
            this.Controls.Add(this.topHeader);
            this.Name = "CrewQueueControl";
            this.Padding = new System.Windows.Forms.Padding(24, 18, 24, 20);
            this.Size = new System.Drawing.Size(1525, 711);
            this.topHeader.ResumeLayout(false);
            this.row3.ResumeLayout(false);
            this.row3.PerformLayout();
            this._pnlTabs.ResumeLayout(false);
            this.row1.ResumeLayout(false);
            this.row1.PerformLayout();
            this.pnlRightRow1.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this._pnlArchiveNotice.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topHeader;
        private System.Windows.Forms.Panel row1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Panel pnlRightRow1;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRefresh;
        private System.Windows.Forms.FlowLayoutPanel _pnlTabs;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterAll;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterPending;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterPreparing;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterReady;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterCompleted;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterCancelled;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterArchived;
        private System.Windows.Forms.Panel row3;
        private System.Windows.Forms.CheckBox _chkSelectAll;
        private System.Windows.Forms.Label _lblSelectedCount;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnArchiveSelected;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnCancelSelected;
        private System.Windows.Forms.Panel _pnlArchiveNotice;
        private System.Windows.Forms.Label lblArchiveNotice;
        private System.Windows.Forms.FlowLayoutPanel _flpOrders;
    }
}
