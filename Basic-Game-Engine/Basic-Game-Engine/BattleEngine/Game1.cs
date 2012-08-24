using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BattleEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameManager gameManager;
        KeyboardState prevKeyboardState;
        
        Menus.MainMenu mainMenu;
        Menus.Popup popup;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            GameSettings.LoadSettings(); //load settings from file
            graphics.PreferredBackBufferWidth = GameSettings.Settings.resolutionWidth;
            graphics.PreferredBackBufferHeight = GameSettings.Settings.resolutionHeight;
            graphics.IsFullScreen = GameSettings.Settings.fullScreen;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameManager = new GameManager(this);
            mainMenu = new Menus.MainMenu(this);
            gameManager.AddScreen(mainMenu);
            popup = new Menus.Popup(this);
            gameManager.AddScreen(popup);
            gameManager.Initialize();
            Components.Add(gameManager);
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            #region Main Menu Open/Close
            if (!prevKeyboardState.Equals(Keyboard.GetState()))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && mainMenu.ScreenState == Menus.ScreenState.Inactive)
                {
                    mainMenu.ScreenState = Menus.ScreenState.TransitioningIn;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Escape) && mainMenu.ScreenState == Menus.ScreenState.Active)
                {
                    mainMenu.ScreenState = Menus.ScreenState.TransitioningOut;
                }
            }
            #endregion

            #region Popup Open/Close
            if (!prevKeyboardState.Equals(Keyboard.GetState()))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.P) && popup.ScreenState == Menus.ScreenState.Inactive)
                {
                    popup.ScreenState = Menus.ScreenState.TransitioningIn;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.P) && popup.ScreenState == Menus.ScreenState.Active)
                {
                    popup.ScreenState = Menus.ScreenState.TransitioningOut;
                }
            }
            #endregion

            base.Update(gameTime);
            prevKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
