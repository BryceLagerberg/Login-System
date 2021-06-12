using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Utilities;


namespace Budget_Tracker.Forms
{
    public partial class Form1 : Form
    {
        Thread TransactionRefresh;
        DateTime? LastRefresh;

        public Form1()
        {
            InitializeComponent();
        }
        
        
        #region Events
        
        // On Load
        private void Form1_Load(object sender, EventArgs e)
        {
            // start your threads
            TransactionRefresh = new Thread(LoadTransactions);
            TransactionRefresh.IsBackground = true;
            TransactionRefresh.Start();
            DrawExpenseChart();
            DrawGainChart();
            
        }
        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        // gains button press event
        private void Button1_Click(object sender, EventArgs e)
        {

            // check that a transaction catagory was selected
            if (comboBox1.SelectedItem != null)
            {

            AddGain(comboBox1.Text, (double)numericUpDown1.Value, textBox1.Text);
            
            // setup the datetime for the sql table transaction 
            string transactionDate = monthCalendar1.SelectionRange.Start.ToShortDateString() + " ";
            transactionDate += DateTime.Now.ToLongTimeString();
            
            // add transaction to the sql table
            Globals._SC.PostTransaction(Globals._LoggedInUser.AccountNumber, transactionDate, (double)numericUpDown1.Value, comboBox1.Text, textBox1.Text);

            // clear the gain fields so its ready for a new transaction
            textBox1.Text = "";
            numericUpDown1.Value = 0;
            comboBox1.SelectedItem = null;
            } else
            {
                MessageBox.Show("To Post a Transaction please select a Catagory first.");
            }
        }

        // Expenses button press event
        private void Button2_Click(object sender, EventArgs e)
        {
            
            // check that a transaction catagory was selected
            if (comboBox2.SelectedItem != null)
            {
                AddLoss(comboBox2.Text, (double)numericUpDown2.Value, textBox2.Text);

                // setup the datetime for the sql table transaction 
                string transactionDate = monthCalendar1.SelectionRange.Start.ToShortDateString() + " ";
                transactionDate += DateTime.Now.ToLongTimeString();

                // add transaction to the sql table
                Globals._SC.PostTransaction(Globals._LoggedInUser.AccountNumber, transactionDate, (double)numericUpDown2.Value, comboBox2.Text, textBox2.Text);

                // clear the gain fields so its ready for a new transaction
                textBox2.Text = "";
                numericUpDown2.Value = 0;
                comboBox2.SelectedItem = null;
            } else
            {
                MessageBox.Show("To Post a Transaction please select a Catagory first.");
            }
        }

        private void MonthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            // Load Gains / Losses based on the selected day (This will require that data is saved somewhere)
        }

        // Update Transaction button press
        private void button3_Click(object sender, EventArgs e)
        {
            // check to see if the transaction is being changed
            if (checkBox1.Checked || checkBox2.Checked || checkBox3.Checked)
            {
                
                // update transaction info in the loggedinuser transactions data
                if (checkBox1.Checked)
                {
                    Globals._LoggedInUser.Transactions[comboBox4.SelectedIndex].TransactionType = comboBox3.Text;
                }
                if (checkBox2.Checked)
                {
                    Globals._LoggedInUser.Transactions[comboBox4.SelectedIndex].Note = textBox5.Text;
                }
                if (checkBox3.Checked)
                {
                    Globals._LoggedInUser.Transactions[comboBox4.SelectedIndex].TransactionValue = (double)numericUpDown3.Value;
                }

                // add transaction edit to the recent activity (listbox1)
                listBox1.Items.Add($"Transaction ID# {Globals._LoggedInUser.Transactions[comboBox4.SelectedIndex].TransactionID.ToString()} Updated");

                //add empty line for better visual
                listBox1.Items.Add("");

                // update the transaction info in the sql database
                Globals._SC.EditTransaction(checkBox1.Checked, checkBox2.Checked, checkBox3.Checked, comboBox4.SelectedIndex);

                // clear the items from the transaction id list on the edit transactions tab
                comboBox4.Items.Clear();

                // clear the all transactions listbox
                listBox2.Items.Clear();

                // reload all the transactions
                LastRefresh = null;

                // reset the pie chart stats
                Utilities.PieChartStats.PayCheck = 0;
                Utilities.PieChartStats.Refund = 0;
                Utilities.PieChartStats.OtherGain = 0;
                Utilities.PieChartStats.Rent = 0;
                Utilities.PieChartStats.Transportation = 0;
                Utilities.PieChartStats.Entertainment = 0;
                Utilities.PieChartStats.OtherExpense = 0;
                Utilities.PieChartStats.Groceries = 0;

                // clear the edit transaction fields
                comboBox4.Text = null;
                comboBox3.Text = null;
                textBox5.Text = null;
                numericUpDown3.ResetText();
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;

            }




        }

        // Delete Transaction button press
        private void button4_Click(object sender, EventArgs e)
        {
            // check that there is a selected transaction to delete
            if (comboBox4.Text.Length > 0)
            {
                // add transaction edit to the recent activity (listbox1)
                listBox1.Items.Add($"Transaction ID# {Globals._LoggedInUser.Transactions[comboBox4.SelectedIndex].TransactionID.ToString()} Deleted");

                //add empty line for better visual
                listBox1.Items.Add("");

                // remove the transaction from the sql transactions table
                Globals._SC.DeleteTransaction(comboBox4.SelectedIndex);

                // clear the items from the transaction id list on the edit transactions tab
                comboBox4.Items.Clear();

                // clear the all transactions listbox
                listBox2.Items.Clear();

                // reload all the transactions
                LastRefresh = null;

                // reset the pie chart stats
                Utilities.PieChartStats.PayCheck = 0;
                Utilities.PieChartStats.Refund = 0;
                Utilities.PieChartStats.OtherGain = 0;
                Utilities.PieChartStats.Rent = 0;
                Utilities.PieChartStats.Transportation = 0;
                Utilities.PieChartStats.Entertainment = 0;
                Utilities.PieChartStats.OtherExpense = 0;
                Utilities.PieChartStats.Groceries = 0;

                // clear the edit transaction fields
                comboBox4.Text = null;
                comboBox3.Text = null;
                textBox5.Text = null;
                numericUpDown3.ResetText();
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
            }
            
        }

        // selected transaction id changed 
        private void ComboBox4_IndexChanged(object sender, EventArgs e)
        {
            // fill in the edit transactoin fields
            textBox5.Text = Globals._LoggedInUser.Transactions[(int)comboBox4.SelectedItem - 1].Note;
            numericUpDown3.Value = (decimal)Globals._LoggedInUser.Transactions[(int)comboBox4.SelectedItem - 1].TransactionValue;
        }

        #endregion




        #region Functions

        // add a grain to the budget tracker
        private void AddGain(string Catagory, double Amount, string Note)
        {
            listBox1.Items.Add($"{Catagory} +${Amount}  Date: {monthCalendar1.SelectionRange.Start.Date.ToShortDateString()}");
            listBox1.Items.Add($"     Note: {Note}");
            listBox1.Items.Add("");

        }
        
        // add a loss to the budget tracker
        private void AddLoss(string Catagory, double Amount, string Note)
        {
            listBox1.Items.Add($"{Catagory} -${Amount}  Date: {monthCalendar1.SelectionRange.Start.Date.ToShortDateString()}");
            listBox1.Items.Add($"    Note: {Note}");
            listBox1.Items.Add("");

        }

        // load the budget tracker transactions
        public void LoadTransactions()
        {
            try
            {
                while (true) // to keep the thread from ending
                {
                    // load the transactions for the current account
                    Globals._LoggedInUser.Transactions = Globals._SC.GetTransactions(Globals._LoggedInUser.AccountNumber);

                    // refresh transactions
                    LoadTransactionsDelegate();

                    Thread.Sleep(1000);
                }
            }
            catch(Exception ex)
            {

            }
        }

        // load budget tracker transactions delegate function
        private void LoadTransactionsDelegate()
        {
            try
            {
                if(this.InvokeRequired == false)
                {
                    // display transactions
                    foreach(Utilities.Transaction t in Globals._LoggedInUser.Transactions)
                    {
                        // new transaction check
                        if(t.TransactionDate > LastRefresh || LastRefresh == null)
                        {
                            // add transaction to the listbox to display it to user
                            listBox2.Items.Add($"ID#: {t.TransactionID}");
                            listBox2.Items.Add($"{t.TransactionType} ${t.TransactionValue}  Date: {t.TransactionDate.Date.ToShortDateString()}");
                            listBox2.Items.Add($"    Note: {t.Note}");
                            listBox2.Items.Add("");

                            // add transaction id to the combobox4 list
                            comboBox4.Items.Add(t.TransactionID);

                            // add the transaction to the piechartstats
                            if (t.TransactionType.ToString() == "Pay Check")
                            {
                                Utilities.PieChartStats.PayCheck += t.TransactionValue;
                                DrawGainChart();
                            }
                            else if (t.TransactionType.ToString() == "Refund")
                            {
                                Utilities.PieChartStats.Refund += t.TransactionValue;
                                DrawGainChart();
                            }
                            else if (t.TransactionType.ToString() == "Other Gain")
                            {
                                Utilities.PieChartStats.OtherGain += t.TransactionValue;
                                DrawGainChart();
                            }
                            else if (t.TransactionType.ToString() == "Rent")
                            {
                                Utilities.PieChartStats.Rent += t.TransactionValue;
                                DrawExpenseChart();
                            }
                            else if (t.TransactionType.ToString() == "Transportation")
                            {
                                Utilities.PieChartStats.Transportation += t.TransactionValue;
                                DrawExpenseChart();
                            }
                            else if (t.TransactionType.ToString() == "Entertainment")
                            {
                                Utilities.PieChartStats.Entertainment += t.TransactionValue;
                                DrawExpenseChart();
                            }
                            else if (t.TransactionType.ToString() == "Other Expense")
                            {
                                Utilities.PieChartStats.OtherExpense += t.TransactionValue;
                                DrawExpenseChart();
                            }
                            else if (t.TransactionType.ToString() == "Groceries")
                            {
                                Utilities.PieChartStats.Groceries += t.TransactionValue;
                                DrawExpenseChart();
                            }

                            // set the new transaction as the last processed transaction
                            LastRefresh = t.TransactionDate;
                        }
                    }
                }
                else
                {
                    this.Invoke(new Action(LoadTransactionsDelegate));
                }
            }catch(Exception ex)
            {

            }
            EarningsStatement();
        }

        // calculate earnings statement
        private void EarningsStatement()
        {
            // initialize variables 
            double Balance = 0;
            double WeekGains = 0;
            double WeekLosses = 0;
            double DayGains = 0;
            double DayLosses = 0;

            foreach(Transaction t in Globals._LoggedInUser.Transactions)
            {
                // check to see if the transaction is a gain
                if (t.TransactionType == "Other Gain" || t.TransactionType == "Pay Check" || t.TransactionType == "Refund")
                {
                    Balance += t.TransactionValue;
                    
                    // check if the transaction happened this week
                    if(DateTime.Now.AddDays(-7) < t.TransactionDate)
                    {
                        WeekGains += t.TransactionValue;
                    }

                    // check if the transaction happened today
                    if (DateTime.Now.AddDays(-1) < t.TransactionDate)
                    {
                        DayGains += t.TransactionValue;
                    }
                }
                else // if the transaction is a loss
                {
                    Balance -= t.TransactionValue;

                    // check if the transaction happened this week
                    if(DateTime.Now.AddDays(-7) < t.TransactionDate)
                    {
                        WeekLosses += t.TransactionValue;
                    }

                    // check if the transaction happend today
                    if (DateTime.Now.AddDays(-1) < t.TransactionDate)
                    {
                        DayLosses += t.TransactionValue;
                    }
                }
            }

            // post the updated Earnings Statement
            label14.Text = "$" + string.Format("{0:0.00}", Balance);
            label11.Text = "$" + string.Format("{0:0.00}", WeekGains);
            label9.Text = "$" + string.Format("{0:0.00}", DayGains);
            label12.Text = "$" + string.Format("{0:0.00}", WeekLosses);
            label10.Text = "$" + string.Format("{0:0.00}", DayLosses);


        }

        // draw the expenses pie chart
        private void DrawExpenseChart()
        {
            chart1.Legends[0].Title = "Catagories";
            chart1.Series["Series1"].Points.Clear();
            chart1.Series["Series1"].Points.AddXY("Rent", Utilities.PieChartStats.Rent);
            chart1.Series["Series1"].Points.AddXY("Groceries", Utilities.PieChartStats.Groceries);
            chart1.Series["Series1"].Points.AddXY("Entertainment", Utilities.PieChartStats.Entertainment);
            chart1.Series["Series1"].Points.AddXY("Transportation", Utilities.PieChartStats.Transportation);
            chart1.Series["Series1"].Points.AddXY("Other", Utilities.PieChartStats.OtherExpense);
            chart1.Series["Series1"].IsValueShownAsLabel = true;
        }

        // draw the gains pie chart
        private void DrawGainChart()
        {
            chart2.Legends[0].Title = "Catagories";
            chart2.Series["Series1"].Points.Clear();
            chart2.Series["Series1"].Points.AddXY("PayCheck", Utilities.PieChartStats.PayCheck);
            chart2.Series["Series1"].Points.AddXY("Refund", Utilities.PieChartStats.Refund);
            chart2.Series["Series1"].Points.AddXY("Other", Utilities.PieChartStats.OtherGain);
            chart2.Series["Series1"].IsValueShownAsLabel = true;
        }

        //abort the transaction thread
        public void AbortThread()
        {
            TransactionRefresh.Abort();
        }

        #endregion

    }
}
