﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Tower : ChessPiece
    {
        public Tower(Player p, int posX, int posY) : base(p, posX, posY) {}
   
        public override Boolean isValidMove(Move move)
        {
            if(move.gettoY() == move.getfromY() || move.gettoX() == move.getfromX())
            {
                System.Console.WriteLine("tower draget är tillåtet");
                return true;
            }
            else
            {
                System.Console.WriteLine("tower draget är inte tillåtet");
                return false;
            }
        }
    }
}
