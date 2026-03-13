using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace project_Bernard_Libert
{
    public partial class Form1 : Form
    {
        string connectionString = "Server=bart.go-ao.be;Database=06IncoThomas;UserID=06IncoThomas;Password=kQ5QYVHpnghg0K9F;";

        DataGridView dataGridView = new DataGridView();

        ComboBox comboBox = new ComboBox();

        Panel panelHome = new Panel();
        Panel PanelRijen = new Panel();

        int aantalProjecten = 0;

        List<Project> alleProjecten = new List<Project>();

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(1200, 800);

            loadMain();
            loadMenuPanel();
            loadHeaderPanel();

            loadProjecten();
        }
        void loadHeaderPanel()
        {
            Panel headerPanel = new Panel();
            headerPanel.Location = new Point(0, 0);
            headerPanel.Size = new Size(1200, 80);
            headerPanel.BackColor = Color.FromArgb(24, 28, 38);
            headerPanel.BorderStyle = BorderStyle.FixedSingle;

            Controls.Add(headerPanel);

            Button buttonAddProject = new Button();
            buttonAddProject.Text = "+ Nieuw Project";
            buttonAddProject.Location = new Point(1000, 15);
            buttonAddProject.Size = new Size(150, 50);
            buttonAddProject.BackColor = Color.FromArgb(0, 122, 204);
            buttonAddProject.FlatStyle = FlatStyle.Flat;
            buttonAddProject.FlatAppearance.BorderSize = 0;
            buttonAddProject.ForeColor = Color.White;
            buttonAddProject.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            headerPanel.Controls.Add(buttonAddProject);

            Label lblTitel = new Label();
            lblTitel.Text = "Projecten\r\nBernard Libert";
            lblTitel.Location = new Point(20, 10);
            lblTitel.ForeColor = Color.White;
            lblTitel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitel.AutoSize = true;
            headerPanel.Controls.Add(lblTitel);

        }
        void loadMenuPanel()
        {
            Panel menuPanel = new Panel();
            menuPanel.Location = new Point(0, 80);
            menuPanel.Size = new Size(200, 720);
            menuPanel.BorderStyle = BorderStyle.FixedSingle;
            menuPanel.BackColor = Color.FromArgb(21, 25, 50);
            Controls.Add(menuPanel);

            Label lblAllProjects = new Label();
            lblAllProjects.BackColor = Color.FromArgb(21, 25, 50);
            lblAllProjects.Location = new Point(0, 0);
            lblAllProjects.Size = new Size(menuPanel.Width, 50);
            lblAllProjects.ForeColor = Color.White;
            lblAllProjects.Text = "Alle Projecten";
            lblAllProjects.BorderStyle = BorderStyle.FixedSingle;
            lblAllProjects.TextAlign = ContentAlignment.MiddleCenter;
            lblAllProjects.Font = new Font(lblAllProjects.Font.FontFamily, 15, FontStyle.Regular);
            menuPanel.Controls.Add(lblAllProjects);
        }
        void loadMain()
        {
            panelHome.Location = new Point(200, 80);
            panelHome.Size = new Size(1000, 720);
            panelHome.ForeColor = Color.White;
            panelHome.BackColor = Color.FromArgb(18, 23, 29);
            panelHome.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(panelHome);

            Panel panelKoprij = new Panel();
            panelKoprij.Location = new Point(20, 10);
            panelKoprij.Size = new Size(600, 40);
            panelKoprij.BackColor = Color.FromArgb(34, 38, 48);
            panelKoprij.ForeColor = Color.White;
            panelHome.Controls.Add(panelKoprij);

            // naam, klant, aannemer
            Label koprijNaam = new Label();
            koprijNaam.Text = "Naam";
            koprijNaam.Location = new Point(0, 5);
            koprijNaam.Size = new Size(200, 30);
            koprijNaam.TextAlign = ContentAlignment.MiddleCenter;
            koprijNaam.Font = new Font(koprijNaam.Font.FontFamily, 12, FontStyle.Regular);
            panelKoprij.Controls.Add(koprijNaam);

            Label koprijKlant = new Label();
            koprijKlant.Text = "Klant";
            koprijKlant.Location = new Point(200, 5);
            koprijKlant.Size = new Size(200, 30);
            koprijKlant.TextAlign = ContentAlignment.MiddleCenter;
            koprijKlant.Font = new Font(koprijNaam.Font.FontFamily, 12, FontStyle.Regular);
            panelKoprij.Controls.Add(koprijKlant);

            Label koprijAannemer = new Label();
            koprijAannemer.Text = "Aannemer";
            koprijAannemer.Location = new Point(400, 5);
            koprijAannemer.Size = new Size(200, 30);
            koprijAannemer.TextAlign = ContentAlignment.MiddleCenter;
            koprijAannemer.Font = new Font(koprijNaam.Font.FontFamily, 12, FontStyle.Regular);
            panelKoprij.Controls.Add(koprijAannemer);


            PanelRijen.Location = new Point(10, 60);
            PanelRijen.Size = new Size(620, 600);
            PanelRijen.BackColor = Color.FromArgb(18, 23, 29);
            PanelRijen.BorderStyle = BorderStyle.FixedSingle;
            PanelRijen.ForeColor = Color.White;
            PanelRijen.AutoScroll = true;
            panelHome.Controls.Add(PanelRijen);

        }
        void loadProjecten()
        {
            projectenInlezen();

            for (int i = 0; i < alleProjecten.Count; i++)
            {
                Panel panelRij = new Panel();
                panelRij.Location = new Point(10, 10 + (i * 40) + (i * 5));
                panelRij.Size = new Size(600, 40);
                panelRij.BackColor = Color.FromArgb(40, 40, 50);
                panelRij.BorderStyle = BorderStyle.FixedSingle;
                PanelRijen.Controls.Add(panelRij);

                Label rijNaam = new Label();
                rijNaam.Text = alleProjecten[i].Naam;
                rijNaam.Location = new Point(0, 5);
                rijNaam.Size = new Size(200, 30);
                rijNaam.TextAlign = ContentAlignment.MiddleCenter;
                rijNaam.Font = new Font(rijNaam.Font.FontFamily, 12, FontStyle.Regular);
                panelRij.Controls.Add(rijNaam);

                Label rijKlant = new Label();
                rijKlant.Text = alleProjecten[i].Klant;
                rijKlant.Location = new Point(200, 5);
                rijKlant.Size = new Size(200, 30);
                rijKlant.TextAlign = ContentAlignment.MiddleCenter;
                rijKlant.Font = new Font(rijKlant.Font.FontFamily, 12, FontStyle.Regular);
                panelRij.Controls.Add(rijKlant);

                Label rijAannemer = new Label();
                rijAannemer.Text = alleProjecten[i].Aannemer;
                rijAannemer.Location = new Point(400, 5);
                rijAannemer.Size = new Size(200, 30);
                rijAannemer.TextAlign = ContentAlignment.MiddleCenter;
                rijAannemer.Font = new Font(rijAannemer.Font.FontFamily, 12, FontStyle.Regular);
                panelRij.Controls.Add(rijAannemer);
            }
        }
        void addRow(FormAdd formAdd)
        {
            string query = "INSERT INTO `Bernard_Libert`(werf, klant, aannemer, extra_info) VALUES (@werf, @klant, @aannemer, @extra_info)";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@werf", formAdd.addWerf);
                    command.Parameters.AddWithValue("@klant", formAdd.addKlant);
                    command.Parameters.AddWithValue("@aannemer", formAdd.addAannemer);
                    command.Parameters.AddWithValue("@extra_info", formAdd.addExtraInfo);
                    int rowsAffected = command.ExecuteNonQuery(); // Voert de INSERT uit
                    Console.WriteLine($"{rowsAffected} rij(en) toegevoegd.");
                }

            }
        }
        void editRow(FormEdit formEdit, string waarde)
        {
            string query = "Update `Bernard_Libert`(werf, klant, aannemer, extra_info) VALUES (@werf, @klant, @aannemer, @extra_info)";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@werf", formEdit.editWerf);
                    command.Parameters.AddWithValue("@klant", formEdit.editKlant);
                    command.Parameters.AddWithValue("@aannemer", formEdit.editAannemer);
                    command.Parameters.AddWithValue("@extra_info", formEdit.editExtraInfo);
                    int rowsAffected = command.ExecuteNonQuery(); // Voert de INSERT uit
                    Console.WriteLine($"{rowsAffected} rij(en) toegevoegd.");
                }

            }
        }
        void deleteRow(string werf)
        {
            string query = "DELETE FROM Bernard_Libert WHERE werf = @werf";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@werf", werf);
                    int rowsAffected = command.ExecuteNonQuery(); // Voert de INSERT uit
                    Console.WriteLine($"{rowsAffected} rij(en) toegevoegd.");
                }

            }
        }
        void projectenInlezen()
        {
            string query = "SELECT * FROM Bernard_Libert";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();
                Console.WriteLine("Connection successfully established!");

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
                            alleProjecten.Add(project);
                        }
                    }
                }
            }
        }
    }
}

