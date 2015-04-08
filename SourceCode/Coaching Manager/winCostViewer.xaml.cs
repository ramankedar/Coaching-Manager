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
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winCostViewer.xaml
    /// </summary>
    public partial class winCostViewer : Window
    {
        public winCostViewer()
        {
            InitializeComponent();
            SetValues();

        }

        private void SetValues()
        {
            lblQuerying.Visibility = Visibility.Collapsed;
            lblWinTitle.Content = Title + " | " + Strings.AppName + " | " + Strings.InstituteName;
            txtYear.Text = DateTime.Now.Year.ToString();
        }

        public class ListItem
        {
            public string Date { get; set; }
            public string Title { get; set; }
            public string Cost { get; set; }
        }

        private void EnableAll()
        {
            lstView.IsEnabled = true;
            txtYear.IsEnabled = true;
            btnPrevious.IsEnabled = true;
            btnNext.IsEnabled = true;
            cmbBxMonth.IsEnabled = true;
            btnQuery.IsEnabled = true;

            lblQuerying.Visibility = Visibility.Collapsed;
        }

        private void DisableAll()
        {
            lstView.IsEnabled = false;
            txtYear.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
            cmbBxMonth.IsEnabled = false;
            btnQuery.IsEnabled = false;

            lblQuerying.Visibility = Visibility.Visible;
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            DisableAll();
            if (GetCosts())
            {
                EnableAll();
            }
        }

        private bool GetCosts()
        {
            try
            {
                string queryString =
                    "SELECT [Date],[Title],[Cost] from TblCost WHERE [Date] BETWEEN @strStartDate AND @strEndDate";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);


                var strStartDate = new DateTime(int.Parse(txtYear.Text), cmbBxMonth.SelectedIndex + 1, 1);
                var strEndDate = new DateTime(int.Parse(txtYear.Text), cmbBxMonth.SelectedIndex + 1, DateTime.DaysInMonth(int.Parse(txtYear.Text), cmbBxMonth.SelectedIndex + 1));


                command.Parameters.AddWithValue("@strStartDate", strStartDate);
                command.Parameters.AddWithValue("@strEndDate", strEndDate);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                // the following code is for smooth UI flow.. :)
                Dispatcher.Invoke(new Action(() =>
                {
                    lstView.Items.Clear();

                    while (reader.Read())
                    {
                        this.lstView.Items.Add(new ListItem
                        {
                            Date = Convert.ToDateTime(reader[0]).ToLongDateString(),
                            Title = reader[1].ToString(),
                            Cost = reader[2].ToString()
                        });
                    }

                }), DispatcherPriority.ContextIdle);

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

        private void btnCornerMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnCornerMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            int i = Convert.ToInt32(txtYear.Text);
            if (i > 2000)
                txtYear.Text = (i - 1).ToString();

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            int i = Convert.ToInt32(txtYear.Text);
            if (i < 2099)
                txtYear.Text = (i + 1).ToString();
        }

        private void txtYear_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!((Convert.ToDecimal(txtYear.Text) > 2000) && (Convert.ToDecimal(txtYear.Text) < 2099)))
            {
                int year = DateTime.Now.Year;
                txtYear.Text = year.ToString();
                cmTools.showInfoMsg(string.Format(Strings.str_invalid_year_changed_to_default_year, year));
            }
        }

        private void txtYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
