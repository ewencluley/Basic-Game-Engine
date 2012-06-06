using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleEngine.Menus.MenuItems
{
    abstract class MenuItem:DrawableGameComponent
    {
        public MenuItem(Game game)
            : base(game)
        {

        }

        public MenuItemState MenuItemState
        {
            get { return state; }
            set { state = value; }
        }
        MenuItemState state;

        public abstract void Draw(GameTime gameTime, Vector2 parentPos);

    }
}
