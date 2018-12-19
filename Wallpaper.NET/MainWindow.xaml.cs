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
        public static MainWindow appWindow;

        public MainWindow()
        {
            InitializeComponent();
            appWindow = this;
        }

        TrayClass trayClass = new TrayClass();
        EasterCalculator easterCalculator = new EasterCalculator();
        WallpaperControl wallpaperControl = new WallpaperControl();

        // When the window loads
        void OnLoad(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.DefaultWallpaper == "") // On first launch
            {
                firstLaunch();
            }
            else
            {
                wallpaperControl.setWallpaper();
                appControl();
                trayClass.ShowIcon();
            }
        }


        // Function to set the prorper wallpaper, and controll the interface
        public void appControl()
        {
            // Get the date
            DateTime today = DateTime.Today;
            string month = today.ToString("MM");
            float day = float.Parse(today.ToString("dd"));

            // Set the date label to current date
            todayLabel.Content = today.ToString("dd.MM.yyyy");


            textBox.Text = wallpaperControl.currentSeason;
            image.Source = new BitmapImage(new Uri(wallpaperControl.currentSeasonPath));
        }



        // On the first launch
        private void firstLaunch()
        {
            // Show the message box to ask for the default wallpaper
            MessageBoxResult msgbox = MessageBox.Show("On first launch you need to choose the default wallpaper", "Default wallpaper", MessageBoxButton.OK, MessageBoxImage.Question);

            // Call the function for the default wallpaper file dialog; when didn't choose anything, close the app
            wallpaperControl.chooseWallpaper(true);
        }


        // Change the default wallpaper
        private void button_Click(object sender, RoutedEventArgs e)
        {
            wallpaperControl.chooseWallpaper(false);
            appControl();
        }

        // Don't close app, when X button clicked
        void HideNotClose(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}