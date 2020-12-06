public void CreateTableInToMdb(string fileNameWithPath)
{
    try
    {
        OleDbConnection myConnection = new OleDbConnection
            ("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" 
                + fileNameWithPath);
        myConnection.Open();
        OleDbCommand myCommand = new OleDbCommand();
        myCommand.Connection = myConnection;
        myCommand.CommandText = 
        "CREATE TABLE my_table([m] NUMBER," + 
            "[n] NUMBER, [k] NUMBER, [j] Number," +
                "[s] NUMBER, [n numbers] TEXT," +
                    "[minium number of sets] NUMBER,"+ 
                        "[answer] TEXT)";
        myCommand.ExecuteNonQuery();
        myCommand.Connection.Close();
    }
    catch { }
}