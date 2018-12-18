using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wallpaper.NET
{
    class WallpaperControl
    {
        // Creme de la creme of .NET programming - .dll files import
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction,
            int uParam, string lpvParam, int fuWinIni);
        private static readonly int SPI_SETDESKWALLPAPER = 0x14;
        private static readonly int SPIF_UPDATEINIFILE = 0x01;
        private static readonly int SPIF_SENDWININICHANGE = 0x02;

        EasterCalculator easterCalculator = new EasterCalculator();

        public void setWallpaper()
        {
            // Get the date
            DateTime today = DateTime.Today;
            string month = today.ToString("MM");
            float day = float.Parse(today.ToString("dd"));
            // Set the wallpaper appropriate to the date
            /// Christmas
            if (month == "12" && day <= 24)
            {
                string christmasPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ChristmasWallpaper.png"); // Create the path
                Properties.Resources.Christmas.Save(christmasPath, System.Drawing.Imaging.ImageFormat.Png); // Save resource to the Temp directory
                currentSeasonPath = christmasPath;
                currentSeason = "Christmas";
                
                // Set the wallpaper
                SetDesktopWallpaper(christmasPath);
            }
            /// Easter
            else if (easterCalculator.EasterSunday(today.Year).AddDays(-7) <= today && easterCalculator.EasterSunday(today.Year) >= today)
            {
                string easterPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "EasterWallpaper.png"); // Create the path
                Properties.Resources.Easter.Save(easterPath, System.Drawing.Imaging.ImageFormat.Png); // Save resource to the Temp directory
                currentSeasonPath = easterPath;
                currentSeason = "Easter";
                
                // Set the wallpaper
                SetDesktopWallpaper(easterPath);
            }
            /// Other dates
            else
            {
                currentSeasonPath = Properties.Settings.Default.DefaultWallpaper;
                currentSeason = "Deafult WP";
                
                // Set the wallpaper
                SetDesktopWallpaper(Properties.Settings.Default.DefaultWallpaper);
            }
        }

        // Quick code, to set the wallpaper
        private void SetDesktopWallpaper(string filename)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

        public string currentSeason;
        public string currentSeasonPath;
    }
}
