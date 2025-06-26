using System.Drawing;
using System.Windows.Forms;
using Tomb_of_the_mask.Entities;
using Tomb_of_the_mask.Mazes;
using Tomb_of_the_mask.Core.GameStates;


namespace Tomb_of_the_mask.Core
{
    public class Game
    {
        private string _projectPath = new DirectoryInfo(
            Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString();
        // Игровые объекты
        private Player _player;
        private Maze _maze;
        
        private List<Coin> _collectedCoins = new List<Coin>();
        
        // Размеры игрового поля
        private int _width;
        private int _height;
        private int _cellSize = 32; // Размер одной клетки в пикселях
        
        public int HighScore { get; private set; }
        
        // Состояния игры
        private GameState _currentState;
        
        private bool _gameRunning = false;
        
        public int TotalCoins { get; private set; }
        public int CollectedCoins { get; private set; }
        
        private Font _font = new Font("Arial", 14, FontStyle.Bold);
        private Brush _textBrush = Brushes.Gold;
        private Point _coinCounterPos = new Point(10, 5);
        
        public Game(int width, int height)
        {
            _width = width;
            _height = height;
            
            Initialize();
        }

        public void CollectCoin(Coin coin)
        {
            CollectedCoins += 1;
        }
        
        private void Initialize()
        {
            HighScore = SaveManager.LoadHighScore();
            // Создаем лабиринт (10x10 клеток для примера)
            int mazeWidth = _width / _cellSize;
            int mazeHeight = _height / _cellSize;
            _maze = new Maze(1);
            
            // Создаем игрока в стартовой позиции
            _player = InitializePlayer();
            
            
            
            // Устанавливаем начальное состояние
            _currentState = new PlayingState(this);
            
            TotalCoins = _maze._coins.Count;
        }

        public Player InitializePlayer()
        {
            string playerSpritePath = Path.Combine(
                _projectPath,
                "Assets\\Player.png"
            );
            string playerDeadState =  Path.Combine(
                _projectPath,
                "Assets\\PlayerDead.png"
            );
            Point startPos = _maze.GetStart();
            _player = new Player(
                startPos.X, startPos.Y,
                _cellSize,
                Image.FromFile(playerSpritePath),
                Image.FromFile(playerDeadState)
                );
            
            return _player;
        }
        
        public void Update()
        {
            _currentState.Update();
            _player.Update();
        }

        public void Start(Graphics g)
        {
            _maze.Draw(g, _cellSize);
        }
        
        public void Render(Graphics g)
        {
            // Очищаем экран
            g.Clear(Color.Black);
            
            // Рендерим текущее состояние
            _currentState.Render(g);
        }
        
        public void HandleInput(Keys key)
        {
            _currentState.HandleInput(key);
        }
        
        public void Resize(int width, int height)
        {
            _width = width;
            _height = height;
            // Можно добавить логику пересчета при изменении размеров
        }
        
        public void RenderUI(Graphics g)
        {
            CollectedCoins = _maze._collectedCoins;
            // Рисуем счетчик монет
            string coinText = $"{CollectedCoins}/{TotalCoins}";
        
            // Фон для читаемости
            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), 
                _coinCounterPos.X - 10, 
                _coinCounterPos.Y - 5, 
                100, 30);
        
            // Иконка монеты
            g.FillEllipse(Brushes.Gold, _coinCounterPos.X, _coinCounterPos.Y, 20, 20);
            g.DrawEllipse(Pens.DarkGoldenrod, _coinCounterPos.X, _coinCounterPos.Y, 20, 20);
        
            // Текст счета
            g.DrawString(coinText, _font, _textBrush, _coinCounterPos.X + 25, _coinCounterPos.Y + 2);
            
            RenderHighScore(g);
        }
        
        private void RenderHighScore(Graphics g)
        {
            string highScoreText = $"Рекорд: {HighScore}";
            SizeF textSize = g.MeasureString(highScoreText, _font);
        
            Point pos = new Point(
                this._width - (int)textSize.Width - 30, 
                5);
        
            // Фон
            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), 
                pos.X - 10, 
                pos.Y - 5, 
                textSize.Width + 20, 
                textSize.Height + 10);
        
            // Текст
            g.DrawString(highScoreText, _font, Brushes.Gold, pos);
            
            CheckCoinCollection();
        
            
        }

        
        
        // Свойства для доступа к игровым объектам
        public Player Player => _player;
        public Maze Maze => _maze;
        public int CellSize => _cellSize;
        
        public void InitializeMaze(int number)
        {
            _maze = new Maze(number);
            TotalCoins = _maze._coins.Count;
        }
        
        public void CheckCoinCollection()
        {
            if (_maze._collectedCoins > HighScore)
            {
                HighScore = _maze._collectedCoins;
                SaveManager.SaveHighScore(HighScore);
            }
            
        }
    }
}