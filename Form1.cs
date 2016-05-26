using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTimer
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private static System.Timers.Timer timer = new System.Timers.Timer();
        private static int timeSeconds, setTime;
        private static System.Media.SoundPlayer player = new System.Media.SoundPlayer();

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        public Form1()
        {
            InitializeComponent();
            setTime = 7;
            timer.Elapsed += subtract;

            textBox1.KeyPress += new KeyPressEventHandler(textBox1_KeyPress);

            int id = 0;     // The id of the hotkey. 
            RegisterHotKey(this.Handle, id, (int)KeyModifier.Control, Keys.NumPad0.GetHashCode());
        }


        private void button1_Click(object sender, EventArgs e)
        {
            timeSeconds = setTime;
            timer.Interval = 1000;
            timer.Start();
        }

        public void subtract(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (timeSeconds == 5)
            {
                Console.WriteLine("5");
                player.Stream = Properties.Resources._5;
                player.Play();
            }
            else if (timeSeconds == 4)
            {
                Console.WriteLine("4");
                player.Stream = Properties.Resources._4;
                player.Play();
            }
            else if (timeSeconds == 3)
            {
                Console.WriteLine("3");
                player.Stream = Properties.Resources._3;
                player.Play();
            }
            else if (timeSeconds == 2)
            {
                Console.WriteLine("2");
                player.Stream = Properties.Resources._2;
                player.Play();
            }
            else if (timeSeconds == 1)
            {
                Console.WriteLine("1");
                player.Stream = Properties.Resources._1;
                player.Play();

            }
            else if (timeSeconds <= 0)
            {

                timer.Stop();
            }
            setText(timeSeconds.ToString());
            timeSeconds -= 1;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                timeSeconds = setTime;
                timer.Interval = 1000;
                timer.Start();

                //Console.WriteLine("Num 0 pressed!");
                // do something
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsDigit(e.KeyChar)) && !(e.KeyChar == (char)Keys.Back))
                e.Handled = true;
        }

        private void ExampleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);       // Unregister hotkey with id 0 before closing the form. You might want to call this more than once with different id values if you are planning to register more than one hotkey.
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                setTime = 0;
            }
            else
            {
                int j;
                if (Int32.TryParse(textBox1.Text, out j))
                    setTime = j;
                else
                    Console.WriteLine("String could not be parsed.");
            }
        }

        delegate void SetTextCallback(string text);

        private void setText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(setText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label1.Text = text;
            }
        }

    }
}
