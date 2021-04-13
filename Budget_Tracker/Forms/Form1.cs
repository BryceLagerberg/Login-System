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
        DateTime LastRefresh;

        public Form1()
        {
            InitializeComponent();
        }

        List<double> _Totals = new List<double>();
        
        
        #region Events
        
        // On Load
        private void Form1_Load(object sender, EventArgs e)
        {
            // start your threads
            TransactionRefresh = new Thread(LoadTransactions);
            TransactionRefresh.IsBackground = true;
            TransactionRefresh.Start();
        }

        // gains button press event
        private void button1_Click(object sender, EventArgs e)
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
        private void button2_Click(object sender, EventArgs e)
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

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            // Load Gains / Losses based on the selected day (This will require that data is saved somewhere)
        }
        #endregion




        #region Functions

        // add a grain to the budget tracker
        private void AddGain(string Catagory, double Amount, string Note)
        {
            listBox1.Items.Add($"{Catagory} +${Amount}  Date: {monthCalendar1.SelectionRange.Start.Date.ToShortDateString()}");
            listBox1.Items.Add($"     Note: {Note}");

            // Add to Totals
            

            // Add Revenue listbox

            // Adjust Daily gains
            
            //label9.Text = 

            // Adjust Weekly Gains
        }
        
        // add a loss to the budget tracker
        private void AddLoss(string Catagory, double Amount, string Note)
        {
            listBox1.Items.Add($"{Catagory} -${Amount}  Date: {monthCalendar1.SelectionRange.Start.Date.ToShortDateString()}");
            listBox1.Items.Add($"    Note: {Note}");

            // add to totals

            // Adjust Daily expenses

            // Adjust Weekly expenses
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
            }catch(Exception ex)
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
                        // new message check
                        if(t.TransactionDate > LastRefresh)
                        {
                            listBox2.Items.Add($"{t.TransactionType} ${t.TransactionValue}  Date: {t.TransactionDate.Date.ToShortDateString()}");
                            listBox2.Items.Add($"    Note: {t.Note}");
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






        #endregion

    }
}
