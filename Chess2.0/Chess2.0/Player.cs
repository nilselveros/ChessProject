﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess2._0
{
    class Player
    {
        private string colour;

        public Player(String colour)
        {
            this.colour = colour;
        }

        public String getColour
        {
            get
            {
                return colour;
            }
        }
    }
}
