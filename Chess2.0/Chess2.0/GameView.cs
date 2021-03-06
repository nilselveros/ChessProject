﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;

namespace Chess2._0
{
  public class GameView
    {
        MainWindow window;
        ChessBoard board;
        Player playerwhite;
        Player playerblack;
        RulesEngine rules;
        string _gamestatus;
        DataStorage ds;
        FileSystemWatcher watcher;

        public String gamestatus
        {
            get { return _gamestatus; }
            set
            {
                _gamestatus = value;
                if(_gamestatus == "black")
                {
                    playerblack.movePiece();
                }
                if(_gamestatus == "white")
                {
                    playerwhite.movePiece();
                }
            }
        }

        public GameView(MainWindow window)
        {
            setupWatcher();
            ds = new DataStorage(watcher);
            this.window = window;
        }

        //Initierar spelare utifrån vad som har valts i menyerna
        public void GameSetup(string gamemode, bool isNewGame, string color)
        {
       
            if (isNewGame)
            {
                ds.removeFile();
            }

            board = new ChessBoard(ds);
            List<Player> players = new List<Player>();

            if (gamemode == "singleplayer")
            {
                switch (color)
                {
                    case "white":
                        playerwhite = new HumanPlayer("white");
                        playerblack = new CPUPlayer("black");
                        playerblack.setupAI(board, this);
                        players.Add(playerwhite);
                        break;

                    case "black":
                        playerwhite = new CPUPlayer("white");
                        playerblack = new HumanPlayer("black");
                        playerwhite.setupAI(board, this);
                        players.Add(playerblack);
                        break;
                }             
            }
            else if(gamemode == "multiplayer")
            {
                playerwhite = new HumanPlayer("white");
                playerblack = new HumanPlayer("black");
                players.Add(playerwhite);
                players.Add(playerblack);
            }
 
            rules = new RulesEngine(board);
            window.setBoard(board.get());
            window.updateTable();
            window.setPlayers(players);
            playerwhite.isPlayersTurn = true;
            playerblack.isPlayersTurn = false;
            gamestatus = "white";
           
        }

        //Lyssnar efter gjorda drag i ui
        public void onMoveCompleted (int[] newMove, Player currentPlayer)
        {
            if (currentPlayer.Color == gamestatus)
            {
                makeMove(newMove[0], newMove[1], newMove[2], newMove[3]);
            }
        }

        //Ändrar gamestatus till gameover, och tar bort lagringsfilen
        public void gameOver()
        {
            gamestatus = gamestatus + " player lost. GAME OVER!";
            ds.removeFile();
            window.gameOver(gamestatus);
        }

        //Kallas på efter att en player försöker göra ett drag för att se till att det är giltigt
        public void makeMove(int fromX, int fromY, int toX, int toY)
        {         
            
            Move move = new Move(fromX, fromY, toX, toY);

            if (rules.isValid(move, gamestatus, board.get()))
            {
                board.updateTable(move);
                window.updateTable();
                ds.SaveData(board.get());

                //Kollar om kungen blev utslagen
                switchTurn();

                if (rules.isCheckMate(gamestatus))
                {
                    gameOver();
                }
                
            }
        }

        //Byter gamestatus beroende på vems tur det är
        public void switchTurn()
        {
            if(gamestatus.Equals("white"))
            {
                gamestatus = "black";
                playerwhite.isPlayersTurn = false;
                playerblack.isPlayersTurn = true;
            }
            else if(gamestatus.Equals("black"))
            {
                gamestatus = "white";
                playerwhite.isPlayersTurn = true;
                playerblack.isPlayersTurn = false;
            }
        }

        public void setupWatcher()
        {
            watcher = new FileSystemWatcher();
            watcher.Path = @".\chessdata\";
            watcher.Filter = "*.xml";
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += new FileSystemEventHandler(xmlwatcher);
            watcher.EnableRaisingEvents = true;
        }   

        private void xmlwatcher(object source, FileSystemEventArgs e)
        {
            
            watcher.EnableRaisingEvents = false;
            board.setBoard(ds.LoadData());
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
            {
                window.setBoard(board.get());
                window.updateTable();
            }));

            watcher.EnableRaisingEvents = true;
             
        }
    }
}
