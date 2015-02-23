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

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() // First Function, From here the program will start work
        {
            InitializeComponent();
            SetValues();

        }

        private void SetValues()
        {
            lblVersion.Content = Strings.AppVersion;
            lblVersion2.Content = Strings.AppVersion;
            lblUser.Content = "Welcome, " + Strings.strUserName;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnStdntInfo_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winStudentInfo win = new winStudentInfo();
            win.Show();
            // this line for close this form. "this" means MainWindow Window.
            this.Close();

            cmTools.AddLog("Open Search Student", this.Title);
        }

        private void btnTchrInfo_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winTeacherInfo win = new winTeacherInfo();
            win.Show();
            // this line for close this form. "this" means MainWindow Window.
            this.Close();

            cmTools.AddLog("Open Search Teacher", this.Title);
        }

        private void btnAddStdnt_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winNewStudent win = new winNewStudent();
            win.Show();
            // this line for close this form.
            this.Close();
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winPayment win = new winPayment();
            win.Show();
            // this line for close this form.
            this.Close();
        }

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnCornerMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnAttendance_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winAddAttendance win = new winAddAttendance();
            win.Show();
            // this line for close this form.
            this.Close();
        }

        private void btnExamResult_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winAddExamResult win = new winAddExamResult();
            win.Show();
            // this line for close this form.
            this.Close();
        }

        private void btnStdntDetails_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winStudentDetails win = new winStudentDetails();
            win.Show();
            // this line for close this form.
            this.Close();

            cmTools.AddLog("Open Students Details", this.Title);
        }

        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winOptions win = new winOptions();
            win.Show();
            // this line for close this form.
            this.Close();
        }

        private void btnInputCancel_Click(object sender, RoutedEventArgs e)
        {
            gridAbout.Visibility = Visibility.Collapsed;
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            gridAbout.Visibility = Visibility.Visible;
        }

        private void btnLogs_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winLogs win = new winLogs();
            win.Show();
            // this line for close this form.
            this.Close();

            cmTools.AddLog("Open Logs", this.Title);
        }

        private void btnAddTeacher_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winNewTeacher win = new winNewTeacher();
            win.Show();
            // this line for close this form.
            this.Close();
        }

        private void btnTeacherDetails_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winTeacherDetails win = new winTeacherDetails();
            win.Show();
            // this line for close this form.
            this.Close();

            cmTools.AddLog("Open Teachers Details", this.Title);
        }

        private void btnPayViewer_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winStudentPayViewer win = new winStudentPayViewer();
            win.Show();
            // this line for close this form.
            this.Close();

            cmTools.AddLog("Open Student Paymant Viewer", this.Title);
        }

        private void btnStdntReport_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            winStudentReport win = new winStudentReport();
            win.Show();
            // this line for close this form.
            this.Close();

            cmTools.AddLog("Open Students Report", this.Title);
        }
    }
}
