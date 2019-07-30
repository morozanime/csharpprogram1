
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private List<Button> buttons;
        private Dictionary<int, Color> usedColors = new Dictionary<int, Color>();
        private Game game;
        private const int cellSize = 80;
        private const int boardSize = 4;
        private Random rnd = new Random((int)DateTime.Now.ToBinary());
        public Form1()
        {
            InitializeComponent();
            buttons = new List<Button>();

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Button btn = new Button();
                    //                    btn.Text = i + "," + j;
                    btn.Location = new Point(j * cellSize + 10, i * cellSize + 10);
                    btn.Size = new Size(cellSize, cellSize);
                    Controls.Add(btn);
                    buttons.Add(btn);
                    btn.Show();
                }
            }
            Button btnUp = new Button();
            btnUp.Text = "UP";
            btnUp.Location = new Point(boardSize * cellSize + 10 + cellSize, boardSize * cellSize / 2 - cellSize + 10);
            btnUp.Size = new Size(cellSize, cellSize);
            btnUp.Click += new System.EventHandler(btnUp_Click);
            Controls.Add(btnUp);
            btnUp.Show();
            Button btnRight = new Button();
            btnRight.Text = ">";
            btnRight.Location = new Point(boardSize * cellSize + 10 + cellSize * 2, boardSize * cellSize / 2 + 10);
            btnRight.Size = new Size(cellSize, cellSize);
            btnRight.Click += new System.EventHandler(btnRight_Click);
            Controls.Add(btnRight);
            btnRight.Show();
            Button btnDown = new Button();
            btnDown.Text = "DOWN";
            btnDown.Location = new Point(boardSize * cellSize + 10 + cellSize, boardSize * cellSize / 2 + cellSize + 10);
            btnDown.Size = new Size(cellSize, cellSize);
            btnDown.Click += new System.EventHandler(btnDown_Click);
            Controls.Add(btnDown);
            btnDown.Show();
            Button btnLeft = new Button();
            btnLeft.Text = "<";
            btnLeft.Location = new Point(boardSize * cellSize + 10, boardSize * cellSize / 2 + 10);
            btnLeft.Size = new Size(cellSize, cellSize);
            btnLeft.Click += new System.EventHandler(btnLeft_Click);
            Controls.Add(btnLeft);
            btnLeft.Show();
            usedColors[0] = Color.White;
            game = new Game(boardSize, updateCaption);
        }

        private int updateCaption(int x, int y, int val)
        {
            if (x >= boardSize || y >= boardSize || x < 0 || y < 0)
                return 1;
            buttons[y * boardSize + x].Text = val > 0 ? val.ToString() : "";
            if (!usedColors.ContainsKey(val))
            {
                usedColors[val] = Color.FromArgb(rnd.Next());
            }
            buttons[y * boardSize + x].BackColor = usedColors[val];
            return 0;
        }
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (game.move(Game.EMoveDir.moveUp) == false)
            {

            }
        }
        private void btnRight_Click(object sender, EventArgs e)
        {
            if (game.move(Game.EMoveDir.moveRight) == false)
            {

            }
        }
        private void btnDown_Click(object sender, EventArgs e)
        {
            if (game.move(Game.EMoveDir.moveDown) == false)
            {

            }
        }
        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (game.move(Game.EMoveDir.moveLeft) == false)
            {

            }
        }
    }
    public class Game
    {
        private int[,] boardData;
        private int boardSize;
        private Func<int, int, int, int> refreshCell;
        public enum EMoveDir
        {
            moveUp,
            moveRight,
            moveDown,
            moveLeft,
        };

        public Game(int size, Func<int, int, int, int> fn)
        {
            boardData = new int[size, size];
            boardSize = size;
            refreshCell = fn;
            move(EMoveDir.moveUp);
        }

        public bool move(EMoveDir direction)
        {
            int[,] old = new int[boardSize, boardSize];
            Array.Copy(boardData, 0, old, 0, boardData.Length);
            for (int x = 0; x < boardSize; x++)
            {
                switch (direction)
                {
                    case EMoveDir.moveUp:
                        moveUp(x);
                        break;
                    case EMoveDir.moveRight:
                        moveRight(x);
                        break;
                    case EMoveDir.moveDown:
                        moveDown(x);
                        break;
                    case EMoveDir.moveLeft:
                        moveLeft(x);
                        break;
                }
            }
            bool flag = place2();
            for (int x = 0; x < boardSize; x++)
                for (int y = 0; y < boardSize; y++)
                    if (old[x, y] != boardData[x, y])
                        refreshCell(x, y, boardData[x, y]);
            return flag;
        }

        private void moveUp(int x)
        {
            List<int> neCells = new List<int>();
            for (int y = 0; y < boardSize; y++)
                if (boardData[x, y] > 0)
                    neCells.Add(boardData[x, y]);
            for (int y = 0; y < neCells.Count() - 1; y++)
                if (neCells[y] == neCells[y + 1])
                {
                    neCells[y] *= 2;
                    neCells.RemoveAt(y + 1);
                }
            for (int y = 0; y < boardSize; y++)
                boardData[x, y] = (y < neCells.Count()) ? neCells[y] : 0;
        }
        private void moveDown(int x)
        {
            List<int> neCells = new List<int>();
            for (int y = 0; y < boardSize; y++)
                if (boardData[x, y] > 0)
                    neCells.Add(boardData[x, y]);
            for (int y = 0; y < neCells.Count() - 1; y++)
                if (neCells[y] == neCells[y + 1])
                {
                    neCells[y] *= 2;
                    neCells.RemoveAt(y + 1);
                }
            for (int y = 0; y < boardSize; y++)
                boardData[x, y] = (y < boardSize - neCells.Count()) ? 0 : neCells[y - (boardSize - neCells.Count())];
        }
        private void moveLeft(int x)
        {
            List<int> neCells = new List<int>();
            for (int y = 0; y < boardSize; y++)
                if (boardData[y, x] > 0)
                    neCells.Add(boardData[y, x]);
            for (int y = 0; y < neCells.Count() - 1; y++)
                if (neCells[y] == neCells[y + 1])
                {
                    neCells[y] *= 2;
                    neCells.RemoveAt(y + 1);
                }
            for (int y = 0; y < boardSize; y++)
                boardData[y, x] = (y < neCells.Count()) ? neCells[y] : 0;
        }
        private void moveRight(int x)
        {
            List<int> neCells = new List<int>();
            for (int y = 0; y < boardSize; y++)
                if (boardData[y, x] > 0)
                    neCells.Add(boardData[y, x]);
            for (int y = 0; y < neCells.Count() - 1; y++)
                if (neCells[y] == neCells[y + 1])
                {
                    neCells[y] *= 2;
                    neCells.RemoveAt(y + 1);
                }
            for (int y = 0; y < boardSize; y++)
                boardData[y, x] = (y < boardSize - neCells.Count()) ? 0 : neCells[y - (boardSize - neCells.Count())];
        }

        struct SemptyCell
        {
            public int x;
            public int y;
            public SemptyCell(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        };
        private bool place2()
        {
            List<SemptyCell> emptyCells = new List<SemptyCell>();
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (boardData[x, y] <= 0)
                    {
                        emptyCells.Add(new SemptyCell(x, y));
                    }
                }
            }
            if (emptyCells.Count() < 2)
                return false;

            int a = 2;
            Random rnd = new Random((int)DateTime.Now.ToBinary());
            while (a > 0)
            {
                int i = Math.Abs(rnd.Next()) % emptyCells.Count();
                boardData[emptyCells[i].x, emptyCells[i].y] = 2;
                emptyCells.RemoveAt(i);
                a--;
            }
            return true;
        }
    }
}
