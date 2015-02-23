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

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winOptions.xaml
    /// </summary>
    public partial class winOptions : Window
    {
        int intOption = 0;

        public winOptions()
        {
            InitializeComponent();
            SetValues();
            if (Strings.IsAdmin)
                GetUsers();
        }

        private void SetValues()
        {
            lblWinTitle.Content = Title + " | " + Strings.AppName + " | " + Strings.InstituteName;
            //InputTextBox.Text = String.Empty;
            if (Strings.IsAdmin == false)
            {
                btnDelUser.IsEnabled = false;
                btnAddUser.IsEnabled = false;
                cmbBxUsers.IsEnabled = false;
                cmbBxUsers.Items.Add(Strings.strUserName);
                cmbBxUsers.SelectedIndex = 0;
            }

        }

        private void ClearInput()
        {
            txtBxUserName.Text = "";
            PassBx.Password = "";
            PassBxRepeat.Password = "";
        }

        private void GetUsers()
        {
            try
            {
                string queryString =
                    "SELECT [User] from TblUser";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                this.cmbBxUsers.Items.Clear();

                while (reader.Read())
                {
                    this.cmbBxUsers.Items.Add(reader[0].ToString());
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                if (cmbBxUsers.Items.Count != 0)
                    cmbBxUsers.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            MainWindow win = new MainWindow();
            win.Show();
            // this line for close this form.
            this.Close();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            ClearInput();
            changeInputBox(1);
            gridInput.Visibility = Visibility.Visible;
            txtBxUserName.Focus();
            intOption = 1; //Add User
        }

        private Boolean AddNewUser()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(@"INSERT INTO TblUser
                        ([User], [Pass], [IsAdmin])
                        VALUES (@User, @Pass, @IsAdmin)", connection);

                command.Parameters.AddWithValue("@User", txtBxUserName.Text.ToLower()); // Datatype: max 255 char
                command.Parameters.AddWithValue("@Pass", cmCrypto.Encrypt(PassBx.Password, PassBx.Password)); // Datatype: max 255 char
                command.Parameters.AddWithValue("@IsAdmin", false); // Datatype: Boolean

                connection.Open();

                command.ExecuteNonQuery();

                MessageBox.Show("User Added Successfully! :)");

                // Release Memory
                connection.Close();
                command.Dispose();
                connection.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void btnInputOK_Click(object sender, RoutedEventArgs e)
        {
            if (intOption == 1) //Add User
            {
                if ((txtBxUserName.Text != "") && (PassBx.Password == PassBxRepeat.Password))
                {
                    if (AddNewUser())
                    {
                        gridInput.Visibility = Visibility.Collapsed;
                        GetUsers();
                    }
                }
                else
                    cmTools.showInfoMsg("Something Wrong! Check boxes again.");
            }
            else if (intOption == 2) // Update User Password
            {
                if (PassBx.Password == PassBxRepeat.Password)
                {
                    if (CheckCurrentPass())
                    {
                        if (UpdateUserPass())
                            gridInput.Visibility = Visibility.Collapsed;
                    }
                    else
                        cmTools.showInfoMsg("Wrong current password! Check boxes again.");

                }
                else
                    cmTools.showInfoMsg("Something Wrong! Check boxes again.");
            }
            else if (intOption == 3) // Update User Name
            {
                if (txtBxUserName.Text != "")
                {
                    if (UpdateUserName())
                    {
                        gridInput.Visibility = Visibility.Collapsed;
                        GetUsers();
                    }
                }
                else
                    cmTools.showInfoMsg("Enter a User Name first!");
            }
        }

        private void btnInputCancel_Click(object sender, RoutedEventArgs e)
        {
            gridInput.Visibility = Visibility.Collapsed;
        }

        private void btnDelUser_Click(object sender, RoutedEventArgs e)
        {
            if (cmbBxUsers.Items.Count > 1)
            {
                if ((Strings.IsAdmin) && (cmbBxUsers.SelectedValue.ToString() == Strings.strUserName))
                    cmTools.showInfoMsg("You cannot delete this account.");
                else if (MessageBox.Show("Are you sure want to delete the user \"" + cmbBxUsers.SelectedValue.ToString() + "\"?", "ATTENTION", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    DeleteUser();
            }
            else
                cmTools.showInfoMsg("You cannot delete this account.");
        }

        private void DeleteUser()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(@"DELETE FROM TblUser
                        WHERE [User] = @User", connection);

                command.Parameters.AddWithValue("@User", cmbBxUsers.SelectedValue.ToString()); // Datatype: max 255 char

                connection.Open();

                command.ExecuteNonQuery();

                MessageBox.Show("User Deleted Successfully! :)");

                // Release Memory
                connection.Close();
                command.Dispose();
                connection.Dispose();

                GetUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnChangePass_Click(object sender, RoutedEventArgs e)
        {
            ClearInput();
            changeInputBox(2);
            gridInput.Visibility = Visibility.Visible;
            //txtBxUserName.Text = cmbBxUsers.SelectedValue.ToString();
            intOption = 2; //Update User Pass
        }

        private Boolean CheckCurrentPass()
        {
            try
            {
                string queryString =
                    "SELECT [user] from TblUser WHERE [User] = @srcUser AND [Pass] = @srcPass";


                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                command.Parameters.AddWithValue("@srcUser", cmbBxUsers.SelectedValue.ToString().ToLower());
                command.Parameters.AddWithValue("@srcPass", cmCrypto.Encrypt(PassBxCurrent.Password, PassBxCurrent.Password));

                //Console.WriteLine(DBconStr);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();
                int count = 0;
                while (reader.Read())
                {
                    count++;
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                if (count == 0)
                    return false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;

        }

        private Boolean UpdateUserPass()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command =
                    new OleDbCommand(@"UPDATE TblUser SET [Pass] = @Pass WHERE [User] = @User", connection);

                command.Parameters.AddWithValue("@Pass", cmCrypto.Encrypt(PassBx.Password, PassBx.Password)); // Datatype: max 255 char
                command.Parameters.AddWithValue("@User", cmbBxUsers.SelectedValue.ToString()); // Datatype: max 255 char

                connection.Open();

                command.ExecuteNonQuery();

                cmTools.AddLog("Password Changed: User \"" + cmbBxUsers.SelectedValue.ToString() + "\".", this.Title);


                MessageBox.Show("User Password Updated Successfully! :)");

                // Release Memory
                connection.Close();
                command.Dispose();
                connection.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void btnChangeUserName_Click(object sender, RoutedEventArgs e)
        {
            ClearInput();
            changeInputBox(3);
            gridInput.Visibility = Visibility.Visible;
            txtBxUserName.Text = cmbBxUsers.SelectedValue.ToString();
            intOption = 3; //Update User Name
        }

        private Boolean UpdateUserName()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(@"UPDATE TblUser
                        SET [User] = @NewUser
                        WHERE [IsAdmin] = @strIsAdmin", connection);

                command.Parameters.AddWithValue("@NewUser", txtBxUserName.Text.ToLower()); // Datatype: max 255 char
                command.Parameters.AddWithValue("@strIsAdmin", true); // Datatype: Boolean

                connection.Open();

                command.ExecuteNonQuery();

                MessageBox.Show("User Name Updated Successfully! :)");

                // Release Memory
                connection.Close();
                command.Dispose();
                connection.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void changeInputBox(int type)
        {
            switch (type)
            {
                case 1: // Add User
                    txtBxUserName.Visibility = Visibility.Visible; //Text Box
                    lblInputUserName.Content = "User Name:";
                    PassBx.Visibility = Visibility.Visible;
                    PassBxRepeat.Visibility = Visibility.Visible;
                    lblInputPass.Visibility = Visibility.Visible;
                    lblPassRep.Visibility = Visibility.Visible;
                    PassBxCurrent.Visibility = Visibility.Collapsed; //Extra Pass Box
                    break;
                case 2: // Update User Pass
                    txtBxUserName.Visibility = Visibility.Collapsed; //Text Box
                    lblInputUserName.Content = "Old Password:";
                    PassBx.Visibility = Visibility.Visible;
                    PassBxRepeat.Visibility = Visibility.Visible;
                    lblInputPass.Visibility = Visibility.Visible;
                    lblPassRep.Visibility = Visibility.Visible;
                    PassBxCurrent.Visibility = Visibility.Visible; //Extra Pass Box
                    break;

                case 3: // Update User Name
                    txtBxUserName.Visibility = Visibility.Visible; //Text Box
                    lblInputUserName.Content = "New User Name:";
                    PassBx.Visibility = Visibility.Hidden;
                    PassBxRepeat.Visibility = Visibility.Hidden;
                    lblInputPass.Visibility = Visibility.Hidden;
                    lblPassRep.Visibility = Visibility.Hidden;
                    PassBxCurrent.Visibility = Visibility.Collapsed; //Extra Pass Box
                    break;
            }

        }

        private void cmbBxUsers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbBxUsers.Items.Count > 0)
            {
                if ((Strings.IsAdmin) && (cmbBxUsers.SelectedValue.ToString() == Strings.strUserName))
                {
                    btnChangeUserName.IsEnabled = true;
                    btnDelUser.IsEnabled = false;
                }
                else if ((Strings.IsAdmin) && (cmbBxUsers.SelectedValue.ToString() != Strings.strUserName))
                {
                    btnChangeUserName.IsEnabled = false;
                    btnDelUser.IsEnabled = true;
                }
            }
        }

    }
}
