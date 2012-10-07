using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BattleEngine.GameElements.Cameras
{
    class MouseCamera: Camera
    {
        Vector3 position = new Vector3(240.0f,180.0f, 240.0f);
        float scrollSpeed = 5f;
        int scrollSpeedFactor = 200;
        int scrollAreaSize = 100;
        Rectangle scrollBounds;
        Game game;
        float rotation=180;
        Vector3 thirdPersonReference = new Vector3(0, 200, -200);

        MouseState lastMouseState, lastMouseMiddleClickState;

        public MouseCamera(Game game)
            :base(game)
        {
            this.game = game;
            scrollBounds = new Rectangle(scrollAreaSize, scrollAreaSize, game.GraphicsDevice.Viewport.Bounds.Right - (2 * scrollAreaSize), game.GraphicsDevice.Viewport.Bounds.Bottom - (2*scrollAreaSize));
        }

        public override void Update(GameTime gameTime)
        {
            #region Keyboard Control
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) position.Z += scrollSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) position.Z -= scrollSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) position.X -= scrollSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) position.X += scrollSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Q)) position.Y += scrollSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.A)) position.Y -= scrollSpeed;
            #endregion

            #region Mouse Control
            MouseState mouseState = Mouse.GetState();
            Matrix rotMatrix = Matrix.CreateRotationY(rotation);
            if(mouseState.MiddleButton.Equals(ButtonState.Released)){
                if (mouseState.X < scrollBounds.Left) position += Vector3.Transform(new Vector3((((float)(scrollBounds.Left - mouseState.X) / scrollSpeedFactor) * scrollSpeed), 0f, 0f), rotMatrix);
                if (mouseState.X > scrollBounds.Right) position += Vector3.Transform(new Vector3((((float)(scrollBounds.Right - mouseState.X) / scrollSpeedFactor) * scrollSpeed), 0f, 0f), rotMatrix);
            }else{
                if(lastMouseState.MiddleButton.Equals(ButtonState.Released)) lastMouseMiddleClickState = mouseState; //if new middle click
                rotation += Math.Min((lastMouseMiddleClickState.X - mouseState.X)/10000f, 10);
            }
            if (mouseState.Y < scrollBounds.Top) position += Vector3.Transform(new Vector3(0f, 0f, (((float)(scrollBounds.Top - mouseState.Y) / scrollSpeedFactor) * scrollSpeed)), rotMatrix);
            if (mouseState.Y > scrollBounds.Bottom) position += Vector3.Transform(new Vector3(0f, 0f, (((float)(scrollBounds.Bottom - mouseState.Y) / scrollSpeedFactor) * scrollSpeed)), rotMatrix);
            int dWheel = mouseState.ScrollWheelValue - lastMouseState.ScrollWheelValue;
            position.Y += (float)((float)dWheel / 50.0f);
            thirdPersonReference.Y += (float)((float)dWheel / 50.0f);
            #endregion
            
            // Calculate the camera's current position.
            Vector3 camTarget = position;
            Matrix rotationMatrix = Matrix.CreateRotationY(rotation);
            // Create a vector pointing the direction the camera is facing.
            Vector3 transformedReference =
                Vector3.Transform(thirdPersonReference, rotationMatrix);
            // Calculate the position the camera is looking at.
            Vector3 camPos = camTarget + transformedReference;
            // Set up the view matrix and projection matrix.
            base.ViewMatrix = Matrix.CreateLookAt(camPos,camTarget, new Vector3(0.0f, 1.0f, 0.0f));
            //base.ViewMatrix =  Matrix.CreateLookAt(lookFrom,position, Vector3.Up); //make a new View Matrix based on the camera's new position
            game.Window.Title = "X:" + position.X + ", Y:" + position.Y + ", Z:" + position.Z;
            lastMouseState = mouseState;
            base.Update(gameTime);
        }
    }
}
