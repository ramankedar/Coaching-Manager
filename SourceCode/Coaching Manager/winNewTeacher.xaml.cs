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

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winNewTeacher.xaml
    /// </summary>
    public partial class winNewTeacher : Window
    {
        public winNewTeacher()
        {
            InitializeComponent();
            SetValues();
        }

        private void SetValues()
        {
            lblWinTitle.Content = Title + " | " + Strings.AppName + " | " + Strings.InstituteName;
            datePickerJoin.SelectedDate = DateTime.Today.Date;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            MainWindow win = new MainWindow();
            win.Show();
            // this line for close this form. "this" means winNewTeacher Window.
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure want to Add this New Teacher?", "ATTENTATION", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                AddNewEntry();
        }

        private void AddNewEntry()
        {
            string connectionString = Strings.DBconStr;
            {
                try
                {
                    OleDbConnection connection = new OleDbConnection(connectionString);
                    OleDbCommand command = new OleDbCommand(@"INSERT INTO TblTeacher 
            ([Name], [Sex], [InstituteName], [Qualification], [Subject], [MobileNumber], [PayScale], [JoinDate], [IsActive])
            VALUES (@Name, @Sex, @InstituteName, @Qualification, @Subject, @MobileNumber, @PayScale, @JoinDate, @IsActive)", connection);

                    command.Parameters.AddWithValue("@Name", txtName.Text); // Datatype: Max 255 Char
                    command.Parameters.AddWithValue("@Sex", cmbBxSex.SelectedIndex); // Datatype: Boolean
                    command.Parameters.AddWithValue("@InstituteName", txtInstitute.Text); // Datatype: Max 255 Char
                    command.Parameters.AddWithValue("@Qualification", txtQualification.Text); // Datatype: Max 255 Char
                    command.Parameters.AddWithValue("@Subject", txtSub.Text); // Datatype: Max 255 Char
                    command.Parameters.AddWithValue("@MobileNumber", (txtMobNo.Text != "") ? txtMobNo.Text : "-"); // Datatype: 50 Char
                    command.Parameters.AddWithValue("@PayScale", (txtPay.Text != "") ? txtPay.Text : "0"); // Datatype: Integer (2 bytes)
                    command.Parameters.AddWithValue("@JoinDate", datePickerJoin.SelectedDate); // Datatype: Date/Time
                    command.Parameters.AddWithValue("@IsActive", true); // Datatype: Boolean

                    if ((txtName.Text == "") || (txtInstitute.Text == "") || (txtQualification.Text == "")
                        || (txtSub.Text == ""))
                    {
                        MessageBox.Show("Fill up all required fields.");
                    }
                    else
                    {

                        connection.Open();

                        command.ExecuteNonQuery();

                        cmTools.AddLog("New Teacher \"" + txtName.Text + "\" Added.", this.Title);
                        MessageBox.Show("New Teacher Added Successfully! :)");

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

        private void txtPay_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
