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

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winStudentDetails.xaml
    /// </summary>
    public partial class winStudentDetails : Window
    {
        public winStudentDetails()
        {
            InitializeComponent();
            SetValues();
        }

        public class listItemStudents
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }

        public class listItemAttendance
        {
            public string attYear { get; set; }
            public string attMonth { get; set; }
            public string TotalStudyDay { get; set; }
            public string Attendance { get; set; }
        }

        public class listItemExamResult
        {
            public string ExamDate { get; set; }
            public string ExamSubjects { get; set; }
            public string TotalMarks { get; set; }
            public string Achievement { get; set; }
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

        private void ResetAllField()
        {
            txtName.Text = "";
            cmbBxSex.SelectedIndex = -1;
            txtFatherName.Text = "";
            txtMotherName.Text = "";
            cmbBxReligion.SelectedIndex = -1;
            txtPresentAddr.Text = "";
            txtPermanentAddr.Text = "";
            txtStdMob.Text = "";
            txtGuardianMob.Text = "";
            txtEmail.Text = "";
            txtInstitute.Text = "";
            txtRoll.Text = "";
            cmbBxGroup.SelectedIndex = -1;
            txtBatchNo.Text = "";
            txtFee.Text = "";
            txtAdmissionDate.Text = "";
            cmbBxClass.SelectedIndex = -1;

            btnCngActive.IsEnabled = false;
            btnUpgradeClass.IsEnabled = false;
            btnEditInfo.IsEnabled = false;
        }

        private void EnableAllField()
        {
            txtName.IsEnabled = true;
            cmbBxSex.IsEnabled = true;
            txtFatherName.IsEnabled = true;
            txtMotherName.IsEnabled = true;
            cmbBxReligion.IsEnabled = true;
            txtPresentAddr.IsEnabled = true;
            txtPermanentAddr.IsEnabled = true;
            txtStdMob.IsEnabled = true;
            txtGuardianMob.IsEnabled = true;
            txtEmail.IsEnabled = true;
            txtInstitute.IsEnabled = true;
            txtRoll.IsEnabled = true;
            cmbBxGroup.IsEnabled = true;
            txtBatchNo.IsEnabled = true;
            txtFee.IsEnabled = true;
            cmbBxClass.IsEnabled = true;
            //txtAdmissionDate.IsEnabled = true;
        }

        private void DisableAllField()
        {
            txtName.IsEnabled = false;
            cmbBxSex.IsEnabled = false;
            txtFatherName.IsEnabled = false;
            txtMotherName.IsEnabled = false;
            cmbBxReligion.IsEnabled = false;
            txtPresentAddr.IsEnabled = false;
            txtPermanentAddr.IsEnabled = false;
            txtStdMob.IsEnabled = false;
            txtGuardianMob.IsEnabled = false;
            txtEmail.IsEnabled = false;
            txtInstitute.IsEnabled = false;
            txtRoll.IsEnabled = false;
            cmbBxGroup.IsEnabled = false;
            txtBatchNo.IsEnabled = false;
            txtFee.IsEnabled = false;
            txtAdmissionDate.IsEnabled = false;
            cmbBxClass.IsEnabled = false;
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
                btnUpgradeClass.IsEnabled = true;
                cmbBxSelClass.IsEnabled = true;
                lstViewStudents.IsEnabled = true;
                btnBack.Content = " Back";

                //Get ID Field from List
                var selectedlistItem = lstViewStudents.SelectedItem as listItemStudents;
                ShowStudentsDetails(selectedlistItem.ID);
            }
        }

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void cmbBxSelClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBxSelClass.SelectedIndex != -1)
                searchStudents();
        }

        private void searchStudents()
        {
            try
            {
                string queryString =
                    "SELECT [ID], [Name] from TblStudent WHERE [Class] = @srcClass AND [IsActive] = @srcIsActive";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                command.Parameters.AddWithValue("@srcClass", cmbBxSelClass.SelectedIndex.ToString());
                command.Parameters.AddWithValue("@srcIsActive", true);

                //Console.WriteLine(command.CommandText.ToString());

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                this.lstViewStudents.Items.Clear();

                while (reader.Read())
                {
                    this.lstViewStudents.Items.Add(new listItemStudents
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

                if (this.lstViewStudents.Items.Count != 0)
                    lstViewStudents.SelectedIndex = 0;
                else
                    ResetAllField();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lstViewStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedlistItem = lstViewStudents.SelectedItems[0] as listItemStudents;
                if (selectedlistItem == null)
                {
                    return;
                }

                ShowStudentsDetails(selectedlistItem.ID);
                btnCngActive.IsEnabled = true;
                btnUpgradeClass.IsEnabled = true;
                btnEditInfo.IsEnabled = true;
            }
            catch (Exception) { }
        }

        private void ShowStudentsDetails(string studentID)
        {
            try
            {
                string queryString =
                    @"SELECT 
                [Name],[Sex],[FatherName],[MotherName],[Religion],[PresentAddress],[PermanentAddress],[StudentMobileNumber],
                [GuardianMobileNumber],[Email],[SchoolName],[Roll],[Group],[Batch],[MonthlyFee],[AdmissionDate]
                from TblStudent WHERE [ID] = @srcID";

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
                    txtFatherName.Text = reader[2].ToString();
                    txtMotherName.Text = reader[3].ToString();
                    cmbBxReligion.SelectedIndex = Convert.ToInt32(reader[4]);
                    txtPresentAddr.Text = reader[5].ToString();
                    txtPermanentAddr.Text = reader[6].ToString();
                    txtStdMob.Text = reader[7].ToString();
                    txtGuardianMob.Text = reader[8].ToString();
                    txtEmail.Text = reader[9].ToString();
                    txtInstitute.Text = reader[10].ToString();
                    txtRoll.Text = reader[11].ToString();
                    cmbBxGroup.SelectedIndex = Convert.ToInt32(reader[12]);
                    txtBatchNo.Text = reader[13].ToString();
                    txtFee.Text = reader[14].ToString();
                    txtAdmissionDate.Text = Convert.ToDateTime(reader[15]).ToShortDateString();
                }

                cmbBxClass.SelectedIndex = cmbBxSelClass.SelectedIndex;

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                //if (this.lstViewStudents.Items.Count != 0)
                //    lstViewStudents.SelectedIndex = 0;

                GetAttendance(studentID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetAttendance(string studentID)
        {
            try
            {

                string queryString =
                    "SELECT [Month], [Year], [TotalStudyDay], [Present] from TblAttendance WHERE [ID] = @srcID";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);


                command.Parameters.AddWithValue("@srcID", studentID);

                //Console.WriteLine(command.CommandText.ToString());

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                this.lstViewAttendance.Items.Clear();

                while (reader.Read())
                {
                    this.lstViewAttendance.Items.Add(new listItemAttendance
                    {
                        attMonth = reader[0].ToString(),
                        attYear = reader[1].ToString(),
                        TotalStudyDay = reader[2].ToString(),
                        Attendance = reader[3].ToString()
                    });
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                //if (this.lstView.Items.Count != 0)
                //    lstView.SelectedIndex = 0;
                GetExamResult(studentID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetExamResult(string studentID)
        {
            try
            {

                string queryString =
                    "SELECT [Date], [Subjects], [TotalMarks], [Achievement] from TblExam WHERE [ID] = @srcID";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);


                command.Parameters.AddWithValue("@srcID", studentID);

                //Console.WriteLine(command.CommandText.ToString());

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                this.lstViewExamResult.Items.Clear();

                while (reader.Read())
                {
                    this.lstViewExamResult.Items.Add(new listItemExamResult
                    {
                        ExamDate = Convert.ToDateTime(reader[0]).ToShortDateString(),
                        ExamSubjects = reader[1].ToString(),
                        TotalMarks = reader[2].ToString(),
                        Achievement = reader[3].ToString()
                    });
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                //if (this.lstView.Items.Count != 0)
                //    lstView.SelectedIndex = 0;
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
                    "SELECT [Year], [Month], [Date], [Fee] from TblFee WHERE [ID] = @srcID";

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

        private void btnEditInfo_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.IsEnabled == true)
            {
                if (SaveStudentAboutInfo())
                {
                    DisableAllField();
                    btnEditInfo.Content = " Edit About";
                    cmTools.showInfoMsg("Info Updated!");
                    btnCngActive.IsEnabled = true;
                    btnUpgradeClass.IsEnabled = true;
                    cmbBxSelClass.IsEnabled = true;
                    lstViewStudents.IsEnabled = true;
                    btnBack.Content = " Back";

                    if (cmbBxClass.SelectedIndex != cmbBxSelClass.SelectedIndex)
                        searchStudents();
                }
                else
                    cmTools.showInfoMsg("Something Wrong!");
            }
            else
            {
                EnableAllField();
                btnCngActive.IsEnabled = false;
                btnUpgradeClass.IsEnabled = false;
                cmbBxSelClass.IsEnabled = false;
                lstViewStudents.IsEnabled = false;
                btnEditInfo.Content = " Save About";
                btnBack.Content = " Cancel";
            }
        }

        private Boolean SaveStudentAboutInfo()
        {
            try
            {

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(@"UPDATE TblStudent 
            SET [Name] = @Name, [Sex] = @Sex, [FatherName] = @FatherName, [MotherName] = @MotherName, [Religion] = @Religion,
            [PresentAddress] = @PresentAddress, [PermanentAddress] = @PermanentAddress,
            [StudentMobileNumber] = @StudentMobileNumber, [GuardianMobileNumber] = @GuardianMobileNumber,
            [Email] = @Email, [SchoolName] = @SchoolName, [Roll] = @Roll, [Class] = @Class, [Group] = @Group, [Batch] = @Batch,
            [MonthlyFee] = @MonthlyFee WHERE [ID] = @ID", connection);

                command.Parameters.AddWithValue("@Name", txtName.Text); // [Required] // Datatype: Max 255 Char
                command.Parameters.AddWithValue("@Sex", cmbBxSex.SelectedIndex); // Datatype: Boolean
                command.Parameters.AddWithValue("@FatherName", (txtFatherName.Text != "") ? txtFatherName.Text : "-"); // Datatype: Max 255 Char
                command.Parameters.AddWithValue("@MotherName", (txtMotherName.Text != "") ? txtMotherName.Text : "-"); // Datatype: Max 255 Char
                command.Parameters.AddWithValue("@Religion", cmbBxReligion.SelectedIndex); // Datatype: Byte [Only number from 0 to 255]
                command.Parameters.AddWithValue("@PresentAddress", (txtPresentAddr.Text != "") ? txtPresentAddr.Text : "-"); // Datatype: Max 255 Char
                command.Parameters.AddWithValue("@PermanentAddress", (txtPermanentAddr.Text != "") ? txtPermanentAddr.Text : "-"); // Datatype: Max 255 Char
                command.Parameters.AddWithValue("@StudentMobileNumber", (txtStdMob.Text != "") ? txtStdMob.Text : "-"); // Datatype: Max 50 Char
                command.Parameters.AddWithValue("@GuardianMobileNumber", (txtGuardianMob.Text != "") ? txtGuardianMob.Text : "-"); // Datatype: Max 50 Char
                command.Parameters.AddWithValue("@Email", (txtEmail.Text != "") ? txtEmail.Text : "-"); // Datatype: Max 255 Char
                command.Parameters.AddWithValue("@SchoolName", txtInstitute.Text); // [Required] // Datatype: Max 255 Char
                command.Parameters.AddWithValue("@Roll", txtRoll.Text); // [Required] // Datatype: Integer (2 bytes)
                command.Parameters.AddWithValue("@Class", cmbBxClass.SelectedIndex.ToString()); // [Required] // Datatype: Byte [Only number from 0 to 255]
                command.Parameters.AddWithValue("@Group", cmbBxGroup.SelectedIndex); // [Required] // Datatype: Byte [Only number from 0 to 255]
                command.Parameters.AddWithValue("@Batch", (txtBatchNo.Text != "") ? txtBatchNo.Text : "-"); // Datatype: Max 255 Char
                command.Parameters.AddWithValue("@MonthlyFee", (txtFee.Text != "") ? txtFee.Text : "0"); // Datatype: Integer (2 bytes)
                //command.Parameters.AddWithValue("@AdmissionDate", datePickerAdmission.SelectedDate); // [Required] // Datatype: Date/Time
                //command.Parameters.AddWithValue("@IsActive", "1"); // Datatype: Boolean
                
                //Get ID Field from List
                var selectedlistItem = lstViewStudents.SelectedItem as listItemStudents;
                command.Parameters.AddWithValue("@ID", selectedlistItem.ID);

                if ((txtName.Text == "") || (txtInstitute.Text == "") || (txtRoll.Text == ""))
                {
                    MessageBox.Show("Fill up all required fields.");
                }
                else
                {

                    connection.Open();

                    command.ExecuteNonQuery();

                    cmTools.AddLog("Info Updated: Student \"" + txtName.Text + "\" (ID: " + selectedlistItem.ID + ")", this.Title);

                    // Release Memory
                    connection.Close();
                    command.Dispose();
                    connection.Dispose();
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

        private void btnUpgradeClass_Click(object sender, RoutedEventArgs e)
        {
            if (cmbBxSelClass.SelectedIndex < 12)
            {
                if (UpgradeOneClass())
                {
                    cmTools.showInfoMsg(txtName.Text + " Upgraded to Class " + (cmbBxSelClass.SelectedIndex + 1).ToString() + ".");
                    searchStudents();
                }
            }
            else
                cmTools.showInfoMsg(txtName.Text + " already in highest class. You can make him/her Inactive.");
        }

        private Boolean UpgradeOneClass()
        {
            try
            {

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(@"UPDATE TblStudent 
            SET [Class] = @Class WHERE [ID] = @ID", connection);

                int intClass = cmbBxSelClass.SelectedIndex + 1;

                command.Parameters.AddWithValue("@Class", intClass); // Datatype: Bytes [int 0 to 255]

                //Get ID Field from List
                var selectedlistItem = lstViewStudents.SelectedItem as listItemStudents;
                command.Parameters.AddWithValue("@ID", selectedlistItem.ID);

                connection.Open();

                command.ExecuteNonQuery();

                // Release Memory
                connection.Close();
                command.Dispose();
                connection.Dispose();

                cmTools.AddLog("Upgraded: Student \"" + txtName.Text + "\" (ID: " + selectedlistItem.ID + ") upgraded to Class " + intClass, this.Title);

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;


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
                OleDbCommand command = new OleDbCommand(@"UPDATE TblStudent 
            SET [IsActive] = @strIsActive WHERE [ID] = @ID", connection);

                command.Parameters.AddWithValue("@strIsActive", IsActive); // Datatype: Boolean

                //Get ID Field from List
                var selectedlistItem = lstViewStudents.SelectedItem as listItemStudents;
                command.Parameters.AddWithValue("@ID", selectedlistItem.ID);

                connection.Open();

                command.ExecuteNonQuery();

                // Release Memory
                connection.Close();
                command.Dispose();
                connection.Dispose();

                cmTools.AddLog("Student \"" + txtName.Text + "\" (ID: " + selectedlistItem.ID + ") marked as Inactive.", this.Title);


                cmTools.showInfoMsg(txtName.Text + " marked as Inactive.");

                searchStudents();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnCornerMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void txtRoll_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
