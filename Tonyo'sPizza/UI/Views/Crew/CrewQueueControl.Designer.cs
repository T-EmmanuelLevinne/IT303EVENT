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

            // topHeader
            this.topHeader.Controls.Add(this.row3);
            this.topHeader.Controls.Add(this._pnlTabs);
            this.topHeader.Controls.Add(this.row1);
            this.topHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.topHeader.Height = 140;
            this.topHeader.Name = "topHeader";

            // row1
            this.row1.Controls.Add(this.pnlRightRow1);
            this.row1.Controls.Add(this.lblSub);
            this.row1.Controls.Add(this.lblTitle);
            this.row1.Dock = System.Windows.Forms.DockStyle.Top;
            this.row1.Height = 52;
            this.row1.Name = "row1";

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Kitchen Order Queue";

            // lblSub
            this.lblSub.AutoSize = true;
            this.lblSub.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.lblSub.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this.lblSub.Location = new System.Drawing.Point(2, 30);
            this.lblSub.Name = "lblSub";
            this.lblSub.Text = "Live order pipeline, preparation controls, receipt slips, and 30-day archive";

            // pnlRightRow1
            this.pnlRightRow1.Controls.Add(this.pnlSearch);
            this.pnlRightRow1.Controls.Add(this._btnRefresh);
            this.pnlRightRow1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRightRow1.Name = "pnlRightRow1";
            this.pnlRightRow1.Size = new System.Drawing.Size(420, 52);

            // pnlSearch
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.BorderColor = Tonyo_sPizza.UI.Theme.AppColors.Border;
            this.pnlSearch.BorderRadius = 8;
            this.pnlSearch.BorderWidth = 1;
            this.pnlSearch.Controls.Add(this._txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearchIcon);
            this.pnlSearch.DrawBorder = true;
            this.pnlSearch.Location = new System.Drawing.Point(4, 6);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(256, 36);

            // lblSearchIcon
            this.lblSearchIcon.Location = new System.Drawing.Point(10, 10);
            this.lblSearchIcon.Name = "lblSearchIcon";
            this.lblSearchIcon.Size = new System.Drawing.Size(16, 16);

            // _txtSearch
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtSearch.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._txtSearch.Location = new System.Drawing.Point(32, 9);
            this._txtSearch.Name = "_txtSearch";
            this._txtSearch.Size = new System.Drawing.Size(214, 17);

            // _btnRefresh
            this._btnRefresh.BorderRadius = 8;
            this._btnRefresh.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._btnRefresh.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnRefresh.IconName = "refresh";
            this._btnRefresh.Location = new System.Drawing.Point(270, 6);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnRefresh.PressedColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryPressed;
            this._btnRefresh.Size = new System.Drawing.Size(145, 36);
            this._btnRefresh.Text = "Refresh Queue";

            // _pnlTabs
            this._pnlTabs.AutoScroll = true;
            this._pnlTabs.Controls.Add(this._btnFilterAll);
            this._pnlTabs.Controls.Add(this._btnFilterPending);
            this._pnlTabs.Controls.Add(this._btnFilterPreparing);
            this._pnlTabs.Controls.Add(this._btnFilterReady);
            this._pnlTabs.Controls.Add(this._btnFilterCompleted);
            this._pnlTabs.Controls.Add(this._btnFilterCancelled);
            this._pnlTabs.Controls.Add(this._btnFilterArchived);
            this._pnlTabs.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlTabs.Height = 44;
            this._pnlTabs.Name = "_pnlTabs";
            this._pnlTabs.Padding = new System.Windows.Forms.Padding(0, 6, 0, 4);
            this._pnlTabs.WrapContents = false;

            ConfigureFilterTab(this._btnFilterAll, "_btnFilterAll", "All Orders", 96);
            ConfigureFilterTab(this._btnFilterPending, "_btnFilterPending", "Pending", 90);
            ConfigureFilterTab(this._btnFilterPreparing, "_btnFilterPreparing", "Preparing", 96);
            ConfigureFilterTab(this._btnFilterReady, "_btnFilterReady", "Ready to Serve", 128);
            ConfigureFilterTab(this._btnFilterCompleted, "_btnFilterCompleted", "Completed", 104);
            ConfigureFilterTab(this._btnFilterCancelled, "_btnFilterCancelled", "Cancelled", 100);
            ConfigureFilterTab(this._btnFilterArchived, "_btnFilterArchived", "Archived (30d)", 128);

            // row3
            this.row3.Controls.Add(this._btnCancelSelected);
            this.row3.Controls.Add(this._btnArchiveSelected);
            this.row3.Controls.Add(this._lblSelectedCount);
            this.row3.Controls.Add(this._chkSelectAll);
            this.row3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.row3.Height = 38;
            this.row3.Name = "row3";

            // _chkSelectAll
            this._chkSelectAll.AutoSize = true;
            this._chkSelectAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this._chkSelectAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._chkSelectAll.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._chkSelectAll.Location = new System.Drawing.Point(2, 9);
            this._chkSelectAll.Name = "_chkSelectAll";
            this._chkSelectAll.Text = "Select All Orders";
            this._chkSelectAll.UseVisualStyleBackColor = true;

            // _lblSelectedCount
            this._lblSelectedCount.AutoSize = true;
            this._lblSelectedCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblSelectedCount.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this._lblSelectedCount.Location = new System.Drawing.Point(138, 10);
            this._lblSelectedCount.Name = "_lblSelectedCount";
            this._lblSelectedCount.Text = "0 selected";

            // _btnArchiveSelected
            this._btnArchiveSelected.BorderRadius = 7;
            this._btnArchiveSelected.Enabled = false;
            this._btnArchiveSelected.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this._btnArchiveSelected.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._btnArchiveSelected.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnArchiveSelected.IconName = "archive";
            this._btnArchiveSelected.Location = new System.Drawing.Point(232, 2);
            this._btnArchiveSelected.Name = "_btnArchiveSelected";
            this._btnArchiveSelected.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnArchiveSelected.PressedColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryPressed;
            this._btnArchiveSelected.Size = new System.Drawing.Size(148, 32);
            this._btnArchiveSelected.Text = "Archive Selected";

            // _btnCancelSelected
            this._btnCancelSelected.BorderRadius = 7;
            this._btnCancelSelected.Enabled = false;
            this._btnCancelSelected.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this._btnCancelSelected.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCancelled;
            this._btnCancelSelected.HoverColor = System.Drawing.Color.FromArgb(254, 202, 202);
            this._btnCancelSelected.IconName = "delete";
            this._btnCancelSelected.Location = new System.Drawing.Point(390, 2);
            this._btnCancelSelected.Name = "_btnCancelSelected";
            this._btnCancelSelected.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCancelledBg;
            this._btnCancelSelected.PressedColor = System.Drawing.Color.FromArgb(252, 165, 165);
            this._btnCancelSelected.Size = new System.Drawing.Size(140, 32);
            this._btnCancelSelected.Text = "Cancel Selected";

            // _pnlArchiveNotice
            this._pnlArchiveNotice.BackColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPendingBg;
            this._pnlArchiveNotice.Controls.Add(this.lblArchiveNotice);
            this._pnlArchiveNotice.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlArchiveNotice.Height = 32;
            this._pnlArchiveNotice.Name = "_pnlArchiveNotice";
            this._pnlArchiveNotice.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this._pnlArchiveNotice.Visible = false;

            // lblArchiveNotice
            this.lblArchiveNotice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblArchiveNotice.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblArchiveNotice.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPending;
            this.lblArchiveNotice.Name = "lblArchiveNotice";
            this.lblArchiveNotice.Text = "Orders moved to archive stay visible for 30 days before automatic pruning.";
            this.lblArchiveNotice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // _flpOrders
            this._flpOrders.AutoScroll = true;
            this._flpOrders.BackColor = Tonyo_sPizza.UI.Theme.AppColors.Background;
            this._flpOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this._flpOrders.Name = "_flpOrders";
            this._flpOrders.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);

            // CrewQueueControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Tonyo_sPizza.UI.Theme.AppColors.Background;
            this.Controls.Add(this._flpOrders);
            this.Controls.Add(this._pnlArchiveNotice);
            this.Controls.Add(this.topHeader);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
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

        private void ConfigureFilterTab(Tonyo_sPizza.UI.Controls.RoundedButton btn, string name, string text, int width)
        {
            btn.BorderRadius = 7;
            btn.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            btn.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            btn.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            btn.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            btn.Name = name;
            btn.NormalColor = System.Drawing.Color.White;
            btn.PressedColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryPressed;
            btn.Size = new System.Drawing.Size(width, 32);
            btn.Text = text;
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
