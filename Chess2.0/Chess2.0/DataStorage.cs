﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Linq;
using System.IO;
using System.Runtime.InteropServices;

namespace Chess2._0
{
  public class DataStorage
    {
        FileSystemWatcher watcher;

        public DataStorage(FileSystemWatcher watch)
        {
            watcher = watch;
        }

        //Läser data från xml-fil, skapar en representation av schackbräädet och returnerar det.
        public ChessPiece[,] LoadData()
        {
            if (!IsFileLocked())
            {
   
            ChessPiece[,] board = new ChessPiece[8, 8];

                XDocument doc = XDocument.Load(@".\chessdata\chessdata.xml");

                var data = from item in doc.Descendants("ChessPiece")
                           select new
                           {
                               posx = item.Element("posX").Value,
                               posy = item.Element("posY").Value,
                               color = item.Element("color").Value,
                               type = item.Element("type").Value
                           };

                foreach (var c in data)
                {
                    switch (c.type)
                    {
                        case "Chess2._0.King":
                            ChessPiece king = new King(c.color, int.Parse(c.posx), int.Parse(c.posy));
                            board[int.Parse(c.posx), int.Parse(c.posy)] = king;
                            break;
                        case "Chess2._0.Queen":
                            ChessPiece queen = new Queen(c.color, int.Parse(c.posx), int.Parse(c.posy));
                            board[int.Parse(c.posx), int.Parse(c.posy)] = queen;
                            break;
                        case "Chess2._0.Runner":
                            ChessPiece runner = new Runner(c.color, int.Parse(c.posx), int.Parse(c.posy));
                            board[int.Parse(c.posx), int.Parse(c.posy)] = runner;
                            break;
                        case "Chess2._0.Horse":
                            ChessPiece horse = new Horse(c.color, int.Parse(c.posx), int.Parse(c.posy));
                            board[int.Parse(c.posx), int.Parse(c.posy)] = horse;
                            break;
                        case "Chess2._0.Tower":
                            ChessPiece tower = new Tower(c.color, int.Parse(c.posx), int.Parse(c.posy));
                            board[int.Parse(c.posx), int.Parse(c.posy)] = tower;
                            break;
                        case "Chess2._0.Farmer":
                            ChessPiece farmer = new Farmer(c.color, int.Parse(c.posx), int.Parse(c.posy));
                            board[int.Parse(c.posx), int.Parse(c.posy)] = farmer;
                            break;
                    }
                }

                return board;
            }
            else
            {
                throw new Exception();
            }            
        }
        

        //Skriver schackbrädet i sitt nuvarande tillstånd till en xml-fil
        public void SaveData(ChessPiece[,] board)
        {
            watcher.EnableRaisingEvents = false;

            if (!IsFileLocked())
            {
                ArrayList list = new ArrayList();
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    for (int y = 0; y < board.GetLength(1); y++)
                    {
                        if (board[x, y] != null)
                        {
                            list.Add(board[x, y]);
                        }
                    }
                }

                //gör om arraylist till array
                ChessPiece[] listan = list.ToArray(typeof(ChessPiece)) as ChessPiece[];

                XDocument xmlDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),

                new XComment("Nils och Fridens coola xml doc!"),

                new XElement("ChessPieces",
                from ChessPiece in listan

                select new XElement("ChessPiece", new XElement("posX", ChessPiece.posX),
                                                  new XElement("posY", ChessPiece.posY),
                                                  new XElement("color", ChessPiece.Color),
                                                  new XElement("type", ChessPiece.GetType())

            )));
                xmlDoc.Save(@".\chessdata\chessdata.xml");

                watcher.EnableRaisingEvents = true;
            }
            else
            {
                throw new Exception();
            }
        }

        //Kollar om filen är låst 
        public bool IsFileLocked()
        {
            bool isLocked = true;
            int i = 0;
            while (isLocked || i < 10)
            {
                try
                {
                    if (File.Exists(@".\chessdata\chessdata.xml"))
                    {
                        using (File.Open(@".\chessdata\chessdata.xml", FileMode.Open)) { }
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (IOException e)
                {
                    i++;
                    new System.Threading.ManualResetEvent(false).WaitOne(1000);
                }
            }
            return isLocked;
        }

        //Ta bort xml-filen
        public void removeFile()
        {
            File.Delete(@".\chessdata\chessdata.xml");
        }
    }
}
