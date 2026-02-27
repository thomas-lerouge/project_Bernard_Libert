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

        public Form1()
        {
            InitializeComponent();

            this.Size = new Size(1000, 800);

            loadHome();
        }
        void loadHome()
        {
            this.Controls.Clear();

            Button buttonNewProject = new Button();
            buttonNewProject.Text = "project toevoegen";
            buttonNewProject.Location = new Point(350, 250);
            buttonNewProject.Size = new Size(300, 100);
            buttonNewProject.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            Controls.Add(buttonNewProject);

            buttonNewProject.Click += (sender, e) =>
            {
                FormAdd formAdd = new FormAdd();
                formAdd.Show();
                formAdd.Ready += (sender2, e2) =>
                {
                    addRow(formAdd);
                    formAdd.Close();
                };
            };

            Button buttonToonProjecten = new Button();
            buttonToonProjecten.Text = "Toon projecten";
            buttonToonProjecten.Location = new Point(350, 100);
            buttonToonProjecten.Size = new Size(300, 100);
            buttonToonProjecten.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            Controls.Add(buttonToonProjecten);

            buttonToonProjecten.Click += (sender, e) =>
            {
                loadProjecten();
            };
        }
        void loadProjecten()
        {
            this.Controls.Clear();

            comboBox.Location = new Point(10, 60);
            Controls.Add(comboBox);

            dataGridView.Location = new Point(10, 100);
            dataGridView.Size = new Size(500, 300);
            dataGridView.ScrollBars = ScrollBars.Vertical;
            Controls.Add(dataGridView);

            PictureBox buttonHome = new PictureBox();
            buttonHome.Size = new Size(40, 40);
            buttonHome.Location = new Point(10, 10);
            buttonHome.Image = Image.FromFile("home.png");
            buttonHome.SizeMode = PictureBoxSizeMode.Zoom;
            buttonHome.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(buttonHome);

            buttonHome.Click += (sender, e) =>
            {
                loadHome();
            };

            Button buttonNewProject = new Button();
            buttonNewProject.Text = "project toevoegen";
            buttonNewProject.Location = new Point(10, 410);
            buttonNewProject.Size = new Size(100, 50);
            Controls.Add(buttonNewProject);

            buttonNewProject.Click += (sender, e) =>
            {
                FormAdd formAdd = new FormAdd();
                formAdd.Show();
                formAdd.Ready += (sender2, e2) =>
                {
                    addRow(formAdd);
                    formAdd.Close();
                };
            };

            Button buttonToonAlles = new Button();
            buttonToonAlles.Text = "toon alle projecten";
            buttonToonAlles.Location = new Point(110, 410);
            buttonToonAlles.Size = new Size(100, 50);
            Controls.Add(buttonToonAlles);

            buttonToonAlles.Click += (sender, e) =>
            {
                comboBox.Text = string.Empty;
                showAllProjects();
            };

            Label lblEditInfo = new Label();
            lblEditInfo.Text = "Klik op de id van een project\r\nom de info ervan te bewerken";
            lblEditInfo.Location = new Point(550, 100);
            lblEditInfo.ForeColor = Color.Red;
            lblEditInfo.BorderStyle = BorderStyle.FixedSingle;
            lblEditInfo.AutoSize = true;
            Controls.Add(lblEditInfo);

            Label lblDeleteInfo = new Label();
            lblDeleteInfo.Text = "Klik op de lege plaats voor een project\r\nom het project te verwijderen";
            lblDeleteInfo.Location = new Point(550, 150);
            lblDeleteInfo.ForeColor = Color.Red;
            lblDeleteInfo.BorderStyle = BorderStyle.FixedSingle;
            lblDeleteInfo.AutoSize = true;
            Controls.Add(lblDeleteInfo);

            dataGridView.CellClick += (sender, e) =>
            {
                // e is van het type DataGridViewCellEventArgs
                int rij = e.RowIndex;
                int kolom = e.ColumnIndex;
                Console.WriteLine($"Je hebt geklikt op rij {rij} en kolom {kolom}");

                if (kolom == 0)
                {
                    string waarde = dataGridView.Rows[rij].Cells[kolom].Value.ToString();
                    FormEdit formEdit = new FormEdit();
                    formEdit.Show();

                    formEdit.EditReady += (sender2, e2) =>
                    {
                        editRow(formEdit, waarde);
                        formEdit.Close();
                    };
                }
                if (kolom == -1)
                {
                    string id = dataGridView.Rows[rij].Cells[kolom + 1].Value.ToString();
                    deleteRow(id);
                }
            };

            showAllProjects();

            loadComboBox(comboBox);

            comboBox.SelectedIndexChanged += (sender, e) =>
            {
                string naam = comboBox.SelectedItem.ToString();
                showProject(naam);
            };
        }
        void loadComboBox(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            string query = "SELECT Naam FROM Demo_Project_Bernard_Libert";
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
                            string naam = reader.GetString("Naam");
                            comboBox.Items.Add(naam);
                        }
                    }
                }

            }
        }
        void showProject(string naam)
        {
            string query = $"select * from Demo_Project_Bernard_Libert where Naam = '{naam}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();
                Console.WriteLine("Connection successfully established!");

                // maak een data adapter (dit maakt ahw een verbinding ts de server en DataTable
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                // maak een DataTable: dit object kan tabel gegevens bijhouden
                DataTable table = new DataTable();
                // vraag aan de data adaptor om de tabel op te vullen
                adapter.Fill(table);
                // gebruik tenslotte de tabel als gegevensbron voor de DataGridView
                dataGridView.DataSource = table;
            }
        }
        void showAllProjects()
        {
            string query = $"select * from Demo_Project_Bernard_Libert";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();
                Console.WriteLine("Connection successfully established!");

                // maak een data adapter (dit maakt ahw een verbinding ts de server en DataTable
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                // maak een DataTable: dit object kan tabel gegevens bijhouden
                DataTable table = new DataTable();
                // vraag aan de data adaptor om de tabel op te vullen
                adapter.Fill(table);
                // gebruik tenslotte de tabel als gegevensbron voor de DataGridView
                dataGridView.DataSource = table;
            }
        }
        void addRow(FormAdd formAdd)
        {
            string query = "INSERT INTO `Demo_Project_Bernard_Libert`(Naam, Locatie, ExtraInfo) VALUES (@Naam, @Locatie, @ExtraInfo)";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Naam", formAdd.addNaam);
                    command.Parameters.AddWithValue("@Locatie", formAdd.addLocatie);
                    command.Parameters.AddWithValue("@ExtraInfo", formAdd.addExtraInfo);
                    int rowsAffected = command.ExecuteNonQuery(); // Voert de INSERT uit
                    Console.WriteLine($"{rowsAffected} rij(en) toegevoegd.");
                    showAllProjects();
                    loadComboBox(comboBox);
                }

            }
        }
        void editRow(FormEdit formEdit, string waarde)
        {
            string query = "UPDATE Demo_Project_Bernard_Libert SET Naam = @Naam, Locatie = @Locatie, ExtraInfo = @ExtraInfo WHERE id = @id";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Naam", formEdit.EditNaam);
                    command.Parameters.AddWithValue("@Locatie", formEdit.EditLocatie);
                    command.Parameters.AddWithValue("@ExtraInfo", formEdit.EditExtraInfo);
                    command.Parameters.AddWithValue("@id", waarde);
                    int rowsAffected = command.ExecuteNonQuery(); // Voert de INSERT uit
                    Console.WriteLine($"{rowsAffected} rij(en) toegevoegd.");
                    showAllProjects();
                    loadComboBox(comboBox);
                }

            }
        }
        void deleteRow(string id)
        {
            string query = "DELETE FROM Demo_Project_Bernard_Libert WHERE id = @id";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@id", id);
                    int rowsAffected = command.ExecuteNonQuery(); // Voert de INSERT uit
                    Console.WriteLine($"{rowsAffected} rij(en) toegevoegd.");
                    showAllProjects();
                }

            }
        }
    }
}

