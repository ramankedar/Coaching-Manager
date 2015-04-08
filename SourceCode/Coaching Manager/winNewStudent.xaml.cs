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

using Microsoft.Win32;
using System;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winNewStudent.xaml
    /// </summary>
    public partial class winNewStudent : Window
    {

        string imgSource = "";

        public winNewStudent()
        {
            InitializeComponent();
            SetValues();
        }

        private void SetValues()
        {
            lblWinTitle.Content = Title + " | " + Strings.AppName + " | " + Strings.InstituteName;
            datePickerAdmission.SelectedDate = DateTime.Today.Date;
            btnDeleteImg.Visibility = Visibility.Collapsed;

            lblQusInstitute.ToolTip = Strings.str_tips_institute_name_change_location;

            cmTools.GetInstituteNames(cmbBxInstituteName.Items);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            MainWindow win = new MainWindow();
            win.Show();
            // this line for close this form. "this" means winNewStudent Window.
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format(Strings.strSureAddNewObject, "Student"), "ATTENTATION", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                AddNewEntry();
        }

        private void AddNewEntry()
        {
            if ((txtName.Text == "") || (cmbBxInstituteName.SelectedIndex == -1) || (cmbBxClass.SelectedIndex == -1) || (txtRoll.Text == ""))
            {
                MessageBox.Show(Strings.strFillupAllFields);
            }
            else
            {
                try
                {
                    OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                    OleDbCommand command = new OleDbCommand(@"INSERT INTO TblStudent 
            ([Name],[Image],[Sex],[FatherName],[MotherName],[Religion],[PresentAddress],[PermanentAddress],[StudentMobileNumber],
            [GuardianMobileNumber],[Email],[SchoolName],[Class],[Roll],[Group],[Batch],[MonthlyFee],[AdmissionDate],[IsActive])
            VALUES (@Name,@Image,@Sex,@FatherName,@MotherName,@Religion,@PresentAddress,@PermanentAddress,@StudentMobileNumber,
            @GuardianMobileNumber,@Email,@SchoolName,@Class,@Roll,@Group,@Batch,@MonthlyFee,@AdmissionDate,@IsActive)", connection);

                    command.Parameters.AddWithValue("@Name", txtName.Text); // [Required] // Datatype: Max 255 Char

                    if (imgSource != "")
                    {
                        // Get image from file
                        FileStream fs = new FileStream(imgSource, FileMode.Open, FileAccess.Read);
                        byte[] imgData = new byte[fs.Length];
                        fs.Read(imgData, 0, Convert.ToInt32(fs.Length));
                        fs.Close();

                        command.Parameters.AddWithValue("@Image", imgData); // Datatype: OLE Object
                    }
                    else
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
                    command.Parameters.AddWithValue("@Class", cmbBxClass.SelectedIndex); // [Required] // Datatype: Byte [Only number from 0 to 255]
                    command.Parameters.AddWithValue("@Roll", txtRoll.Text); // [Required] // Datatype: Integer (2 bytes)
                    command.Parameters.AddWithValue("@Group", cmbBxGroup.SelectedIndex); // [Required] // Datatype: Byte [Only number from 0 to 255]

                    if (txtBatchNo.Text != "")
                        command.Parameters.AddWithValue("@Batch", txtBatchNo.Text); // Datatype: Max 255 Char
                    else
                        command.Parameters.AddWithValue("@Batch", DBNull.Value); // Datatype: Max 255 Char

                    command.Parameters.AddWithValue("@MonthlyFee", (txtFee.Text != "") ? txtFee.Text : "0"); // Datatype: Integer (2 bytes)
                    command.Parameters.AddWithValue("@AdmissionDate", datePickerAdmission.SelectedDate); // [Required] // Datatype: Date/Time
                    command.Parameters.AddWithValue("@IsActive", true); // Datatype: Boolean



                    connection.Open();

                    command.ExecuteNonQuery();

                    int intID = NewStudentId();

                    cmTools.AddLog(String.Format(Strings.strLogNewStudentAdded, txtName.Text, intID, cmbBxClass.SelectedIndex), this.Title);
                    cmTools.showInfoMsg(String.Format(Strings.strLogNewStudentAdded, txtName.Text, intID, cmbBxClass.SelectedIndex));

                    // Release Memory
                    connection.Close();
                    command.Dispose();
                    connection.Dispose();

                    // To Show a window we need to write below two line
                    MainWindow win = new MainWindow();
                    win.Show();
                    // this line for close this form. "this" means winNewStudent Window.
                    this.Close();



                    // Release Memory
                    command.Dispose();
                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private int NewStudentId()
        {
            int count = 1;
            try
            {
                string strQuery = "SELECT [ID] FROM TblStudent";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(strQuery, connection);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    count++;
                }

                // Release Memory
                connection.Close();
                command.Dispose();
                connection.Dispose();

                return count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
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
    }
}
