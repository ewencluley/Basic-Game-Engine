using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using BattleEngine.Menus;
using Microsoft.Xna.Framework.Input;

namespace BattleEngine
{
    class GameManager:DrawableGameComponent
    {
        #region Fields
        List<Screen> screens = new List<Screen>();
        Level currentLevel;
        #endregion

        public GameManager(Game game)
            :base(game)
        {
            currentLevel = new Level(game);
        }

        public override void Initialize()
        {
            currentLevel.Initialize();
            base.Initialize();
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
            bool gameActive = true;
            foreach (Screen activeScreen in screens) //make sure no screens are overlaying the game
            {
                if (gameActive && (activeScreen.ScreenState == ScreenState.Active
                                    || activeScreen.ScreenState == ScreenState.TransitioningIn
                                    || activeScreen.ScreenState == ScreenState.TransitioningOut) && !(activeScreen is Popup)) gameActive = false;
            }
            if(gameActive) currentLevel.Update(gameTime);// if no menus are overlaying the level, then update it.
            foreach (Screen screen in screens)
            {
                if (screen.ScreenState != ScreenState.Inactive)
                {
                    screen.Update(gameTime);
                }
            }

            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            bool gameActive = true;
            foreach (Screen activeScreen in screens)
            {
                if (gameActive && activeScreen.ScreenState == ScreenState.Active && !(activeScreen is Popup)) gameActive = false;
            }
            //if (gameActive) currentLevel.Draw(gameTime);
            currentLevel.Draw(gameTime);
            foreach (Screen screen in screens)
            {
                if (screen.ScreenState != ScreenState.Inactive)
                {
                    screen.Draw(gameTime);
                }
            }
            base.Draw(gameTime);

            
        }

    }
}
