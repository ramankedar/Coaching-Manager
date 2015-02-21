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

// This file only Work if you set its propertise "Build Action" equal ot "Page".. :)

using System;
using System.Threading;
using System.Windows;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex = new Mutex(true, "IWCoachingManager");
        private static winWelcome WinWelcome = null;

        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                App app = new App();
                WinWelcome = new winWelcome();
                app.Run(WinWelcome);
                mutex.ReleaseMutex();
            }
            else
            {
                // something
                WinWelcome.WindowState = WindowState.Normal;
            }
        }
    }
}
