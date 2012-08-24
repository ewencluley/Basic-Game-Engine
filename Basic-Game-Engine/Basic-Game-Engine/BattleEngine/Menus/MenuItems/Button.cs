using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleEngine.Menus.MenuItems
{
    class Button:MenuItem
    {
        Texture2D buttonImage;
        String buttonImageName;
        SpriteBatch spriteBatch;
        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
        }

        MouseState prevMouseState;

        public delegate void ButtonClicked(object sender);
        public event ButtonClicked clickedEvent;

        public Button(Game game, String buttonImageName, Vector2 position)
            : base(game)
        {
            this.buttonImageName = buttonImageName;
            this.position = position;
        }

        public override void Initialize()
        {
            LoadContent();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            buttonImage = Game.Content.Load<Texture2D>(buttonImageName);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton.Equals(ButtonState.Pressed) && MouseOverButton(Mouse.GetState()) && !prevMouseState.Equals(Mouse.GetState()))
            {
                clickedEvent(this);//fire the event
                //System.Console.WriteLine("clicked!");
            }
            if(MouseOverButton(Mouse.GetState()))
            {
                MenuItemState = Menus.MenuItemState.MouseOver;
            }
            base.Update(gameTime);
            prevMouseState = Mouse.GetState();
        }

        private bool MouseOverButton(MouseState mouse)
        {
            if ((mouse.X >= (position.X + buttonImage.Bounds.Left)) && (mouse.X <= (position.X + buttonImage.Bounds.Right)))
            {
                if ((mouse.Y >= (position.Y + buttonImage.Bounds.Top)) && (mouse.Y <= (position.Y + buttonImage.Bounds.Bottom)))
                {
                    return true;
                }
            }
            return false;
        }
        public override void Draw(GameTime gameTime, Vector2 parentPos)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(buttonImage, parentPos + position, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public Vector2 ButtonDimensions()
        {
            return new Vector2(buttonImage.Width, buttonImage.Height);
        }
    }
}
