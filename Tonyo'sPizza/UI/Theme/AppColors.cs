using System.Drawing;

namespace Tonyo_sPizza.UI.Theme
{
    public static class AppColors
    {
        // GoFood Brand Colors (Vibrant Rose / Magenta theme)
        public static readonly Color Primary = Color.FromArgb(190, 24, 93);         // #BE185D
        public static readonly Color PrimaryDark = Color.FromArgb(159, 18, 57);     // #9F1239
        public static readonly Color PrimaryLight = Color.FromArgb(244, 63, 94);     // #F43F5E
        public static readonly Color PrimarySoft = Color.FromArgb(255, 241, 242);    // #FFF1F2
        public static readonly Color PrimaryBorder = Color.FromArgb(254, 205, 211);  // #FECDD3

        // Neutral & Backgrounds
        public static readonly Color Background = Color.FromArgb(248, 250, 252);    // #F8FAFC
        public static readonly Color CardBackground = Color.White;
        public static readonly Color SidebarBackground = Color.White;
        public static readonly Color Border = Color.FromArgb(226, 232, 240);         // #E2E8F0
        public static readonly Color BorderLight = Color.FromArgb(241, 245, 249);    // #F1F5F9

        // Typography
        public static readonly Color TextDark = Color.FromArgb(15, 23, 42);          // #0F172A
        public static readonly Color TextMedium = Color.FromArgb(51, 65, 85);        // #334155
        public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);      // #64748B
        public static readonly Color TextLight = Color.FromArgb(148, 163, 184);      // #94A3B8

        // Status Colors
        public static readonly Color StatusPending = Color.FromArgb(217, 119, 6);     // #D97706 (Amber)
        public static readonly Color StatusPendingBg = Color.FromArgb(254, 243, 199); // #FEF3C7

        public static readonly Color StatusPreparing = Color.FromArgb(37, 99, 235);   // #2563EB (Blue)
        public static readonly Color StatusPreparingBg = Color.FromArgb(219, 234, 254);// #DBEAFE

        public static readonly Color StatusReady = Color.FromArgb(124, 58, 237);      // #7C3AED (Purple)
        public static readonly Color StatusReadyBg = Color.FromArgb(237, 233, 254);   // #EDE9FE

        public static readonly Color StatusCompleted = Color.FromArgb(5, 150, 105);   // #059669 (Emerald)
        public static readonly Color StatusCompletedBg = Color.FromArgb(209, 250, 229);// #D1FAE5

        public static readonly Color StatusCancelled = Color.FromArgb(220, 38, 38);   // #DC2626 (Red)
        public static readonly Color StatusCancelledBg = Color.FromArgb(254, 226, 226);// #FEE2E2

        // Card Banner Gradient
        public static readonly Color BannerGradientStart = Color.FromArgb(190, 24, 93);  // #BE185D
        public static readonly Color BannerGradientEnd = Color.FromArgb(131, 24, 67);    // #831843
    }
}
