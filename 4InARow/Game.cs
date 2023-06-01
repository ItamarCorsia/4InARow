using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FourInARow
{
    class Game
    {
        public enum GameStatus
        {
            PLAY,
            WIN,
            EVEN
        }
        private enum BoardCell
        {
            EMPTY,
            player1,
            player2
        }
        public GameStatus Status { get; private set; }
        private BoardCell nextBoardCell;
        private const int RowSize = 7;
        private const int PLAY = 0;
        private const int WIN = 1;
        private const int EVEN = 2;
        private const string PLAY_DISPLAY = " Play now";
        private const string EVEN_DISPLAY = "Game Even";
        private const string WIN_DISPLAY = " Win";
        private const int EMPTY = 0;
        private const int player1 = 1;
        private const int player2 = 2;
        private const int DEFAULT_ROW_SIZE = 7;
        public int next, count;
        private string[] drawSign;
        public int[,] board;
        public int newRow;
        public int status { get; set; }
        public string Display { get; private set; }
        public bool win;
        
        public Game()
        {
            InitGame();
        }
        private void InitGame()
        {
            drawSign = new string[] { "", "player1", "player2" };
            board = new int[7, 7];
            ResetGame();
        }
        public void ResetGame()
        {
            Status = GameStatus.PLAY;
            count = 0;
            nextBoardCell = BoardCell.player1;
            Display = drawSign[(int)nextBoardCell] + PLAY_DISPLAY;
            for (int i = 0; i < RowSize; i++)
                for (int j = 0; j < RowSize; j++)
                    board[i, j] = (int)BoardCell.EMPTY;
        }

        public string Move(string rowcol)
        {
            int col = int.Parse(rowcol.Substring(1, 1));
            int row = ReturnEmptyRow(col);
            if (board[row, col] == (int)BoardCell.EMPTY && Status == GameStatus.PLAY)
            {
                win = false;
                count++;
                board[row, col] = (int)nextBoardCell;
                UpdateStatus();
                if (Status == GameStatus.PLAY)
                    nextBoardCell = (nextBoardCell == BoardCell.player1) ? BoardCell.player2 : BoardCell.player1;
                else if (Status == GameStatus.EVEN)
                    Display = EVEN_DISPLAY;
                else
                    Display = drawSign[(int)nextBoardCell] + WIN_DISPLAY;
            }
            else
                win = true;
            if (count == RowSize * RowSize)
                Status = GameStatus.EVEN;

            return row + (string.Empty + col);
        }

        private void UpdateStatus()
        {
            bool win = CheckDiagonals(); 
            if (!win)
                win = (CheckColumns() || CheckRows());
            if (!win)
                if (count < RowSize * RowSize)
                    Status = GameStatus.PLAY;
                else
                    Status = GameStatus.EVEN;
            else
                Status = GameStatus.WIN;
        }

        private bool CheckDiagonals()
        {

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if ((board[row, col] == player1) && (board[row + 1, col + 1] == player1) && (board[row + 2, col + 2] == player1) && (board[row + 3, col + 3] == player1))
                    {
                        return true;
                        status = WIN;
                    }
                    if ((board[row, col] == player2) && (board[row + 1, col + 1] == player2) && (board[row + 2, col + 2] == player2) && (board[row + 3, col + 3] == player2))
                    {
                        return true;
                        status = WIN;
                    }
                }
            }

            int num = 6;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if ((board[num, j] == player1) && (board[num-1, j + 1] == player1) && (board[ num - 2,j+2] == player1) && (board[ num - 3,j+3] == player1))
                    {
                        return true;
                        status = WIN;
                    }

                    if ((board[num, j] == player2) && (board[num - 1, j + 1] == player2) && (board[num - 2, j + 2] == player2) && (board[num - 3, j + 3] == player2))
                    {
                        return true;
                        status = WIN;
                    }
                }

                num--;

            }
            return false;
        }
        public int ReturnEmptyRow(int col)
        {   
            for (int i = 6; i >= 0; i--)
            {
                if (board[i, col] == EMPTY)
                {
                   return i;
                }
            }
            return 0;
        }
        
        public bool CheckRows()
        {
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if ((board[row, col] == player1) && (board[row, col + 1] == player1) && (board[row, col + 2] == player1) && (board[row, col + 3] == player1))
                    {
                        return true;
                        status = WIN;
                    }
                    if ((board[row, col] == player2) && (board[row, col + 1] == player2) && (board[row, col + 2] == player2) && (board[row, col + 3] == player2))
                    {
                        return true;
                        status = WIN;
                    }
                }
            }
            return false;
        }
        public bool CheckColumns()
        {
            for (int col = 0; col < 7; col++)
            {
                for (int row = 0; row < 4; row++)
                {
                    if ((board[row, col] == player1) && (board[row+1, col] == player1) && (board[row+ 2, col ] == player1) && (board[row + 3, col] == player1))
                    {
                        return true;
                        status = WIN;
                    }
                    if ((board[row, col] == player2) && (board[row+ 1, col ] == player2) && (board[row+ 2, col ] == player2) && (board[row + 3, col] == player2))
                    {
                        return true;
                        status = WIN;
                    }
                }
            }
            return false;
        }
    }
}