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
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winNewTeacher.xaml
    /// </summary>
    public partial class winNewTeacher : Window
    {

        string imgSource = "";

        public winNewTeacher()
        {
            InitializeComponent();
            SetValues();
        }

        private void SetValues()
        {
            lblWinTitle.Content = Title + " | " + Strings.AppName + " | " + Strings.InstituteName;
            datePickerJoin.SelectedDate = DateTime.Today.Date;
            btnDeleteImg.Visibility = Visibility.Collapsed;
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
            if (MessageBox.Show(string.Format(Strings.strSureAddNewObject, "Teacher"), "ATTENTATION", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
            ([Name], [Image], [Sex], [InstituteName], [Qualification], [Subject], [MobileNumber], [PayScale], [JoinDate], [IsActive])
            VALUES (@Name, @Image, @Sex, @InstituteName, @Qualification, @Subject, @MobileNumber, @PayScale, @JoinDate, @IsActive)", connection);

                    command.Parameters.AddWithValue("@Name", txtName.Text); // Datatype: Max 255 Char

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
                    command.Parameters.AddWithValue("@InstituteName", txtInstitute.Text); // Datatype: Max 255 Char
                    command.Parameters.AddWithValue("@Qualification", txtQualification.Text); // Datatype: Max 255 Char
                    command.Parameters.AddWithValue("@Subject", txtSub.Text); // Datatype: Max 255 Char

                    if (txtMobNo.Text != "")
                        command.Parameters.AddWithValue("@MobileNumber", txtMobNo.Text); // Datatype: 50 Char
                    else
                        command.Parameters.AddWithValue("@MobileNumber", DBNull.Value); // Datatype: 50 Char

                    command.Parameters.AddWithValue("@PayScale", (txtPay.Text != "") ? txtPay.Text : "0"); // Datatype: Integer (2 bytes)
                    command.Parameters.AddWithValue("@JoinDate", datePickerJoin.SelectedDate); // Datatype: Date/Time
                    command.Parameters.AddWithValue("@IsActive", true); // Datatype: Boolean

                    if ((txtName.Text == "") || (txtInstitute.Text == "") || (txtQualification.Text == "")
                        || (txtSub.Text == ""))
                    {
                        MessageBox.Show(Strings.strFillupAllFields);
                    }
                    else
                    {

                        connection.Open();

                        command.ExecuteNonQuery();

                        cmTools.AddLog(string.Format(Strings.strLogNewTeacherAdded, txtName.Text, NewStudentId()), this.Title);
                        cmTools.showInfoMsg(string.Format(Strings.strLogNewTeacherAdded, txtName.Text, NewStudentId()));

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

        private int NewStudentId()
        {
            int count = 1;
            try
            {
                string strQuery = "SELECT [ID] FROM TblTeacher";

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

        private void txtPay_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
                        imgTeacher.Source = new BitmapImage(new Uri(openFileDlg.FileName));
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
            imgTeacher.Source = null;
            imgSource = "";
            btnDeleteImg.Visibility = Visibility.Collapsed;
        }
    }
}
