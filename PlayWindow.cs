using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Threading;

namespace Worms
{
    public partial class PlayWindow : Form
    {
        private int step = 0;
        public List<Player> players;
        public Bitmap prefImage;
        public const int g = 10;
        public int wind = 0;
        public const int playerHeight = 30;
        public const int playerWidth = 21;
        bool flag = false;
        private static Random rnd = new Random();
        public GroundArray Surface;
        private int[,] _field;
        public int[,] Field { get { return _field; } }
        public PlayWindow()
        {
            InitializeComponent();
            players = new List<Player>();
            _field = new int[this.Height, this.Width];
            prefImage = new Bitmap(BackgroundImage);            
            Surface = new GroundArray(this);
            players.Add(new Player(new Point(rnd.Next(5, Field.GetLength(1) - playerWidth - 5), 0), this));
            //players.Add(new Player(new Point(rnd.Next(5, Field.GetLength(1) - playerWidth - 5), 0), this));
            Time.Start();
        }

        private void PlayWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            switch (e.KeyChar)
            {
                case char k when k == 'a' || k == 'ф' || k == 'A' || k == 'Ф':
                    Control.MovePlayer(0, players[step%2]);
                    break;
                case char k when k == 'd' || k == 'в' || k == 'D' || k == 'В':
                    Control.MovePlayer(1, players[step % 2]);
                    break;
                case char k when k == 'F' || k == 'f' || k == 'а' || k == 'А':
                        Control.PlayerFrontJump(players[step%2]);
                    break;
                case char k when k == 'R' || k == 'r' || k == 'к'|| k == 'К':
                    Control.PlayerBackJump(players[step % 2]);
                    break;
            }
        }



        private void PlayWindow_Paint(object sender, PaintEventArgs e)
        {
            Surface.Show();
        }


        private void Time_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].PlayerStand())
                    players[i].Fly();
                else if (!players[i].IsMoving)
                {
                    if (step % 2 == i % 2)
                        players[i].state = 0;
                    else
                        players[i].state = 4;
                    players[i].Show(players[i].LeftUpper);
                }
                players[i].IsMoving = false;
            }
        }

        private void ShowStructure_Click(object sender, EventArgs e)
        {
            Graphics g = CreateGraphics();
            for (int i = 0; i < Field.GetLength(0); i++)
                for(int j = 0; j < Field.GetLength(1); j++)
                {
                    switch(Field[i,j])
                    {
                        case 0:
                            g.FillRectangle(new Pen(Color.Blue).Brush, new Rectangle(new Point(j, i), new Size(1, 1)));
                            break;
                        case 1:
                            g.FillRectangle(new Pen(Color.Salmon).Brush, new Rectangle(new Point(j, i), new Size(1, 1)));
                            break;
                        case 2:
                            g.FillRectangle(new Pen(Color.Green).Brush, new Rectangle(new Point(j, i), new Size(1, 1)));
                            break;
                    }
                }
            g.Dispose();
        }
    }
}
