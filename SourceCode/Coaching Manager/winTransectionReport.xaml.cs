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
    /// Interaction logic for winTransectionReport.xaml
    /// </summary>
    public partial class winTransectionReport : Window
    {
        string monthName;
        DateTime strStartDate;
        DateTime strEndDate;

        public winTransectionReport()
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

        public class ListItemTransection
        {
            public string Month { get; set; }
            public string Get { get; set; }
            public string Give { get; set; }
            public string Cost { get; set; }
            public string Profit { get; set; }

            public string ForeColorProfit { get; set; }
        }

        private void DisableAll()
        {
            lstView.IsEnabled = false;
            txtYear.IsEnabled = false;
            btnQuery.IsEnabled = false;
            btnPrint.IsEnabled = false;
            btnZoomIn.IsEnabled = false;
            btnZoomOut.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
            lblQuerying.Visibility = Visibility.Visible;
        }

        private void EnableAll()
        {
            lstView.IsEnabled = true;
            txtYear.IsEnabled = true;
            btnQuery.IsEnabled = true;
            btnPrint.IsEnabled = true;
            btnZoomIn.IsEnabled = true;
            btnZoomOut.IsEnabled = true;
            btnPrevious.IsEnabled = true;
            btnNext.IsEnabled = true;
            lblQuerying.Visibility = Visibility.Collapsed;
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            lstView.Items.Clear();

            DisableAll();
            if (UpdateTable())
            {
                lblReportTitle.Content = string.Format(Strings.str_finance_report_header, txtYear.Text);
                lblReportTitle.Visibility = Visibility.Visible;
                EnableAll();
            }

        }

        private Boolean UpdateTable()
        {

            // the following code is for smooth UI flow.. :)
            Dispatcher.Invoke(new Action(() =>
            {
                this.lstView.Items.Clear();

                int count = 1, intGets, intGives, intCosts, intProfits;
                int intTotalGets = 0, intTotalGives = 0, intTotalCosts = 0, intTotalProfits = 0;

                for (; count <= 12; count++)
                {
                    intGets = GetGets(count);
                    intTotalGets += intGets;

                    intGives = GetGives(count);
                    intTotalGives += intGives;

                    intCosts = GetCosts(count);
                    intTotalCosts += intCosts;

                    intProfits = intGets - (intGives + intCosts);
                    intTotalProfits += intProfits;

                    switch (count)
                    {
                        case 1:
                            monthName = "January";
                            break;
                        case 2:
                            monthName = "February";
                            break;
                        case 3:
                            monthName = "March";
                            break;
                        case 4:
                            monthName = "April";
                            break;
                        case 5:
                            monthName = "May";
                            break;
                        case 6:
                            monthName = "June";
                            break;
                        case 7:
                            monthName = "July";
                            break;
                        case 8:
                            monthName = "August";
                            break;
                        case 9:
                            monthName = "September";
                            break;
                        case 10:
                            monthName = "October";
                            break;
                        case 11:
                            monthName = "November";
                            break;
                        case 12:
                            monthName = "December";
                            break;
                    }

                    //var selectedlistItem = lstView.Items[count] as ListItemTransection;
                    this.lstView.Items.Add(new ListItemTransection
                    {
                        Month = monthName,
                        Get = intGets.ToString() + Strings.str_currency_end_sign,
                        Give = intGives.ToString() + Strings.str_currency_end_sign,
                        Cost = intCosts.ToString() + Strings.str_currency_end_sign,
                        Profit = intProfits.ToString() + Strings.str_currency_end_sign,
                        ForeColorProfit = ProfitColor(intProfits)
                    });
                }

                this.lstView.Items.Add(new ListItemTransection
                {
                    Month = "------------------------------",
                    Get = "------------------------------",
                    Give = "------------------------------",
                    Cost = "------------------------------",
                    Profit = "------------------------------",
                    ForeColorProfit = "Black"
                });

                this.lstView.Items.Add(new ListItemTransection
                    {
                        Month = "TOTAL",
                        Get = intTotalGets.ToString() + Strings.str_currency_end_sign,
                        Give = intTotalGives.ToString() + Strings.str_currency_end_sign,
                        Cost = intTotalCosts.ToString() + Strings.str_currency_end_sign,
                        Profit = intTotalProfits.ToString() + Strings.str_currency_end_sign,
                        ForeColorProfit = ProfitColor(intTotalProfits)
                    });

            }), DispatcherPriority.ContextIdle);

            return true;

        }

        private string ProfitColor(int ProfitValue)
        {

            if (ProfitValue > 0)
                return "DarkGreen";
            else if (ProfitValue == 0)
                return "Goldenrod";
            else
                return "Red";

        }

        private int GetGets(int Month) // All Getting payments
        {
            try
            {
                string queryString =
                    "SELECT [Fee] from TblFee WHERE [Year] = @Year AND [Month] = @Month";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);


                command.Parameters.AddWithValue("@Year", txtYear.Text);
                command.Parameters.AddWithValue("@Month", Month);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                int TotalFee = 0;

                while (reader.Read())
                {
                    TotalFee += int.Parse(reader[0].ToString());
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                return TotalFee;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        private int GetGives(int Month) // All Giving payments
        {
            try
            {
                string queryString =
                    "SELECT [Amount] from TblTeacherPayment WHERE [Year] = @Year AND [Month] = @Month";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);


                command.Parameters.AddWithValue("@Year", txtYear.Text);
                command.Parameters.AddWithValue("@Month", Month);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                int TotalPay = 0;

                while (reader.Read())
                {
                    TotalPay += int.Parse(reader[0].ToString());
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                return TotalPay;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        private int GetCosts(int Month) // All Extra Costs
        {
            /* 
             * Remember:
             * Costs are added/subtracted from previous month
             */

            try
            {
                string queryString =
                    "SELECT [Cost] from TblCost WHERE [Date] BETWEEN @strStartDate AND @strEndDate";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                if (Month == 12)
                {
                    /* 
                     * if Month is December then Cost will added/subtracted from next year's January month
                     */

                    strStartDate = new DateTime(int.Parse(txtYear.Text) + 1, 1, 1);
                    strEndDate = new DateTime(int.Parse(txtYear.Text) + 1, 1, DateTime.DaysInMonth(int.Parse(txtYear.Text) + 1, 1));
                }
                else
                {
                    strStartDate = new DateTime(int.Parse(txtYear.Text), Month + 1, 1);
                    strEndDate = new DateTime(int.Parse(txtYear.Text), Month + 1, DateTime.DaysInMonth(int.Parse(txtYear.Text), Month + 1));
                }

                command.Parameters.AddWithValue("@strStartDate", strStartDate);
                command.Parameters.AddWithValue("@strEndDate", strEndDate);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                int TotalPay = 0;

                while (reader.Read())
                {
                    TotalPay += int.Parse(reader[0].ToString());
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                return TotalPay;

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
                printDialog.PrintVisual(gFinanceReport, Strings.str_print_finance_report);

                //cmTools.AddLog(Strings.str_, this.Title);
            }
        }
    }
}
