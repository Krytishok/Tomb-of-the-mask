using System.Drawing;
using System.Windows.Forms;

namespace Tomb_of_the_mask.Core
{
    public abstract class GameState
    {
        public abstract void Update();
        public abstract void Render(Graphics g);
        public abstract void HandleInput(Keys key);
    }
}