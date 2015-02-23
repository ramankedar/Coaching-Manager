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

/// All frequently used Strings 

using System;
using System.IO;

namespace Coaching_Manager
{
    class Strings
    {
        // used [public] for use all file
        // used [const] bcoz sometimes many body asked for unchangable string
        // used [static], so that it can be changable
        // used [readonly], so that those are act like const
        public const String InstituteName = "ASCEND Academic Care";
        public static readonly String AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public static readonly Version AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        //DB
        public static String DBconStr;

        public const string PassPhrase = "xF2uLpCZDG9A2L2iELS8PgreMIzH1zS0";

        public static string strUserName;
        public static Boolean IsAdmin = false;

        public static string strDBFilePath = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.CommonApplicationData), @"Imaginative World\Coaching Manager\IW.CM.DB.dll");
    }
}
