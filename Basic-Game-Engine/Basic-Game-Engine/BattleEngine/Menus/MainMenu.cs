using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BattleEngine.Menus.MenuItems;
namespace BattleEngine.Menus
{
    class MainMenu:Screen
    {
        const float FINISHED_TOLLERANCE = 0.1f;
        const float TRANSITION_SPEED = 0.09f;
        Vector2 offscreenPosition;
        Vector2 onscreenPosition;

        KeyboardState prevKeyboardState;

        Texture2D img;
        SpriteBatch spriteBatch;

        public MainMenu(Game game)
            : base(game)
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            
            offscreenPosition = new Vector2(-1024, 0);
            onscreenPosition = Vector2.Zero;
            Button play = new TextButton(game, "button", new Vector2(200, 200), "Resume");
            play.clickedEvent +=new Button.ButtonClicked(play_clickedEvent);
            Button nothing = new TextButton(game, "button", new Vector2(200, 300), "Exit Game");
            nothing.clickedEvent +=new Button.ButtonClicked(exit_clickedEvent);
            MenuItems.Add(play);
            MenuItems.Add(nothing);

        }

        void play_clickedEvent(object sender)
        {
            System.Console.WriteLine("Clicked event");
            ScreenState = Menus.ScreenState.TransitioningOut;
        }

        void exit_clickedEvent(object sender)
        {
            System.Console.WriteLine("exit event");
            Game.Exit();
        }

        protected override void LoadContent()
        {
            img = Game.Content.Load<Texture2D>("MainMenu");
            base.LoadContent();
        }

        public override void TransitionIn(GameTime gt)
        {
            //transition in animation
            Position = Vector2.Lerp(Position, onscreenPosition, TRANSITION_SPEED);
            if (Vector2.Distance(Position, onscreenPosition) < FINISHED_TOLLERANCE) base.ScreenState = Menus.ScreenState.Active;
        }

        public override void TransitionOut(GameTime gt)
        {
            //transition out animation
            Position = Vector2.Lerp(Position, offscreenPosition, TRANSITION_SPEED);
            if (Vector2.Distance(Position, offscreenPosition) < FINISHED_TOLLERANCE) base.ScreenState = Menus.ScreenState.Inactive;
        }

        public override void Update(GameTime gameTime)
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.A) && !prevKeyboardState.Equals(Keyboard.GetState()))
            {
                switch(ScreenState){
                    case Menus.ScreenState.Active: ScreenState = Menus.ScreenState.TransitioningOut;  break;
                    case Menus.ScreenState.Inactive: ScreenState = Menus.ScreenState.TransitioningIn; break;
                    case Menus.ScreenState.TransitioningOut: ScreenState = Menus.ScreenState.TransitioningIn;  break;
                    case Menus.ScreenState.TransitioningIn: ScreenState = Menus.ScreenState.TransitioningOut;  break;
                }
            }
            prevKeyboardState = Keyboard.GetState();
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(img, Position, Color.White);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
