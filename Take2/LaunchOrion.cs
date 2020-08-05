using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Take2
{
    public partial class LaunchOrion : Form
    {
        string connectionString;
        public LaunchOrion()
        {
            Thread thread = new Thread(new ThreadStart(StartScreen));
            thread.Start();
            Thread.Sleep(2000);
            InitializeComponent();
            thread.Abort();
            //this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
        }



        /// <summary>
        /// Splash Screen
        /// </summary>
        public void StartScreen()
        {
            Application.Run(new Startup_Screen());
        }

        /// <summary>
        /// Binding navigator menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orionTableBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.orionTableBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.take2DatabaseDataSet);
        }

        /// <summary>
        /// Updates database with info from table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.orionTableTableAdapter.Fill(this.take2DatabaseDataSet.OrionTable);
        }

        /// <summary>
        /// Launches Orion based on selected IP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxLaunchButton_Click(object sender, EventArgs e)
        {
            if (uxDataTable.CurrentCell == null) return;

            var selectedCell = uxDataTable.CurrentCell.Value.ToString();

            if (Regex.IsMatch(selectedCell, @"^\d")) // if it starts with num (IP)
            {
                var options = new ChromeOptions
                {
                    AcceptInsecureCertificates = true // For untrusted certificates
                };
                var driver = new ChromeDriver(options);
                driver.Url = $"https://{selectedCell}";
                Login(driver, "novatech", "novatech");
            }
            else MessageBox.Show("You can launch an Orion by selecting the IP address, then click 'Launch Orion.'", ":("); // doesn't start with num and won't launch
        }

        /// <summary>
        /// Logs into Orion.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        void Login(ChromeDriver driver, string username, string password)
        {
            try
            {
                driver.FindElementById("username").SendKeys(username);
                driver.FindElementById("password").SendKeys(password);
                driver.FindElementByXPath("/html/body/div[2]/form/div/div[8]/input").Click();
            }
            catch (Exception)
            {
                MessageBox.Show("Error launching: HTML element has been renamed", ":(");
            }
        }
    }
}
