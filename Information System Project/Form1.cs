using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Information_System_Project
{
    public partial class Form1 : Form
    {
        int cnt = 1;
        Queue<int> vs;
        List<int> totalList = new List<int>();
        bool[] judgeNumber = new bool[55];
        OpenFileDialog openFileDialog1 = new OpenFileDialog();

        public Form1()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Times",60);
            listView1.Columns.Add("Number", 60);
            listView1.Columns.Add("Data",244);
        }
        

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown3.Maximum = numericUpDown2.Value;

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown4.Maximum = numericUpDown3.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown5.Maximum = numericUpDown4.Value;
        }

        private void button1_Click(object sender, EventArgs e)//Confim button
        {
            button1.Enabled=false;
            ChooseTotalList();
            //DisableNumericUpDown();
            textBox2.Enabled = false;
            button1.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = true;
        }

        private async void button2_Click(object sender, EventArgs e)// Run buttom
        {
            button2.Enabled = false;
            Algorithm algorithm = new Algorithm((int)numericUpDown2.Value, (int)numericUpDown3.Value,
                (int)numericUpDown4.Value, (int)numericUpDown5.Value, totalList, judgeNumber);
            if (numericUpDown4.Value == numericUpDown5.Value)
            {
                vs= await Task.Run(()=>algorithm.ExecuteAlgorithm1());
            }
            else
            {
                vs = await Task.Run(()=>algorithm.ExecuteAlgorithm2());
            }
            //InsertToMdb(openFileDialog1.FileName);
            //UpdateToMdb(openFileDialog1.FileName);
            textBox3.Text = GetSeries2();
            //textBox3.Enabled = false;
            

        }

        private void button3_Click(object sender, EventArgs e)// File button
        {
           
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Database files (*.mdb)|*.mdb";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DisableNumericUpDown();
                textBox1.Enabled = false;
                button1.Enabled = true;
                textBox2.Text = openFileDialog1.FileName;
                CreateTableInToMdb(openFileDialog1.FileName);
            }
            
        }

        private void button4_Click(object sender, EventArgs e)//Clear function
        {
            InitializeFunctionForClear();
        }

        private void button5_Click(object sender, EventArgs e)//Open the file button
        {
            Process proc = new Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = openFileDialog1.FileName;
            proc.Start();
        }

        private void button6_Click(object sender, EventArgs e)//Insert to database button
        {
            InsertToMdb(openFileDialog1.FileName);
            string[] arr = new string[3];
            arr[0] = cnt.ToString();
            arr[1] = vs.Count.ToString();
            arr[2] = numericUpDown1.Value.ToString() + " " + numericUpDown2.Value.ToString() +
                " " + numericUpDown3.Value.ToString() + " " + numericUpDown4.Value.ToString() + " " + numericUpDown5.Value.ToString();
            ListViewItem itm=new ListViewItem(arr);
            listView1.Items.Add(itm);
            cnt++;
        }

        private void button7_Click(object sender, EventArgs e)//reset button
        {
            listView1.Items.Clear();
            cnt = 1;
            DeleteAllRecordFromMdb(openFileDialog1.FileName);
        }

        private void ChooseTotalList()
        {
            InitializeJudgeNumber();
            totalList.Clear();
            var rand = new Random();
            StringBuilder str = new StringBuilder();
            for (int i = 1; i <= numericUpDown2.Value; i++)
            {
                int randNumber = rand.Next(1, (int)numericUpDown1.Value + 1);
                if (!judgeNumber[randNumber])
                {
                    judgeNumber[randNumber] = true;
                    totalList.Add(randNumber);
                    str.Append(totalList[i - 1].ToString());
                    str.Append(" ");
                }
                else
                {
                    i--;
                }
            }
            textBox1.Text = str.ToString();
        }
        private void InitializeJudgeNumber()
        {
            for (int i = 0; i < judgeNumber.Length; i++)
                judgeNumber[i] = false;
        }

        private void InitializeFunctionForClear()
        {
            EnableNumericUpDown();
            button1.Enabled = true;
            textBox1.Enabled = true;
            textBox1.Clear();
            //textBox2.Enabled = true;
            //textBox2.Clear();
            textBox3.Clear();
            button3.Enabled = true;
        }

        private void DisableNumericUpDown()
        {
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;
            numericUpDown4.Enabled = false;
            numericUpDown5.Enabled = false;
        }

        private void EnableNumericUpDown()
        {
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
            numericUpDown4.Enabled = true;
            numericUpDown5.Enabled = true;
        }

        private void CreateTableInToMdb(string fileNameWithPath)
        {
            try
            {
                OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + fileNameWithPath);
                myConnection.Open();
                OleDbCommand myCommand = new OleDbCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = "CREATE TABLE my_table" +
                    "([order] NUMBER, "+
                    "[m] NUMBER, " +
                    "[n] NUMBER, " +
                    "[k] NUMBER, " +
                    "[j] Number," +
                    "[s] NUMBER, " +
                    "[n numbers] TEXT," +
                    "[minium number of sets] NUMBER, " +
                    "[answer] TEXT)";
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch { }
        }

        private void InsertToMdb(string fileNameWithPath)
        {
            var con = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + fileNameWithPath);
            var cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "insert into my_table ([order],[m],[n],[k],[j],[s],[n numbers],[minium number of sets], [answer])  " +
                "values (@order, @m, @n, @k,@j,@s,@series1, @number, @answer);";
            cmd.Parameters.AddWithValue("@order", cnt);
            cmd.Parameters.AddWithValue("@m", numericUpDown1.Value);
            cmd.Parameters.AddWithValue("@n", numericUpDown2.Value);
            cmd.Parameters.AddWithValue("@k", numericUpDown3.Value);
            cmd.Parameters.AddWithValue("@j", numericUpDown4.Value);
            cmd.Parameters.AddWithValue("@s", numericUpDown5.Value);
            cmd.Parameters.AddWithValue("@series1", series1Fordb());
            cmd.Parameters.AddWithValue("@number", vs.Count());
            cmd.Parameters.AddWithValue("@answer", series2Fordb());
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void DeleteRecordFromMdb(string fileNameWithPath,string num)
        {
            int number = Int32.Parse(num);
            var con = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + fileNameWithPath);
            var cmd = new OleDbCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "DELETE FROM [my_table] " +
                "WHERE [order]=" + number + "";
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void DeleteAllRecordFromMdb(string fileNameWithPath)
        {
            var con = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + fileNameWithPath);
            var cmd = new OleDbCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "DELETE FROM [my_table] ";
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private string series1Fordb()
        {
            string series1 = "";
            foreach (var num in totalList)
            {
                series1 += num.ToString();
                series1 += " ";
            }
            return series1;
        }

        private string series2Fordb()
        {
            string series2 = "";
            int index = 0;
            foreach(var num in vs)
            {
                if (index != 0)
                    series2 += ";  ";
                for (int i = 0; i < numericUpDown2.Value; i++)
                {
                    if (((1 << i) & num) != 0)
                    {
                        series2 += totalList[i].ToString();
                        series2 += " ";
                    }
                }
                
                index++;
            }
            return series2;
        }

        private string GetSeries2()
        {
            string series2 = "";
            foreach(var num in vs)
            {
                for(int i = 0; i < numericUpDown2.Value; i++)
                {
                    if (((1 << i) & num)!=0)
                    {
                        series2 += totalList[i].ToString();
                        series2 += " ";
                    }
                }
                series2 += "\r\n";
            }
            return series2;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var selectedItemText = (listView1.SelectedItem ?? "(none)").ToString();
            //MessageBox.Show("Selected: " + selectedItemText);

        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count >= 1 && e.Button==MouseButtons.Right)
            {
                ListViewItem item = listView1.SelectedItems[0];

                //here i check for the Mouse pointer location on click if its contained 
                // in the actual selected item's bounds or not .
                // cuz i ran into a problem with the ui once because of that ..
                if (item.Bounds.Contains(e.Location))
                {
                    ContextMenu cm = new ContextMenu();
                    MenuItem menuItemForDelete = new MenuItem();
                    menuItemForDelete.Text = "Delete";
                    menuItemForDelete.Click += new EventHandler(menuItemForDelete_Click);
                    cm.MenuItems.Add(menuItemForDelete);
                    listView1.ContextMenu = cm;
                }
            }
        }

        private void menuItemForDelete_Click(object sender,EventArgs e)
        {
            var element = listView1.SelectedItems[0];
            DeleteRecordFromMdb(openFileDialog1.FileName, element.SubItems[0].Text);
            listView1.Items.Remove(listView1.SelectedItems[0]);
            
        }
    }
       
}
