using System.Data.SqlClient;

namespace Project.Repository;

public abstract class Base
{
    private readonly string _connectionString;
    public Base()
    {
        _connectionString = "Server=(local); Database=MVVMLoginDb; Integrated Security=true";
    }
    protected SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
