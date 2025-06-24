// Maze.cs
using System.Drawing;
using Tomb_of_the_mask.Entities;

namespace Tomb_of_the_mask.Mazes
{
    public class Maze
    {
        private string _projectPath = new DirectoryInfo(
            Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString();
        public int _width { get; }
        public int _height { get; }
        public Point Start { get; private set; }
        public Point Exit { get; private set; }
        
        public int levelNumber { get; private set; }
        public bool IsNextLevel { get; set; }
    
        private char[,] _grid;
        private List<Coin> _coins;
        
        public bool _isGameOver { get; private set; }
        
        private Pen _pen;
        
        private Image _image = Image.FromFile("C:\\Users\\Илья\\RiderProjects\\Tomb of the mask\\Tomb of the mask\\Assets\\Barrier.png");

        public Maze(int number)
        {
            Console.WriteLine(number);
            levelNumber = number;
            _isGameOver = false;
            IsNextLevel = false;
            var levelData = LevelLoader.LoadLevelData(_projectPath, number);
            _width = levelData.Width;
            _height = levelData.Height;
            _grid = new char[_width, _height];
            Start = levelData.Start;
            Exit = levelData.Exit;

            // Заполняем сетку
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _grid[x, y] = levelData.Grid[y][x]; 
                }
            }
            InitializeObjects();
        }
        

        public bool IsWall(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height)
                return true;
            
            return _grid[x, y] == '#' || _grid[x, y] == 'X' || _grid[x, y] == 'E';
        }

        public List<Coin> GetCoins()
        {
            return _coins;
        }

        public void RemoveCoin(Coin coin)
        {
            _grid[coin.X, coin.Y] = '.';
        }

        public void RemoveCoins(List<Coin> coins)
        {
            foreach (Coin coin in coins)
            {
                _coins.Remove(coin);
            }
        }

        private void InitializeObjects()
        {
            _coins = new List<Coin>();
            _pen = new Pen(Color.DarkViolet, 1f);
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_grid[x, y] == 'C')
                    {
                        _coins.Add(new Coin(x, y));
                    }
                }
            }
        }

        public void Draw(Graphics g, int cellSize)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var brush = Brushes.Black;
                    switch (_grid[x, y])
                    {
                        case '#': // Стена
                            brush = Brushes.DarkBlue;
                            break;
                        case '.': // Проход
                            brush = Brushes.Black;
                            break;
                        case 'C': // Монетка
                            brush = Brushes.Gold;
                            break;
                        case 'M': // Враг
                            brush = Brushes.Brown;
                            break;
                        case 'S': // Старт
                            brush = Brushes.BlueViolet;
                            Start = new Point(x, y);
                            break;
                        case 'X': // Шипы
                            brush = Brushes.Red;
                            break;
                        case 'E': // Выход
                            brush = Brushes.Orange;
                            break;
                    }
                    if (_grid[x, y] == 'C')
                    {
                        g.FillEllipse(Brushes.Yellow, 
                            x * cellSize + cellSize / 2, 
                            y * cellSize + cellSize / 2,
                            cellSize / 5, cellSize / 5);
                    }
                    if (_grid[x, y] == 'X')
                    {
                        g.DrawImage(_image, x*cellSize+1, y*cellSize+1, cellSize, cellSize);
                    }
                    if (_grid[x, y] == '#')
                    {
                        
                        g.DrawRectangle(_pen, x*cellSize, y*cellSize, cellSize, cellSize);
                    }

                    if (_grid[x, y] == 'E')
                    {
                        g.FillRectangle(brush, x*cellSize, y*cellSize, cellSize, cellSize);
                    }
                    
                    
                }
            }
        }

        public void NextLevel(int number)
        {
            levelNumber = number;
            IsNextLevel = true;
        }

        public void RestartGame()
        {
            _isGameOver = true;
        }

        public Point GetStart()
        {
            return Start;
        }
        
        // Новые методы для работы с объектами
        public bool IsExit(int x, int y) => _grid[x, y] == 'E';
        public bool IsObstacle(int x, int y) => _grid[x, y] == 'X';
        public bool IsCoin(int x, int y) => _grid[x, y] == 'C';
        public bool IsEnemy(int x, int y) => _grid[x, y] == 'M';
        public void CollectCoin(int x, int y) => _grid[x, y] = '.'; // Убираем монетку
    }
}