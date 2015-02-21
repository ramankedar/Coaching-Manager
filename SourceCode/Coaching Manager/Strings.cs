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

/// All the Strings used frequently

using System;

namespace Coaching_Manager
{
    class Strings
    {
        // used [public] for use all file
        // used [const] bcoz sometimes many body asked for unchangable string
        public const String InstituteName = "ASCEND Academic Care";

        //DB
        public static String DBconStr =
@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\IW.CM.DB.accdb;
Jet OLEDB:Database Password='';";

        public const string PassPhrase = "xF2uLpCZDG9A2L2iELS8PgreMIzH1zS0";

        public static string strUserName;
        public static Boolean IsAdmin = false;
    }
}
