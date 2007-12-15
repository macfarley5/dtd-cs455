using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.IO;

namespace TD3d
{
    class HUD
    {
        GameWindow Window;
        SpriteBatch spriteBatch;
        Texture2D tophud, righthud, zero, one, two, three, four, five, 
                  six, seven, eight, nine, slash, empty, gameover;

        public HUD(GameWindow Window, GraphicsDeviceManager graphics, ContentManager content) 
        {
            this.Window = Window;
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            tophud = content.Load<Texture2D>("Content/HUD/tophud");
            righthud = content.Load<Texture2D>("Content/HUD/righthud");
            zero = content.Load<Texture2D>("Content/HUD/zero");
            one = content.Load<Texture2D>("Content/HUD/one");
            two = content.Load<Texture2D>("Content/HUD/two");
            three = content.Load<Texture2D>("Content/HUD/three");
            four = content.Load<Texture2D>("Content/HUD/four");
            five = content.Load<Texture2D>("Content/HUD/five");
            six = content.Load<Texture2D>("Content/HUD/six");
            seven = content.Load<Texture2D>("Content/HUD/seven");
            eight = content.Load<Texture2D>("Content/HUD/eight");
            nine = content.Load<Texture2D>("Content/HUD/nine");
            slash = content.Load<Texture2D>("Content/HUD/slash");
            empty = content.Load<Texture2D>("Content/HUD/empty");
            gameover = content.Load<Texture2D>("Content/HUD/gameover");
        }

        public void Draw(int score)
        {            
            Texture2D tensImage = empty;
            Texture2D onesImage = empty;
            int ones = 0;
            int xModifier;

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);

            if(score == 0)
                spriteBatch.Draw(gameover, new Rectangle((Window.ClientBounds.Width - gameover.Width) / 2, (Window.ClientBounds.Height - gameover.Height) / 2, gameover.Width, gameover.Height), Color.White);

            while (score % 10 != 0)
            {
                score--;
                ones++;
            }

            switch (score)
            {
                case 0: tensImage = empty; break;
                case 10: tensImage = one; break;
                case 20: tensImage = two; break;
            }

            switch (ones)
            {
                case 0: onesImage = zero; break;
                case 1: onesImage = one; break;
                case 2: onesImage = two; break;
                case 3: onesImage = three; break;
                case 4: onesImage = four; break;
                case 5: onesImage = five; break;
                case 6: onesImage = six; break;
                case 7: onesImage = seven; break;
                case 8: onesImage = eight; break;
                case 9: onesImage = nine; break;
            }

            if (score < 10)
            {
                xModifier = -15;
            }
            else xModifier = 0;
                        
            spriteBatch.Draw(tophud, new Rectangle((Window.ClientBounds.Width - tophud.Width) / 2, 0, tophud.Width, tophud.Height), Color.White);
            spriteBatch.Draw(righthud, new Rectangle((Window.ClientBounds.Width - righthud.Width), 40, righthud.Width, righthud.Height), Color.White);
            spriteBatch.Draw(tensImage, new Rectangle((Window.ClientBounds.Width - (5 * tensImage.Width)) / 2, 25, tensImage.Width, tensImage.Height), Color.White);
            spriteBatch.Draw(onesImage, new Rectangle((Window.ClientBounds.Width - (3 * onesImage.Width - 10)) / 2 + xModifier, 25, onesImage.Width, onesImage.Height), Color.White);
            spriteBatch.Draw(slash, new Rectangle((Window.ClientBounds.Width - (slash.Width)) / 2 + xModifier, 25, slash.Width, slash.Height), Color.White);
            spriteBatch.Draw(two, new Rectangle((Window.ClientBounds.Width + (1 * two.Width)) / 2 + xModifier, 25, two.Width, two.Height), Color.White);
            spriteBatch.Draw(zero, new Rectangle((Window.ClientBounds.Width + (3 * zero.Width)) / 2 + xModifier, 25, zero.Width, zero.Height), Color.White);
            spriteBatch.End();
        }
    }
}
