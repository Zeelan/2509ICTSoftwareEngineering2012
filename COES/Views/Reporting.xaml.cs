using COES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace COES.Views
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Reporting : UserControl
    {
        public Reporting()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            if (reportSel.SelectedItem!=null)
            {
                String qt = reportSel.SelectedValue.ToString();
                string sql = "";
                DataTable dt = new DataTable();

                switch(qt)
                {

                    case "Day":
                    sql = "select * from customer_order where customer_order.created_date >= date('now'); ";
                    dt = DatabaseManager.Query(sql);
                    myDG.DataSource = dt;
                    break;

                    case "Week":
                    sql = "select * from customer_order where customer_order.created_date >= date('now','-7 day'); ";
                    dt = DatabaseManager.Query(sql);
                    myDG.DataSource = dt;
                    break;

                    case "Month":
                    sql = "select * from customer_order where customer_order.created_date >= date('now','-1 month');";
                    dt = DatabaseManager.Query(sql);
                    myDG.DataSource = dt;
                    break;

                    case "Year":
                    sql = "select * from customer_order where customer_order.created_date >= date('now','-12 month'); ";
                    dt = DatabaseManager.Query(sql);
                    myDG.DataSource = dt;
                    break;

                    case "All Time":
                    sql = "select * from customer_order  ";
                    dt = DatabaseManager.Query(sql);
                    myDG.DataSource = dt;
                    break;
                }
            }
            else
            {
                MessageBox.Show("Please select a report to run");
            }
        }
    }
}
