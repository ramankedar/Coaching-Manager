using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Coaching_Manager
{
    /// <summary>
    /// Interaction logic for winStudentIDcard.xaml
    /// </summary>
    public partial class winStudentIDcard : Window
    {

        int counter = 0;

        public winStudentIDcard()
        {
            InitializeComponent();
            SetValues();
            HideAllIDs();
        }

        private void SetValues()
        {
            imgStdnt_Default.Visibility = Visibility.Collapsed;

            lblWinTitle.Content = Title + " | " + Strings.AppName + " | " + Strings.InstituteName;

            lblYear_0.Content = DateTime.Now.Year;
            lblYear_1.Content = DateTime.Now.Year;
            lblYear_2.Content = DateTime.Now.Year;
            lblYear_3.Content = DateTime.Now.Year;
            lblYear_4.Content = DateTime.Now.Year;
            lblYear_5.Content = DateTime.Now.Year;
            lblYear_6.Content = DateTime.Now.Year;
            lblYear_7.Content = DateTime.Now.Year;
        }

        private void gMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // To Show a window we need to write below two line
            MainWindow win = new MainWindow();
            win.Show();
            // this line for close this form.
            this.Close();
        }

        private void btnCornerMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(gridIDs, Strings.str_print_id_card_title);

                //cmTools.AddLog(Strings.str_, this.Title);
            }
        }

        private void cmbBxSelClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            counter = 0;
            searchStudents();
        }

        private void searchStudents()
        {
            try
            {
                string strSQL =
                    "SELECT [ID], [Name], [Image], [Class] from TblStudent WHERE [Class] = "
                    + cmbBxSelClass.SelectedIndex.ToString() + " AND [IsActive] = true";

                OleDbConnection connection = new OleDbConnection(Strings.DBconStr);

                OleDbDataAdapter da = new OleDbDataAdapter(strSQL, Strings.DBconStr);

                DataSet ds = new DataSet();

                da.Fill(ds);

                HideAllIDs();

                int count = ds.Tables[0].Rows.Count;

                if (count > 0)
                {

                    for (int i = 0; counter < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[counter].ItemArray[2].ToString() != "")
                        {
                            byte[] data = (byte[])ds.Tables[0].Rows[counter].ItemArray[2];

                            MemoryStream strm = new MemoryStream();
                            strm.Write(data, 0, data.Length);
                            strm.Position = 0;
                            System.Drawing.Image img = System.Drawing.Image.FromStream(strm);

                            BitmapImage bi = new BitmapImage();
                            bi.BeginInit();
                            MemoryStream ms = new MemoryStream();
                            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                            ms.Seek(0, SeekOrigin.Begin);

                            bi.StreamSource = ms;
                            bi.EndInit();
                            (FindName("imgStdnt" + i) as Image).Source = bi;
                        }
                        else
                            (FindName("imgStdnt" + i) as Image).Source = imgStdnt_Default.Source;

                        (FindName("lblName" + i) as Label).Content = ds.Tables[0].Rows[counter].ItemArray[1].ToString();
                        (FindName("lblClass" + i) as Label).Content = ds.Tables[0].Rows[counter].ItemArray[3].ToString();
                        (FindName("lblID" + i) as Label).Content = Convert.ToInt32(ds.Tables[0].Rows[counter].ItemArray[0].ToString()).ToString("D6");

                        (FindName("bdr_Card_" + i) as Border).Visibility = Visibility.Visible;
                        (FindName("btnRemove" + i) as Button).Visibility = Visibility.Visible;
                        

                        counter++;

                        // We have only 8 id card space in one page
                        if (i >= 7)
                            break;

                    }

                    lblIndex.Content = string.Format(Strings.str_id_view_number, counter, count);

                    if (counter == count)
                    {
                        counter = 0;
                        btnNext.IsEnabled = false;
                    }
                    else
                        btnNext.IsEnabled = true;
                }
                else
                    lblIndex.Content = string.Format(Strings.str_id_view_number, 0, 0);

                connection.Close();
                // Release Memory
                connection.Dispose();
                da.Dispose();
                ds.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HideAllIDs()
        {
            bdr_Card_0.Visibility = Visibility.Hidden;
            bdr_Card_1.Visibility = Visibility.Hidden;
            bdr_Card_2.Visibility = Visibility.Hidden;
            bdr_Card_3.Visibility = Visibility.Hidden;
            bdr_Card_4.Visibility = Visibility.Hidden;
            bdr_Card_5.Visibility = Visibility.Hidden;
            bdr_Card_6.Visibility = Visibility.Hidden;
            bdr_Card_7.Visibility = Visibility.Hidden;

            btnRemove0.Visibility = Visibility.Hidden;
            btnRemove1.Visibility = Visibility.Hidden;
            btnRemove2.Visibility = Visibility.Hidden;
            btnRemove3.Visibility = Visibility.Hidden;
            btnRemove4.Visibility = Visibility.Hidden;
            btnRemove5.Visibility = Visibility.Hidden;
            btnRemove6.Visibility = Visibility.Hidden;
            btnRemove7.Visibility = Visibility.Hidden;

            scrollView.ScrollToHome();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            searchStudents();
        }

        private void btnRemove0_Click(object sender, RoutedEventArgs e)
        {
            bdr_Card_0.Visibility = Visibility.Hidden;
            btnRemove0.Visibility = Visibility.Hidden;
        }

        private void btnRemove1_Click(object sender, RoutedEventArgs e)
        {
            bdr_Card_1.Visibility = Visibility.Hidden;
            btnRemove1.Visibility = Visibility.Hidden;
        }

        private void btnRemove2_Click(object sender, RoutedEventArgs e)
        {
            bdr_Card_2.Visibility = Visibility.Hidden;
            btnRemove2.Visibility = Visibility.Hidden;
        }

        private void btnRemove3_Click(object sender, RoutedEventArgs e)
        {
            bdr_Card_3.Visibility = Visibility.Hidden;
            btnRemove3.Visibility = Visibility.Hidden;
        }

        private void btnRemove4_Click(object sender, RoutedEventArgs e)
        {
            bdr_Card_4.Visibility = Visibility.Hidden;
            btnRemove4.Visibility = Visibility.Hidden;
        }

        private void btnRemove5_Click(object sender, RoutedEventArgs e)
        {
            bdr_Card_5.Visibility = Visibility.Hidden;
            btnRemove5.Visibility = Visibility.Hidden;
        }

        private void btnRemove6_Click(object sender, RoutedEventArgs e)
        {
            bdr_Card_6.Visibility = Visibility.Hidden;
            btnRemove6.Visibility = Visibility.Hidden;
        }

        private void btnRemove7_Click(object sender, RoutedEventArgs e)
        {
            bdr_Card_7.Visibility = Visibility.Hidden;
            btnRemove7.Visibility = Visibility.Hidden;
        }
    }
}
