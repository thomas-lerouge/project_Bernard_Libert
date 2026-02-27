using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_Bernard_Libert
{
    internal class FormAdd : Form
    {
        public event EventHandler Ready;
        public string addNaam;
        public string addLocatie;
        public string addExtraInfo;
        public FormAdd()
        {
            this.Size = new Size(700, 500);

            Label lblNaam = new Label();
            lblNaam.Text = "Naam:";
            lblNaam.Location = new Point(10, 100);
            Controls.Add(lblNaam);

            TextBox textBoxNaam = new TextBox();
            textBoxNaam.Location = new Point(10, 120);
            Controls.Add(textBoxNaam);

            Label lblLocatie = new Label();
            lblLocatie.Text = "Locatie:";
            lblLocatie.Location = new Point(160, 100);
            Controls.Add(lblLocatie);

            TextBox textBoxLocatie = new TextBox();
            textBoxLocatie.Location = new Point(160, 120);
            Controls.Add(textBoxLocatie);

            Label lblExtraInfo = new Label();
            lblExtraInfo.Text = "Extra info:";
            lblExtraInfo.Location = new Point(320, 100);
            Controls.Add(lblExtraInfo);

            TextBox textBoxExtraInfo = new TextBox();
            textBoxExtraInfo.Location = new Point(320, 120);
            Controls.Add(textBoxExtraInfo);

            Button btnAdd = new Button();
            btnAdd.Text = "Add";
            btnAdd.Location = new Point(470, 120);
            Controls.Add(btnAdd);

            btnAdd.Click += (sender, e) =>
            {
                addLocatie = textBoxLocatie.Text;
                addExtraInfo = textBoxExtraInfo.Text;
                addNaam = textBoxNaam.Text;

                Ready?.Invoke(this, EventArgs.Empty);
            };
        }
    }
}
