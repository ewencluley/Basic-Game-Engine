using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using BattleEngine.Menus;

namespace BattleEngine
{
    class GameManager:DrawableGameComponent
    {
        #region Fields
        List<Screen> screens = new List<Screen>();
        #endregion

        public GameManager(Game game)
            :base(game)
        {

        }

        public void AddScreen(Screen screen)
        {
            screen.Initialize();
            screens.Add(screen);
            screen.ScreenState = ScreenState.TransitioningIn;
        }

        public void RemoveScreen(Screen screen)
        {
            screen.ScreenState = ScreenState.TransitioningOut;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Screen screen in screens)
            {
                screen.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Screen screen in screens)
            {
                screen.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

    }
}
