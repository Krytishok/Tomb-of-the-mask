using System;
using System.Drawing;
using System.Windows.Forms;
using Tomb_of_the_mask.Core;
using Tomb_of_the_mask.Entities;
using Tomb_of_the_mask.Mazes;

namespace Tomb_of_the_mask
{
    public partial class MainForm : Form
    {
        private Game _game;
        private BufferedGraphicsContext _context;
        private BufferedGraphics _buffer;
        private System.Windows.Forms.Timer _gameTimer;
        
        private bool _isFirstLoad = true;
        public MainForm()
        {
            // 1. Сначала настраиваем форму
            this.Text = "Tomb of the Mask";
            this.ClientSize = new Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true; // Дополнительная буферизация
            
            // 3. Настраиваем графический буфер
            SetupGraphicsBuffer();
            
            // 4. Настраиваем таймер
            _gameTimer = new System.Windows.Forms.Timer();
            _gameTimer.Interval = 16;
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Start();
            
            // 5. Подписываемся на события
            this.KeyDown += MainForm_KeyDown;
            this.Paint += MainForm_Paint;
            this.Resize += MainForm_Resize;
            this.Load += MainForm_Load;
        }
        
        private void SetupGraphicsBuffer()
        {
            _context = BufferedGraphicsManager.Current;
            _buffer = _context.Allocate(this.CreateGraphics(), this.ClientRectangle);
            
            // Заливаем черным фон при инициализации
            _buffer.Graphics.Clear(Color.Black);
        }
        
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (_game != null && _buffer != null)
            {
                _game.Update();
                this.Invalidate();
                CheckStates();
                
            }
        }
        
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (_buffer != null && _game != null)
                {
                    _game.Render(_buffer.Graphics);
                    _buffer.Render(e.Graphics);
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки (в релизе можно убрать)
                Console.WriteLine($"Render error: {ex.Message}");
            }
        }
        
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            _game?.HandleInput(e.KeyCode);
        }
        
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                SetupGraphicsBuffer();
                _game?.Resize(this.ClientSize.Width, this.ClientSize.Height);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
            {
                _isFirstLoad = false;
        
                // Инициализируем игру с размерами клиентской области
                _game = new Game(this.ClientSize.Width, this.ClientSize.Height);
                // Отрисовываем лабиринт один раз
                RedrawMaze();
            }
        }
        
        private void RedrawMaze()
        {
            using (var bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height))
            using (var g = Graphics.FromImage(bmp))
            {
                
                _game.Render(g);
                this.BackgroundImage = (Bitmap)bmp.Clone();
            }
        }
        
        private void CheckStates()
        {
            if (_game.Maze._isGameOver)
            {
                _game = new Game(this.ClientSize.Width, this.ClientSize.Height);
            }

            if (_game.Maze.IsNextLevel )
            {
                Console.WriteLine("Next level");
                _game.InitializeMaze(_game.Maze.levelNumber+1);
                _game.InitializePlayer();
            }
            
        }


    }
}