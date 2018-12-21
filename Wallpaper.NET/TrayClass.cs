using System;
using System.Windows;
using Wallpaper.NET;

namespace Wallpaper.NET
{
    public class TrayClass
    {

        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private System.Windows.Forms.ContextMenu _contextMenu;

        public void ShowIcon()
        {
            // Create the objects
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _contextMenu = new System.Windows.Forms.ContextMenu();

            // Prepare the icon
            _notifyIcon.Icon = Wallpaper.NET.Properties.Resources.TrayIcon;
            _notifyIcon.ContextMenu = _contextMenu;
            _notifyIcon.Click += new EventHandler(NotifyIcon1_Click);

            // Prepare the context menu
            _contextMenu.MenuItems.Add("Show", Item1_onClick);
            _contextMenu.MenuItems.Add("Close", Item2_onClick);

            // Launch the icon
            _notifyIcon.Visible = true;

        }

        private void Item1_onClick(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }
        private void Item2_onClick(object sender, EventArgs e)
        {
            MessageBoxResult clsMsgBox = MessageBox.Show("Are you sure, that you would like to close the app?", "Close", MessageBoxButton.YesNo, MessageBoxImage.None);
            if (clsMsgBox == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        private void NotifyIcon1_Click(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }
    }
}