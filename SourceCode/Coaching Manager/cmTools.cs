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
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Coaching_Manager
{
    class cmTools
    {

        static StreamWriter sWriter;
        static FileStream fStream;

        /// Tips:
        /// use static for making a function

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

        public static void GetInstituteNames(ItemCollection items)
        {
            if (File.Exists(Strings.AppDataLocation + "cm.institutesName.iwdb"))
            {
                items.Clear();

                string s;

                try
                {
                    fStream = new FileStream(Strings.AppDataLocation + "cm.institutesName.iwdb", FileMode.Open);
                }
                catch (IOException ex)
                {
                    cmTools.showInfoMsg(Strings.str_error_open_file + Environment.NewLine + ex.Message);
                }

                StreamReader fstr_in = new StreamReader(fStream);

                try
                {
                    while ((s = fstr_in.ReadLine()) != null)
                    {
                        items.Add(s);
                    }
                }
                catch (IOException ex)
                {
                    cmTools.showInfoMsg(Strings.str_error_on_reading_file + Environment.NewLine + ex.Message);
                }

                fstr_in.Close();
            }
        }

        public static void AddInstituteName(string institute) //append unknown institutes
        {
            try
            {
                sWriter = new StreamWriter(Strings.AppDataLocation + "cm.institutesName.iwdb", true);
            }
            catch (IOException ex)
            {
                cmTools.showInfoMsg(Strings.str_error_open_file + Environment.NewLine + ex.Message);
            }

            try
            {
                sWriter.Write(institute + Environment.NewLine);
            }
            catch (IOException ex)
            {
                cmTools.showInfoMsg(Strings.str_error_on_writing_file + Environment.NewLine + ex.Message);
            }

            sWriter.Close();

        }

        /*
        Public Sub CheckUpdate()

        If My.Computer.Network.IsAvailable Then

            Dim sfile As String = "latestver.iwconf"
            Dim URL As String = "http://imaginativeworld.org/update/iwst/"
            'Dim URL As String = "http://localhost/update/iwst/" 'For test

            Dim myWebClient As New System.Net.WebClient

            Try
                Dim file As New System.IO.StreamReader(myWebClient.OpenRead(URL & sfile))

                Dim Contents As String = file.ReadToEnd()
                file.Close()
                Dim Content() As String = Contents.Split("|")

                '0.0.0.0|Dl Link

                '(0) = Application Version
                '(1) = Download Link

                If Version.Parse(Content(0)) > My.Application.Info.Version Then

                    UpdateAvailable = True

                Else

                    UpdateAvailable = False

                End If

                STNewVersion = Content(0).ToString
                STUDownload = Content(1).ToString


            Catch Ex As Exception

                UpdateError = True
                STUpdateErr = Ex.ToString

            End Try

        Else
            UpdateError = True
        End If

        UpdateCheckComplete = True

        End Sub
         */
    }
}
