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
using System.Text.RegularExpressions;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winStdntInfo.xaml
    /// </summary>
    public partial class winStudentInfo : Window
    {

        public winStudentInfo()
        {
            InitializeComponent();
            SetValues();
        }

        public class listItem
        {
            public string CoachingRoll { get; set; }
            public string StudentName { get; set; }
            public string SchoolName { get; set; }
            public string Class { get; set; }
            public string IsActive { get; set; }
            public string ForeColor { get; set; }
        }

        private void SetValues()
        {
            lblWinTitle.Content = Title + " | " + Strings.AppName + " | " + Strings.InstituteName;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            MainWindow win = new MainWindow();
            win.Show();
            // this line for close this form. "this" means winStudentInfo Window.
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winNewStudent win = new winNewStudent();
            win.Show();
            // this line for close this form.
            this.Close();
        }

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if ((txtBoxStdntName.Text != "") || (txtBoxSclName.Text != "")
                || (txtBoxCoachingRoll.Text != "") || (cmbBxClass.SelectedIndex != -1)
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
                    "SELECT [ID], [Name], [SchoolName], [Class], [IsActive] from TblStudent WHERE";

                if (txtBoxStdntName.Text != "")
                    if (IsMulti)
                        queryString += " AND [Name] LIKE @srcName";
                    else
                    {
                        queryString += " [Name] LIKE @srcName";
                        IsMulti = true;
                    }

                if (txtBoxSclName.Text != "")
                    if (IsMulti)
                        queryString += " AND [SchoolName] LIKE @srcScl";
                    else
                    {
                        queryString += " [SchoolName] LIKE @srcScl";
                        IsMulti = true;
                    }

                if (txtBoxCoachingRoll.Text != "")
                    if (IsMulti)
                        queryString += " AND [ID] LIKE @srcID";
                    else
                    {
                        queryString += " [ID] LIKE @srcID";
                        IsMulti = true;
                    }

                if (cmbBxClass.SelectedIndex != -1)
                    if (IsMulti)
                        queryString += " AND [Class] LIKE @srcClass";
                    else
                    {
                        queryString += " [Class] LIKE @srcClass";
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


                if (txtBoxStdntName.Text != "")
                    command.Parameters.AddWithValue("@srcStr", "%" + txtBoxStdntName.Text + "%");

                if (txtBoxSclName.Text != "")
                    command.Parameters.AddWithValue("@srcScl", "%" + txtBoxSclName.Text + "%");

                if (txtBoxCoachingRoll.Text != "")
                    command.Parameters.AddWithValue("@srcID", txtBoxCoachingRoll.Text);

                if (cmbBxClass.SelectedIndex != -1)
                    command.Parameters.AddWithValue("@srcClass", cmbBxClass.SelectedIndex.ToString());

                if (cmbBxIsActive.SelectedIndex != -1)
                    command.Parameters.AddWithValue("@srcIsActive", (cmbBxIsActive.SelectedIndex == 0) ? true : false);

                //Console.WriteLine(command.CommandText.ToString());

                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();

                this.lstView.Items.Clear();

                while (reader.Read())
                {
                    this.lstView.Items.Add(new listItem
                    {
                        CoachingRoll = reader[0].ToString(),
                        StudentName = reader[1].ToString(),
                        SchoolName = reader[2].ToString(),
                        Class = reader[3].ToString(),
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

        private void lstView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedlistItem = lstView.SelectedItems[0] as listItem;
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

        private void btnCngActive_Click(object sender, RoutedEventArgs e)
        {
            var selectedlistItem = lstView.SelectedItem as listItem;
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
                OleDbCommand command = new OleDbCommand(@"UPDATE TblStudent 
            SET [IsActive] = @strIsActive WHERE [ID] = @ID", connection);

                command.Parameters.AddWithValue("@strIsActive", IsActive); // Datatype: Boolean

                //Get ID Field from List
                var selectedlistItem = lstView.SelectedItem as listItem;
                command.Parameters.AddWithValue("@ID", selectedlistItem.CoachingRoll);

                connection.Open();

                command.ExecuteNonQuery();

                // Release Memory
                connection.Close();
                command.Dispose();
                connection.Dispose();

                string mark = (IsActive) ? "Inactive" : "Active";

                cmTools.AddLog("Student \"" + selectedlistItem.StudentName + "\" (ID: " + selectedlistItem.CoachingRoll + ") marked as " + mark + ".", this.Title);

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

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lblTotalResult.Content = "Ready";
            txtBoxStdntName.Text = "";
            txtBoxSclName.Text = "";
            txtBoxCoachingRoll.Text = "";
            cmbBxClass.SelectedIndex = -1;
            cmbBxIsActive.SelectedIndex = -1;

            lstView.Items.Clear();
        }

        private void btnCornerMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void txtBoxCoachingRoll_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

