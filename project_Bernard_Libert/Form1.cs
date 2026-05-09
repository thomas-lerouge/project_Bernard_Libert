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
        DatabaseManager db = new DatabaseManager();

        List<string> logLijst = new List<string>();

        DataGridView dataGridView = new DataGridView();

        ComboBox comboBox = new ComboBox();

        Panel panelHome = new Panel();
        Panel PanelRijen = new Panel();
        Panel PanelDetails = new Panel();
        Panel panelAddRow = new Panel();
        Panel panelEditRow = new Panel();

        int aantalProjecten = 0;

        List<Project> alleProjecten = new List<Project>();

        Label lblInfoNaam = new Label();
        Label lblInfoKlant = new Label();
        Label lblInfoAannemer = new Label();
        Label lblInfoExtraInfo = new Label();

        TextBox textBoxWerfEdit = new TextBox();
        TextBox textBoxKlantEdit = new TextBox();
        TextBox textBoxAannemerEdit = new TextBox();
        TextBox textBoxExtraInfoEdit = new TextBox();


        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(1200, 800);

            loadMain();
            loadMenuPanel();
            loadHeaderPanel();
            loadDetailsPanel();
            loadPanelAddRow();
            panelAddRow.Visible = false;

            loadPanelEditRow();
            panelEditRow.Visible = false;

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

            buttonAddProject.Click += (sender, e) =>
            {
                panelAddRow.Visible = true;
                panelHome.Visible = false;
            };


            Label lblTitel = new Label();
            lblTitel.Text = "Projecten\r\nBernard Libert";
            lblTitel.Location = new Point(20, 10);
            lblTitel.ForeColor = Color.White;
            lblTitel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitel.AutoSize = true;
            headerPanel.Controls.Add(lblTitel);

            Panel panelZoeken = new Panel();
            panelZoeken.Location = new Point(450, 20);
            panelZoeken.Size = new Size(280, 38);
            panelZoeken.BackColor = Color.FromArgb(40, 44, 58);
            panelZoeken.BorderStyle = BorderStyle.FixedSingle;
            headerPanel.Controls.Add(panelZoeken);

            TextBox textBoxZoeken = new TextBox();
            textBoxZoeken.Location = new Point(8, 8);
            textBoxZoeken.Size = new Size(262, 22);
            textBoxZoeken.BackColor = Color.FromArgb(40, 44, 58);
            textBoxZoeken.ForeColor = Color.White;
            textBoxZoeken.BorderStyle = BorderStyle.None;
            textBoxZoeken.Font = new Font("Segoe UI", 11);
            panelZoeken.Controls.Add(textBoxZoeken);

            textBoxZoeken.TextChanged += (sender, e) =>
            {
                ZoekProjecten(textBoxZoeken.Text);
            };

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
            panelHome.Size = new Size(640, 720);
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
        void loadDetailsPanel()
        {
            PanelDetails.Location = new Point(840, 80);
            PanelDetails.Size = new Size(360, 720);
            PanelDetails.BackColor = Color.FromArgb(18, 23, 29);
            PanelDetails.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(PanelDetails);

            Label lblInfoTitel = new Label();
            lblInfoTitel.Text = "Project Details";
            lblInfoTitel.Location = new Point(10, 10);
            lblInfoTitel.Size = new Size(320, 30);
            lblInfoTitel.ForeColor = Color.White;
            lblInfoTitel.Font = new Font(lblInfoTitel.Font.FontFamily, 15, FontStyle.Bold);
            lblInfoTitel.TextAlign = ContentAlignment.MiddleCenter;
            lblInfoTitel.BorderStyle = BorderStyle.FixedSingle;
            PanelDetails.Controls.Add(lblInfoTitel);

            lblInfoNaam.Text = "Naam: ";
            lblInfoNaam.Location = new Point(10, 50);
            lblInfoNaam.Size = new Size(320, 30);
            lblInfoNaam.ForeColor = Color.White;
            lblInfoNaam.Font = new Font(lblInfoNaam.Font.FontFamily, 12, FontStyle.Regular);
            lblInfoNaam.TextAlign = ContentAlignment.MiddleCenter;
            PanelDetails.Controls.Add(lblInfoNaam);

            lblInfoKlant.Text = "Klant: ";
            lblInfoKlant.Location = new Point(10, 90);
            lblInfoKlant.Size = new Size(320, 30);
            lblInfoKlant.ForeColor = Color.White;
            lblInfoKlant.Font = new Font(lblInfoKlant.Font.FontFamily, 12, FontStyle.Regular);
            lblInfoKlant.TextAlign = ContentAlignment.MiddleLeft;
            PanelDetails.Controls.Add(lblInfoKlant);

            lblInfoAannemer.Text = "Aannemer: ";
            lblInfoAannemer.Location = new Point(10, 130);
            lblInfoAannemer.Size = new Size(320, 30);
            lblInfoAannemer.ForeColor = Color.White;
            lblInfoAannemer.Font = new Font(lblInfoAannemer.Font.FontFamily, 12, FontStyle.Regular);
            lblInfoAannemer.TextAlign = ContentAlignment.MiddleLeft;
            PanelDetails.Controls.Add(lblInfoAannemer);

            lblInfoExtraInfo.Text = "Extra Info: ";
            lblInfoExtraInfo.Location = new Point(10, 170);
            lblInfoExtraInfo.Size = new Size(320, 30);
            lblInfoExtraInfo.ForeColor = Color.White;
            lblInfoExtraInfo.Font = new Font(lblInfoExtraInfo.Font.FontFamily, 12, FontStyle.Regular);
            lblInfoExtraInfo.TextAlign = ContentAlignment.MiddleLeft;
            PanelDetails.Controls.Add(lblInfoExtraInfo);

            Button btnEdit = new Button();
            btnEdit.Text = "Edit";
            btnEdit.Location = new Point(10, 210);
            btnEdit.Size = new Size(80, 30);
            btnEdit.BackColor = Color.FromArgb(0, 122, 204);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.ForeColor = Color.White;
            PanelDetails.Controls.Add(btnEdit);

            btnEdit.Click += (sender, e) =>
            {
                if (lblInfoNaam.Text != "Naam: ")
                {

                    Project editProject = new Project();

                    foreach (Project project in alleProjecten)
                    {
                        if (project.Naam == lblInfoNaam.Text)
                        {
                            editProject = project;
                            editTextBoxAanpassen(editProject);
                        }
                    }

                }
            };

            Button btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.Location = new Point(100, 210);
            btnDelete.Size = new Size(80, 30);
            btnDelete.BackColor = Color.FromArgb(200, 0, 0);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.ForeColor = Color.White;
            PanelDetails.Controls.Add(btnDelete);

            btnDelete.Click += (sender, e) =>
            {
                if (lblInfoNaam.Text != "Naam: ")
                {
                    DialogResult result = MessageBox.Show(
                        "Weet je zeker dat je \"" + lblInfoNaam.Text + "\" wil verwijderen?",
                        "Project verwijderen",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (result == DialogResult.Yes)
                    {
                        string werf = lblInfoNaam.Text;
                        db.DeleteProject(werf);
                        LogToevoegen("Project verwijderd: " + werf);
                        loadProjecten();
                        lblInfoNaam.Text = "Naam: ";
                        lblInfoKlant.Text = "Klant: ";
                        lblInfoAannemer.Text = "Aannemer: ";
                        lblInfoExtraInfo.Text = "Extra Info: ";
                    }
                }
            };

        }
        void loadProjecten()
        {
            alleProjecten = db.GetProjecten();

            ToonProjecten(alleProjecten);
        }
        void ToonProjecten(List<Project> projecten)
        {
            PanelRijen.Controls.Clear();

            for (int i = 0; i < projecten.Count; i++)
            {
                Project project = projecten[i];
                Panel panelRij = new Panel();
                panelRij.Location = new Point(10, 10 + (i * 40) + (i * 5));
                panelRij.Size = new Size(600, 40);
                panelRij.BackColor = Color.FromArgb(40, 40, 50);
                panelRij.BorderStyle = BorderStyle.FixedSingle;
                PanelRijen.Controls.Add(panelRij);

                panelRij.Click += (sender, e) => loadDetails(project);

                Label rijNaam = new Label();
                rijNaam.Text = project.Naam;
                rijNaam.Location = new Point(0, 5);
                rijNaam.Size = new Size(200, 30);
                rijNaam.TextAlign = ContentAlignment.MiddleCenter;
                rijNaam.Font = new Font(rijNaam.Font.FontFamily, 12, FontStyle.Regular);
                panelRij.Controls.Add(rijNaam);
                rijNaam.Click += (sender, e) => loadDetails(project);

                Label rijKlant = new Label();
                rijKlant.Text = project.Klant;
                rijKlant.Location = new Point(200, 5);
                rijKlant.Size = new Size(200, 30);
                rijKlant.TextAlign = ContentAlignment.MiddleCenter;
                rijKlant.Font = new Font(rijKlant.Font.FontFamily, 12, FontStyle.Regular);
                panelRij.Controls.Add(rijKlant);
                rijKlant.Click += (sender, e) => loadDetails(project);

                Label rijAannemer = new Label();
                rijAannemer.Text = project.Aannemer;
                rijAannemer.Location = new Point(400, 5);
                rijAannemer.Size = new Size(200, 30);
                rijAannemer.TextAlign = ContentAlignment.MiddleCenter;
                rijAannemer.Font = new Font(rijAannemer.Font.FontFamily, 12, FontStyle.Regular);
                panelRij.Controls.Add(rijAannemer);
                rijAannemer.Click += (sender, e) => loadDetails(project);
            }
        }
        void loadPanelAddRow()
        {
            panelAddRow.Location = panelHome.Location;
            panelAddRow.Size = panelHome.Size;
            panelAddRow.BackColor = Color.FromArgb(18, 23, 29);
            panelAddRow.BorderStyle = BorderStyle.FixedSingle;
            panelAddRow.ForeColor = Color.White;
            Controls.Add(panelAddRow);

            Label lblWerf = new Label();
            lblWerf.Text = "Werf:";
            lblWerf.Size = new Size(80, 30);
            lblWerf.Location = new Point(10, 10);
            panelAddRow.Controls.Add(lblWerf);

            TextBox textBoxWerf = new TextBox();
            textBoxWerf.Location = new Point(100, 10);
            panelAddRow.Controls.Add(textBoxWerf);

            Label lblKlant = new Label();
            lblKlant.Text = "Klant:";
            lblKlant.Size = new Size(80, 30);
            lblKlant.Location = new Point(10, 60);
            panelAddRow.Controls.Add(lblKlant);

            TextBox textBoxKlant = new TextBox();
            textBoxKlant.Location = new Point(100, 60);
            panelAddRow.Controls.Add(textBoxKlant);

            Label lblAannemer = new Label();
            lblAannemer.Text = "Aannemer:";
            lblAannemer.Size = new Size(80, 30);
            lblAannemer.Location = new Point(10, 110);
            panelAddRow.Controls.Add(lblAannemer);

            TextBox textBoxAannemer = new TextBox();
            textBoxAannemer.Location = new Point(100, 110);
            panelAddRow.Controls.Add(textBoxAannemer);

            Label lblExtraInfo = new Label();
            lblExtraInfo.Text = "Extra info:";
            lblExtraInfo.Size = new Size(80, 30);
            lblExtraInfo.Location = new Point(10, 160);
            panelAddRow.Controls.Add(lblExtraInfo);

            TextBox textBoxExtraInfo = new TextBox();
            textBoxExtraInfo.Location = new Point(100, 160);
            panelAddRow.Controls.Add(textBoxExtraInfo);

            Button btnAdd = new Button();
            btnAdd.Text = "Add";
            btnAdd.Location = new Point(100, 210);
            btnAdd.BackColor = Color.FromArgb(0, 122, 204);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            panelAddRow.Controls.Add(btnAdd);

            btnAdd.Click += (sender, e) =>
            {

                Project nieuwProject = new Project();
                nieuwProject.Naam = textBoxWerf.Text;
                nieuwProject.Klant = textBoxKlant.Text;
                nieuwProject.Aannemer = textBoxAannemer.Text;
                nieuwProject.ExtraInfo = textBoxExtraInfo.Text;

                bool bestaatAl = false;

                foreach (Project project in alleProjecten)
                {
                    if (nieuwProject.Naam == project.Naam)
                    {
                        bestaatAl = true;
                    }
                }

                if (!bestaatAl)
                {
                    db.AddProject(nieuwProject);
                    LogToevoegen("Project toegevoegd: " + nieuwProject.Naam);
                }
                else
                {
                    MessageBox.Show("Project bestaat al.");
                }

                textBoxAannemer.Text = "";
                textBoxExtraInfo.Text = "";
                textBoxKlant.Text = "";
                textBoxWerf.Text = "";

                panelAddRow.Visible = false;
                panelHome.Visible = true;

                loadProjecten();
            };

            Button btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(200, 210);
            btnCancel.BackColor = Color.FromArgb(200, 0, 0);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            panelAddRow.Controls.Add(btnCancel);

            btnCancel.Click += (sender, e) =>
            {
                textBoxAannemer.Text = "";
                textBoxExtraInfo.Text = "";
                textBoxKlant.Text = "";
                textBoxWerf.Text = "";

                panelAddRow.Visible = false;
                panelHome.Visible = true;
            };
        }
        void loadPanelEditRow()
        {
            panelEditRow.Location = panelHome.Location;
            panelEditRow.Size = panelHome.Size;
            panelEditRow.BackColor = Color.FromArgb(18, 23, 29);
            panelEditRow.BorderStyle = BorderStyle.FixedSingle;
            panelEditRow.ForeColor = Color.White;
            Controls.Add(panelEditRow);

            Label lblWerf = new Label();
            lblWerf.Text = "Werf:";
            lblWerf.Size = new Size(80, 30);
            lblWerf.Location = new Point(10, 10);
            panelEditRow.Controls.Add(lblWerf);

            textBoxWerfEdit.Location = new Point(100, 10);
            panelEditRow.Controls.Add(textBoxWerfEdit);

            Label lblKlant = new Label();
            lblKlant.Text = "Klant:";
            lblKlant.Size = new Size(80, 30);
            lblKlant.Location = new Point(10, 60);
            panelEditRow.Controls.Add(lblKlant);

            textBoxKlantEdit.Location = new Point(100, 60);
            panelEditRow.Controls.Add(textBoxKlantEdit);

            Label lblAannemer = new Label();
            lblAannemer.Text = "Aannemer:";
            lblAannemer.Size = new Size(80, 30);
            lblAannemer.Location = new Point(10, 110);
            panelEditRow.Controls.Add(lblAannemer);

            textBoxAannemerEdit.Location = new Point(100, 110);
            panelEditRow.Controls.Add(textBoxAannemerEdit);

            Label lblExtraInfo = new Label();
            lblExtraInfo.Text = "Extra info:";
            lblExtraInfo.Size = new Size(80, 30);
            lblExtraInfo.Location = new Point(10, 160);
            panelEditRow.Controls.Add(lblExtraInfo);

            textBoxExtraInfoEdit.Location = new Point(100, 160);
            panelEditRow.Controls.Add(textBoxExtraInfoEdit);


            Button btnEdit = new Button();
            btnEdit.Text = "Edit";
            btnEdit.Location = new Point(100, 210);
            btnEdit.BackColor = Color.FromArgb(0, 122, 204);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.FlatAppearance.BorderSize = 0;
            panelEditRow.Controls.Add(btnEdit);

            btnEdit.Click += (sender, e) =>
            {
                Project editProject = new Project();

                editProject.Naam = textBoxWerfEdit.Text;
                editProject.Klant = textBoxKlantEdit.Text;
                editProject.Aannemer = textBoxAannemerEdit.Text;
                editProject.ExtraInfo = textBoxExtraInfoEdit.Text;

                string oudeWerf = lblInfoNaam.Text;

                db.EditProject(editProject, oudeWerf);

                LogToevoegen("Project bewerkt: " + editProject.Naam);

                textBoxAannemerEdit.Text = "";
                textBoxExtraInfoEdit.Text = "";
                textBoxKlantEdit.Text = "";
                textBoxWerfEdit.Text = "";

                panelEditRow.Visible = false;
                panelHome.Visible = true;

                loadProjecten();

                lblInfoNaam.Text = editProject.Naam;
                lblInfoAannemer.Text = "Aannemer:   " + editProject.Aannemer;
                lblInfoKlant.Text = "Klant:   " + editProject.Klant;
                lblInfoExtraInfo.Text = "Extra Info:   " + editProject.ExtraInfo;
            };

            Button btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(200, 210);
            btnCancel.BackColor = Color.FromArgb(200, 0, 0);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            panelEditRow.Controls.Add(btnCancel);

            btnCancel.Click += (sender, e) =>
            {
                textBoxAannemerEdit.Text = "";
                textBoxExtraInfoEdit.Text = "";
                textBoxKlantEdit.Text = "";
                textBoxWerfEdit.Text = "";

                panelEditRow.Visible = false;
                panelHome.Visible = true;
             
            };
        }
       
        void loadDetails(Project project)
        {
            lblInfoNaam.Text = project.Naam;
            lblInfoKlant.Text = "Klant:   " + project.Klant;
            lblInfoAannemer.Text = "Aannemer:   " + project.Aannemer;
            lblInfoExtraInfo.Text = "Extra Info:   " + project.ExtraInfo;
        }
        void editTextBoxAanpassen(Project editProject)
        {
            panelEditRow.Visible = true;
            panelHome.Visible = false;

            textBoxAannemerEdit.Text = editProject.Aannemer;
            textBoxExtraInfoEdit.Text = editProject.ExtraInfo;
            textBoxKlantEdit.Text = editProject.Klant;
            textBoxWerfEdit.Text = editProject.Naam;
        }
        void ZoekProjecten(string zoekTekst)
        {
            List<Project> gefilterdeProjecten = new List<Project>();

            foreach (Project project in alleProjecten)
            {
                if (project.Naam.ToLower().Contains(zoekTekst.ToLower()) ||
                    project.Klant.ToLower().Contains(zoekTekst.ToLower()) ||
                    project.Aannemer.ToLower().Contains(zoekTekst.ToLower()))
                {
                    gefilterdeProjecten.Add(project);
                }
            }

            ToonProjecten(gefilterdeProjecten);
        }
        void LogToevoegen(string actie)
        {
            string tijdstip = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            logLijst.Add(tijdstip + " — " + actie);
        }
    }
    // meer indelingen
    // sorteer knop maken
    // datum toevoegen
}