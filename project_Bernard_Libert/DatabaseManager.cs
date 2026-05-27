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
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Project project = new Project();

                        project.Naam = reader.GetString("werf");
                        project.Klant = reader.GetString("klant");
                        project.Aannemer = reader.GetString("aannemer");

                        project.Adres = reader.GetString("adres");
                        project.TypeProject = reader.GetString("typeProject");

                        project.StartDatum = reader.GetDateTime("startdatum");
                        project.VerwachteEindDatum = reader.GetDateTime("verwachteEinddatum");
                        project.WerkelijkeEindDatum = reader.GetDateTime("werkelijkeEinddatum");

                        project.ExtraInfo = reader.GetString("extra_info");

                        projecten.Add(project);
                    }
                }
            }

            return projecten;
        }

        public void AddProject(Project project)
        {
            string query =
            @"INSERT INTO Bernard_Libert
            (werf, klant, aannemer, adres, typeProject,
            startdatum, verwachteEinddatum,
            werkelijkeEinddatum, extra_info)

            VALUES
            (@werf,@klant,@aannemer,@adres,@typeProject,
            @start,@verwacht,@werkelijk,@extra)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@werf", project.Naam);
                command.Parameters.AddWithValue("@klant", project.Klant);
                command.Parameters.AddWithValue("@aannemer", project.Aannemer);

                command.Parameters.AddWithValue("@adres", project.Adres);
                command.Parameters.AddWithValue("@typeProject", project.TypeProject);

                command.Parameters.AddWithValue("@start", project.StartDatum);
                command.Parameters.AddWithValue("@verwacht", project.VerwachteEindDatum);
                command.Parameters.AddWithValue("@werkelijk", project.WerkelijkeEindDatum);

                command.Parameters.AddWithValue("@extra", project.ExtraInfo);

                command.ExecuteNonQuery();
            }
        }

        public void EditProject(Project project, string oudeWerf)
        {
            string query =
            @"UPDATE Bernard_Libert
            SET
            werf=@werf,
            klant=@klant,
            aannemer=@aannemer,
            adres=@adres,
            typeProject=@typeProject,
            startdatum=@start,
            verwachteEinddatum=@verwacht,
            werkelijkeEinddatum=@werkelijk,
            extra_info=@extra

            WHERE werf=@oudeWerf";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@oudeWerf", oudeWerf);

                command.Parameters.AddWithValue("@werf", project.Naam);
                command.Parameters.AddWithValue("@klant", project.Klant);
                command.Parameters.AddWithValue("@aannemer", project.Aannemer);

                command.Parameters.AddWithValue("@adres", project.Adres);
                command.Parameters.AddWithValue("@typeProject", project.TypeProject);

                command.Parameters.AddWithValue("@start", project.StartDatum);
                command.Parameters.AddWithValue("@verwacht", project.VerwachteEindDatum);
                command.Parameters.AddWithValue("@werkelijk", project.WerkelijkeEindDatum);

                command.Parameters.AddWithValue("@extra", project.ExtraInfo);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteProject(string werf)
        {
            string query = "DELETE FROM Bernard_Libert WHERE werf=@werf";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@werf", werf);

                command.ExecuteNonQuery();
            }
        }
    }
}