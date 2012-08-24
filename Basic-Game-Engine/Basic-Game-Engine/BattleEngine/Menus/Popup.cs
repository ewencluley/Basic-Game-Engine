using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace BattleEngine.Menus
{
    class Popup:Screen
    {
        float opacity = 0.0f;
        KeyboardState prevKeyboardState;
        SpriteBatch spriteBatch;
        Texture2D img;

        public Popup(Game game)
            : base(game)
        {

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            img = Game.Content.Load<Texture2D>("Lighthouse");
            base.LoadContent();
        }

        public override void TransitionIn(GameTime gt)
        {
            //transition in animation
            if(opacity < 1.0f) opacity += (float)gt.ElapsedGameTime.TotalSeconds;
            else base.ScreenState = Menus.ScreenState.Active;
        }

        public override void TransitionOut(GameTime gt)
        {
            //transition out animation
            if(opacity > 0.0f) opacity -= (float)gt.ElapsedGameTime.TotalSeconds;
            else base.ScreenState = Menus.ScreenState.Inactive;
        }

        public override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.A) && !prevKeyboardState.Equals(Keyboard.GetState()))
            {
                switch (ScreenState)
                {
                    case Menus.ScreenState.Active: ScreenState = Menus.ScreenState.TransitioningOut; break;
                    case Menus.ScreenState.Inactive: ScreenState = Menus.ScreenState.TransitioningIn; break;
                    case Menus.ScreenState.TransitioningOut: ScreenState = Menus.ScreenState.TransitioningIn; break;
                    case Menus.ScreenState.TransitioningIn: ScreenState = Menus.ScreenState.TransitioningOut; break;
                }
            }
            prevKeyboardState = Keyboard.GetState();
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(img, Position, new Color(new Vector4(opacity,opacity,opacity, opacity)));
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
