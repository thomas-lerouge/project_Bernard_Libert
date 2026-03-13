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

            this.WindowState = FormWindowState.Maximized;

            loadHome();
            loadMenuPanel();
        }
        void loadMenuPanel()
        {
            Panel MenuPanel = new Panel();
            MenuPanel.Size = new Size(this.Width / 5, this.Height);
            MenuPanel.BackColor = Color.Gray;
            Controls.Add(MenuPanel);

            Label lblHome = new Label();
            lblHome.BackColor = Color.DarkGray;
            lblHome.Location = new Point(0, 0);
            lblHome.Size = new Size(MenuPanel.Width, 50);
            lblHome.ForeColor = Color.White;
            lblHome.Text = "home";
            lblHome.TextAlign = ContentAlignment.MiddleCenter;
            lblHome.Font = new Font(lblHome.Font.FontFamily,15,FontStyle.Bold);
            MenuPanel.Controls.Add(lblHome);
        }
        void loadHome()
        {
            this.Controls.Clear();

            Panel panelHome = new Panel();
            panelHome.Size = new Size(this.Width/5 * 4, this.Height);
            panelHome.Location = new Point(this.Width / 5, 0);
            Controls.Add(panelHome);

            Button buttonNewProject = new Button();
            buttonNewProject.Text = "projectje toevoegen";
            buttonNewProject.Location = new Point(350, 250);
            buttonNewProject.Size = new Size(300, 100);
            buttonNewProject.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            panelHome.Controls.Add(buttonNewProject);

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
            panelHome.Controls.Add(buttonToonProjecten);

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
                loadMenuPanel();
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
                    string werf = dataGridView.Rows[rij].Cells[kolom + 1].Value.ToString();
                    deleteRow(werf);
                }
            };

            showAllProjects();

            loadComboBox(comboBox);

            comboBox.SelectedIndexChanged += (sender, e) =>
            {
                string werf = comboBox.SelectedItem.ToString();
                showProject(werf);
            };
        }
        void loadComboBox(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            string query = "SELECT werf FROM Bernard_Libert";
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
                            string werf = reader.GetString("werf");
                            comboBox.Items.Add(werf);
                        }
                    }
                }

            }
        }
        void showProject(string werf)
        {
            string query = $"select * from Bernard_Libert where werf = '{werf}'";

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
            string query = $"select * from Bernard_Libert";

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
                    showAllProjects();
                    loadComboBox(comboBox);
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
                    showAllProjects();
                    loadComboBox(comboBox);
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
                    showAllProjects();
                    loadComboBox(comboBox);
                }

            }
        }
    }
}

