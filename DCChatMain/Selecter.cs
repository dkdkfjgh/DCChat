using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCChatMain
{

    public partial class Selecter : Form
    {


        public string URL = "http://gall.dcinside.com/board/lists/?id=";

        public Selecter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            URL += "baseball_new7";
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            URL += "football_new6";
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            URL += "leagueoflegends2";
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            URL += "ib_new";
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text.StartsWith("http://gall.dcinside.com"))
                {
                    URL = textBox1.Text;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("http://gall.dcinside.com 으로 시작해야함", "Error");
                }
            }
        }

        private void Selecter_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            URL += "game1_new";
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            URL = "http://gall.dcinside.com/mgallery/board/lists?id=aoegame";
            this.Close();
        }
    }


}
