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
    /// Interaction logic for winStudentReport.xaml
    /// </summary>
    public partial class winStudentReport : Window
    {
        public winStudentReport()
        {
            InitializeComponent();
            SetValues();
        }

        private void SetValues()
        {
            //for getting last month
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var first = month.AddMonths(-1);

            lblWinTitle.Content = Title + " | " + Strings.AppName + " | " + Strings.InstituteName;
            txtYear.Text = DateTime.Now.Year.ToString();
            cmbBxMonth.SelectedIndex = first.Month - 1;
            lblQuerying.Visibility = Visibility.Collapsed;
            lblReportTitle.Visibility = Visibility.Collapsed;
        }

        public class ListItem
        {
            public string ID { get; set; }
            public string StudentName { get; set; }
            public string Attendance { get; set; }

            public string day1 { get; set; }
            public string day2 { get; set; }
            public string day3 { get; set; }
            public string day4 { get; set; }
            public string day5 { get; set; }
            public string day6 { get; set; }
            public string day7 { get; set; }
            public string day8 { get; set; }
            public string day9 { get; set; }
            public string day10 { get; set; }
            public string day11 { get; set; }
            public string day12 { get; set; }
            public string day13 { get; set; }
            public string day14 { get; set; }
            public string day15 { get; set; }
            public string day16 { get; set; }
            public string day17 { get; set; }
            public string day18 { get; set; }
            public string day19 { get; set; }
            public string day20 { get; set; }
            public string day21 { get; set; }
            public string day22 { get; set; }
            public string day23 { get; set; }
            public string day24 { get; set; }
            public string day25 { get; set; }
            public string day26 { get; set; }
            public string day27 { get; set; }
            public string day28 { get; set; }
            public string day29 { get; set; }
            public string day30 { get; set; }
            public string day31 { get; set; }

            public string ForeColorAttendance { get; set; }
            public string ForeColor1 { get; set; }
            public string ForeColor2 { get; set; }
            public string ForeColor3 { get; set; }
            public string ForeColor4 { get; set; }
            public string ForeColor5 { get; set; }
            public string ForeColor6 { get; set; }
            public string ForeColor7 { get; set; }
            public string ForeColor8 { get; set; }
            public string ForeColor9 { get; set; }
            public string ForeColor10 { get; set; }
            public string ForeColor11 { get; set; }
            public string ForeColor12 { get; set; }
            public string ForeColor13 { get; set; }
            public string ForeColor14 { get; set; }
            public string ForeColor15 { get; set; }
            public string ForeColor16 { get; set; }
            public string ForeColor17 { get; set; }
            public string ForeColor18 { get; set; }
            public string ForeColor19 { get; set; }
            public string ForeColor20 { get; set; }
            public string ForeColor21 { get; set; }
            public string ForeColor22 { get; set; }
            public string ForeColor23 { get; set; }
            public string ForeColor24 { get; set; }
            public string ForeColor25 { get; set; }
            public string ForeColor26 { get; set; }
            public string ForeColor27 { get; set; }
            public string ForeColor28 { get; set; }
            public string ForeColor29 { get; set; }
            public string ForeColor30 { get; set; }
            public string ForeColor31 { get; set; }

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

        private void btnCornerMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
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

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void DisableAll()
        {
            lstView.IsEnabled = false;
            cmbBxSelClass.IsEnabled = false;
            txtYear.IsEnabled = false;
            btnQuery.IsEnabled = false;
            cmbBxMonth.IsEnabled = false;
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
            cmbBxMonth.IsEnabled = true;
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
                    lblReportTitle.Content = string.Format(Strings.str_student_report_header, cmbBxMonth.Text, txtYear.Text, cmbBxSelClass.Text);
                    lblReportTitle.Visibility = Visibility.Visible;
                    EnableAll();
                }
            }), DispatcherPriority.ContextIdle);
        }

        private Boolean UpdateTable()
        {
            /*
             * Theory
             * ------
             * -> Update Table with Students Name
             * -> Get Students Attendance and fill the table
             * -> Get Students Exam Result one by one using loop and fill the table
             */
            Dispatcher.Invoke(new Action(() =>
            {
                if (ReSetAll())
                {
                    int month = cmbBxMonth.SelectedIndex + 1;

                    int TotalStudent = SearchStudent(cmbBxSelClass.SelectedIndex);
                    if (TotalStudent > 0)
                    {
                        if (GetAttendance(month, txtYear.Text))
                        {
                            for (int i = 0; i < TotalStudent; i++)
                            {
                                var selectedlistItem = lstView.Items[i] as ListItem;
                                GetExamResults(selectedlistItem.ID, i, month, Convert.ToInt32(txtYear.Text));
                            }
                        }
                    }
                }
            }), DispatcherPriority.ContextIdle);

            return true;

        }

        private Boolean ReSetAll()
        {
            Dispatcher.Invoke(new Action(() =>
               {
                   this.lstView.Items.Clear();

                   colAttendance.Header = "Attendance";

                   for (int i = 1; i <= 31; i++)
                       (FindName("colDay" + i) as System.Windows.Controls.GridViewColumn).Width = 0;
               }), DispatcherPriority.ContextIdle);

            return true;
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

        private bool GetAttendance(int strMonth, string strYear)
        {
            try
            {

                string queryString =
                    "SELECT [ID], [TotalStudyDay], [Present] from TblAttendance WHERE [Year] = @strYear AND [Month] = @strMonth";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);


                command.Parameters.AddWithValue("@strYear", strYear);
                command.Parameters.AddWithValue("@strMonth", strMonth);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                bool flag = false;

                while (reader.Read())
                {

                    for (int i = 0; i < lstView.Items.Count; i++)
                    {
                        var selectedlistItem = lstView.Items[i] as ListItem;
                        if (reader[0].ToString() == selectedlistItem.ID)
                        {
                            selectedlistItem.Attendance = reader[2].ToString() + " (" + reader[1].ToString() + ")";
                            selectedlistItem.ForeColorAttendance = AchievementColor(reader[1].ToString(), reader[2].ToString());
                            flag = true;
                        }
                    }
                }
                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                return flag;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error On Querying Attendance!" + Environment.NewLine + ex.Message);
                return false;
            }
        }

        private void GetExamResults(string studentID, int RowNumber, int intMonth, int intYear)
        {
            try
            {
                string queryString =
                    "SELECT [Date], [Achievement], [TotalMarks]  from TblExam WHERE [ID] = @strID AND [Date] BETWEEN @strStartDate AND @strEndDate";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                var strStartDate = new DateTime(intYear, intMonth, 1);
                var strEndDate = new DateTime(intYear, intMonth, DateTime.DaysInMonth(intYear, intMonth));

                command.Parameters.AddWithValue("@strID", studentID);
                command.Parameters.AddWithValue("@strStartDate", strStartDate);
                command.Parameters.AddWithValue("@strEndDate", strEndDate);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                var selectedlistItem = lstView.Items[RowNumber] as ListItem;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        switch (Convert.ToDateTime(reader[0]).Day)
                        {
                            case 1:
                                selectedlistItem.day1 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor1 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 2:
                                selectedlistItem.day2 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor2 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 3:
                                selectedlistItem.day3 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor3 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 4:
                                selectedlistItem.day4 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor4 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 5:
                                selectedlistItem.day5 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor5 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 6:
                                selectedlistItem.day6 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor6 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 7:
                                selectedlistItem.day7 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor7 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 8:
                                selectedlistItem.day8 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor8 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 9:
                                selectedlistItem.day9 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor9 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 10:
                                selectedlistItem.day10 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor10 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 11:
                                selectedlistItem.day11 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor11 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 12:
                                selectedlistItem.day12 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor12 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 13:
                                selectedlistItem.day13 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor13 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 14:
                                selectedlistItem.day14 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor14 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 15:
                                selectedlistItem.day15 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor15 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 16:
                                selectedlistItem.day16 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor16 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 17:
                                selectedlistItem.day17 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor17 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 18:
                                selectedlistItem.day18 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor18 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 19:
                                selectedlistItem.day19 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor19 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 20:
                                selectedlistItem.day20 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor20 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 21:
                                selectedlistItem.day21 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor21 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 22:
                                selectedlistItem.day22 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor22 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 23:
                                selectedlistItem.day23 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor23 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 24:
                                selectedlistItem.day24 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor24 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 25:
                                selectedlistItem.day25 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor25 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 26:
                                selectedlistItem.day26 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor26 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 27:
                                selectedlistItem.day27 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor27 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 28:
                                selectedlistItem.day28 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor28 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 29:
                                selectedlistItem.day29 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor29 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 30:
                                selectedlistItem.day30 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor30 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                            case 31:
                                selectedlistItem.day31 = reader[1].ToString() + " (" + reader[2].ToString() + ")";
                                selectedlistItem.ForeColor31 = AchievementColor(reader[2].ToString(), reader[1].ToString());
                                break;
                        }

                        (FindName("colDay" + Convert.ToDateTime(reader[0]).Day) as System.Windows.Controls.GridViewColumn)
                            .Width = 120;
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

        private string AchievementColor(string TotalMarks, string Achievement)
        {

            float Achieve = Convert.ToInt32(Achievement) * 100 / Convert.ToInt32(TotalMarks);

            if (Achieve > 80)
                return "DarkGreen";
            else if (Achieve > 70)
                return "LimeGreen";
            else if (Achieve > 50)
                return "Goldenrod";
            else if (Achieve > 30)
                return "Tomato";
            else
                return "Red";

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
                printDialog.PrintVisual(gStudentReport, Strings.str_print_student_report);

                //cmTools.AddLog(Strings.str_, this.Title);
            }

        }
    }
}
