using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gierka__Catch_ECTS
{
    public partial class Form1 : Form
    {
        int halfIndex;
        public Form1()
        {
            InitializeComponent();
            trackBar1.Minimum = 1;
            trackBar1.Maximum = 9;
            halfIndex = (trackBar1.Maximum + trackBar1.Minimum) / 2;  //zeby ustawiac poczatkowa wartosc trackBar1 na srodku i do poprawnego dzialania poziomu trudnosci
            trackBar1.Value = halfIndex;                              //  
            nowy.Click += new System.EventHandler(this.myScore); //ta linia tutaj zeby punkty mogly sie resetowac, a nie dodawac w nieskonczonosc podczas ich zdobycia
            //timer1.Interval = 1500;
            //textBox3.Text = "1500"; // tego textBoxa nie bedzie, tylko do sprawdzania
        }
        Button nowy = new Button();
        Random rnd = new Random();
        Image CW10 = Image.FromFile("CW10.jpg");
        Image PP = Image.FromFile("pp.jpg");
        //Image B = Image.FromFile("B.png");
        Point loc = new Point();
        Size size = new Size(115, 90);
        int counter = 0; //calosc
        int score = 0; //zdobyte
        int t=1500; //wartosc medium

        private void button1_Click(object sender, EventArgs e)
        {
            // dodac timer do rozpoczecia gry
            // im wiecej punktow tym trudniej
            //counter = 0;
            //score = 0;
            nowy.Visible = true;

            textBox1.Visible = false;
            button1.Visible = false;
            label5.Visible = false;

            button2.Visible = true;
            textBox2.Visible = true;
            
            this.BackgroundImage = CW10;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            //nowy.BackgroundImage = B;
            //nowy.BackgroundImageLayout = ImageLayout.Stretch;
            nowy.Text = "ECTS";
            nowy.Font = new Font("Comic Sans MS", 24);
            nowy.BackColor = Color.PeachPuff;
 
            groupBox1.BackColor = Color.SandyBrown;
            label1.ForeColor = Color.Navy;
            label2.ForeColor = Color.Navy;
            label3.ForeColor = Color.Navy;
            label4.ForeColor = Color.Navy;

            nowy.Size = size;
            loc = nowy.Location;
            loc.X = rnd.Next(900);
            loc.Y = rnd.Next(300);
            nowy.Location = loc;
            Controls.Add(nowy);

            //timer1.Enabled = true;
            //timer1.Interval = 1; //zeby nie czekac
            //timer1.Enabled = false;
            timer1.Enabled = true;
            textBox2.Text = score.ToString() + "/" + counter.ToString() + " ECTS";
            //button1.Click += new System.EventHandler(this.timer1_Tick);
        }

        private void changePosition()
        {
            //timer1.Enabled = true;
            if (nowy.Visible == true)   //zeby nie tylko zmienialo pozycje bez delaya, ale zeby button znikal(z delayem) i pojawial sie
            {
                nowy.Visible = false;
                timer1.Enabled = false;
            }
            else
            {
                nowy.Visible = true;
                timer1.Enabled = false;
            }
            //textBox3.Text = Convert.ToString(timer1.Interval); // jaki interwał
            timer1.Enabled = true;
            //Point loc = nowy.Location;
            loc.X = rnd.Next(900);
            loc.Y = rnd.Next(300);
            nowy.Location = loc;
            counter++;

            if(nowy.Visible == true)
                textBox2.Text = score.ToString() + "/" + (counter--).ToString() + " ECTS"; //zeby nie naliczalo podwojnie countera jak bedzie znikac(zeby punkty sie zgadzaly)
            else
                textBox2.Text = score.ToString() + "/" + counter.ToString() + " ECTS";     //zwykly counter

            Controls.Add(nowy);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {            
            changePosition();

            timer1.Interval = t; //do utrzymania poprawnosci dzialania timera w dalszych krokach(po zmianie poziomu trudnosci)
            timer1.Interval += rnd.Next((int)(-timer1.Interval * 0.2), (int)(timer1.Interval * 0.2)); //mala losowosc momentu pojawiania sie buttona i jego znikania

            //textBox3.Text = Convert.ToString(timer1.Interval); do eksperymentow
        }

            private void myScore(object sender, EventArgs e)
        {
            //timer1.Enabled = false; //nie musi to byc
            //timer1.Enabled = true;  //

            changePosition();
            //counter++;  
            score++;

            textBox2.Text = score.ToString() + "/" + counter.ToString() + " ECTS"; 
            //Controls.Add(nowy);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.BackColor = Color.MidnightBlue;
            label1.ForeColor = Color.Red;
            label2.ForeColor = Color.Red;
            label3.ForeColor = Color.Red;
            label4.ForeColor = Color.Red;

            using (StreamWriter saveScore = new StreamWriter("Score.txt", true)) //jesli na nowym obiekcie wykonamy wszystkie akcje w klamrach, plik sie zamyka i zapisuje, wywalając go z pamieći RAM
            {
                String sDate = DateTime.Now.ToString();
                saveScore.WriteLine(textBox1.Text + "   <|>   " + textBox2.Text + "   <|>   " + sDate);
            }
            textBox1.Visible = true;
            button1.Visible = true;
            label5.Visible = true;

            button2.Visible = false;
            textBox2.Visible = false;

            timer1.Enabled = false; //zatrzymanie timera zeby button nie latał po skoczeniu gry

            nowy.Visible = false;
            this.BackgroundImage = PP;

            textBox1.Text = ""; //reset licznika punktacji i wymazanie textboxów
            score = 0;          //
            counter = 0;        //
            textBox2.Text = ""; //

            trackBar1.Value = (trackBar1.Maximum + trackBar1.Minimum) / 2; //4 linijki w STOP dla poprawnego dzialania poziomu trudnosci przy kolejnych grach
            halfIndex = (trackBar1.Maximum + trackBar1.Minimum) / 2;       //
            timer1.Interval = 1500;                                        //
            t = 1500;                                                      //
        }

        private void trackBar1_Scroll(object sender, EventArgs e) //wlasciwe działanie poziomu trudnosci
        {
            int ratioChanger = 250;

            if (trackBar1.Value < halfIndex)
            {
                t += (Math.Abs(halfIndex - trackBar1.Value)) * ratioChanger;
                halfIndex = trackBar1.Value;
                timer1.Interval = t;
            }
            else if (trackBar1.Value > halfIndex)
            {
                t -= (Math.Abs(halfIndex - trackBar1.Value)) * ratioChanger;
                halfIndex = trackBar1.Value;
                timer1.Interval = t; 
            }

            //textBox3.Text = Convert.ToString(timer1.Interval); //do eksperymentow
        }
    }
}
