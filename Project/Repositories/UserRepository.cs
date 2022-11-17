using Project.Model;
using Project.Repository;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System;
using System.Collections.Generic;

namespace Project.Repositories;

public class UserRepository : Base, IUserRepository
{
    public void Add(User userModel)
    {
        throw new NotImplementedException();
    }

    public bool AuthenticateUser(NetworkCredential credential)
    {
        bool validUser;
        using (var connection = GetConnection())
        using (var command = new SqlCommand())
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "select *from [User] where username=@username and [password]=@password";
            command.Parameters.Add("@username", SqlDbType.NVarChar).Value = credential.UserName;
            command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
            validUser = command.ExecuteScalar() == null ? false : true;
        }
        return validUser;
    }

    public void Edit(User userModel)
    {
        throw new NotImplementedException();
    }
    public IEnumerable<User> GetByAll()
    {
        throw new NotImplementedException();
    }
    public User GetById(int id)
    {
        throw new NotImplementedException();
    }
    public User GetByUsername(string username)
    {
        User user = null;
        using (var connection = GetConnection())
        using (var command = new SqlCommand())
        {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "select *from [User] where username=@username";
            command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User()
                    {
                        Id = reader[0].ToString(),
                        Username = reader[1].ToString(),
                        Password = string.Empty,
                        Name = reader[3].ToString(),
                        Lastname = reader[4].ToString(),
                        Email = reader[5].ToString(),
                    };
                }
            }
        }
        return user;   
    }

    public void Remove(int id)
    {
        throw new NotImplementedException();
    }
}
