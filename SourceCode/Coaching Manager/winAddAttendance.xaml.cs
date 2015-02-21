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
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.OleDb;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winAddAttendance.xaml
    /// </summary>
    public partial class winAddAttendance : Window
    {
        Boolean IsAdded = false;

        public winAddAttendance()
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
            var lastday = month.AddDays(-1);

            lblWinTitle.Content = Title + " | " + Strings.InstituteName;
            txtYear.Text = DateTime.Today.Year.ToString();
            cmbBxMonth.SelectedIndex = first.Month - 1;
            txtTotalDay.Text = lastday.Day.ToString();
        }

        public class listItem
        {
            public string CoachingRoll { get; set; }
            public string StudentName { get; set; }
            public string StudentPresent { get; set; }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (IsAdded)
                cmTools.AddLog("Attendance Added.", this.Title);

            // To Show a window we need to write below two line
            MainWindow win = new MainWindow();
            win.Show();
            // this line for close this form.
            this.Close();
        }

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void cmbBxClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBxClass.SelectedIndex != -1)
                searchNow();
        }

        private void searchNow()
        {
            try
            {
                string queryString =
                    "SELECT [ID], [Name] from TblStudent WHERE [Class] = @srcClass AND [IsActive] = @srcIsActive";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                command.Parameters.AddWithValue("@srcClass", cmbBxClass.SelectedIndex.ToString());

                command.Parameters.AddWithValue("@srcIsActive", true);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                this.lstView.Items.Clear();

                while (reader.Read())
                {
                    this.lstView.Items.Add(new listItem
                    {
                        CoachingRoll = reader[0].ToString(),
                        StudentName = reader[1].ToString()
                    });
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                if (this.lstView.Items.Count != 0)
                    lstView.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lstView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtID.Text = "";
            lblName.Content = "...";
            try
            {
                var selectedlistItem = lstView.SelectedItems[0] as listItem;
                if (selectedlistItem == null)
                {
                    return;
                }

                txtID.Text = selectedlistItem.CoachingRoll;
                lblName.Content = selectedlistItem.StudentName;

                if (string.IsNullOrEmpty(selectedlistItem.StudentPresent))
                    btnAdd.Content = " Add";
                else
                    btnAdd.Content = " Update";
            }
            catch (Exception) { }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if ((lstView.Items.Count != 0) && (lstView.SelectedIndex != 0))
                lstView.SelectedIndex = lstView.SelectedIndex - 1;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if ((lstView.Items.Count != 0) && (lstView.SelectedIndex != lstView.Items.Count - 1))
                lstView.SelectedIndex = lstView.SelectedIndex + 1;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var selectedlistItem = lstView.SelectedItem as listItem;

            if (lblName.Content.ToString() != "...")
                if (string.IsNullOrEmpty(selectedlistItem.StudentPresent))
                    AddAttendance();
                else
                    UpdateAttendance();

        }

        private void AddAttendance()
        {

            {
                try
                {
                    OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                    OleDbCommand command = new OleDbCommand(@"INSERT INTO TblAttendance 
            ([ID], [Month], [Year], [TotalStudyDay], [Present])
            VALUES (@ID, @Month, @Year, @TotalStudyDay, @Present)", connection);

                    //int intMonth = cmbBxMonth.SelectedIndex + 1;

                    command.Parameters.AddWithValue("@ID", txtID.Text); // Datatype: Integer (2 bytes)
                    command.Parameters.AddWithValue("@Month", cmbBxMonth.SelectedIndex + 1); // Datatype: Integer (2 bytes)
                    command.Parameters.AddWithValue("@Year", txtYear.Text); // Datatype: Integer (2 bytes)
                    command.Parameters.AddWithValue("@TotalStudyDay", txtTotalDay.Text); // Datatype: Byte [Only number from 0 to 255)
                    command.Parameters.AddWithValue("@Present", txtPresent.Text); // Datatype: Byte [Only number from 0 to 255)

                    if ((txtID.Text == "") || (txtYear.Text == "") || (txtTotalDay.Text == "")
                        || (txtPresent.Text == ""))
                    {
                        cmTools.showInfoMsg("Fill up all required fields.");
                    }
                    else
                    {

                        connection.Open();

                        command.ExecuteNonQuery();

                        //Update Present Field
                        var selectedlistItem = lstView.SelectedItem as listItem;
                        selectedlistItem.StudentPresent = txtPresent.Text;
                        lstView.Items.Refresh();

                        // Go to next row
                        if (chkGoNextStdnt.IsChecked == true)
                            if ((lstView.Items.Count != 0) && (lstView.SelectedIndex != lstView.Items.Count - 1))
                                lstView.SelectedIndex = lstView.SelectedIndex + 1;
                            else
                                cmTools.showInfoMsg("No more student found!");

                        IsAdded = true;

                    }

                    // Release Memory
                    connection.Close();
                    command.Dispose();
                    connection.Dispose();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateAttendance()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(@"UPDATE TblAttendance 
SET [Present] = @strPresent, [TotalStudyDay] = @strTotalStudyDay 
WHERE [ID] = @strID AND [Month] = @strMonth AND [Year] = @strYear", connection);

                command.Parameters.AddWithValue("@strID", txtID.Text); // Datatype: Integer (2 bytes)
                command.Parameters.AddWithValue("@strMonth", cmbBxMonth.SelectedIndex + 1); // Datatype: Integer (2 bytes)
                command.Parameters.AddWithValue("@strYear", txtYear.Text); // Datatype: Integer (2 bytes)
                command.Parameters.AddWithValue("@strPresent", txtTotalDay.Text); // Datatype: Byte [Only number from 0 to 255)
                command.Parameters.AddWithValue("@strTotalStudyDay", txtPresent.Text); // Datatype: Byte [Only number from 0 to 255)

                connection.Open();

                command.ExecuteNonQuery();

                // Release Memory
                connection.Close();
                command.Dispose();
                connection.Dispose();

                //Update Present Field
                var selectedlistItem = lstView.SelectedItem as listItem;
                selectedlistItem.StudentPresent = txtPresent.Text;
                lstView.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPresent_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPresent.Text != "")
                if (Convert.ToInt32(txtPresent.Text) > Convert.ToInt32(txtTotalDay.Text))
                {
                    cmTools.showInfoMsg("Total Present is greter then Total Study Day!");
                    txtPresent.Text = txtTotalDay.Text;
                }
        }

        private void btnCornerMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
