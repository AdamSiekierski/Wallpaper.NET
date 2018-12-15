using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Diagnostics;
using Tray;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;

namespace Wallpaper.NET
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        TrayClass trayClass = new TrayClass();
        EasterCalculator easterCalculator = new EasterCalculator();

        // When the window loads
        void OnLoad(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.DefaultWallpaper == "") // On first launch
            {
                firstLaunch();
            }
            else
            {
                setWallpaper();
                trayClass.ShowIcon();
            }
        }

        // Creme de la creme of .NET programming - .dll files import
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction,
            int uParam, string lpvParam, int fuWinIni);
        private static readonly int SPI_SETDESKWALLPAPER = 0x14;
        private static readonly int SPIF_UPDATEINIFILE = 0x01;
        private static readonly int SPIF_SENDWININICHANGE = 0x02;

        // Function to set the prorper wallpaper, and controll the interface
        private void setWallpaper()
        {
            // Get the date
            DateTime today = DateTime.Today;
            //DateTime today = new DateTime(2019, 04, 19); /// That is a backdoor for testing
            string month = today.ToString("MM");
            float day = float.Parse(today.ToString("dd"));

            // Set the date label to current date
            todayLabel.Content = today.ToString("dd.MM.yyyy");

            // Set the wallpaper appropriate to the date
            /// Christmas
            if (month == "12" && day <= 24)
            {
                string christmasPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Christmas"); // Create the path
                Properties.Resources.Christmas.Save(christmasPath, System.Drawing.Imaging.ImageFormat.Png); // Save a resource to Temp directory
                // User interface
                textBox.Text = "Christmas";
                image.Source = new BitmapImage(new Uri(christmasPath));
                // Set the wallpaper
                SetDesktopWallpaper(christmasPath);
            }
            /// Easter
            else if (easterCalculator.EasterSunday(today.Year).AddDays(-7) <= today && easterCalculator.EasterSunday(today.Year) >= today)
            {
                string easterPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Easter"); // Create the path
                Properties.Resources.Easter.Save(easterPath, System.Drawing.Imaging.ImageFormat.Png); // Save resource to the Temp directory
                // User interface
                textBox.Text = "Easter";
                image.Source = new BitmapImage(new Uri(easterPath));
                // Set the wallpaper
                SetDesktopWallpaper(easterPath);
            }
            /// Other dates
            else
            {
                textBox.Text = "Default WP";
                image.Source = new BitmapImage(new Uri(Properties.Settings.Default.DefaultWallpaper, UriKind.RelativeOrAbsolute));
                SetDesktopWallpaper(Properties.Settings.Default.DefaultWallpaper);
            }
        }


        // Quick code, to set the wallpaper
        private void SetDesktopWallpaper(string filename)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }


        // On the first launch
        private void firstLaunch()
        {
            // Show the message box to ask for the default wallpaper
            MessageBoxResult msgbox = MessageBox.Show("On first launch you need to choose the default wallpaper", "Default wallpaper", MessageBoxButton.OK, MessageBoxImage.Question);

            // Call the function for the default wallpaper file dialog; when didn't choose anything, close the app
            chooseWallpaper(true);
        }

        // Choose the default wallpeper dialog
        private void chooseWallpaper(bool FirstLaunch)
        {
            OpenFileDialog dlg = new OpenFileDialog(); // Create the file dialog object

            dlg.Filter = "Image files|*.jpeg; *.png; *.jpg; *.gif"; // Set filters for image file types
            dlg.Title = "Choose default wallpaper (Only the first use!)"; // Set the title of dialog

            Nullable<bool> result = dlg.ShowDialog(); // Show the dialog box
            if (result == true)
            {
                Properties.Settings.Default.DefaultWallpaper = dlg.FileName;
                Properties.Settings.Default.Save();
                setWallpaper();
            }
            else
            {
                if (FirstLaunch == true)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        // Change the default wallpaper
        private void button_Click(object sender, RoutedEventArgs e)
        {
            chooseWallpaper(false);
            setWallpaper();
        }

        // Don't close app, when X button clicked
        void HideNotClose(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}