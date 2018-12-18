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
        private void appControl()
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
                appControl();
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