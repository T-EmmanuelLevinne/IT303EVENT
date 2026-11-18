using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Controls
{
    /// <summary>
    /// Windows 11-inspired floating context menu with native DWM rounded corners,
    /// soft shadow elevation, spacious padding, and consistent submenu chrome.
    /// </summary>
    public class ModernContextMenu : ContextMenuStrip
    {
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWCP_ROUND = 2;
        private const int DWMWA_BORDER_COLOR = 34;

        private bool _isNativeDwmRounded;

        public bool IsNativeDwmRounded => _isNativeDwmRounded;

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int ellipseWidth, int ellipseHeight);

        public ModernContextMenu()
        {
            ConfigureDropDown(this, out _isNativeDwmRounded);
            DoubleBuffered = true;
            UIHelper.EnableDoubleBuffering(this);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            _isNativeDwmRounded = TryApplyDwmWindowRounding(Handle);
            if (!_isNativeDwmRounded)
            {
                ApplyFallbackRegion(this);
            }
        }

        protected override void OnOpening(System.ComponentModel.CancelEventArgs e)
        {
            base.OnOpening(e);
            if (!e.Cancel)
            {
                ApplyDynamicMinimumWidth(this);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (IsHandleCreated && !_isNativeDwmRounded)
            {
                ApplyFallbackRegion(this);
            }
        }

        protected override void OnItemAdded(ToolStripItemEventArgs e)
        {
            base.OnItemAdded(e);
            ConfigureMenuItemRecursively(e.Item);
        }

        private void ConfigureMenuItemRecursively(ToolStripItem item)
        {
            if (item is ToolStripSeparator sep)
            {
                sep.Margin = new Padding(0, 3, 0, 3);
                sep.AutoSize = false;
                sep.Height = 8;
                return;
            }

            if (item is ToolStripMenuItem menuItem)
            {
                menuItem.DropDownOpening -= MenuItem_DropDownOpening;
                menuItem.DropDownOpening += MenuItem_DropDownOpening;

                if (menuItem.HasDropDownItems)
                {
                    ConfigureDropDown(menuItem.DropDown, out _);
                }
            }
        }

        private void MenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem mi && mi.HasDropDownItems)
            {
                ConfigureDropDown(mi.DropDown, out bool nativeRounded);
                ApplyDynamicMinimumWidth(mi.DropDown);
                if (mi.DropDown.IsHandleCreated)
                {
                    nativeRounded = TryApplyDwmWindowRounding(mi.DropDown.Handle);
                    if (!nativeRounded) ApplyFallbackRegion(mi.DropDown);
                }
            }
        }

        public static void ApplyDynamicMinimumWidth(ToolStripDropDown dropDown)
        {
            if (dropDown == null) return;
            int maxWidth = 224;
            foreach (ToolStripItem item in dropDown.Items)
            {
                if (item is ToolStripMenuItem mi)
                {
                    int prefW = mi.GetPreferredSize(Size.Empty).Width + 8;
                    if (prefW > maxWidth) maxWidth = prefW;
                }
            }
            dropDown.MinimumSize = new Size(maxWidth, 0);
        }

        public static void ConfigureDropDown(ToolStripDropDown dropDown, out bool isNativeRounded)
        {
            isNativeRounded = false;
            if (dropDown == null) return;

            dropDown.Renderer = new ModernMenuRenderer();
            dropDown.BackColor = Color.FromArgb(252, 250, 248);
            dropDown.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            dropDown.ForeColor = AppColors.TextDark;
            dropDown.Padding = new Padding(4, 6, 4, 6);
            dropDown.DropShadowEnabled = true;
            UIHelper.EnableDoubleBuffering(dropDown);

            if (dropDown is ToolStripDropDownMenu ddm)
            {
                ddm.ShowImageMargin = true;
                ddm.ShowCheckMargin = false;
                ddm.ImageScalingSize = new Size(18, 18);
            }

            dropDown.HandleCreated -= DropDown_HandleCreated;
            dropDown.HandleCreated += DropDown_HandleCreated;
            dropDown.SizeChanged -= DropDown_SizeChanged;
            dropDown.SizeChanged += DropDown_SizeChanged;

            if (dropDown.IsHandleCreated)
            {
                isNativeRounded = TryApplyDwmWindowRounding(dropDown.Handle);
            }
        }

        private static void DropDown_HandleCreated(object sender, EventArgs e)
        {
            if (sender is ToolStripDropDown dd)
            {
                bool rounded = TryApplyDwmWindowRounding(dd.Handle);
                dd.Tag = rounded ? "DWM_ROUND" : "GDI_ROUND";
                if (!rounded)
                {
                    ApplyFallbackRegion(dd);
                }
            }
        }

        private static void DropDown_SizeChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripDropDown dd && dd.IsHandleCreated && (dd.Tag as string) == "GDI_ROUND")
            {
                ApplyFallbackRegion(dd);
            }
        }

        internal static bool IsDropDownUsingNativeRounding(ToolStrip strip)
        {
            if (strip is ModernContextMenu m) return m.IsNativeDwmRounded;
            if (strip is ToolStripDropDown dd && (dd.Tag as string) == "DWM_ROUND") return true;
            return false;
        }

        private static bool TryApplyDwmWindowRounding(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return false;
            try
            {
                int pref = DWMWCP_ROUND;
                int hr = DwmSetWindowAttribute(handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref pref, sizeof(int));
                if (hr == 0)
                {
                    // Warm stone subtle border (COLORREF = 0x00BBGGRR for #DED9D0)
                    int borderRef = 0x00D0D9DE;
                    DwmSetWindowAttribute(handle, DWMWA_BORDER_COLOR, ref borderRef, sizeof(int));
                    return true;
                }
            }
            catch
            {
                // Older Windows versions without DWM corner attribute fall back cleanly
            }
            return false;
        }

        private static void ApplyFallbackRegion(Control control)
        {
            try
            {
                if (control.Width <= 0 || control.Height <= 0) return;
                IntPtr hRgn = CreateRoundRectRgn(0, 0, control.Width + 1, control.Height + 1, 14, 14);
                control.Region = Region.FromHrgn(hRgn);
            }
            catch { }
        }
    }

    public class ModernMenuItem : ToolStripMenuItem
    {
        public ModernMenuItem() { }

        public ModernMenuItem(string text) : base(text) { }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            Size pref = base.GetPreferredSize(constrainingSize);
            return new Size(Math.Max(216, pref.Width + 14), Math.Max(34, pref.Height));
        }
    }
}
