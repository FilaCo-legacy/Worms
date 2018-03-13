using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Worms
{
    public class Control
    {
        public static void MovePlayer(int direction, Player curPlayer)
        {
            curPlayer.Move(direction);
        }
        public static void PlayerFrontJump(Player curPlayer)
        {
            curPlayer.FrontJump();
        }
        public static void PlayerBackJump(Player curPlayer)
        {
            curPlayer.BackJump();
        }
    }
}
