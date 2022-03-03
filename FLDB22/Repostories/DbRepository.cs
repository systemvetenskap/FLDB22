using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLDB22.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FLDB22.Repostories
{
    internal class DbRepository
    {
        private string _connectionString;

        public DbRepository()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<DbRepository>()
                        .Build();

            _connectionString = config.GetConnectionString("dbConn");
        }
        public void AddPerson()
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                StringBuilder sql = new StringBuilder("insert into person ");
                sql.AppendLine("(firstname, lastname, rank_id) ");
                sql.AppendLine("values('Anna', 'Pirate Queen', 6) ");
           

                using var command = new NpgsqlCommand(sql.ToString(), conn);
                var result = command.ExecuteScalar();
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "23503")
                {
                    if (ex.ConstraintName== "person_sex_id_fkey")
                    {
                        throw new Exception("fel kön", ex);

                    }
                    throw new Exception("Du försöker ge en rank till en pirat som inte existerar", ex);

                }
                throw new Exception("Allvarligt fel, får inte kontakt med databasen", ex);
            }
        }

        public Person GetPersonById(int id)
        {
            Person? person = null;

            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                StringBuilder sql = new StringBuilder("select ");
                sql.AppendLine("id, firstname, lastname ");
                sql.AppendLine("from persons ");
                sql.AppendLine("where id =@id  ");

                using var command = new NpgsqlCommand(sql.ToString(), conn);
                command.Parameters.AddWithValue("id", id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        person = new Person()
                        {
                            Id = reader.GetInt32(0),
                            Firstname = (string)reader["firstname"],
                            Lastname = reader["lastname"] == DBNull.Value ? null : (string?)reader["lastname"]
                        };
                    }
                }
            }
            catch (PostgresException ex)
            {

                throw new Exception("Allvarligt fel, får inte kontakt med databasen",ex);
            }
            return person;
        }

        public List<Person> GetPersons()
        {
            var people = new List<Person>();
            using var conn = new NpgsqlConnection(_connectionString);
            // Kopplar upp mig mot databasen.
            conn.Open();
            

            using var cmd = new NpgsqlCommand();
            cmd.CommandText ="select * from person";
            cmd.Connection = conn;
            Person? person = null; 
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    person = new Person()
                    {
                        Id = reader.GetInt32(0),
                        Firstname = (string)reader["firstname"],
                        Lastname =reader["lastname"] == DBNull.Value ? null : (string?)reader["lastname"]
                    };
                    people.Add(person);
                }
            }


            return people;
        }
    }
}
 