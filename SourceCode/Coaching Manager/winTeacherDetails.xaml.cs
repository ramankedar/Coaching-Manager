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
using System.Text.RegularExpressions;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winTeacherDetails.xaml
    /// </summary>
    public partial class winTeacherDetails : Window
    {
        public winTeacherDetails()
        {
            InitializeComponent();
            SetValues();
            searchTeachers();
        }

        public class listItemTeachers
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }

        public class listItemPayment
        {
            public string PayYear { get; set; }
            public string PayMonth { get; set; }
            public string PayDate { get; set; }
            public string Amount { get; set; }
        }

        private void SetValues()
        {
            lblWinTitle.Content = Title + " | " + Strings.AppName + " | " + Strings.InstituteName;
        }

        private void EnableAllField()
        {
            txtName.IsEnabled = true;
            cmbBxSex.IsEnabled = true;
            txtInstitute.IsEnabled = true;
            txtQualification.IsEnabled = true;
            txtSub.IsEnabled = true;
            txtMobNo.IsEnabled = true;
            txtPayScale.IsEnabled = true;
        }

        private void DisableAllField()
        {
            txtName.IsEnabled = false;
            cmbBxSex.IsEnabled = false;
            txtInstitute.IsEnabled = false;
            txtQualification.IsEnabled = false;
            txtSub.IsEnabled = false;
            txtMobNo.IsEnabled = false;
            txtPayScale.IsEnabled = false;
        }

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void searchTeachers()
        {
            try
            {
                string queryString =
                    "SELECT [ID], [Name] from TblTeacher WHERE [IsActive] = @srcIsActive";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                command.Parameters.AddWithValue("@srcIsActive", true);

                //Console.WriteLine(command.CommandText.ToString());

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                this.lstViewTeachers.Items.Clear();

                while (reader.Read())
                {
                    this.lstViewTeachers.Items.Add(new listItemTeachers
                    {
                        ID = reader[0].ToString(),
                        Name = reader[1].ToString()
                    });
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                if (this.lstViewTeachers.Items.Count != 0)
                    lstViewTeachers.SelectedIndex = 0;
                //else
                //    ResetAllField();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.IsEnabled == false)
            {
                // To Show a window we need to write below two line
                MainWindow win = new MainWindow();
                win.Show();
                // this line for close this form.
                this.Close();
            }
            else
            {
                DisableAllField();
                btnEditInfo.Content = " Edit About";
                btnCngActive.IsEnabled = true;
                lstViewTeachers.IsEnabled = true;
                btnBack.Content = " Back";

                //Get ID Field from List
                var selectedlistItem = lstViewTeachers.SelectedItem as listItemTeachers;
                ShowTeachersDetails(selectedlistItem.ID);
            }
        }

        private void ShowTeachersDetails(string studentID)
        {
            try
            {
                string queryString =
                    @"SELECT 
                [Name], [Sex], [InstituteName], [Qualification], [Subject], [MobileNumber], [PayScale], [JoinDate]
                from TblTeacher WHERE [ID] = @srcID";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                command.Parameters.AddWithValue("@srcID", studentID);

                //Console.WriteLine(command.CommandText.ToString());

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                //this.lstViewStudents.Items.Clear();

                while (reader.Read())
                {
                    txtName.Text = reader[0].ToString();
                    cmbBxSex.SelectedIndex = Convert.ToInt32(reader[1]);
                    txtInstitute.Text = reader[2].ToString();
                    txtQualification.Text = reader[3].ToString();
                    txtSub.Text = reader[4].ToString();
                    txtMobNo.Text = reader[5].ToString();
                    txtPayScale.Text = reader[6].ToString();
                    txtJoinDate.Text = Convert.ToDateTime(reader[7]).ToShortDateString();
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();


                GetPayment(studentID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetPayment(string studentID)
        {
            try
            {

                string queryString =
                    "SELECT [Year], [Month], [Date], [Amount] from TblTeacherPayment WHERE [ID] = @srcID";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                command.Parameters.AddWithValue("@srcID", studentID);

                //Console.WriteLine(command.CommandText.ToString());

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                this.lstViewPayment.Items.Clear();

                while (reader.Read())
                {
                    this.lstViewPayment.Items.Add(new listItemPayment
                    {
                        PayYear = reader[0].ToString(),
                        PayMonth = reader[1].ToString(),
                        PayDate = Convert.ToDateTime(reader[2]).ToShortDateString(),
                        Amount = reader[3].ToString()
                    });
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                //if (this.lstView.Items.Count != 0)
                //    lstView.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lstViewTeachers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedlistItem = lstViewTeachers.SelectedItems[0] as listItemTeachers;
                if (selectedlistItem == null)
                {
                    return;
                }

                ShowTeachersDetails(selectedlistItem.ID);
                btnCngActive.IsEnabled = true;
                btnEditInfo.IsEnabled = true;
            }
            catch (Exception) { }
        }

        private void btnCngActive_Click(object sender, RoutedEventArgs e)
        {
            ChangeActivity(false);
        }

        private void ChangeActivity(Boolean IsActive)
        {
            try
            {

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(@"UPDATE TblTeacher 
            SET [IsActive] = @strIsActive WHERE [ID] = @ID", connection);

                command.Parameters.AddWithValue("@strIsActive", IsActive); // Datatype: Boolean

                //Get ID Field from List
                var selectedlistItem = lstViewTeachers.SelectedItem as listItemTeachers;
                command.Parameters.AddWithValue("@ID", selectedlistItem.ID);

                connection.Open();

                command.ExecuteNonQuery();

                // Release Memory
                connection.Close();
                command.Dispose();
                connection.Dispose();

                cmTools.showInfoMsg(txtName.Text + " marked as Inactive.");

                searchTeachers();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnEditInfo_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.IsEnabled == true)
            {
                if (SaveTeacherAboutInfo())
                {
                    DisableAllField();
                    btnEditInfo.Content = " Edit About";
                    cmTools.showInfoMsg("Info Updated!");
                    btnCngActive.IsEnabled = true;
                    lstViewTeachers.IsEnabled = true;
                    btnBack.Content = " Back";
                }
                else
                    cmTools.showInfoMsg("Something Wrong!");
            }
            else
            {
                EnableAllField();
                btnCngActive.IsEnabled = false;
                lstViewTeachers.IsEnabled = false;
                btnEditInfo.Content = " Save About";
                btnBack.Content = " Cancel";
            }
        }

        private Boolean SaveTeacherAboutInfo()
        {
            try
            {

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(@"UPDATE TblTeacher 
            SET [Name] = @Name, [Sex] = @Sex, [InstituteName] = @InstituteName, [Qualification] = @Qualification,
[Subject] = @Subject, [MobileNumber] = @MobileNumber, [PayScale] = @PayScale
WHERE [ID] = @ID", connection);

                command.Parameters.AddWithValue("@Name", txtName.Text); // Datatype: Max 255 Char
                command.Parameters.AddWithValue("@Sex", cmbBxSex.SelectedIndex); // Datatype: Boolean
                command.Parameters.AddWithValue("@InstituteName", txtInstitute.Text); // Datatype: Max 255 Char
                command.Parameters.AddWithValue("@Qualification", txtQualification.Text); // Datatype: Max 255 Char
                command.Parameters.AddWithValue("@Subject", txtSub.Text); // Datatype: Max 255 Char
                command.Parameters.AddWithValue("@MobileNumber", (txtMobNo.Text != "") ? txtMobNo.Text : "-"); // Datatype: 50 Char
                command.Parameters.AddWithValue("@PayScale", (txtPayScale.Text != "") ? txtPayScale.Text : "0"); // Datatype: Integer (2 bytes)
                //command.Parameters.AddWithValue("@JoinDate", datePickerJoin.SelectedDate); // Datatype: Date/Time
                //command.Parameters.AddWithValue("@IsActive", true); // Datatype: Boolean

                //Get ID Field from List
                var selectedlistItem = lstViewTeachers.SelectedItem as listItemTeachers;
                command.Parameters.AddWithValue("@ID", selectedlistItem.ID);

                if ((txtName.Text == "") || (txtInstitute.Text == "") || (txtQualification.Text == "")
                        || (txtSub.Text == ""))
                {
                    MessageBox.Show("Fill up all required fields.");
                }
                else
                {

                    connection.Open();

                    command.ExecuteNonQuery();

                    // Release Memory
                    connection.Close();
                    command.Dispose();
                    connection.Dispose();

                    cmTools.AddLog("Info Updated: Teacher \"" + txtName.Text + "\" (ID: " + selectedlistItem.ID + ")", this.Title);
                }

                // Release Memory
                command.Dispose();
                connection.Dispose();

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;
        }

        private void btnCornerMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void txtPayScale_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
