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
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winStudentPayViewer.xaml
    /// </summary>
    public partial class winStudentPayViewer : Window
    {
        public winStudentPayViewer()
        {
            InitializeComponent();
            SetValues();
        }

        private void SetValues()
        {
            lblWinTitle.Content = Title + " | " + Strings.AppName + " | " + Strings.InstituteName;
            txtYear.Text = DateTime.Now.Year.ToString();

            lblQuerying.Visibility = Visibility.Collapsed;
            lblReportTitle.Visibility = Visibility.Collapsed;
        }

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public class ListItem
        {
            public string ID { get; set; }
            public string StudentName { get; set; }

            public string Jan { get; set; }
            public string Feb { get; set; }
            public string Mar { get; set; }
            public string Apr { get; set; }
            public string May { get; set; }
            public string Jun { get; set; }
            public string Jul { get; set; }
            public string Aug { get; set; }
            public string Sep { get; set; }
            public string Oct { get; set; }
            public string Nov { get; set; }
            public string Dec { get; set; }
        }

        private void DisableAll()
        {
            lstView.IsEnabled = false;
            cmbBxSelClass.IsEnabled = false;
            txtYear.IsEnabled = false;
            btnQuery.IsEnabled = false;
            btnPrint.IsEnabled = false;
            btnZoomIn.IsEnabled = false;
            btnZoomOut.IsEnabled = false;
            lblQuerying.Visibility = Visibility.Visible;
        }

        private void EnableAll()
        {
            lstView.IsEnabled = true;
            cmbBxSelClass.IsEnabled = true;
            txtYear.IsEnabled = true;
            btnQuery.IsEnabled = true;
            btnPrint.IsEnabled = true;
            btnZoomIn.IsEnabled = true;
            btnZoomOut.IsEnabled = true;
            lblQuerying.Visibility = Visibility.Collapsed;
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            // the following code is for smooth UI flow.. :)
            Dispatcher.Invoke(new Action(() =>
            {
                lstView.Items.Clear();

                DisableAll();
                if (UpdateTable())
                {
                    lblReportTitle.Content = string.Format(Strings.str_student_payment_report_header, txtYear.Text, cmbBxSelClass.Text);
                    lblReportTitle.Visibility = Visibility.Visible;
                    EnableAll();
                }
            }), DispatcherPriority.ContextIdle);
        }

        private Boolean UpdateTable()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                int TotalStudent;
                TotalStudent = SearchStudent(cmbBxSelClass.SelectedIndex);

                for (int count = 0; count < TotalStudent; count++)
                {
                    var selectedlistItem = lstView.Items[count] as ListItem;
                    GetPayment(selectedlistItem.ID, count);
                }
            }), DispatcherPriority.ContextIdle);

            return true;

        }

        private void GetPayment(string studentID, int RowNumber)
        {
            try
            {
                string queryString =
                    "SELECT [Month], [Fee] from TblFee WHERE [ID] = @srcID AND [Year] = @strYear";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);


                command.Parameters.AddWithValue("@srcID", studentID);
                command.Parameters.AddWithValue("@strYear", txtYear.Text);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                var selectedlistItem = lstView.Items[RowNumber] as ListItem;

                while (reader.Read())
                {
                    switch (Convert.ToInt32(reader[0]))
                    {
                        case 1:
                            selectedlistItem.Jan = reader[1].ToString();
                            break;
                        case 2:
                            selectedlistItem.Feb = reader[1].ToString();
                            break;
                        case 3:
                            selectedlistItem.Mar = reader[1].ToString();
                            break;
                        case 4:
                            selectedlistItem.Apr = reader[1].ToString();
                            break;
                        case 5:
                            selectedlistItem.May = reader[1].ToString();
                            break;
                        case 6:
                            selectedlistItem.Jun = reader[1].ToString();
                            break;
                        case 7:
                            selectedlistItem.Jul = reader[1].ToString();
                            break;
                        case 8:
                            selectedlistItem.Aug = reader[1].ToString();
                            break;
                        case 9:
                            selectedlistItem.Sep = reader[1].ToString();
                            break;
                        case 10:
                            selectedlistItem.Oct = reader[1].ToString();
                            break;
                        case 11:
                            selectedlistItem.Nov = reader[1].ToString();
                            break;
                        case 12:
                            selectedlistItem.Dec = reader[1].ToString();
                            break;
                    }

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

        private int SearchStudent(int Class)
        {
            try
            {
                string queryString =
                    "SELECT [ID], [Name] from TblStudent WHERE [Class] = @srcClass AND [IsActive] = @srcIsActive";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                command.Parameters.AddWithValue("@srcClass", Class.ToString());

                command.Parameters.AddWithValue("@srcIsActive", true);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                this.lstView.Items.Clear();

                while (reader.Read())
                {
                    this.lstView.Items.Add(new ListItem
                    {
                        ID = reader[0].ToString(),
                        StudentName = reader[1].ToString()
                    });
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                return this.lstView.Items.Count;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            MainWindow win = new MainWindow();
            win.Show();
            // this line for close this form.
            this.Close();
        }

        private void btnCornerMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
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

        private void btnCornerMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void txtYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (lstView.FontSize < 20)
                lstView.FontSize = ++lstView.FontSize;
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (lstView.FontSize > 10)
                lstView.FontSize = --lstView.FontSize;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(gPayment, Strings.str_print_payment_report);

                //cmTools.AddLog(Strings.str_, this.Title);
            }
        }
    }
}
