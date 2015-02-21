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
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.OleDb;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winStdntInfo.xaml
    /// </summary>
    public partial class winTeacherInfo : Window
    {

        public winTeacherInfo()
        {
            InitializeComponent();
            SetValues();
            lblWinTitle.Content = Title + " | " + Strings.InstituteName;
        }

        public class ListItem
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string InstituteName { get; set; }
            public string JoinDate { get; set; }
            public string IsActive { get; set; }
            public string ForeColor { get; set; }
        }

        private void SetValues()
        {
            lblWinTitle.Content = this.Title;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            MainWindow win = new MainWindow();
            win.Show();
            // this line for close this form. "this" means winStudentInfo Window.
            this.Close();
        }

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if ((txtBoxTchrName.Text != "") || (txtBoxInstitute.Text != "")
                || (txtBoxQualification.Text != "") || (txtBoxSub.Text != "")
                || (cmbBxIsActive.SelectedIndex != -1))
                searchNow();
            else
                cmTools.showInfoMsg("Fill any search criteria first!");
        }

        private void searchNow()
        {
            try
            {
                Boolean IsMulti = false;

                string queryString =
                    "SELECT [ID],[Name],[InstituteName],[JoinDate],[IsActive] from TblTeacher WHERE";

                if (txtBoxTchrName.Text != "")
                    if (IsMulti)
                        queryString += " AND [Name] LIKE @srcName";
                    else
                    {
                        queryString += " [Name] LIKE @srcName";
                        IsMulti = true;
                    }

                if (txtBoxInstitute.Text != "")
                    if (IsMulti)
                        queryString += " AND [InstituteName] LIKE @srcInstituteName";
                    else
                    {
                        queryString += " [InstituteName] LIKE @srcInstituteName";
                        IsMulti = true;
                    }

                if (txtBoxQualification.Text != "")
                    if (IsMulti)
                        queryString += " AND [Qualification] LIKE @srcQualification";
                    else
                    {
                        queryString += " [Qualification] LIKE @srcQualification";
                        IsMulti = true;
                    }

                if (txtBoxSub.Text != "")
                    if (IsMulti)
                        queryString += " AND [Subject] LIKE @srcSubject";
                    else
                    {
                        queryString += " [Subject] LIKE @srcSubject";
                        IsMulti = true;
                    }

                if (cmbBxIsActive.SelectedIndex != -1)
                    if (IsMulti)
                        queryString += " AND [IsActive] LIKE @srcIsActive";
                    else
                    {
                        queryString += " [IsActive] LIKE @srcIsActive";
                        IsMulti = true;
                    }

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(queryString, connection);


                if (txtBoxTchrName.Text != "")
                    command.Parameters.AddWithValue("@srcName", "%" + txtBoxTchrName.Text + "%");

                if (txtBoxInstitute.Text != "")
                    command.Parameters.AddWithValue("@srcInstituteName", "%" + txtBoxInstitute.Text + "%");

                if (txtBoxQualification.Text != "")
                    command.Parameters.AddWithValue("@srcQualification", txtBoxQualification.Text);

                if (txtBoxSub.Text != "")
                    command.Parameters.AddWithValue("@srcSubject", txtBoxSub.Text);

                if (cmbBxIsActive.SelectedIndex != -1)
                    command.Parameters.AddWithValue("@srcIsActive", (cmbBxIsActive.SelectedIndex == 0) ? true : false);

                //Console.WriteLine(command.CommandText.ToString());

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                this.lstView.Items.Clear();

                while (reader.Read())
                {
                    this.lstView.Items.Add(new ListItem
                    {
                        ID = reader[0].ToString(),
                        Name = reader[1].ToString(),
                        InstituteName = reader[2].ToString(),
                        JoinDate = Convert.ToDateTime(reader[3]).ToShortDateString(),
                        IsActive = (reader[4].ToString() == "True") ? "" : "",
                        ForeColor = (reader[4].ToString() == "True") ? "Green" : "Red"
                    });
                }

                reader.Close();
                connection.Close();
                // Release Memory
                command.Dispose();
                connection.Dispose();

                if (lstView.Items.Count == 0)
                {
                    lblTotalResult.Content = "0 Result Found";
                    btnCngActive.IsEnabled = false;
                }
                else
                    lblTotalResult.Content = lstView.Items.Count + " Result(s) Found";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lblTotalResult.Content = "Ready";
            txtBoxTchrName.Text = "";
            txtBoxInstitute.Text = "";
            txtBoxQualification.Text = "";
            txtBoxSub.Text = "";
            cmbBxIsActive.SelectedIndex = -1;

            lstView.Items.Clear();
        }

        private void btnCngActive_Click(object sender, RoutedEventArgs e)
        {
            var selectedlistItem = lstView.SelectedItem as ListItem;
            if (selectedlistItem.ForeColor == "Red")
                ChangeActivity(true);
            else
                ChangeActivity(false);
        }

        private void ChangeActivity(Boolean IsActive)
        {
            try
            {

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);
                OleDbCommand command = new OleDbCommand(@"UPDATE TblTeacher
            SET [IsActive] = @strIsActive WHERE [ID] = @ID", connection);

                command.Parameters.AddWithValue("@strIsActive", IsActive); // Datatype: Boolean

                //Get ID Field from List
                var selectedlistItem = lstView.SelectedItem as ListItem;
                command.Parameters.AddWithValue("@ID", selectedlistItem.ID);

                connection.Open();

                command.ExecuteNonQuery();

                // Release Memory
                connection.Close();
                command.Dispose();
                connection.Dispose();

                //ChangeTable

                selectedlistItem.IsActive = (IsActive) ? "" : "";
                selectedlistItem.ForeColor = (IsActive) ? "Green" : "Red";
                btnCngActive.Content = (IsActive) ? " Make Inactive" : " Make Active";
                lstView.Items.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void lstView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedlistItem = lstView.SelectedItems[0] as ListItem;
                if (selectedlistItem == null)
                {
                    return;
                }

                //ShowStudentsDetails(selectedlistItem.ID);
                btnCngActive.IsEnabled = true;
                if (selectedlistItem.ForeColor == "Red")
                    btnCngActive.Content = " Make Active";
                else
                    btnCngActive.Content = " Make Inactive";
            }
            catch (Exception) { }
        }

        private void btnCornerMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
