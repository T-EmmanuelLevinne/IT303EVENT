namespace Tonyo_sPizza.UI.Views.Customer
{
    partial class CustomerMenuControl
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
            this._pnlRightSidebar = new System.Windows.Forms.Panel();
            this._flpCartItems = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlBottomCheckout = new System.Windows.Forms.Panel();
            this._btnCheckout = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.pnlTotals = new System.Windows.Forms.Panel();
            this._lblTotal = new System.Windows.Forms.Label();
            this.lblTotTitle = new System.Windows.Forms.Label();
            this.pnlLine = new System.Windows.Forms.Panel();
            this._lblDiscount = new System.Windows.Forms.Label();
            this.lblDiscTitle = new System.Windows.Forms.Label();
            this._lblServiceFee = new System.Windows.Forms.Label();
            this.lblFeeTitle = new System.Windows.Forms.Label();
            this._lblSubtotal = new System.Windows.Forms.Label();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.pnlPromo = new System.Windows.Forms.Panel();
            this._btnApplyPromo = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._txtPromoCode = new System.Windows.Forms.TextBox();
            this.pnlOrderHeader = new System.Windows.Forms.Panel();
            this._chkCartSelectAll = new System.Windows.Forms.CheckBox();
            this.lblOrderTitle = new System.Windows.Forms.Label();
            this.pnlAddress = new System.Windows.Forms.Panel();
            this._lblCartAddress = new System.Windows.Forms.Label();
            this.lblAddrHeader = new System.Windows.Forms.Label();
            this.pnlBrandCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblBrandSub = new System.Windows.Forms.Label();
            this.lblBrandName = new System.Windows.Forms.Label();
            this.pnlLogoCircle = new System.Windows.Forms.Panel();
            this._pnlMainContent = new System.Windows.Forms.Panel();
            this._flpFoodCards = new System.Windows.Forms.FlowLayoutPanel();
            this.foodHeader = new System.Windows.Forms.Panel();
            this.lblFoodTitle = new System.Windows.Forms.Label();
            this._flpCategories = new System.Windows.Forms.FlowLayoutPanel();
            this.catHeader = new System.Windows.Forms.Panel();
            this.lblCategoryTitle = new System.Windows.Forms.Label();
            this.banner = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.btnOrderNow = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.lblBannerSub = new System.Windows.Forms.Label();
            this.lblBannerTitle = new System.Windows.Forms.Label();
            this._pnlRightSidebar.SuspendLayout();
            this.pnlBottomCheckout.SuspendLayout();
            this.pnlTotals.SuspendLayout();
            this.pnlPromo.SuspendLayout();
            this.pnlOrderHeader.SuspendLayout();
            this.pnlAddress.SuspendLayout();
            this.pnlBrandCard.SuspendLayout();
            this._pnlMainContent.SuspendLayout();
            this.foodHeader.SuspendLayout();
            this.catHeader.SuspendLayout();
            this.banner.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnlRightSidebar
            // 
            this._pnlRightSidebar.BackColor = System.Drawing.Color.White;
            this._pnlRightSidebar.Controls.Add(this._flpCartItems);
            this._pnlRightSidebar.Controls.Add(this.pnlBottomCheckout);
            this._pnlRightSidebar.Controls.Add(this.pnlOrderHeader);
            this._pnlRightSidebar.Controls.Add(this.pnlAddress);
            this._pnlRightSidebar.Controls.Add(this.pnlBrandCard);
            this._pnlRightSidebar.Dock = System.Windows.Forms.DockStyle.Right;
            this._pnlRightSidebar.Location = new System.Drawing.Point(928, 0);
            this._pnlRightSidebar.Name = "_pnlRightSidebar";
            this._pnlRightSidebar.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this._pnlRightSidebar.Size = new System.Drawing.Size(320, 650);
            this._pnlRightSidebar.TabIndex = 0;
            // 
            // _flpCartItems
            // 
            this._flpCartItems.AutoScroll = true;
            this._flpCartItems.BackColor = System.Drawing.Color.White;
            this._flpCartItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this._flpCartItems.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._flpCartItems.Location = new System.Drawing.Point(16, 190);
            this._flpCartItems.Name = "_flpCartItems";
            this._flpCartItems.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this._flpCartItems.Size = new System.Drawing.Size(288, 203);
            this._flpCartItems.TabIndex = 3;
            this._flpCartItems.WrapContents = false;
            // 
            // pnlBottomCheckout
            // 
            this.pnlBottomCheckout.BackColor = System.Drawing.Color.White;
            this.pnlBottomCheckout.Controls.Add(this._btnCheckout);
            this.pnlBottomCheckout.Controls.Add(this.pnlTotals);
            this.pnlBottomCheckout.Controls.Add(this.pnlPromo);
            this.pnlBottomCheckout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomCheckout.Location = new System.Drawing.Point(16, 393);
            this.pnlBottomCheckout.Name = "pnlBottomCheckout";
            this.pnlBottomCheckout.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlBottomCheckout.Size = new System.Drawing.Size(288, 245);
            this.pnlBottomCheckout.TabIndex = 4;
            // 
            // _btnCheckout
            // 
            this._btnCheckout.BorderRadius = 10;
            this._btnCheckout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._btnCheckout.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this._btnCheckout.ForeColor = System.Drawing.Color.White;
            this._btnCheckout.Location = new System.Drawing.Point(0, 201);
            this._btnCheckout.Name = "_btnCheckout";
            this._btnCheckout.NormalColor = System.Drawing.Color.FromArgb(234, 88, 12);
            this._btnCheckout.HoverColor = System.Drawing.Color.FromArgb(249, 115, 22);
            this._btnCheckout.Size = new System.Drawing.Size(288, 44);
            this._btnCheckout.TabIndex = 2;
            this._btnCheckout.Text = "Checkout Selected";
            // 
            // pnlTotals
            // 
            this.pnlTotals.Controls.Add(this._lblTotal);
            this.pnlTotals.Controls.Add(this.lblTotTitle);
            this.pnlTotals.Controls.Add(this.pnlLine);
            this.pnlTotals.Controls.Add(this._lblDiscount);
            this.pnlTotals.Controls.Add(this.lblDiscTitle);
            this.pnlTotals.Controls.Add(this._lblServiceFee);
            this.pnlTotals.Controls.Add(this.lblFeeTitle);
            this.pnlTotals.Controls.Add(this._lblSubtotal);
            this.pnlTotals.Controls.Add(this.lblSubTitle);
            this.pnlTotals.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTotals.Location = new System.Drawing.Point(0, 44);
            this.pnlTotals.Name = "pnlTotals";
            this.pnlTotals.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.pnlTotals.Size = new System.Drawing.Size(288, 108);
            this.pnlTotals.TabIndex = 1;
            // 
            // _lblTotal
            // 
            this._lblTotal.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this._lblTotal.ForeColor = System.Drawing.Color.FromArgb(194, 65, 12);
            this._lblTotal.Location = new System.Drawing.Point(140, 78);
            this._lblTotal.Name = "_lblTotal";
            this._lblTotal.Size = new System.Drawing.Size(145, 24);
            this._lblTotal.TabIndex = 8;
            this._lblTotal.Text = "₱0.00";
            this._lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotTitle
            // 
            this.lblTotTitle.AutoSize = true;
            this.lblTotTitle.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblTotTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTotTitle.Location = new System.Drawing.Point(0, 80);
            this.lblTotTitle.Name = "lblTotTitle";
            this.lblTotTitle.Size = new System.Drawing.Size(48, 21);
            this.lblTotTitle.TabIndex = 7;
            this.lblTotTitle.Text = "Total";
            // 
            // pnlLine
            // 
            this.pnlLine.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlLine.Location = new System.Drawing.Point(0, 72);
            this.pnlLine.Name = "pnlLine";
            this.pnlLine.Size = new System.Drawing.Size(285, 1);
            this.pnlLine.TabIndex = 6;
            // 
            // _lblDiscount
            // 
            this._lblDiscount.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._lblDiscount.ForeColor = System.Drawing.Color.FromArgb(234, 88, 12);
            this._lblDiscount.Location = new System.Drawing.Point(160, 48);
            this._lblDiscount.Name = "_lblDiscount";
            this._lblDiscount.Size = new System.Drawing.Size(125, 18);
            this._lblDiscount.TabIndex = 5;
            this._lblDiscount.Text = "-₱0.00";
            this._lblDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDiscTitle
            // 
            this.lblDiscTitle.AutoSize = true;
            this.lblDiscTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDiscTitle.ForeColor = System.Drawing.Color.FromArgb(234, 88, 12);
            this.lblDiscTitle.Location = new System.Drawing.Point(0, 48);
            this.lblDiscTitle.Name = "lblDiscTitle";
            this.lblDiscTitle.Size = new System.Drawing.Size(54, 15);
            this.lblDiscTitle.TabIndex = 4;
            this.lblDiscTitle.Text = "Discount";
            // 
            // _lblServiceFee
            // 
            this._lblServiceFee.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._lblServiceFee.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._lblServiceFee.Location = new System.Drawing.Point(160, 26);
            this._lblServiceFee.Name = "_lblServiceFee";
            this._lblServiceFee.Size = new System.Drawing.Size(125, 18);
            this._lblServiceFee.TabIndex = 3;
            this._lblServiceFee.Text = "₱2.00";
            this._lblServiceFee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFeeTitle
            // 
            this.lblFeeTitle.AutoSize = true;
            this.lblFeeTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFeeTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblFeeTitle.Location = new System.Drawing.Point(0, 26);
            this.lblFeeTitle.Name = "lblFeeTitle";
            this.lblFeeTitle.Size = new System.Drawing.Size(64, 15);
            this.lblFeeTitle.TabIndex = 2;
            this.lblFeeTitle.Text = "Service Fee";
            // 
            // _lblSubtotal
            // 
            this._lblSubtotal.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._lblSubtotal.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._lblSubtotal.Location = new System.Drawing.Point(160, 4);
            this._lblSubtotal.Name = "_lblSubtotal";
            this._lblSubtotal.Size = new System.Drawing.Size(125, 18);
            this._lblSubtotal.TabIndex = 1;
            this._lblSubtotal.Text = "₱0.00";
            this._lblSubtotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSubTitle.Location = new System.Drawing.Point(0, 4);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(51, 15);
            this.lblSubTitle.TabIndex = 0;
            this.lblSubTitle.Text = "Subtotal";
            // 
            // pnlPromo
            // 
            this.pnlPromo.Controls.Add(this._btnApplyPromo);
            this.pnlPromo.Controls.Add(this._txtPromoCode);
            this.pnlPromo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPromo.Location = new System.Drawing.Point(0, 8);
            this.pnlPromo.Name = "pnlPromo";
            this.pnlPromo.Size = new System.Drawing.Size(288, 36);
            this.pnlPromo.TabIndex = 0;
            // 
            // _btnApplyPromo
            // 
            this._btnApplyPromo.BorderRadius = 6;
            this._btnApplyPromo.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this._btnApplyPromo.ForeColor = System.Drawing.Color.FromArgb(194, 65, 12);
            this._btnApplyPromo.Location = new System.Drawing.Point(190, 4);
            this._btnApplyPromo.Name = "_btnApplyPromo";
            this._btnApplyPromo.NormalColor = System.Drawing.Color.FromArgb(255, 237, 213);
            this._btnApplyPromo.HoverColor = System.Drawing.Color.FromArgb(254, 215, 170);
            this._btnApplyPromo.Size = new System.Drawing.Size(95, 30);
            this._btnApplyPromo.TabIndex = 1;
            this._btnApplyPromo.Text = "Promo";
            // 
            // _txtPromoCode
            // 
            this._txtPromoCode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._txtPromoCode.Location = new System.Drawing.Point(0, 6);
            this._txtPromoCode.Name = "_txtPromoCode";
            this._txtPromoCode.Size = new System.Drawing.Size(180, 23);
            this._txtPromoCode.TabIndex = 0;
            // 
            // pnlOrderHeader
            // 
            this.pnlOrderHeader.Controls.Add(this._chkCartSelectAll);
            this.pnlOrderHeader.Controls.Add(this.lblOrderTitle);
            this.pnlOrderHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOrderHeader.Location = new System.Drawing.Point(16, 156);
            this.pnlOrderHeader.Name = "pnlOrderHeader";
            this.pnlOrderHeader.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.pnlOrderHeader.Size = new System.Drawing.Size(288, 34);
            this.pnlOrderHeader.TabIndex = 2;
            // 
            // _chkCartSelectAll
            // 
            this._chkCartSelectAll.AutoSize = true;
            this._chkCartSelectAll.Checked = true;
            this._chkCartSelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkCartSelectAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this._chkCartSelectAll.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this._chkCartSelectAll.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this._chkCartSelectAll.Location = new System.Drawing.Point(230, 4);
            this._chkCartSelectAll.Name = "_chkCartSelectAll";
            this._chkCartSelectAll.Size = new System.Drawing.Size(39, 19);
            this._chkCartSelectAll.TabIndex = 1;
            this._chkCartSelectAll.Text = "All";
            this._chkCartSelectAll.UseVisualStyleBackColor = true;
            // 
            // lblOrderTitle
            // 
            this.lblOrderTitle.AutoSize = true;
            this.lblOrderTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblOrderTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblOrderTitle.Location = new System.Drawing.Point(0, 4);
            this.lblOrderTitle.Name = "lblOrderTitle";
            this.lblOrderTitle.Size = new System.Drawing.Size(81, 21);
            this.lblOrderTitle.TabIndex = 0;
            this.lblOrderTitle.Text = "My Order";
            // 
            // pnlAddress
            // 
            this.pnlAddress.Controls.Add(this._lblCartAddress);
            this.pnlAddress.Controls.Add(this.lblAddrHeader);
            this.pnlAddress.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAddress.Location = new System.Drawing.Point(16, 96);
            this.pnlAddress.Name = "pnlAddress";
            this.pnlAddress.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlAddress.Size = new System.Drawing.Size(288, 60);
            this.pnlAddress.TabIndex = 1;
            this.pnlAddress.Visible = false;
            // 
            // _lblCartAddress
            // 
            this._lblCartAddress.AutoEllipsis = true;
            this._lblCartAddress.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._lblCartAddress.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._lblCartAddress.Location = new System.Drawing.Point(2, 24);
            this._lblCartAddress.Name = "_lblCartAddress";
            this._lblCartAddress.Size = new System.Drawing.Size(280, 32);
            this._lblCartAddress.TabIndex = 1;
            this._lblCartAddress.Text = "Dine-In / Counter Pickup";
            // 
            // lblAddrHeader
            // 
            this.lblAddrHeader.AutoSize = true;
            this.lblAddrHeader.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblAddrHeader.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblAddrHeader.Location = new System.Drawing.Point(2, 6);
            this.lblAddrHeader.Name = "lblAddrHeader";
            this.lblAddrHeader.Size = new System.Drawing.Size(109, 13);
            this.lblAddrHeader.TabIndex = 0;
            this.lblAddrHeader.Text = "DELIVERY ADDRESS";
            // 
            // pnlBrandCard
            // 
            this.pnlBrandCard.BackColor = System.Drawing.Color.White;
            this.pnlBrandCard.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlBrandCard.BorderRadius = 12;
            this.pnlBrandCard.BorderWidth = 1;
            this.pnlBrandCard.Controls.Add(this.lblBrandSub);
            this.pnlBrandCard.Controls.Add(this.lblBrandName);
            this.pnlBrandCard.Controls.Add(this.pnlLogoCircle);
            this.pnlBrandCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBrandCard.DrawBorder = true;
            this.pnlBrandCard.Location = new System.Drawing.Point(16, 12);
            this.pnlBrandCard.Name = "pnlBrandCard";
            this.pnlBrandCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlBrandCard.Size = new System.Drawing.Size(288, 84);
            this.pnlBrandCard.TabIndex = 0;
            // 
            // lblBrandSub
            // 
            this.lblBrandSub.AutoSize = true;
            this.lblBrandSub.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblBrandSub.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblBrandSub.Location = new System.Drawing.Point(60, 42);
            this.lblBrandSub.Name = "lblBrandSub";
            this.lblBrandSub.Size = new System.Drawing.Size(157, 15);
            this.lblBrandSub.TabIndex = 2;
            this.lblBrandSub.Text = "Artisan Wood-Fired Pizzeria";
            // 
            // lblBrandName
            // 
            this.lblBrandName.AutoSize = true;
            this.lblBrandName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBrandName.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblBrandName.Location = new System.Drawing.Point(60, 15);
            this.lblBrandName.Name = "lblBrandName";
            this.lblBrandName.Size = new System.Drawing.Size(156, 25);
            this.lblBrandName.TabIndex = 1;
            this.lblBrandName.Text = "TONYO\'S PIZZA";
            // 
            // pnlLogoCircle
            // 
            this.pnlLogoCircle.Location = new System.Drawing.Point(12, 16);
            this.pnlLogoCircle.Name = "pnlLogoCircle";
            this.pnlLogoCircle.Size = new System.Drawing.Size(40, 40);
            this.pnlLogoCircle.TabIndex = 0;
            // 
            // _pnlMainContent
            // 
            this._pnlMainContent.AutoScroll = true;
            this._pnlMainContent.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this._pnlMainContent.Controls.Add(this._flpFoodCards);
            this._pnlMainContent.Controls.Add(this.foodHeader);
            this._pnlMainContent.Controls.Add(this._flpCategories);
            this._pnlMainContent.Controls.Add(this.catHeader);
            this._pnlMainContent.Controls.Add(this.banner);
            this._pnlMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlMainContent.Location = new System.Drawing.Point(0, 0);
            this._pnlMainContent.Name = "_pnlMainContent";
            this._pnlMainContent.Padding = new System.Windows.Forms.Padding(24, 16, 24, 24);
            this._pnlMainContent.Size = new System.Drawing.Size(928, 650);
            this._pnlMainContent.TabIndex = 1;
            // 
            // _flpFoodCards
            // 
            this._flpFoodCards.AutoSize = true;
            this._flpFoodCards.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._flpFoodCards.Dock = System.Windows.Forms.DockStyle.Top;
            this._flpFoodCards.Location = new System.Drawing.Point(24, 284);
            this._flpFoodCards.Name = "_flpFoodCards";
            this._flpFoodCards.Padding = new System.Windows.Forms.Padding(0, 8, 0, 24);
            this._flpFoodCards.Size = new System.Drawing.Size(880, 32);
            this._flpFoodCards.TabIndex = 4;
            // 
            // foodHeader
            // 
            this.foodHeader.Controls.Add(this.lblFoodTitle);
            this.foodHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.foodHeader.Location = new System.Drawing.Point(24, 248);
            this.foodHeader.Name = "foodHeader";
            this.foodHeader.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.foodHeader.Size = new System.Drawing.Size(880, 36);
            this.foodHeader.TabIndex = 3;
            // 
            // lblFoodTitle
            // 
            this.lblFoodTitle.AutoSize = true;
            this.lblFoodTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblFoodTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblFoodTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblFoodTitle.Location = new System.Drawing.Point(0, 10);
            this.lblFoodTitle.Name = "lblFoodTitle";
            this.lblFoodTitle.Size = new System.Drawing.Size(248, 25);
            this.lblFoodTitle.TabIndex = 0;
            this.lblFoodTitle.Text = "Popular Food & Pizza Menu";
            // 
            // _flpCategories
            // 
            this._flpCategories.AutoScroll = true;
            this._flpCategories.Dock = System.Windows.Forms.DockStyle.Top;
            this._flpCategories.Location = new System.Drawing.Point(24, 196);
            this._flpCategories.Name = "_flpCategories";
            this._flpCategories.Padding = new System.Windows.Forms.Padding(0, 4, 0, 6);
            this._flpCategories.Size = new System.Drawing.Size(880, 52);
            this._flpCategories.TabIndex = 2;
            this._flpCategories.WrapContents = false;
            // 
            // catHeader
            // 
            this.catHeader.Controls.Add(this.lblCategoryTitle);
            this.catHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.catHeader.Location = new System.Drawing.Point(24, 160);
            this.catHeader.Name = "catHeader";
            this.catHeader.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.catHeader.Size = new System.Drawing.Size(880, 36);
            this.catHeader.TabIndex = 1;
            // 
            // lblCategoryTitle
            // 
            this.lblCategoryTitle.AutoSize = true;
            this.lblCategoryTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCategoryTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblCategoryTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblCategoryTitle.Location = new System.Drawing.Point(0, 10);
            this.lblCategoryTitle.Name = "lblCategoryTitle";
            this.lblCategoryTitle.Size = new System.Drawing.Size(89, 25);
            this.lblCategoryTitle.TabIndex = 0;
            this.lblCategoryTitle.Text = "Category";
            // 
            // banner
            // 
            this.banner.Controls.Add(this.btnOrderNow);
            this.banner.Controls.Add(this.lblBannerSub);
            this.banner.Controls.Add(this.lblBannerTitle);
            this.banner.Dock = System.Windows.Forms.DockStyle.Top;
            this.banner.DrawBorder = false;
            this.banner.GradientAngle = 45F;
            this.banner.GradientEndColor = System.Drawing.Color.FromArgb(234, 88, 12);
            this.banner.GradientStartColor = System.Drawing.Color.FromArgb(249, 115, 22);
            this.banner.Location = new System.Drawing.Point(24, 16);
            this.banner.Margin = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.banner.Name = "banner";
            this.banner.Padding = new System.Windows.Forms.Padding(24, 18, 24, 18);
            this.banner.Size = new System.Drawing.Size(880, 144);
            this.banner.TabIndex = 0;
            // 
            // btnOrderNow
            // 
            this.btnOrderNow.BorderRadius = 8;
            this.btnOrderNow.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnOrderNow.ForeColor = System.Drawing.Color.FromArgb(194, 65, 12);
            this.btnOrderNow.Location = new System.Drawing.Point(24, 98);
            this.btnOrderNow.Name = "btnOrderNow";
            this.btnOrderNow.NormalColor = System.Drawing.Color.White;
            this.btnOrderNow.HoverColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.btnOrderNow.Size = new System.Drawing.Size(110, 32);
            this.btnOrderNow.TabIndex = 2;
            this.btnOrderNow.Text = "Order Now";
            // 
            // lblBannerSub
            // 
            this.lblBannerSub.BackColor = System.Drawing.Color.Transparent;
            this.lblBannerSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBannerSub.ForeColor = System.Drawing.Color.FromArgb(254, 205, 211);
            this.lblBannerSub.Location = new System.Drawing.Point(24, 65);
            this.lblBannerSub.Name = "lblBannerSub";
            this.lblBannerSub.Size = new System.Drawing.Size(340, 30);
            this.lblBannerSub.TabIndex = 1;
            this.lblBannerSub.Text = "Use promo code TONYO25 at checkout for fresh delicious pizza.";
            // 
            // lblBannerTitle
            // 
            this.lblBannerTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblBannerTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblBannerTitle.ForeColor = System.Drawing.Color.White;
            this.lblBannerTitle.Location = new System.Drawing.Point(24, 12);
            this.lblBannerTitle.Name = "lblBannerTitle";
            this.lblBannerTitle.Size = new System.Drawing.Size(320, 50);
            this.lblBannerTitle.TabIndex = 0;
            this.lblBannerTitle.Text = "Get Discount Voucher\r\nUp To 25%";
            // 
            // CustomerMenuControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this._pnlMainContent);
            this.Controls.Add(this._pnlRightSidebar);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "CustomerMenuControl";
            this.Size = new System.Drawing.Size(1248, 650);
            this._pnlRightSidebar.ResumeLayout(false);
            this.pnlBottomCheckout.ResumeLayout(false);
            this.pnlTotals.ResumeLayout(false);
            this.pnlTotals.PerformLayout();
            this.pnlPromo.ResumeLayout(false);
            this.pnlPromo.PerformLayout();
            this.pnlOrderHeader.ResumeLayout(false);
            this.pnlOrderHeader.PerformLayout();
            this.pnlAddress.ResumeLayout(false);
            this.pnlAddress.PerformLayout();
            this.pnlBrandCard.ResumeLayout(false);
            this.pnlBrandCard.PerformLayout();
            this._pnlMainContent.ResumeLayout(false);
            this._pnlMainContent.PerformLayout();
            this.foodHeader.ResumeLayout(false);
            this.foodHeader.PerformLayout();
            this.catHeader.ResumeLayout(false);
            this.catHeader.PerformLayout();
            this.banner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _pnlRightSidebar;
        private System.Windows.Forms.FlowLayoutPanel _flpCartItems;
        private System.Windows.Forms.Panel pnlBottomCheckout;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnCheckout;
        private System.Windows.Forms.Panel pnlTotals;
        private System.Windows.Forms.Label _lblTotal;
        private System.Windows.Forms.Label lblTotTitle;
        private System.Windows.Forms.Panel pnlLine;
        private System.Windows.Forms.Label _lblDiscount;
        private System.Windows.Forms.Label lblDiscTitle;
        private System.Windows.Forms.Label _lblServiceFee;
        private System.Windows.Forms.Label lblFeeTitle;
        private System.Windows.Forms.Label _lblSubtotal;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Panel pnlPromo;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnApplyPromo;
        private System.Windows.Forms.TextBox _txtPromoCode;
        private System.Windows.Forms.Panel pnlOrderHeader;
        private System.Windows.Forms.CheckBox _chkCartSelectAll;
        private System.Windows.Forms.Label lblOrderTitle;
        private System.Windows.Forms.Panel pnlAddress;
        private System.Windows.Forms.Label _lblCartAddress;
        private System.Windows.Forms.Label lblAddrHeader;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlBrandCard;
        private System.Windows.Forms.Label lblBrandSub;
        private System.Windows.Forms.Label lblBrandName;
        private System.Windows.Forms.Panel pnlLogoCircle;
        private System.Windows.Forms.Panel _pnlMainContent;
        private System.Windows.Forms.FlowLayoutPanel _flpFoodCards;
        private System.Windows.Forms.Panel foodHeader;
        private System.Windows.Forms.Label lblFoodTitle;
        private System.Windows.Forms.FlowLayoutPanel _flpCategories;
        private System.Windows.Forms.Panel catHeader;
        private System.Windows.Forms.Label lblCategoryTitle;
        private Tonyo_sPizza.UI.Controls.RoundedPanel banner;
        private Tonyo_sPizza.UI.Controls.RoundedButton btnOrderNow;
        private System.Windows.Forms.Label lblBannerSub;
        private System.Windows.Forms.Label lblBannerTitle;
    }
}
