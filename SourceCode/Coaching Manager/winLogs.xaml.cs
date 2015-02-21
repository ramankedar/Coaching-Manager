/// <aboutDev>
/// 
/// Project:
///     Coaching Manager (Coaching Mangement System)
/// 
/// Documentation:
///     Md. Mahmudul Hasan Shohag
///     Founder, CEO of Imaginative World
///     http://shohag.imaginativeworld.org
///     
/// Lisence:
///     Opensource project lisense under MPL 2.0.
///     Copyright © Imaginative World. All rights researved.
///     http://imaginativeworld.org
/// 
/// **************************************************
///     This Source Code Form is subject to the
///     terms of the Mozilla Public License, v.
///     2.0. If a copy of the MPL was not
///     distributed with this file, You can obtain
///     one at http://mozilla.org/MPL/2.0/.
/// **************************************************
/// 
/// </aboutDev>

using System;
using System.Windows;
using System.Windows.Input;
using System.Data.OleDb;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winLogs.xaml
    /// </summary>
    public partial class winLogs : Window
    {
        public winLogs()
        {
            InitializeComponent();
            SetValues();
            GetLogs();
        }

        private void SetValues()
        {
            lblWinTitle.Content = Title + " | " + Strings.InstituteName;

            if (Strings.IsAdmin)
                btnClear.IsEnabled = true;
            else
                btnClear.IsEnabled = false;
        }

        public class ListItem
        {
            public string Logs { get; set; }
        }

        private void GetLogs()
        {
            try
            {
                string queryString =
                    "SELECT [Log] FROM TblLogs";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                this.lstView.Items.Clear();

                while (reader.Read())
                {
                    this.lstView.Items.Add(new ListItem
                    {
                        Logs = reader[0].ToString()
                    });
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            MainWindow win = new MainWindow();
            win.Show();
            // this line for close this form. "this" means winStudentInfo Window.
            this.Close();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure want to Clear Logs?", "ATTENTATION", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                if (ClearLogs())
                {
                    lstView.Items.Clear();
                    cmTools.showInfoMsg("All Logs deleted successfully!");
                }
        }

        private Boolean ClearLogs()
        {
            try
            {
                string queryString =
                    "DELETE FROM TblLogs";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                this.lstView.Items.Clear();

                while (reader.Read())
                {
                    this.lstView.Items.Add(new ListItem
                    {
                        Logs = reader[0].ToString()
                    });
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void btnCornerMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCornerMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }
    }
}
