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
        int TransactionsLoaded;

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

            // add transaction to the sql table
            Globals._SC.PostTransaction(Globals._LoggedInUser.AccountNumber, monthCalendar1.SelectionRange.Start, (int)numericUpDown1.Value, comboBox1.Text, textBox1.Text);

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

                // add transaction to the sql table
                Globals._SC.PostTransaction(Globals._LoggedInUser.AccountNumber, monthCalendar1.SelectionRange.Start, (int)numericUpDown2.Value, comboBox2.Text, textBox2.Text);

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
                        if(t.TransactionNumber > TransactionsLoaded)
                        {
                            listBox2.Items.Add($"{t.TransactionType} ${t.TransactionValue}  Date: {t.TransactionDate.Date.ToShortDateString()}");
                            listBox2.Items.Add($"    Note: {t.Note}");
                            TransactionsLoaded = t.TransactionNumber;
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
            int Balance = 0;
            int WeekGains = 0;
            int WeekLosses = 0;
            int DayGains = 0;
            int DayLosses = 0;

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
            label14.Text = "$" + Balance + ".00";
            label11.Text = "$" + WeekGains + ".00";
            label9.Text = "$" + DayGains + ".00";
            label12.Text = "$" + WeekLosses + ".00";
            label10.Text = "$" + DayLosses + ".00";


        }






        #endregion

    }
}
