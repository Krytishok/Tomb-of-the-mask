using System.Drawing;
using Tomb_of_the_mask.Mazes;

namespace Tomb_of_the_mask.Entities
{
    public class Player
    {
        public float X { get; private set; } // Теперь float для плавности
        public float Y { get; private set; }
        public int Size { get; }
    
        private float _targetX;
        private float _targetY;
        private float _moveSpeed = 0.5f; // Скорость перемещения (меньше = плавнее)
    
        public bool IsMoving => X != _targetX || Y != _targetY;

        private int _rotation;
        
        private Image _sprite;
        private Image _deadstate;
        
        public Player(int x, int y, int size, Image sprite, Image deadstate)
        {
            X = x;
            Y = y;
            _targetX = x;
            _targetY = y;
            Size = size;
            _sprite = sprite;
            _deadstate = deadstate;
            
        }
        
        public void Move(int dx, int dy, Maze maze)
        {
            float newX = X;
            float newY = Y;
            
            
            List<Coin> CollectedCoins = new List<Coin>();
            
            // Двигаемся пока не встретим стену
            while (!maze.IsWall((Convert.ToInt16(newX + dx)), Convert.ToInt16(newY + dy)))
            {
                Coin nextCoin = new Coin(Convert.ToInt16(newX + dx), Convert.ToInt16(newY + dy));
                CollectedCoins.Add(nextCoin);
                newX += dx;
                newY += dy;
                

            }
            if (maze.IsExit((Convert.ToInt16(newX + dx)), Convert.ToInt16(newY + dy)))
            {
                maze.IsNextLevel = true;
            }
            if(newX == X && newY == Y) return;
            if (IsMoving) return;
            _targetX = newX;
            _targetY = newY;
            InitializeRotation(newX, newY);
            
            foreach (var coin in CollectedCoins)
            {
                maze.RemoveCoin(coin);
            }
            if (maze.IsObstacle((Convert.ToInt16(newX + dx)), Convert.ToInt16(newY + dy)))
            {
                _sprite = _deadstate;
                if (MessageBox.Show("Вы проиграли!") == DialogResult.OK)
                {
                    
                    maze.RestartGame();
                }
                
            }
            
            
            
        }
        
        public void Update()
        {
            // Плавное перемещение к цели
            if (IsMoving)
            {
                X = Lerp(X, _targetX, _moveSpeed);
                Y = Lerp(Y, _targetY, _moveSpeed);
            
                // Резкая остановка при приближении
                if (MathF.Abs(X - _targetX) < 0.01f) X = _targetX;
                if (MathF.Abs(Y - _targetY) < 0.01f) Y = _targetY;
            }
        }
        
        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public void Draw(Graphics g)
        {
            Bitmap src = _sprite as Bitmap;
            g.DrawImage(src, X*Size, Y*Size, Size, Size);
        }
        

        private void InitializeRotation(float newX, float newY)
        {
            ReturnToZeroRotation();
            if (X - newX < 0) //Вправо
            {
                _rotation = 270;
                _sprite.RotateFlip(RotateFlipType.Rotate270FlipNone);
                Console.WriteLine(X);
                Console.WriteLine(newX);
            }
            else if (X - newX > 0) //Влево
            {
                _rotation = 90;
                _sprite.RotateFlip(RotateFlipType.Rotate90FlipNone);
                
            }

            if (Y - newY > 0) // Вверх
            {
                _rotation = 180;
                _sprite.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
            else if (Y - newY < 0)
            {
                _rotation = 0;
            }
            
        }

        private void ReturnToZeroRotation()
        {
            if (_rotation == 90)
            {
                _sprite.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }

            if (_rotation == 180)
            {
                _sprite.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }

            if (_rotation == 270)
            {
                _sprite.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            _rotation = 0;
            
        }
        
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Math.Clamp(t, 0, 1); // Clamp ограничивает t диапазоном [0, 1]
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)X, (int)Y, 1, 1);
        }
    }
    
}