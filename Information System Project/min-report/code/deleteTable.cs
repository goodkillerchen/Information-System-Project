private void DeleteRecordFromMdb
    (string fileNameWithPath,string num)
{
    int number = Int32.Parse(num);
    var con = new OleDbConnection
        ("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " 
            + fileNameWithPath);
    var cmd = new OleDbCommand();
    con.Open();
    cmd.Connection = con;
    cmd.CommandText = "DELETE FROM [my_table] " +
        "WHERE [order]=" + number + "";
    cmd.ExecuteNonQuery();
    con.Close();
}

private void DeleteAllRecordFromMdb
    (string fileNameWithPath)
{
    var con = new OleDbConnection
        ("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " 
            + fileNameWithPath);
    var cmd = new OleDbCommand();
    con.Open();
    cmd.Connection = con;
    cmd.CommandText = "DELETE FROM [my_table] ";
    cmd.ExecuteNonQuery();
    con.Close();
}