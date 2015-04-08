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
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winStudentDetails.xaml
    /// </summary>
    public partial class winStudentDetails : Window
    {

        string imgSource = "";

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
            public int attYear { get; set; }
            public int attMonth { get; set; }
            public int TotalStudyDay { get; set; }
            public int Attendance { get; set; }
        }

        public class listItemExamResult
        {
            public string ExamDate { get; set; }
            public string ExamSubjects { get; set; }
            public int TotalMarks { get; set; }
            public int Achievement { get; set; }
        }

        public class listItemPayment
        {
            public int PayYear { get; set; }
            public int PayMonth { get; set; }
            public string PayDate { get; set; }
            public int Amount { get; set; }
        }

        private void SetValues()
        {
            lblWinTitle.Content = Title + " | " + Strings.AppName + " | " + Strings.InstituteName;
            btnBrowseImg.Visibility = Visibility.Collapsed;
            btnDeleteImg.Visibility = Visibility.Collapsed;

            lblQusInstitute.Visibility = Visibility.Hidden;

            lblQusInstitute.ToolTip = Strings.str_tips_institute_name_change_location;

            cmTools.GetInstituteNames(cmbBxInstituteName.Items);

            if (Strings.IsAdmin == false)
            {
                btnDelAttendance.Visibility = Visibility.Collapsed;
                btnDelExam.Visibility = Visibility.Collapsed;
                btnDelPayment.Visibility = Visibility.Collapsed;
            }
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
            cmbBxInstituteName.SelectedIndex = -1;
            txtRoll.Text = "";
            cmbBxGroup.SelectedIndex = -1;
            txtBatchNo.Text = "";
            txtFee.Text = "";
            txtAdmissionDate.Text = "";
            cmbBxClass.SelectedIndex = -1;
            imgStdnt.Source = null;

            lblQusInstitute.Visibility = Visibility.Hidden;

            btnCngActive.IsEnabled = false;
            btnUpgradeClass.IsEnabled = false;
            btnEditInfo.IsEnabled = false;

            lstViewAttendance.Items.Clear();
            lstViewExamResult.Items.Clear();
            lstViewPayment.Items.Clear();

            scrollView.ScrollToTop();
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
            cmbBxInstituteName.IsEnabled = true;
            txtRoll.IsEnabled = true;
            cmbBxGroup.IsEnabled = true;
            txtBatchNo.IsEnabled = true;
            txtFee.IsEnabled = true;
            cmbBxClass.IsEnabled = true;

            lblQusInstitute.Visibility = Visibility.Visible;

            btnBrowseImg.Visibility = Visibility.Visible;
            if (imgStdnt.Source != null)
                btnDeleteImg.Visibility = Visibility.Visible;

            scrollView.ScrollToTop();
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
            cmbBxInstituteName.IsEnabled = false;
            txtRoll.IsEnabled = false;
            cmbBxGroup.IsEnabled = false;
            txtBatchNo.IsEnabled = false;
            txtFee.IsEnabled = false;
            txtAdmissionDate.IsEnabled = false;
            cmbBxClass.IsEnabled = false;


            lblQusInstitute.Visibility = Visibility.Hidden;

            btnBrowseImg.Visibility = Visibility.Collapsed;
            btnDeleteImg.Visibility = Visibility.Collapsed;

            imgSource = "";
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

                //if (this.lstViewStudents.Items.Count != 0)
                //    lstViewStudents.SelectedIndex = 0;
                //else
                //    ResetAllField();
                // Upper code decrease speed..
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
                scrollView.ScrollToTop();
            }
            catch (Exception) { }
        }

        private void ShowStudentsDetails(string studentID)
        {
            try
            {
                string queryString =
                    @"SELECT 
                [Name],[Image],[Sex],[FatherName],[MotherName],[Religion],[PresentAddress],[PermanentAddress],[StudentMobileNumber],
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


                    if (reader[1].ToString() != "")
                    {
                        byte[] data = (byte[])reader[1];

                        MemoryStream strm = new MemoryStream();
                        strm.Write(data, 0, data.Length);
                        strm.Position = 0;
                        System.Drawing.Image img = System.Drawing.Image.FromStream(strm);

                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                        ms.Seek(0, SeekOrigin.Begin);

                        bi.StreamSource = ms;
                        bi.EndInit();

                        imgStdnt.Source = bi;
                    }
                    else
                        imgStdnt.Source = null;

                    cmbBxSex.SelectedIndex = Convert.ToInt32(reader[2]);
                    txtFatherName.Text = reader[3].ToString();
                    txtMotherName.Text = reader[4].ToString();
                    cmbBxReligion.SelectedIndex = Convert.ToInt32(reader[5]);
                    txtPresentAddr.Text = reader[6].ToString();
                    txtPermanentAddr.Text = reader[7].ToString();
                    txtStdMob.Text = reader[8].ToString();
                    txtGuardianMob.Text = reader[9].ToString();
                    txtEmail.Text = reader[10].ToString();

                    string instituteName = reader[11].ToString();

                    if (cmbBxInstituteName.Items.IndexOf(instituteName) == -1)
                    {
                        cmTools.AddInstituteName(instituteName);
                        cmTools.GetInstituteNames(cmbBxInstituteName.Items);
                        cmbBxInstituteName.SelectedIndex = cmbBxInstituteName.Items.IndexOf(instituteName);
                    }
                    else
                        cmbBxInstituteName.SelectedIndex = cmbBxInstituteName.Items.IndexOf(instituteName);

                    //txtInstitute.Text = reader[11].ToString();

                    txtRoll.Text = reader[12].ToString();
                    cmbBxGroup.SelectedIndex = Convert.ToInt32(reader[13]);
                    txtBatchNo.Text = reader[14].ToString();
                    txtFee.Text = reader[15].ToString();
                    txtAdmissionDate.Text = Convert.ToDateTime(reader[16]).ToShortDateString();
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
                        attMonth = Convert.ToInt32(reader[0].ToString()),
                        attYear = Convert.ToInt32(reader[1].ToString()),
                        TotalStudyDay = Convert.ToInt32(reader[2].ToString()),
                        Attendance = Convert.ToInt32(reader[3].ToString())
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
                        TotalMarks = Convert.ToInt32(reader[2]),
                        Achievement = Convert.ToInt32(reader[3])
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
                        PayYear = Convert.ToInt32(reader[0].ToString()),
                        PayMonth = Convert.ToInt32(reader[1].ToString()),
                        PayDate = Convert.ToDateTime(reader[2]).ToShortDateString(),
                        Amount = Convert.ToInt32(reader[3].ToString())
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
                    btnCngActive.IsEnabled = true;
                    btnUpgradeClass.IsEnabled = true;
                    cmbBxSelClass.IsEnabled = true;
                    lstViewStudents.IsEnabled = true;

                    btnBack.Content = " Back";

                    if (cmbBxClass.SelectedIndex != cmbBxSelClass.SelectedIndex)
                        searchStudents();
                }
                else
                    cmTools.showInfoMsg(Strings.str_something_wrong);
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
            if ((txtName.Text == "") || (cmbBxInstituteName.SelectedIndex == -1) || (txtRoll.Text == ""))
            {
                MessageBox.Show(Strings.strFillupAllFields);
            }
            else
            {
                try
                {

                    OleDbConnection connection = new OleDbConnection(Strings.DBconStr);

                    /*
                     * Important
                     * Must maintain the AddWithValue field order with comment sql,
                     * Otherwise sql give you weird (O.o) result..
                     */

                    OleDbCommand command = new OleDbCommand(@"UPDATE TblStudent 
SET [Name] = @Name"
    + (((imgStdnt.Source != null) && (imgSource == "")) ? "" : ",[Image] = @Image") +
    @", [Sex] = @Sex, [FatherName] = @FatherName, [MotherName] = @MotherName, [Religion] = @Religion,
[PresentAddress] = @PresentAddress, [PermanentAddress] = @PermanentAddress,
[StudentMobileNumber] = @StudentMobileNumber, [GuardianMobileNumber] = @GuardianMobileNumber,
[Email] = @Email, [SchoolName] = @SchoolName, [Class] = @strClass, [Roll] = @Roll,  [Group] = @Group, [Batch] = @Batch,
[MonthlyFee] = @MonthlyFee WHERE [ID] = @ID", connection);

                    command.Parameters.AddWithValue("@Name", txtName.Text); // [Required] // Datatype: Max 255 Char

                    if ((imgStdnt.Source != null) && (imgSource != ""))
                    {
                        // Get image from file
                        FileStream fs = new FileStream(imgSource, FileMode.Open, FileAccess.Read);
                        byte[] imgData = new byte[fs.Length];
                        fs.Read(imgData, 0, Convert.ToInt32(fs.Length));
                        fs.Close();

                        command.Parameters.AddWithValue("@Image", imgData); // Datatype: OLE Object
                    }
                    else if ((imgStdnt.Source == null) && (imgSource == ""))
                        command.Parameters.AddWithValue("@Image", DBNull.Value); // Datatype: OLE Object

                    command.Parameters.AddWithValue("@Sex", cmbBxSex.SelectedIndex); // Datatype: Boolean

                    if (txtFatherName.Text != "")
                        command.Parameters.AddWithValue("@FatherName", txtFatherName.Text); // Datatype: Max 255 Char
                    else
                        command.Parameters.AddWithValue("@FatherName", DBNull.Value); // Datatype: Max 255 Char

                    if (txtMotherName.Text != "")
                        command.Parameters.AddWithValue("@MotherName", txtMotherName.Text); // Datatype: Max 255 Char
                    else
                        command.Parameters.AddWithValue("@MotherName", DBNull.Value); // Datatype: Max 255 Char

                    command.Parameters.AddWithValue("@Religion", cmbBxReligion.SelectedIndex); // Datatype: Byte [Only number from 0 to 255]

                    if (txtPresentAddr.Text != "")
                        command.Parameters.AddWithValue("@PresentAddress", txtPresentAddr.Text); // Datatype: Max 255 Char
                    else
                        command.Parameters.AddWithValue("@PresentAddress", DBNull.Value); // Datatype: Max 255 Char

                    if (txtPermanentAddr.Text != "")
                        command.Parameters.AddWithValue("@PermanentAddress", txtPermanentAddr.Text); // Datatype: Max 255 Char
                    else
                        command.Parameters.AddWithValue("@PermanentAddress", DBNull.Value); // Datatype: Max 255 Char

                    if (txtStdMob.Text != "")
                        command.Parameters.AddWithValue("@StudentMobileNumber", txtStdMob.Text); // Datatype: Max 50 Char
                    else
                        command.Parameters.AddWithValue("@StudentMobileNumber", DBNull.Value); // Datatype: Max 50 Char

                    if (txtGuardianMob.Text != "")
                        command.Parameters.AddWithValue("@GuardianMobileNumber", txtGuardianMob.Text); // Datatype: Max 50 Char
                    else
                        command.Parameters.AddWithValue("@GuardianMobileNumber", DBNull.Value); // Datatype: Max 50 Char

                    if (txtEmail.Text != "")
                        command.Parameters.AddWithValue("@Email", txtEmail.Text); // Datatype: Max 255 Char
                    else
                        command.Parameters.AddWithValue("@Email", DBNull.Value); // Datatype: Max 255 Char

                    command.Parameters.AddWithValue("@SchoolName", cmbBxInstituteName.SelectedValue); // [Required] // Datatype: Max 255 Char

                    command.Parameters.AddWithValue("@strClass", cmbBxClass.SelectedIndex); // [Required] // Datatype: Byte [Only number from 0 to 255]

                    command.Parameters.AddWithValue("@Roll", txtRoll.Text); // [Required] // Datatype: Integer (2 bytes)
                    command.Parameters.AddWithValue("@Group", cmbBxGroup.SelectedIndex); // [Required] // Datatype: Byte [Only number from 0 to 255]

                    if (txtBatchNo.Text != "")
                        command.Parameters.AddWithValue("@Batch", txtBatchNo.Text); // Datatype: Max 255 Char
                    else
                        command.Parameters.AddWithValue("@Batch", DBNull.Value); // Datatype: Max 255 Char

                    command.Parameters.AddWithValue("@MonthlyFee", (txtFee.Text != "") ? txtFee.Text : "0"); // Datatype: Integer (2 bytes)
                    //command.Parameters.AddWithValue("@AdmissionDate", datePickerAdmission.SelectedDate); // [Required] // Datatype: Date/Time
                    //command.Parameters.AddWithValue("@IsActive", "1"); // Datatype: Boolean

                    //Get ID Field from List
                    var selectedlistItem = lstViewStudents.SelectedItem as listItemStudents;
                    command.Parameters.AddWithValue("@ID", selectedlistItem.ID);



                    connection.Open();

                    command.ExecuteNonQuery();

                    // Release Memory
                    connection.Close();
                    command.Dispose();
                    connection.Dispose();

                    cmTools.AddLog(string.Format(Strings.str_object_info_updated, "Student", txtName.Text, selectedlistItem.ID), this.Title);
                    cmTools.showInfoMsg(string.Format(Strings.str_object_info_updated, "Student", txtName.Text, selectedlistItem.ID));



                    // Release Memory
                    command.Dispose();
                    connection.Dispose();

                    return true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return false;
        }

        private void btnUpgradeClass_Click(object sender, RoutedEventArgs e)
        {
            if (cmbBxSelClass.SelectedIndex < 12)
            {
                if (UpgradeOneClass())
                {
                    searchStudents();
                }
            }
            else
                cmTools.showInfoMsg(string.Format(Strings.str_student_already_in_highest_class, txtName.Text));
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

                cmTools.AddLog(string.Format(Strings.str_log_student_upgraded_one_class, txtName.Text, selectedlistItem.ID, intClass), this.Title);
                cmTools.showInfoMsg(string.Format(Strings.str_log_student_upgraded_one_class, txtName.Text, selectedlistItem.ID, intClass));

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

                cmTools.AddLog(string.Format(Strings.str_log_object_active_mark_changed, "Student", txtName.Text, selectedlistItem.ID, "Inactive"), this.Title);
                cmTools.showInfoMsg(string.Format(Strings.str_log_object_active_mark_changed, "Student", txtName.Text, selectedlistItem.ID, "Inactive"));

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

        private void btnBrowseImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "JPEG Image|*.jpg";
            openFileDlg.Title = Strings.AppName + " | Open Student Image";
            openFileDlg.Multiselect = false;
            openFileDlg.CheckFileExists = true;

            if ((bool)openFileDlg.ShowDialog())
            {
                if (openFileDlg.FileName != "")
                {
                    // Check file size
                    FileInfo ImgInfo = new FileInfo(openFileDlg.FileName);
                    if ((ImgInfo.Length / 1024) <= 512)
                    {
                        imgStdnt.Source = new BitmapImage(new Uri(openFileDlg.FileName));
                        imgSource = openFileDlg.FileName;

                        btnDeleteImg.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        imgSource = "";
                        cmTools.showInfoMsg(Strings.strImgBigFileSize);
                    }
                }
                else
                    imgSource = "";
            }
            else
                imgSource = "";
        }

        private void btnDeleteImg_Click(object sender, RoutedEventArgs e)
        {
            imgStdnt.Source = null;
            imgSource = "";
            btnDeleteImg.Visibility = Visibility.Collapsed;
        }

        private void btnDelAttendance_Click(object sender, RoutedEventArgs e)
        {
            if (lstViewAttendance.SelectedIndex != -1)
                DeleteAttendance();
        }

        private void DeleteAttendance()
        {
            if (MessageBox.Show(Strings.str_confirmation_delete_attendance, Strings.str_attention, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                    OleDbCommand command = new OleDbCommand(@"DELETE FROM TblAttendance
WHERE [ID] = @strID AND [Month] = @strMonth AND [Year] = @strYear", connection);

                    var selectedlistItem = lstViewAttendance.SelectedItem as listItemAttendance;
                    var selectedStudentListItem = lstViewStudents.SelectedItem as listItemStudents;

                    command.Parameters.AddWithValue("@strID", selectedStudentListItem.ID);
                    command.Parameters.AddWithValue("@strMonth", selectedlistItem.attMonth);
                    command.Parameters.AddWithValue("@strYear", selectedlistItem.attYear);
                    connection.Open();

                    command.ExecuteNonQuery();

                    // Release Memory
                    connection.Close();
                    command.Dispose();
                    connection.Dispose();

                    lstViewAttendance.Items.RemoveAt(lstViewAttendance.SelectedIndex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnDelExam_Click(object sender, RoutedEventArgs e)
        {
            if (lstViewExamResult.SelectedIndex != -1)
                DeleteExam();
        }

        private void DeleteExam()
        {
            if (MessageBox.Show(Strings.str_confirmation_delete_exam, Strings.str_attention, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                    OleDbCommand command = new OleDbCommand(@"DELETE FROM TblExam
WHERE [ID] = @strID AND [Date] = @strDate AND [Subjects] = @strSubject", connection);

                    var selectedlistItem = lstViewExamResult.SelectedItem as listItemExamResult;
                    var selectedStudentListItem = lstViewStudents.SelectedItem as listItemStudents;

                    command.Parameters.AddWithValue("@strID", selectedStudentListItem.ID);
                    command.Parameters.AddWithValue("@strDate", Convert.ToDateTime(selectedlistItem.ExamDate));
                    command.Parameters.AddWithValue("@strSubject", selectedlistItem.ExamSubjects);
                    connection.Open();

                    command.ExecuteNonQuery();

                    // Release Memory
                    connection.Close();
                    command.Dispose();
                    connection.Dispose();

                    lstViewExamResult.Items.RemoveAt(lstViewExamResult.SelectedIndex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnDelPayment_Click(object sender, RoutedEventArgs e)
        {
            if (lstViewPayment.SelectedIndex != -1)
                DeletePayment();
        }

        private void DeletePayment()
        {
            if (MessageBox.Show(Strings.str_confirmation_delete_payment, Strings.str_attention, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                    OleDbCommand command = new OleDbCommand(@"DELETE FROM TblFee
WHERE [ID] = @strID AND [Year] = @strYear AND [Month] = @strMonth", connection);

                    var selectedlistItem = lstViewPayment.SelectedItem as listItemPayment;
                    var selectedStudentListItem = lstViewStudents.SelectedItem as listItemStudents;

                    command.Parameters.AddWithValue("@strID", selectedStudentListItem.ID);
                    command.Parameters.AddWithValue("@strYear", selectedlistItem.PayYear);
                    command.Parameters.AddWithValue("@strMonth", selectedlistItem.PayMonth);
                    connection.Open();

                    command.ExecuteNonQuery();

                    // Release Memory
                    connection.Close();
                    command.Dispose();
                    connection.Dispose();

                    lstViewPayment.Items.RemoveAt(lstViewPayment.SelectedIndex);
                }
                catch (OleDbException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
