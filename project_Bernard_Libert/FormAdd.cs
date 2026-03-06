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
        public string addWerf;
        public string addKlant;
        public string addAannemer;
        public string addExtraInfo;
        public FormAdd()
        {
            this.Size = new Size(700, 500);

            Label lblWerf = new Label();
            lblWerf.Text = "Werf:";
            lblWerf.Location = new Point(10, 100);
            Controls.Add(lblWerf);

            TextBox textBoxWerf = new TextBox();
            textBoxWerf.Location = new Point(10, 120);
            Controls.Add(textBoxWerf);

            Label lblKlant = new Label();
            lblKlant.Text = "Klant:";
            lblKlant.Location = new Point(160, 100);
            Controls.Add(lblKlant);

            TextBox textBoxKlant = new TextBox();
            textBoxKlant.Location = new Point(160, 120);
            Controls.Add(textBoxKlant);

            Label lblAannemer = new Label();
            lblAannemer.Text = "Aannemer:";
            lblAannemer.Location = new Point(320, 100);
            Controls.Add(lblAannemer);

            TextBox textBoxAannemer = new TextBox();
            textBoxAannemer.Location = new Point(320, 120);
            Controls.Add(textBoxAannemer);

            Label lblExtraInfo = new Label();
            lblExtraInfo.Text = "Extra info:";
            lblExtraInfo.Location = new Point(470, 100);
            Controls.Add(lblExtraInfo);

            TextBox textBoxExtraInfo = new TextBox();
            textBoxExtraInfo.Location = new Point(470, 120);
            Controls.Add(textBoxExtraInfo);

            Button btnAdd = new Button();
            btnAdd.Text = "Add";
            btnAdd.Location = new Point(620, 120);
            Controls.Add(btnAdd);

            btnAdd.Click += (sender, e) =>
            {
                addKlant = textBoxKlant.Text;
                addAannemer = textBoxAannemer.Text;
                addWerf = textBoxWerf.Text;
                addExtraInfo = textBoxExtraInfo.Text;

                Ready?.Invoke(this, EventArgs.Empty);
            };
        }
    }
}
