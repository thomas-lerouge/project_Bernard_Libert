using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_Bernard_Libert
{
    internal class DatabaseManager
    {
        string connectionString = "Server=bart.go-ao.be;Database=06IncoThomas;UserID=06IncoThomas;Password=kQ5QYVHpnghg0K9F;";

        public List<Project> GetProjecten()
        {
            List<Project> projecten = new List<Project>();

            string query = "SELECT * FROM Bernard_Libert";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project project = new Project();

                            project.Naam = reader.GetString("werf");
                            project.Klant = reader.GetString("klant");
                            project.Aannemer = reader.GetString("aannemer");
                            project.ExtraInfo = reader.GetString("extra_info");
                            try
                            {
                                project.Datum = reader.GetDateTime("datum");
                            }
                            catch
                            {

                            }
                            projecten.Add(project);
                        }
                    }
                }
            }

            return projecten;
        }

        public void AddProject(Project project)
        {
            string query = "INSERT INTO Bernard_Libert(werf, klant, aannemer, datum, extra_info) VALUES (@werf, @klant, @aannemer, @datum, @extra_info)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@werf", project.Naam);
                    command.Parameters.AddWithValue("@klant", project.Klant);
                    command.Parameters.AddWithValue("@aannemer", project.Aannemer);
                    command.Parameters.AddWithValue("@datum", project.Datum);
                    command.Parameters.AddWithValue("@extra_info", project.ExtraInfo);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void EditProject(Project project, string oudeWerf)
        {
            string query = "UPDATE Bernard_Libert SET werf = @werf, klant = @klant, aannemer = @aannemer, datum = @datum, extra_info = @extra_info WHERE werf = @oudeWerf";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@oudeWerf", oudeWerf);
                    command.Parameters.AddWithValue("@werf", project.Naam);
                    command.Parameters.AddWithValue("@klant", project.Klant);
                    command.Parameters.AddWithValue("@aannemer", project.Aannemer);
                    command.Parameters.AddWithValue("@datum", project.Datum);
                    command.Parameters.AddWithValue("@extra_info", project.ExtraInfo);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteProject(string werf)
        {
            string query = "DELETE FROM Bernard_Libert WHERE werf = @werf";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@werf", werf);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
