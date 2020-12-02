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
        Queue<int> vs;
        List<int> totalList = new List<int>();
        bool[] judgeNumber = new bool[46];
        OpenFileDialog openFileDialog1 = new OpenFileDialog();

        public Form1()
        {
            InitializeComponent();
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
            DisableNumericUpDown();
            textBox1.Enabled = false;
            button1.Enabled = true;
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Database files (*.mdb)|*.mdb";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
            CreateTableInToMdb(openFileDialog1.FileName);
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
        }

        private void ChooseTotalList()
        {
            InitializeJudgeNumber();
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
            textBox1.Enabled = true;
            textBox1.Clear();
            //textBox2.Enabled = true;
            textBox2.Clear();
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

        public void CreateTableInToMdb(string fileNameWithPath)
        {
            try
            {
                OleDbConnection myConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + fileNameWithPath);
                myConnection.Open();
                OleDbCommand myCommand = new OleDbCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = "CREATE TABLE my_table([m] NUMBER, [n] NUMBER, [k] NUMBER, [j] Number," +
                    "[s] NUMBER, [n numbers] TEXT,[minium number of sets] NUMBER, [answer] TEXT)";
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch { }
        }

        public void InsertToMdb(string fileNameWithPath)
        {
            var con = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + fileNameWithPath);
            var cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "insert into my_table ([m],[n],[k],[j],[s],[n numbers],[minium number of sets], [answer])  " +
                "values (@m, @n, @k,@j,@s,@series1, @number, @answer);";
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

       
    }
       
}
