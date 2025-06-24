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
        
        // Состояния игры
        private GameState _currentState;
        
        private bool _gameRunning = false;
        
        public Game(int width, int height)
        {
            _width = width;
            _height = height;
            Console.WriteLine(_width);
            Console.WriteLine(_height);
            
            Initialize();
        }

        public void CollectCoin(Coin coin)
        {
            _collectedCoins.Add(coin);
        }
        
        private void Initialize()
        {
            // Создаем лабиринт (10x10 клеток для примера)
            int mazeWidth = _width / _cellSize;
            int mazeHeight = _height / _cellSize;
            _maze = new Maze(1);
            
            // Создаем игрока в стартовой позиции
            _player = InitializePlayer();
            
            
            
            // Устанавливаем начальное состояние
            _currentState = new PlayingState(this);
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

        
        
        // Свойства для доступа к игровым объектам
        public Player Player => _player;
        public Maze Maze => _maze;
        public int CellSize => _cellSize;
        
        public void InitializeMaze(int number) => _maze = new Maze(number);
    }
}