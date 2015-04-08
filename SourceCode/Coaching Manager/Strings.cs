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

        public const string AppSourceURL = "https://imaginativeworld.github.io/Coaching-Manager";
        public const string InstituteName = "ASCEND Academic Care";
        public static readonly string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public static readonly Version AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

#if DEBUG

        public static readonly string AppDataLocation =
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

#else
        
        public static readonly string AppDataLocation =
            Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.CommonApplicationData), @"Imaginative World\Coaching Manager\");

#endif
        //DB
        public static String DBconStr;
        // Crypto phrases.. Must Change those for Better Database security
        public const String PassPhrase = "xF2uLpCZDG9A2L2iELS8PgreMIzH1zS0";
        public const String DbEncryptedPass = "6pbzTfdZljrijyP2Ul6T6Q==";
        public const String SaltValue = "IrD69icOQmoF6Cwe";

        public static String strUserName;
        public static Boolean IsAdmin = false;

        public static readonly String strDBFilePath = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.CommonApplicationData), @"Imaginative World\Coaching Manager\IW.CM.DB.dll");

        //About Institute
        public const string str_institute_info =
@"-------------------------------[ About Institute ]-------------------------------
	ASCEND Academic Care
	Director: Md. Shameem Reza
	E-mail: shameemreza47@gmail.com
	Mobile: +880-1824-998329
---------------------------------------------------------------------------------";

        /// <summary>
        /// {0} = user name
        /// </summary>
        public const string str_welcome = "Welcome, {0}";

        // Hints
        public const string str_btn_tips_search_students = "Search Students";
        public const string str_btn_tips_add_student = "Add New Student";
        public const string str_btn_tips_student_details = "View Students Details";
        public const string str_btn_tips_attendance = "Add Students Attendance";
        public const string str_btn_tips_exam_result = "Add Students Exam Results";
        public const string str_btn_tips_search_teacher = "Search Teachers";
        public const string str_btn_tips_add_teacher = "Add New Teachers";
        public const string str_btn_tips_teacher_details = "View Teachers Details";
        public const string str_btn_tips_add_transaction = "Add New Transaction";
        public const string str_btn_tips_print_ids = "Print Student ID Cards";
        public const string str_btn_tips_student_report = "View Student Reports";
        public const string str_btn_tips_pay_report = "View Student Payment Reports";
        public const string str_btn_tips_finance_report = "View Finance Reports";
        public const string str_btn_tips_cost_report = "View Cost Reports";
        public const string str_btn_tips_about = "About";
        public const string str_btn_tips_logs = "View Logs";
        public const string str_btn_tips_options = "View Options";
        public const string str_btn_tips_exit = "Exit Coaching Manager";

        public const string str_tips_enter_new_institute_name = "Enter new institute name here.";
        public const string str_tips_institute_name_change_location = "You can add/remove Institute names from Options window.";

        //Image
        public const string strImgBigFileSize = "File size greter then 512KiloByte! Please select a smaller file size image.";
        public const string strFillupAllFields = "Fill up all required fields.";

        /// <summary>
        /// {0} = Name
        /// {1} = ID number
        /// {2} = Class
        /// </summary>
        public const string strLogNewStudentAdded = "New Student \"{0}\" (ID: {1}) Added to \"Class {2}\".";

        /// <summary>
        /// {0} = Name
        /// {1} = ID number
        /// </summary>
        public const string strLogNewTeacherAdded = "New Teacher \"{0}\" (ID: {1}) Added.";

        /// <summary>
        /// {0} = Student OR Teacher
        /// </summary>
        public const string strSureAddNewObject = "Are you sure want to Add this New {0}?";

        public const string str_attendance_added = "Attendance Added.";

        public const string str_no_more_student_found = "No more student found!";

        public const string str_attendance_greter_then_total_study_day = "Total Present is greter then Total Study Day!";

        public const string str_total_day_greter_then_month_size = "Total Day is greter then month size!";

        /// <summary>
        /// {0} = Year
        /// </summary>
        public const string str_invalid_year_changed_to_default_year = "Invalid Year! Year changed to {0}.";

        public const string str_exam_result_added = "Exam Result Added.";

        public const string str_achievement_greter_then_total_marks = "Achievement is greter then Total Marks!";

        // Payment
        public const string str_transaction_confirmation = "Are you sure want to Add this Transaction?";
        public const string str_invalid_id = "Invalid ID!";
        public const string str_zero_tk_payment = "0 Tk Payment!!!";

        /// <summary>
        /// Type = Student or Teacher
        /// </summary>
        public const string str_select_what_type = "Select type first!";

        public const string str_fee_added = "Fee Added Successfully! :)";

        public const string str_payment_added = "Payment Added Successfully! :)";
        public const string str_cost_added = "Cost Added Successfully! :)";

        /// <summary>
        /// {0} = Student or Teacher
        /// </summary>
        public const string str_no_object_found_with_this_id = "No {0} Found with this ID!";

        public const string str_log_payment_receipt_printed = "Transaction: Payment Receipt Printed.";

        public const string str_close_pay_reciept_confirmation = "Are you sure want to close the Pay Reciept?";

        /// <summary>
        /// {0} = transaction ID
        /// {1} = fee amount
        /// {2} = student Name
        /// {3} = student ID
        /// </summary>
        public const string str_log_student_transaction_details =
            "Transaction {0}: Fee \"{1}\" Tk is given by the Student \"{2}\" (ID: {3}).";

        /// <summary>
        /// {0} = transaction ID
        /// {1} = fee amount
        /// {2} = teacher Name
        /// {3} = teacher ID
        /// </summary>
        public const string str_log_teacher_transaction_details =
            "Transaction {0}: Fee \"{1}\" Tk is given by the Teacher \"{2}\" (ID: {3}).";

        /// <summary>
        /// {0} = Cost
        /// {1} = Title
        /// </summary>
        public const string str_log_cost_details =
            "Cost: Cost \"{0}\" Tk for \"{1}\" is added.";

        public const string str_attention = "ATTENTION";

        //search

        public const string str_fill_any_search_criteria_for_search = "Fill any search criteria first!";

        public const string str_zero_result_found = "0 Result Found";

        /// <summary>
        /// {0} = Total Result number
        /// </summary>
        public const string str_total_result_found = "{0} Result(s) Found";

        public const string str_ready = "Ready";

        /// <summary>
        /// {0} = Student/Teacher
        /// {1} = Name
        /// {2} = ID
        /// {3} = active/inactive
        /// </summary>
        public const string str_log_object_active_mark_changed = "{0} \"{1}\" (ID: {2}) marked as {3}.";

        // student details
        public const string str_something_wrong = "Something Wrong!";

        /// <summary>
        /// {0} = Student Name
        /// {1} = Student ID
        /// {2} = New Class
        /// </summary>
        public const string str_log_student_upgraded_one_class =
            "Upgraded: Student \"{0}\" (ID: {1}) upgraded to Class {2}";

        /// <summary>
        /// {0} = Student Name
        /// </summary>
        public const string str_student_already_in_highest_class = "{0} already in highest class. You can make him/her Inactive.";

        /// <summary>
        /// {0} = Student/Teacher
        /// {1} = Name
        /// {2} = ID
        /// </summary>
        public const string str_object_info_updated = "Info Updated: {0} \"{1}\" (ID: {2})";

        // Student Report

        /// <summary>
        /// {0} = Month Name
        /// {1} = Year
        /// {2} = Class
        /// </summary>
        public const string str_student_report_header = InstituteName + " - Students Report {0} {1} : Class {2}";

        // Finance Report

        /// <summary>
        /// {0} = Year
        /// </summary>
        public const string str_finance_report_header = InstituteName + " - Finance Report {0}";
        // Student Payment Viewer

        /// <summary>
        /// {0} = Year
        /// {1} = Class
        /// </summary>
        public const string str_student_payment_report_header = InstituteName + " - Payment Report {0} : Class {1}";

        /// <summary>
        /// {0} = Shows Total
        /// {1} = Total Students
        /// </summary>
        public const string str_id_view_number = "IDs {0} of {1}";

        //Errors
        public const string str_error_open_file = "Cannot Open File!";
        public const string str_error_on_writing_file = "Error Writing File!";
        public const string str_error_on_reading_file = "Error Reading File!";
        public const string str_err_already_in_institute_list = "The Institute name already in Institute list!";

        //Prints
        public const string str_print_id_card_title = "Print Student IDs";
        public const string str_print_payment_receipt_title = "Print Payment Receipt";
        public const string str_print_payment_report = "Print Payment Report";
        public const string str_print_student_report = "Print Student Report";
        public const string str_print_finance_report = "Print Finance Report";

        public const string str_lbl_cost_title = "Title:";
        public const string str_lbl_year_title = "Year:";

        public const string str_currency_end_sign = "/=";

        //Confermations
        public const string str_confirmation_delete_attendance = "Are you sure want to Delete the selected attendance data?";
        public const string str_confirmation_delete_exam = "Are you sure want to Delete the selected exam data?";
        public const string str_confirmation_delete_payment = "Are you sure want to Delete the selected payment data?";
        
    }
}

/*
 * ErrCode
 * -------
 * 
 * CM#0001 : Restore Database File => Copy problem
 * CM#0002 : Save Database File => Copy Problem
 * 
 */

/*
 * Short Cut Keys
 * --------------
 * 
 * Ctrl + E, D == Format Code
 * 
 */
