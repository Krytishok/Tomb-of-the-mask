// MazeGenerator.cs
using System;
using System.Collections.Generic;

namespace Tomb_of_the_mask.Mazes
{
    public class MazeGenerator
    {
        private int _width, _height;
        private char[,] _grid; // Изменено на char
        private Random _random;

        private readonly int blockSize = 5;
        private readonly List<char[,]> _blockTemplates; // Изменено на char

        public MazeGenerator(int width, int height)
        {
            if (width % blockSize != 0) width += blockSize - (width % blockSize);
            if (height % blockSize != 0) height += blockSize - (height % blockSize);

            _width = width;
            _height = height;
            _grid = new char[_width, _height];
            _random = new Random();

            // Инициализация сетки
            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                    _grid[x, y] = '#'; // По умолчанию все стены

            _blockTemplates = GenerateTemplates();
        }

        public char[,] Generate()
        {
            for (int y = 0; y < _height; y += blockSize)
            {
                for (int x = 0; x < _width; x += blockSize)
                {
                    var template = _blockTemplates[_random.Next(_blockTemplates.Count)];
                    ApplyBlock(template, x, y);
                }
            }

            // Добавим старт и выход
            _grid[_width / 2, 0] = 'S'; // Старт
            _grid[_width / 2, _height - 1] = 'E'; // Выход

            return _grid;
        }

        private void ApplyBlock(char[,] block, int startX, int startY)
        {
            for (int y = 0; y < blockSize; y++)
            {
                for (int x = 0; x < blockSize; x++)
                {
                    int gx = startX + x;
                    int gy = startY + y;

                    if (gx < _width && gy < _height)
                        _grid[gx, gy] = block[x, y];
                }
            }
        }

        private List<char[,]> GenerateTemplates()
        {
            var templates = new List<char[,]>();

            // 1. Прямой проход (вертикальный)
            templates.Add(new char[5, 5]
            {
                { '#', '#', '.', '#', '#' },
                { '#', '#', '.', '#', '#' },
                { '#', '#', '.', '#', '#' },
                { '#', '#', '.', '#', '#' },
                { '#', '#', '.', '#', '#' }
            });

            // 2. Поворот (сверху влево)
            templates.Add(new char[5, 5]
            {
                { '#', '#', '.', '.', '.' },
                { '#', '#', '.', '#', '#' },
                { '#', '#', '.', '#', '#' },
                { '#', '#', '.', '#', '#' },
                { '#', '#', '.', '#', '#' }
            });

            // 3. Тупик с монеткой
            templates.Add(new char[5, 5]
            {
                { '#', '#', '.', '#', '#' },
                { '#', '#', '.', '#', '#' },
                { '#', '#', 'C', '#', '#' }, // Монетка
                { '#', '#', '.', '#', '#' },
                { '#', '#', '#', '#', '#' }
            });

            // 4. Горизонтальный проход
            templates.Add(new char[5, 5]
            {
                { '#', '#', '#', '#', '#' },
                { '.', '.', '.', '.', '.' },
                { '.', '.', '.', '.', '.' },
                { '.', '.', '.', '.', '.' },
                { '#', '#', '#', '#', '#' }
            });

            // 5. Перекресток
            templates.Add(new char[5, 5]
            {
                { '#', '#', '.', '#', '#' },
                { '#', '#', '.', '#', '#' },
                { '.', '.', '.', '.', '.' },
                { '#', '#', '.', '#', '#' },
                { '#', '#', '.', '#', '#' }
            });

            // 6. Т-образный переход
            templates.Add(new char[5, 5]
            {
                { '#', '#', '.', '#', '#' },
                { '#', '#', '.', '#', '#' },
                { '.', '.', '.', '.', '.' },
                { '#', '#', '.', '#', '#' },
                { '#', '#', '#', '#', '#' }
            });

            // 7. Комната с врагом
            templates.Add(new char[5, 5]
            {
                { '#', '#', '.', '#', '#' },
                { '#', '.', '.', '.', '#' },
                { '.', '.', 'M', '.', '.' }, // Враг
                { '#', '.', '.', '.', '#' },
                { '#', '#', '.', '#', '#' }
            });

            // 8. Зигзаг с монетками
            templates.Add(new char[5, 5]
            {
                { '#', '#', '.', '#', '#' },
                { '#', 'C', '.', '#', '#' }, // Монетка
                { '#', '#', '.', '.', '.' },
                { '#', '#', 'C', '#', '#' }, // Монетка
                { '#', '#', '#', '#', '#' }
            });

            // 9. Вертикальный проход с платформами
            templates.Add(new char[5, 5]
            {
                { '#', '#', '.', '#', '#' },
                { '#', '#', '#', '#', '#' },
                { '#', '#', '.', '#', '#' },
                { '#', '#', '#', '#', '#' },
                { '#', '#', '.', '#', '#' }
            });

            // 10. Широкий коридор с врагом
            templates.Add(new char[5, 5]
            {
                { '#', '.', '.', '.', '#' },
                { '#', '.', '#', '.', '#' },
                { '#', '.', 'M', '.', '#' }, // Враг
                { '#', '.', '#', '.', '#' },
                { '#', '.', '.', '.', '#' }
            });

            // 11. Платформы с монетками
            templates.Add(new char[5, 5]
            {
                { '#', '#', '#', '#', '#' },
                { '#', 'C', '#', 'C', '#' }, // Монетки
                { '#', '#', '.', '#', '#' },
                { '#', 'C', '#', 'C', '#' }, // Монетки
                { '#', '#', '#', '#', '#' }
            });

            // 12. Лабиринтный блок
            templates.Add(new char[5, 5]
            {
                { '#', '.', '#', '.', '#' },
                { '.', '.', '#', '.', '.' },
                { '#', '.', '.', '.', '#' },
                { '.', '.', '#', '.', '.' },
                { '#', '.', '#', '.', '#' }
            });

            return templates;
        }
    }
}