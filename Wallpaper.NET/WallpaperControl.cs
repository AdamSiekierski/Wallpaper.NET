using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wallpaper.NET;
using System.Windows;

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
            ///DateTime today = DateTime.Today;
            DateTime today = new DateTime(2018, 12, 27);
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

        // Choose the default wallpeper dialog
        public void chooseWallpaper(bool FirstLaunch)
        {
            OpenFileDialog dlg = new OpenFileDialog(); // Create the file dialog object

            dlg.Filter = "Image files|*.jpeg; *.png; *.jpg; *.gif"; // Set filters for image file types
            dlg.Title = "Choose default wallpaper (Only the first use!)"; // Set the title of dialog

            DialogResult result = dlg.ShowDialog(); // Show the dialog box
            if (result == DialogResult.Yes)
            {
                Properties.Settings.Default.DefaultWallpaper = dlg.FileName;
                Properties.Settings.Default.Save();
                MainWindow.appWindow.appControl();
            }
            else
            {
                if (FirstLaunch == true)
                {
                    System.Windows.Application.Current.Shutdown();
                }
            }
        }

        public string currentSeason;
        public string currentSeasonPath;
    }
}
