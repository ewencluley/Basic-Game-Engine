using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BattleEngine.Menus.MenuItems;

namespace BattleEngine.Menus
{
    abstract class  Screen:DrawableGameComponent
    {
        public List<MenuItem> MenuItems
        {
            get { return menuItems; }
        }
        List<MenuItem> menuItems = new List<MenuItem>();

        public ScreenState ScreenState
        {
            get { return state; }
            set { state = value; }
        }
        ScreenState state;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        public Screen(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            LoadContent();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            foreach (MenuItem item in menuItems)
            {
                item.Initialize();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (state == ScreenState.TransitioningIn)
            {
                //transitioning in activity
                TransitionIn(gameTime);
            }
            else if (state == ScreenState.TransitioningOut)
            {
                //transitioning out activity
                TransitionOut(gameTime);
            }
            foreach (MenuItem item in menuItems)
            {
                item.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public abstract void TransitionIn(GameTime gt);
        public abstract void TransitionOut(GameTime gt);

        public abstract void TriggerTransitionIn();
        public abstract void TriggerTransitionOut();

        public override void Draw(GameTime gameTime)
        {
            foreach (MenuItem item in menuItems)
            {
                item.Draw(gameTime, position);
            }
            base.Draw(gameTime);
        }

    }
}
