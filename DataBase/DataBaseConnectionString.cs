namespace SuperBasketBall.DataBase;

public class DataBaseConnectionString
{
    private static string _connectionString;

    public static string ConnectionString
    {
        get => _connectionString;
    }
}