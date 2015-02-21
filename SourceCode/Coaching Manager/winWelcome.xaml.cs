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
using Microsoft.Win32;
using System.Threading;
using System.IO;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winWelcome.xaml
    /// </summary>
    public partial class winWelcome : Window
    {


        public winWelcome()
        {
            InitializeComponent();
            SetValues();
        }

        private void SetValues()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Content = version;
            gUnlockBox.Visibility = Visibility.Hidden;

            if (!IsDBexist())
            {
                if (MessageBox.Show("ATTENTATION: Database file not found!\n\nAre you sure want to create a new Database file?", "Coaching Manager", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (!CopyDBfile())
                    {
                        cmTools.showInfoMsg("Something wrong with your file permission!");
                        Application.Current.Shutdown();
                    }
                }
                else
                    Application.Current.Shutdown();
            }

            SetDBstr();
        }

        #region "Database Check and Create/Copy"

        /// <summary>
        ///  As C:\Program files\ has permission problem. So we need to copy the db file to "C:\ProgramData folder
        /// </summary>

        private Boolean IsDBexist()
        {
            if (System.IO.File.Exists(Strings.strDBFilePath))
                return true;
            else
                return false;
        }

        private Boolean CopyDBfile()
        {
            string DesPath = Path.GetDirectoryName(Strings.strDBFilePath);

            try
            {
                if (!System.IO.Directory.Exists(DesPath))
                {
                    System.IO.Directory.CreateDirectory(DesPath);
                }

                System.IO.File.Copy(
                    Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "IW.CM.DB.dll"),
                    Strings.strDBFilePath, false);

                return true;

            }
            catch (Exception ex)
            {
                cmTools.showInfoMsg(ex.Message);
                return false;
            }
        }

        #endregion

        private void SetDBstr()
        {
            //Making Database Connection String
            //            Strings.DBconStr =
            //@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\IW.CM.DB.accdb;
            //Jet OLEDB:Database Password='" + cmCrypto.Decrypt("6pbzTfdZljrijyP2Ul6T6Q==", Strings.PassPhrase) + "';";

            Strings.DBconStr =
@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + Strings.strDBFilePath +
@"';Jet OLEDB:Database Password='" + cmCrypto.Decrypt("6pbzTfdZljrijyP2Ul6T6Q==", Strings.PassPhrase) + "';";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            gUnlockBox.Visibility = Visibility.Visible;
            txtUser.Focus();
            btnGo.IsDefault = true;

            //change IsDefault, so that we can click it by pressing <Enter>
            btnAdmin.IsDefault = false;
            btnGo.IsDefault = true;
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            if ((txtUser.Text != "") && (PassBox.Password != ""))
            {
                if (IsCorrectUserPass())
                {
                    cmTools.AddLog("Login as " + Strings.strUserName, this.Title);

                    // To Show a window we need to write below two line
                    MainWindow win = new MainWindow();
                    win.Show();
                    // this line for close this form.
                    this.Close();
                }
                else
                {
                    cmTools.showInfoMsg("Unlock Failed! Try again.");
                    PassBox.Password = "";
                }
            }
            else
                cmTools.showInfoMsg("Fill all fields first!");
        }

        private Boolean IsCorrectUserPass()
        {
            string user = "", pass = "";
            Boolean isAdmin = false;
            try
            {
                string queryString =
                    "SELECT * from TblUser WHERE [User] = @srcUser AND [Pass] = @srcPass";


                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);

                command.Parameters.AddWithValue("@srcUser", txtUser.Text.ToLower());
                command.Parameters.AddWithValue("@srcPass", cmCrypto.Encrypt(PassBox.Password, PassBox.Password));

                //Console.WriteLine(DBconStr);

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user = reader[0].ToString();
                    pass = reader[1].ToString();
                    isAdmin = Convert.ToBoolean(reader[2]);
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                if (user == "")
                    return false;
                else
                {
                    Strings.IsAdmin = (isAdmin) ? true : false;
                    Strings.strUserName = user;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;

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
