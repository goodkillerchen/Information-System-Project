public void InsertToMdb(string fileNameWithPath)
{
    var con = new OleDbConnection(
        "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " 
            + fileNameWithPath);
    var cmd = new OleDbCommand();
    cmd.Connection = con;
    cmd.CommandText = "insert into my_table ([m],[n],[k],[j]," + 
        "[s],[n numbers],[minium number of sets], [answer])" +
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