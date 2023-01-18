using Microsoft.VisualBasic.Devices;
using System.Runtime.InteropServices;

namespace ParseMusicScoreY
{
    public partial class Form1 : Form
    {
        private SynchronizationContext? synchronizationContext;
        public Form1()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void SetTips(object label)
        {
            label1.Text = label.ToString();
        }

        private void StartPaly()
        {
            Thread.Sleep(2000);
            int rhythmTime = int.Parse(textBox2.Text);
            var fs = File.OpenText(textBox1.Text);
            string? line;
            while ((line = fs.ReadLine()) != null)
            {
                synchronizationContext!.Post(SetTips, line);
                if (line.IndexOf('/') == -1)
                {
                    continue;
                }
                List<string> rhythms = new List<string>(line.Split('/'));
                rhythms.RemoveAt(rhythms.Count - 1);
                foreach (string rhythm in rhythms)
                {
                    List<string> ls = new List<string>();
                    int s = -1;
                    for (int i = 0; i < rhythm.Length; i++)
                    {
                        if (rhythm[i] == '(')
                        {
                            s = i;
                        }
                        else if (rhythm[i] == ')')
                        {
                            ls.Add(rhythm.Substring(s + 1, i - s - 1));
                            s = -1;
                        }
                        else if (s == -1 && rhythm[i] >= 'A' && rhythm[i] <= 'Z')
                        {
                            ls.Add(rhythm[i].ToString());
                        }
                    }
                    if (ls.Count > 0)
                    {
                        int sleepTime = rhythmTime / ls.Count;
                        foreach (string l in ls)
                        {
                            for(int i = 0; i < l.Length; i++)
                            {
                                PressKey((Keys)l[i], false);
                            }
                            for (int i = 0; i < l.Length; i++)
                            {
                                PressKey((Keys)l[i], true);
                            }
                            Thread.Sleep(sleepTime);
                        }
                    }
                    else
                    {
                        Thread.Sleep(rhythmTime);
                    }
                }
            }
            fs.Close();
            synchronizationContext.Post(SetEnable, null);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        public static void PressKey(Keys key, bool up)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;
            if (up)
            {

                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);

            }
            else
            {

                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);

            }
        }

        private void MySendKey(object key)
        {
            SendKeys.Send(key.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            var ts = new Thread(new ThreadStart(StartPaly));
            ts.Start();
        }

        private void SetEnable(object _o)
        {
            button2.Enabled = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}