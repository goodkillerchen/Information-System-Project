using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Information_System_Project
{
    public partial class Form1 : Form
    {
        List<int> totalList = new List<int>();
        bool[] judgeNumber = new bool[46];
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

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

        private void button1_Click(object sender, EventArgs e)
        {
            ChooseTotalList();
            DisableNumericUpDown();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Algorithm algorithm = new Algorithm((int)numericUpDown2.Value, (int)numericUpDown3.Value, 
                (int)numericUpDown4.Value, (int)numericUpDown5.Value, totalList, judgeNumber);
            textBox2.Text=algorithm.EexcuteAlgorithm().ToString();
            
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
        private void DisableNumericUpDown()
        {
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;
            numericUpDown4.Enabled = false;
            numericUpDown5.Enabled = false;
            textBox1.Enabled = false;
        }
    }
}
