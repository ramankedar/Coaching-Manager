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

namespace Coaching_Manager
{
    class cmTools
    {
        //use static for making a function

        public static void showInfoMsg(string msg)
        {
            MessageBox.Show(msg, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void AddLog(string LogMessage, string WindowName)
        {
            string strDate = DateTime.Now.ToShortDateString();
            string strTime = DateTime.Now.ToShortTimeString();
            string final = strDate + " # " + strTime + " # " + WindowName + " : " + LogMessage;

            try
            {
                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(@"INSERT INTO TblLogs ([Log]) VALUES (@strLog)", connection);

                command.Parameters.AddWithValue("@strLog", final); // Datatype: max 255 char

                connection.Open();

                command.ExecuteNonQuery();

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
}
