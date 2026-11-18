namespace Tonyo_sPizza.UI.Views.Customer
{
    partial class CustomerOrdersControl
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
            this.header = new System.Windows.Forms.Panel();
            this.title = new System.Windows.Forms.Label();
            this._btnRefresh = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._flpOrders = new System.Windows.Forms.FlowLayoutPanel();
            this.header.SuspendLayout();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.Controls.Add(this._btnRefresh);
            this.header.Controls.Add(this.title);
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Height = 60;
            this.header.Location = new System.Drawing.Point(24, 24);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(1200, 60);
            this.header.TabIndex = 0;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.title.Location = new System.Drawing.Point(0, 10);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(287, 25);
            this.title.TabIndex = 0;
            this.title.Text = "My Order History & Live Status";
            // 
            // _btnRefresh
            // 
            this._btnRefresh.BorderRadius = 8;
            this._btnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnRefresh.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._btnRefresh.IconName = "refresh";
            this._btnRefresh.Location = new System.Drawing.Point(480, 10);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnRefresh.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnRefresh.PressedColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryPressed;
            this._btnRefresh.Size = new System.Drawing.Size(136, 36);
            this._btnRefresh.TabIndex = 1;
            this._btnRefresh.Text = "Refresh Status";
            // 
            // _flpOrders
            // 
            this._flpOrders.AutoScroll = true;
            this._flpOrders.BackColor = Tonyo_sPizza.UI.Theme.AppColors.Background;
            this._flpOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this._flpOrders.Location = new System.Drawing.Point(24, 84);
            this._flpOrders.Name = "_flpOrders";
            this._flpOrders.Padding = new System.Windows.Forms.Padding(0, 12, 0, 12);
            this._flpOrders.Size = new System.Drawing.Size(1200, 508);
            this._flpOrders.TabIndex = 1;
            // 
            // CustomerOrdersControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Tonyo_sPizza.UI.Theme.AppColors.Background;
            this.Controls.Add(this._flpOrders);
            this.Controls.Add(this.header);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "CustomerOrdersControl";
            this.Padding = new System.Windows.Forms.Padding(24);
            this.Size = new System.Drawing.Size(1248, 616);
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel header;
        private System.Windows.Forms.Label title;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRefresh;
        private System.Windows.Forms.FlowLayoutPanel _flpOrders;
    }
}
