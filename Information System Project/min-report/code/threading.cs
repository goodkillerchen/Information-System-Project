private async void button2_Click(object sender, EventArgs e)
// Run buttom
{
    button2.Enabled = false;
    Algorithm algorithm = new Algorithm(
        (int)numericUpDown2.Value, 
            (int)numericUpDown3.Value,
                (int)numericUpDown4.Value, 
                    (int)numericUpDown5.Value, 
                        totalList, judgeNumber);
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