using System.Drawing;
using System.Windows.Forms;
using Tomb_of_the_mask.Entities;

namespace Tomb_of_the_mask.Core.GameStates
{
    public class PlayingState : GameState
    {
        private Game _game;
        
        public PlayingState(Game game)
        {
            _game = game;
        }
        
        public override void Update()
        {
            
        }
        
        public override void Render(Graphics g)
        {
            _game.Player.Draw(g);
            _game.Maze.Draw(g, _game.CellSize);
            _game.RenderUI(g);
            
        }
        
        public override void HandleInput(Keys key)
        {
            var player = _game.Player;
            var maze = _game.Maze;
            
            switch (key)
            {
                case Keys.Left:
                    MovePlayerUntilWall(-1, 0);
                    break;
                case Keys.Right:
                    MovePlayerUntilWall(1, 0);
                    break;
                case Keys.Up:
                    MovePlayerUntilWall(0, -1);
                    break;
                case Keys.Down:
                    MovePlayerUntilWall(0, 1);
                    break;
            }
            
            void MovePlayerUntilWall(int dx, int dy)
            {
                player.Move(dx, dy, maze, _game);
            }
            
        }

        
        
        
    }
}