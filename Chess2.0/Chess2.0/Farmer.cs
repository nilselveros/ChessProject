﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess2._0
{
    class Farmer : ChessPiece
    {
        public Farmer(Player p, int posX, int posY) : base(p, posX, posY) { }

        public override Boolean isValidMove(Move move)
        {
            //Check if the move is valid for white pieces
            if (player is HumanPlayer)
            {
                if (move.gettoY() - move.getfromY() == 2 && move.gettoX() == move.getfromX() && move.getfromY() == 1)
                    
                if (move.gettoY() - move.getfromY() == 2 && move.gettoX() == move.getfromX() 
                    && move.getfromY() == 1)
                {
                    return true;
                }

                if (move.gettoY() - move.getfromY() == 1 && (move.gettoX() - move.getfromX() == -1 ||
                    (move.gettoX() - move.getfromX() == 1 && move.gettoY() - move.getfromY() == 1) ||
                    (move.gettoX() == move.getfromX() && move.gettoY() - move.getfromY() == 1)))
                {
                    return true;
                }
            }

            //Check if the move is valid for black pieces
            if (player is CPUPlayer)
            {
                if (move.gettoY() - move.getfromY() == -2 && move.gettoX() == move.getfromX() 
                    && move.getfromY() == 6)
                {
                    return true;
                }

                if ((move.gettoY() - move.getfromY() == -1 && move.gettoX() - move.getfromX() == 1) ||
                    (move.gettoX() - move.getfromX() == -1 && (move.gettoY() - move.getfromY() == -1) ||
                    (move.gettoX() == move.getfromX() && move.gettoY() - move.getfromY() == -1)))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
