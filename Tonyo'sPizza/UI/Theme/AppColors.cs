using System.Drawing;

namespace Tonyo_sPizza.UI.Theme
{
    public static class AppColors
    {
        // Tonyo's Brick-Oven Artisanal Pizzeria Palette
        public static readonly Color Primary = Color.FromArgb(184, 50, 39);          // #B83227 (Wood-Fired Brick Red)
        public static readonly Color PrimaryDark = Color.FromArgb(143, 35, 26);      // #8F231A (Roasted Terracotta)
        public static readonly Color PrimaryLight = Color.FromArgb(206, 64, 52);     // #CE4034 (Warm Pomodoro)
        public static readonly Color PrimarySoft = Color.FromArgb(253, 242, 240);    // #FDF2F0 (Warm Blush Cream)
        public static readonly Color PrimaryBorder = Color.FromArgb(242, 210, 205);  // #F2D2CD (Subtle Terracotta Tint)

        // Neutral Warm Surfaces
        public static readonly Color Background = Color.FromArgb(250, 248, 245);     // #FAF8F5 (Warm Parchment Ivory)
        public static readonly Color SurfaceAlt = Color.FromArgb(243, 240, 234);     // #F3F0EA (Warm Sand)
        public static readonly Color CardBackground = Color.White;
        public static readonly Color SidebarBackground = Color.White;
        public static readonly Color Border = Color.FromArgb(230, 225, 216);         // #E6E1D8 (Warm Stone Hairline)
        public static readonly Color BorderLight = Color.FromArgb(242, 239, 233);    // #F2EFE9

        // Espresso & Stone Typography
        public static readonly Color TextDark = Color.FromArgb(31, 27, 24);          // #1F1B18 (Espresso Ink)
        public static readonly Color TextMedium = Color.FromArgb(74, 67, 61);        // #4A433D (Warm Charcoal)
        public static readonly Color TextMuted = Color.FromArgb(120, 113, 108);      // #78716C (Stone Gray)
        public static readonly Color TextLight = Color.FromArgb(168, 162, 158);      // #A8A29E (Soft Warm Gray)

        // Semantic Status Colors
        public static readonly Color StatusPending = Color.FromArgb(180, 83, 9);     // #B45309 (Warm Amber)
        public static readonly Color StatusPendingBg = Color.FromArgb(254, 243, 199);// #FEF3C7

        public static readonly Color StatusPreparing = Color.FromArgb(29, 78, 216);  // #1D4ED8 (Cobalt)
        public static readonly Color StatusPreparingBg = Color.FromArgb(219, 234, 254);// #DBEAFE

        public static readonly Color StatusReady = Color.FromArgb(109, 40, 217);     // #6D28D9 (Deep Plum)
        public static readonly Color StatusReadyBg = Color.FromArgb(237, 233, 254);  // #EDE9FE

        public static readonly Color StatusCompleted = Color.FromArgb(21, 128, 61);  // #15803D (Forest Green)
        public static readonly Color StatusCompletedBg = Color.FromArgb(220, 252, 231);// #DCFCE7

        public static readonly Color StatusCancelled = Color.FromArgb(185, 28, 28);  // #B91C1C (Crimson)
        public static readonly Color StatusCancelledBg = Color.FromArgb(254, 226, 226);// #FEE2E2

        public static readonly Color StatusNeutral = Color.FromArgb(87, 83, 78);     // #57534E
        public static readonly Color StatusNeutralBg = Color.FromArgb(245, 245, 244);// #F5F5F4

        // Hero Banner Gradient (Brick Oven Charcoal-to-Ember)
        public static readonly Color BannerGradientStart = Color.FromArgb(143, 35, 26); // #8F231A
        public static readonly Color BannerGradientEnd = Color.FromArgb(196, 61, 43);   // #C43D2B

        // Secondary Button Palette
        public static readonly Color SecondaryNormal = Color.FromArgb(243, 240, 234);
        public static readonly Color SecondaryHover = Color.FromArgb(232, 227, 219);
        public static readonly Color SecondaryPressed = Color.FromArgb(218, 212, 203);

        public static Color Lerp(Color from, Color to, float t)
        {
            if (t <= 0f) return from;
            if (t >= 1f) return to;
            int a = (int)(from.A + (to.A - from.A) * t);
            int r = (int)(from.R + (to.R - from.R) * t);
            int g = (int)(from.G + (to.G - from.G) * t);
            int b = (int)(from.B + (to.B - from.B) * t);
            return Color.FromArgb(ClampByte(a), ClampByte(r), ClampByte(g), ClampByte(b));
        }

        private static int ClampByte(int val) => val < 0 ? 0 : (val > 255 ? 255 : val);
    }
}
