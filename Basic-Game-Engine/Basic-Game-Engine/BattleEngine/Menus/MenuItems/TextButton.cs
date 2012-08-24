using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BattleEngine.Menus.MenuItems
{
    class TextButton:Button
    {
        SpriteFont font;
        String label;
        Vector2 textOffset = Vector2.Zero;
        Vector2 centre;
        SpriteBatch spriteBatch;

        public TextButton(Game game, String buttonImageName, Vector2 position, String label)
            : base(game, buttonImageName, position)
        {
            this.label = label;
            
            

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            font = Game.Content.Load<SpriteFont>("Ariel");
            textOffset = Vector2.Divide(font.MeasureString(label), -2.0f); //negate it so it can be added the the centre
            centre = base.Position + Vector2.Divide(base.ButtonDimensions(), 2.0f);
        }

        public override void Draw(GameTime gameTime, Vector2 parentPos)
        {
            base.Draw(gameTime, parentPos);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, label, centre + parentPos + textOffset, Color.White);
            spriteBatch.End();
        }
    }
}
