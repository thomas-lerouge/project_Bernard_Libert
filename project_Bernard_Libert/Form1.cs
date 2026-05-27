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
using System.IO;

namespace project_Bernard_Libert
{
    public partial class Form1 : Form
    {
        DatabaseManager db = new DatabaseManager();

        List<string> logLijst = new List<string>();
        string logBestand = "log.txt";

        Panel panelHome = new Panel();
        Panel PanelRijen = new Panel();
        Panel PanelDetails = new Panel();
        Panel panelAddRow = new Panel();
        Panel panelEditRow = new Panel();
        Panel panelLog = new Panel();

        bool oplopend = true;

        List<Project> alleProjecten = new List<Project>();

        Label lblInfoNaam = new Label();
        Label lblInfoKlant = new Label();
        Label lblInfoAannemer = new Label();
        Label lblInfoAdres = new Label();
        Label lblInfoType = new Label();
        Label lblInfoStart = new Label();
        Label lblInfoVerwacht = new Label();
        Label lblInfoWerkelijk = new Label();
        Label lblInfoExtraInfo = new Label();

        TextBox textBoxWerfEdit = new TextBox();
        TextBox textBoxKlantEdit = new TextBox();
        TextBox textBoxAannemerEdit = new TextBox();
        TextBox textBoxExtraInfoEdit = new TextBox();
        TextBox textBoxAdresEdit = new TextBox();
        ComboBox comboTypeEdit = new ComboBox();
        DateTimePicker dtpStartEdit = new DateTimePicker();
        DateTimePicker dtpVerwachtEdit = new DateTimePicker();
        DateTimePicker dtpWerkelijkEdit = new DateTimePicker();


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

            loadLogPanel();
            panelLog.Visible = false;

            LaadLogBestand();

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
            buttonAddProject.Text = "✚ Nieuw Project";
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

            Button btnSorteer = new Button();
            btnSorteer.Text = "⇅";
            btnSorteer.Location = new Point(740, 22);
            btnSorteer.Size = new Size(40, 35);
            btnSorteer.BackColor = Color.FromArgb(40, 44, 58);
            btnSorteer.ForeColor = Color.White;
            btnSorteer.FlatStyle = FlatStyle.Flat;
            btnSorteer.FlatAppearance.BorderSize = 0;
            btnSorteer.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            headerPanel.Controls.Add(btnSorteer);

            btnSorteer.Click += (sender, e) =>
            {
                SorteerProjecten();
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

            lblAllProjects.Click += (sender, e) =>
            {
                panelHome.Visible = true;
                panelLog.Visible = false;
                panelEditRow.Visible = false;
                panelAddRow.Visible = false;
                loadProjecten();
            };

            Label lblLog = new Label();
            lblLog.BackColor = Color.FromArgb(21, 25, 50);
            lblLog.Location = new Point(0, 50);
            lblLog.Size = new Size(menuPanel.Width, 50);
            lblLog.ForeColor = Color.White;
            lblLog.Text = "Log";
            lblLog.BorderStyle = BorderStyle.FixedSingle;
            lblLog.TextAlign = ContentAlignment.MiddleCenter;
            lblLog.Font = new Font(lblLog.Font.FontFamily, 15, FontStyle.Regular);
            menuPanel.Controls.Add(lblLog);

            lblLog.Click += (sender, e) =>
            {
                panelHome.Visible = false;
                panelLog.Visible = true;
                loadLog();
            };
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
            PanelDetails.AutoScroll = true;
            Controls.Add(PanelDetails);

            PanelDetails.AutoScroll = true;

            int xTitel = 10;
            int xWaarde = 140;

            Font titelFont = new Font("Segoe UI", 11, FontStyle.Bold);
            Font waardeFont = new Font("Segoe UI", 11, FontStyle.Regular);

            // Naam

            lblInfoNaam.Location = new Point(0, 20);
            lblInfoNaam.Size = new Size(360, 50);
            lblInfoNaam.ForeColor = Color.White;
            lblInfoNaam.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblInfoNaam.ForeColor = Color.LightBlue;
            lblInfoNaam.TextAlign = ContentAlignment.MiddleCenter;
            PanelDetails.Controls.Add(lblInfoNaam);


            // Klant
            Label lblTitelKlant = new Label();
            lblTitelKlant.Text = "Klant:";
            lblTitelKlant.Location = new Point(xTitel, 100);
            lblTitelKlant.Size = new Size(120, 30);
            lblTitelKlant.ForeColor = Color.LightBlue;
            lblTitelKlant.Font = titelFont;
            PanelDetails.Controls.Add(lblTitelKlant);

            lblInfoKlant.Location = new Point(xWaarde, 100);
            lblInfoKlant.Size = new Size(200, 30);
            lblInfoKlant.ForeColor = Color.White;
            lblInfoKlant.Font = waardeFont;
            PanelDetails.Controls.Add(lblInfoKlant);


            // Aannemer
            Label lblTitelAannemer = new Label();
            lblTitelAannemer.Text = "Aannemer:";
            lblTitelAannemer.Location = new Point(xTitel, 140);
            lblTitelAannemer.Size = new Size(120, 30);
            lblTitelAannemer.ForeColor = Color.LightBlue;
            lblTitelAannemer.Font = titelFont;
            PanelDetails.Controls.Add(lblTitelAannemer);

            lblInfoAannemer.Location = new Point(xWaarde, 140);
            lblInfoAannemer.Size = new Size(200, 30);
            lblInfoAannemer.ForeColor = Color.White;
            lblInfoAannemer.Font = waardeFont;
            PanelDetails.Controls.Add(lblInfoAannemer);


            // Adres
            Label lblTitelAdres = new Label();
            lblTitelAdres.Text = "Adres:";
            lblTitelAdres.Location = new Point(xTitel, 180);
            lblTitelAdres.Size = new Size(120, 30);
            lblTitelAdres.ForeColor = Color.LightBlue;
            lblTitelAdres.Font = titelFont;
            PanelDetails.Controls.Add(lblTitelAdres);

            lblInfoAdres.Location = new Point(xWaarde, 180);
            lblInfoAdres.Size = new Size(200, 60);
            lblInfoAdres.ForeColor = Color.White;
            lblInfoAdres.Font = waardeFont;
            lblInfoAdres.AutoSize = false;
            PanelDetails.Controls.Add(lblInfoAdres);


            // Type
            Label lblTitelType = new Label();
            lblTitelType.Text = "Type:";
            lblTitelType.Location = new Point(xTitel, 250);
            lblTitelType.Size = new Size(120, 30);
            lblTitelType.ForeColor = Color.LightBlue;
            lblTitelType.Font = titelFont;
            PanelDetails.Controls.Add(lblTitelType);

            lblInfoType.Location = new Point(xWaarde, 250);
            lblInfoType.Size = new Size(200, 30);
            lblInfoType.ForeColor = Color.White;
            lblInfoType.Font = waardeFont;
            PanelDetails.Controls.Add(lblInfoType);


            // Start
            Label lblTitelStart = new Label();
            lblTitelStart.Text = "Start:";
            lblTitelStart.Location = new Point(xTitel, 290);
            lblTitelStart.Size = new Size(120, 30);
            lblTitelStart.ForeColor = Color.LightBlue;
            lblTitelStart.Font = titelFont;
            PanelDetails.Controls.Add(lblTitelStart);

            lblInfoStart.Location = new Point(xWaarde, 290);
            lblInfoStart.Size = new Size(200, 30);
            lblInfoStart.ForeColor = Color.White;
            lblInfoStart.Font = waardeFont;
            PanelDetails.Controls.Add(lblInfoStart);


            // Verwachte eind
            Label lblTitelVerwacht = new Label();
            lblTitelVerwacht.Text = "Verwacht:";
            lblTitelVerwacht.Location = new Point(xTitel, 330);
            lblTitelVerwacht.Size = new Size(120, 30);
            lblTitelVerwacht.ForeColor = Color.LightBlue;
            lblTitelVerwacht.Font = titelFont;
            PanelDetails.Controls.Add(lblTitelVerwacht);

            lblInfoVerwacht.Location = new Point(xWaarde, 330);
            lblInfoVerwacht.Size = new Size(200, 30);
            lblInfoVerwacht.ForeColor = Color.White;
            lblInfoVerwacht.Font = waardeFont;
            PanelDetails.Controls.Add(lblInfoVerwacht);


            // Werkelijke eind
            Label lblTitelWerkelijk = new Label();
            lblTitelWerkelijk.Text = "Werkelijk:";
            lblTitelWerkelijk.Location = new Point(xTitel, 370);
            lblTitelWerkelijk.Size = new Size(120, 30);
            lblTitelWerkelijk.ForeColor = Color.LightBlue;
            lblTitelWerkelijk.Font = titelFont;
            PanelDetails.Controls.Add(lblTitelWerkelijk);

            lblInfoWerkelijk.Location = new Point(xWaarde, 370);
            lblInfoWerkelijk.Size = new Size(200, 30);
            lblInfoWerkelijk.ForeColor = Color.White;
            lblInfoWerkelijk.Font = waardeFont;
            PanelDetails.Controls.Add(lblInfoWerkelijk);


            // Extra info
            Label lblTitelExtra = new Label();
            lblTitelExtra.Text = "Opmerking:";
            lblTitelExtra.Location = new Point(xTitel, 420);
            lblTitelExtra.Size = new Size(120, 30);
            lblTitelExtra.ForeColor = Color.LightBlue;
            lblTitelExtra.Font = titelFont;
            PanelDetails.Controls.Add(lblTitelExtra);

            lblInfoExtraInfo.Location = new Point(xWaarde, 420);
            lblInfoExtraInfo.Size = new Size(200, 100);
            lblInfoExtraInfo.ForeColor = Color.White;
            lblInfoExtraInfo.Font = waardeFont;
            PanelDetails.Controls.Add(lblInfoExtraInfo);

            Button btnEdit = new Button();
            btnEdit.Text = "🖉 Edit";
            btnEdit.Location = new Point(20, 550);
            btnEdit.Size = new Size(80, 30);
            btnEdit.BackColor = Color.FromArgb(0, 122, 204);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.ForeColor = Color.White;
            PanelDetails.Controls.Add(btnEdit);

            btnEdit.Click += (sender, e) =>
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
            };

            Button btnDelete = new Button();
            btnDelete.Text = "🗑️ Delete";
            btnDelete.Location = new Point(150, 550);
            btnDelete.Size = new Size(80, 30);
            btnDelete.BackColor = Color.FromArgb(200, 0, 0);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.ForeColor = Color.White;
            PanelDetails.Controls.Add(btnDelete);

            btnDelete.Click += (sender, e) =>
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
                        lblInfoNaam.Text = "";
                        lblInfoKlant.Text = "";
                        lblInfoAannemer.Text = "";
                        lblInfoExtraInfo.Text = "";
                        lblInfoAdres.Text = "";
                        lblInfoType.Text = "";
                        lblInfoStart.Text = "";
                        lblInfoVerwacht.Text = "";
                        lblInfoWerkelijk.Text = "";

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
            textBoxWerf.Size = new Size(250, 30);
            panelAddRow.Controls.Add(textBoxWerf);


            Label lblKlant = new Label();
            lblKlant.Text = "Klant:";
            lblKlant.Size = new Size(80, 30);
            lblKlant.Location = new Point(10, 60);
            panelAddRow.Controls.Add(lblKlant);

            TextBox textBoxKlant = new TextBox();
            textBoxKlant.Location = new Point(100, 60);
            textBoxKlant.Size = new Size(250, 30);
            panelAddRow.Controls.Add(textBoxKlant);


            Label lblAannemer = new Label();
            lblAannemer.Text = "Aannemer:";
            lblAannemer.Size = new Size(80, 30);
            lblAannemer.Location = new Point(10, 110);
            panelAddRow.Controls.Add(lblAannemer);

            TextBox textBoxAannemer = new TextBox();
            textBoxAannemer.Location = new Point(100, 110);
            textBoxAannemer.Size = new Size(250, 30);
            panelAddRow.Controls.Add(textBoxAannemer);


            Label lblAdres = new Label();
            lblAdres.Text = "Adres:";
            lblAdres.Size = new Size(80, 30);
            lblAdres.Location = new Point(10, 160);
            panelAddRow.Controls.Add(lblAdres);

            TextBox textBoxAdres = new TextBox();
            textBoxAdres.Location = new Point(100, 160);
            textBoxAdres.Size = new Size(250, 30);
            panelAddRow.Controls.Add(textBoxAdres);


            Label lblType = new Label();
            lblType.Text = "Type:";
            lblType.Size = new Size(80, 30);
            lblType.Location = new Point(10, 210);
            panelAddRow.Controls.Add(lblType);

            ComboBox comboType = new ComboBox();
            comboType.Location = new Point(100, 210);
            comboType.Size = new Size(250, 30);

            comboType.Items.Add("Nieuwbouw");
            comboType.Items.Add("Renovatie");
            comboType.Items.Add("Uitbreiding");
            comboType.Items.Add("Appartement");
            comboType.Items.Add("Kantoor");

            panelAddRow.Controls.Add(comboType);


            Label lblStart = new Label();
            lblStart.Text = "Start:";
            lblStart.Size = new Size(80, 30);
            lblStart.Location = new Point(10, 260);
            panelAddRow.Controls.Add(lblStart);

            DateTimePicker dtpStart = new DateTimePicker();
            dtpStart.Location = new Point(100, 260);
            dtpStart.Size = new Size(250, 30);
            dtpStart.Format = DateTimePickerFormat.Short;
            dtpStart.Value = DateTime.Today;
            panelAddRow.Controls.Add(dtpStart);


            Label lblVerwacht = new Label();
            lblVerwacht.Text = "Verwacht:";
            lblVerwacht.Size = new Size(80, 30);
            lblVerwacht.Location = new Point(10, 310);
            panelAddRow.Controls.Add(lblVerwacht);

            DateTimePicker dtpVerwacht = new DateTimePicker();
            dtpVerwacht.Location = new Point(100, 310);
            dtpVerwacht.Size = new Size(250, 30);
            dtpVerwacht.Format = DateTimePickerFormat.Short;
            panelAddRow.Controls.Add(dtpVerwacht);


            Label lblWerkelijk = new Label();
            lblWerkelijk.Text = "Werkelijk:";
            lblWerkelijk.Size = new Size(80, 30);
            lblWerkelijk.Location = new Point(10, 360);
            panelAddRow.Controls.Add(lblWerkelijk);

            DateTimePicker dtpWerkelijk = new DateTimePicker();
            dtpWerkelijk.Location = new Point(100, 360);
            dtpWerkelijk.Size = new Size(250, 30);
            dtpWerkelijk.Format = DateTimePickerFormat.Short;
            panelAddRow.Controls.Add(dtpWerkelijk);


            Label lblExtraInfo = new Label();
            lblExtraInfo.Text = "Extra info:";
            lblExtraInfo.Size = new Size(80, 30);
            lblExtraInfo.Location = new Point(10, 410);
            panelAddRow.Controls.Add(lblExtraInfo);

            TextBox textBoxExtraInfo = new TextBox();
            textBoxExtraInfo.Location = new Point(100, 410);
            textBoxExtraInfo.Size = new Size(250, 70);
            textBoxExtraInfo.Multiline = true;
            panelAddRow.Controls.Add(textBoxExtraInfo);


            Button btnAdd = new Button();
            btnAdd.Text = "Add";
            btnAdd.Location = new Point(100, 520);
            btnAdd.Size = new Size(100, 35);
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
                nieuwProject.Adres = textBoxAdres.Text;
                nieuwProject.TypeProject = comboType.Text;
                nieuwProject.StartDatum = dtpStart.Value;
                nieuwProject.VerwachteEindDatum = dtpVerwacht.Value;
                nieuwProject.WerkelijkeEindDatum = dtpWerkelijk.Value;
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
            btnCancel.Location = new Point(220, 520);
            btnCancel.Size = new Size(100, 35);
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
            textBoxWerfEdit.Size = new Size(250, 30);
            panelEditRow.Controls.Add(textBoxWerfEdit);


            Label lblKlant = new Label();
            lblKlant.Text = "Klant:";
            lblKlant.Size = new Size(80, 30);
            lblKlant.Location = new Point(10, 60);
            panelEditRow.Controls.Add(lblKlant);

            textBoxKlantEdit.Location = new Point(100, 60);
            textBoxKlantEdit.Size = new Size(250, 30);
            panelEditRow.Controls.Add(textBoxKlantEdit);


            Label lblAannemer = new Label();
            lblAannemer.Text = "Aannemer:";
            lblAannemer.Size = new Size(80, 30);
            lblAannemer.Location = new Point(10, 110);
            panelEditRow.Controls.Add(lblAannemer);

            textBoxAannemerEdit.Location = new Point(100, 110);
            textBoxAannemerEdit.Size = new Size(250, 30);
            panelEditRow.Controls.Add(textBoxAannemerEdit);


            Label lblAdres = new Label();
            lblAdres.Text = "Adres:";
            lblAdres.Size = new Size(80, 30);
            lblAdres.Location = new Point(10, 160);
            panelEditRow.Controls.Add(lblAdres);

            textBoxAdresEdit.Location = new Point(100, 160);
            textBoxAdresEdit.Size = new Size(250, 30);
            panelEditRow.Controls.Add(textBoxAdresEdit);


            Label lblType = new Label();
            lblType.Text = "Type:";
            lblType.Size = new Size(80, 30);
            lblType.Location = new Point(10, 210);
            panelEditRow.Controls.Add(lblType);

            comboTypeEdit.Location = new Point(100, 210);
            comboTypeEdit.Size = new Size(250, 30);

            comboTypeEdit.Items.Add("Nieuwbouw");
            comboTypeEdit.Items.Add("Renovatie");
            comboTypeEdit.Items.Add("Uitbreiding");
            comboTypeEdit.Items.Add("Appartement");
            comboTypeEdit.Items.Add("Kantoor");

            panelEditRow.Controls.Add(comboTypeEdit);


            Label lblStart = new Label();
            lblStart.Text = "Start:";
            lblStart.Size = new Size(80, 30);
            lblStart.Location = new Point(10, 260);
            panelEditRow.Controls.Add(lblStart);

            dtpStartEdit.Location = new Point(100, 260);
            dtpStartEdit.Size = new Size(250, 30);
            dtpStartEdit.Format = DateTimePickerFormat.Short;
            dtpStartEdit.Value = DateTime.Today;
            panelEditRow.Controls.Add(dtpStartEdit);


            Label lblVerwacht = new Label();
            lblVerwacht.Text = "Verwacht:";
            lblVerwacht.Size = new Size(80, 30);
            lblVerwacht.Location = new Point(10, 310);
            panelEditRow.Controls.Add(lblVerwacht);

            dtpVerwachtEdit.Location = new Point(100, 310);
            dtpVerwachtEdit.Size = new Size(250, 30);
            dtpVerwachtEdit.Format = DateTimePickerFormat.Short;
            panelEditRow.Controls.Add(dtpVerwachtEdit);


            Label lblWerkelijk = new Label();
            lblWerkelijk.Text = "Werkelijk:";
            lblWerkelijk.Size = new Size(80, 30);
            lblWerkelijk.Location = new Point(10, 360);
            panelEditRow.Controls.Add(lblWerkelijk);

            dtpWerkelijkEdit.Location = new Point(100, 360);
            dtpWerkelijkEdit.Size = new Size(250, 30);
            dtpWerkelijkEdit.Format = DateTimePickerFormat.Short;
            panelEditRow.Controls.Add(dtpWerkelijkEdit);


            Label lblExtraInfo = new Label();
            lblExtraInfo.Text = "Extra info:";
            lblExtraInfo.Size = new Size(80, 30);
            lblExtraInfo.Location = new Point(10, 410);
            panelEditRow.Controls.Add(lblExtraInfo);

            textBoxExtraInfoEdit.Location = new Point(100, 410);
            textBoxExtraInfoEdit.Size = new Size(250, 70);
            textBoxExtraInfoEdit.Multiline = true;
            panelEditRow.Controls.Add(textBoxExtraInfoEdit);


            Button btnEdit = new Button();
            btnEdit.Text = "Edit";
            btnEdit.Location = new Point(100, 520);
            btnEdit.Size = new Size(100, 35);
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
                editProject.Adres = textBoxAdresEdit.Text;
                editProject.TypeProject = comboTypeEdit.Text;
                editProject.StartDatum = dtpStartEdit.Value;
                editProject.VerwachteEindDatum = dtpVerwachtEdit.Value;
                editProject.WerkelijkeEindDatum = dtpWerkelijkEdit.Value;
                editProject.ExtraInfo = textBoxExtraInfoEdit.Text;

                string oudeWerf = lblInfoNaam.Text;

                db.EditProject(editProject, oudeWerf);

                LogToevoegen("Project bewerkt: " + editProject.Naam);

                textBoxAannemerEdit.Text = "";
                textBoxExtraInfoEdit.Text = "";
                textBoxKlantEdit.Text = "";
                textBoxWerfEdit.Text = "";
                textBoxAdresEdit.Text = "";

                panelEditRow.Visible = false;
                panelHome.Visible = true;

                loadProjecten();

                lblInfoNaam.Text = editProject.Naam;
                lblInfoKlant.Text = editProject.Klant;
                lblInfoAannemer.Text = editProject.Aannemer;
                lblInfoAdres.Text = editProject.Adres;
                lblInfoType.Text = editProject.TypeProject;
                lblInfoStart.Text = editProject.StartDatum.ToShortDateString();
                lblInfoVerwacht.Text = editProject.VerwachteEindDatum.ToShortDateString();
                lblInfoWerkelijk.Text = editProject.WerkelijkeEindDatum.ToShortDateString();
                lblInfoExtraInfo.Text = editProject.ExtraInfo;
            };


            Button btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(220, 520);
            btnCancel.Size = new Size(100, 35);
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
        void loadLogPanel()
        {
            panelLog.Location = panelHome.Location;
            panelLog.Size = panelHome.Size;
            panelLog.BackColor = Color.FromArgb(18, 23, 29);
            panelLog.BorderStyle = BorderStyle.FixedSingle;
            panelLog.AutoScroll = true;

            Controls.Add(panelLog);
        }

        void loadDetails(Project project)
        {
            lblInfoNaam.Text = project.Naam;
            lblInfoKlant.Text = project.Klant;
            lblInfoAannemer.Text = project.Aannemer;
            lblInfoAdres.Text = project.Adres;
            lblInfoType.Text = project.TypeProject;
            lblInfoStart.Text = project.StartDatum.ToShortDateString();
            lblInfoVerwacht.Text = project.VerwachteEindDatum.ToShortDateString();
            lblInfoWerkelijk.Text = project.WerkelijkeEindDatum.ToShortDateString();
            lblInfoExtraInfo.Text = project.ExtraInfo;
        }
        void editTextBoxAanpassen(Project editProject)
        {
            panelEditRow.Visible = true;
            panelHome.Visible = false;

            textBoxAannemerEdit.Text = editProject.Aannemer;
            textBoxExtraInfoEdit.Text = editProject.ExtraInfo;
            textBoxKlantEdit.Text = editProject.Klant;
            textBoxWerfEdit.Text = editProject.Naam;
            textBoxAdresEdit.Text = editProject.Adres;

            comboTypeEdit.Text = editProject.TypeProject;

            dtpStartEdit.Value = editProject.StartDatum;
            dtpVerwachtEdit.Value = editProject.VerwachteEindDatum;
            dtpWerkelijkEdit.Value = editProject.WerkelijkeEindDatum;
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
            string logAdd = (tijdstip + " — " + actie);
            logLijst.Add(logAdd);
            File.AppendAllText(logBestand, logAdd + Environment.NewLine);
        }
        void loadLog()
        {
            panelLog.Controls.Clear();

            Label titel = new Label();
            titel.Text = "Logboek";
            titel.Location = new Point(10, 10);
            titel.Size = new Size(600, 30);
            titel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            titel.ForeColor = Color.White;

            panelLog.Controls.Add(titel);

            for (int i = 0; i < logLijst.Count; i++)
            {
                Label lblLogItem = new Label();

                lblLogItem.Text = logLijst[i];
                lblLogItem.Location = new Point(10, 60 + i * 35);
                lblLogItem.Size = new Size(580, 30);

                lblLogItem.ForeColor = Color.White;

                panelLog.Controls.Add(lblLogItem);
            }
        }
        void LaadLogBestand()
        {
            if (File.Exists(logBestand))
            {
                logLijst = File.ReadAllLines(logBestand).ToList();
            }
        }
        void SorteerProjecten()
        {
            if (oplopend)
            {
                List<Project> gesorteerdeProjecten = alleProjecten.OrderBy(project => project.Naam).ToList();
                ToonProjecten(gesorteerdeProjecten);
            }
            else
            {
                List<Project> gesorteerdeProjecten = alleProjecten.OrderByDescending(project => project.Naam).ToList();
                ToonProjecten(gesorteerdeProjecten);
            }
            oplopend = !oplopend;
        }
    }
    // meer indelingen
    //  achternaam-voornaam-Eindproject (zip)
    //  * Project
    //  * DevBlog (link + pdf)
    //  * Feedback
    //  * Referentielijst (.pdf)
    //  * Presentatie (.pptx)
}