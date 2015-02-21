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
using System.Windows;
using System.Windows.Input;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winNewStudent.xaml
    /// </summary>
    public partial class winNewStudent : Window
    {
        public winNewStudent()
        {
            InitializeComponent();
            SetValues();
        }

        private void SetValues()
        {
            lblWinTitle.Content = Title + " | " + Strings.InstituteName;
            datePickerAdmission.SelectedDate = DateTime.Today.Date;
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
            if (MessageBox.Show("Are you sure want to Add this New Student?", "ATTENTATION", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                AddNewEntry();

        }

        private void AddNewEntry()
        {
            {
                try
                {

                    OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                    OleDbCommand command = new OleDbCommand(@"INSERT INTO TblStudent 
            ([Name],[Sex],[FatherName],[MotherName],[Religion],[PresentAddress],[PermanentAddress],[StudentMobileNumber],
            [GuardianMobileNumber],[Email],[SchoolName],[Class],[Roll],[Group],[Batch],[MonthlyFee],[AdmissionDate],[IsActive])
            VALUES (@Name,@Sex,@FatherName,@MotherName,@Religion,@PresentAddress,@PermanentAddress,@StudentMobileNumber,
            @GuardianMobileNumber,@Email,@SchoolName,@Class,@Roll,@Group,@Batch,@MonthlyFee,@AdmissionDate,@IsActive)", connection);

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
                    command.Parameters.AddWithValue("@Class", cmbBxClass.SelectedIndex); // [Required] // Datatype: Byte [Only number from 0 to 255]
                    command.Parameters.AddWithValue("@Roll", txtRoll.Text); // [Required] // Datatype: Integer (2 bytes)
                    command.Parameters.AddWithValue("@Group", cmbBxGroup.SelectedIndex); // [Required] // Datatype: Byte [Only number from 0 to 255]
                    command.Parameters.AddWithValue("@Batch", (txtBatchNo.Text != "") ? txtBatchNo.Text : "-"); // Datatype: Max 255 Char
                    command.Parameters.AddWithValue("@MonthlyFee", (txtFee.Text != "") ? txtFee.Text : "0"); // Datatype: Integer (2 bytes)
                    command.Parameters.AddWithValue("@AdmissionDate", datePickerAdmission.SelectedDate); // [Required] // Datatype: Date/Time
                    command.Parameters.AddWithValue("@IsActive", true); // Datatype: Boolean

                    if ((txtName.Text == "") || (txtInstitute.Text == "") || (txtRoll.Text == ""))
                    {
                        MessageBox.Show("Fill up all required fields.");
                    }
                    else
                    {

                        connection.Open();

                        command.ExecuteNonQuery();

                        cmTools.AddLog("New Student \"" + txtName.Text + "\" Added to \"Class " + cmbBxClass.SelectedIndex + "\".", this.Title);
                        MessageBox.Show("New Student Added Successfully! :)");

                        // Release Memory
                        connection.Close();
                        command.Dispose();
                        connection.Dispose();

                        // To Show a window we need to write below two line
                        MainWindow win = new MainWindow();
                        win.Show();
                        // this line for close this form. "this" means winNewStudent Window.
                        this.Close();

                    }

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

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnCornerMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
