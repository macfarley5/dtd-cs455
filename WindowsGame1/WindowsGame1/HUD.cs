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
                  six, seven, eight, nine, slash, empty, gameover, dollar,
                  upgrade, towerdata, scout, hover, upgradehover, upgradefade,
                  smallzero, smallone, smalltwo, smallthree, smallfour,
                  smallfive, smallsix, smallseven, smalleight, smallnine,
                  cash, upgradecost, smalldollar;
        Tower selectedTower;

        public HUD(GameWindow Window, GraphicsDeviceManager graphics, ContentManager content) 
        {
            this.Window = Window;

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            tophud = content.Load<Texture2D>("Content/HUD/tophud");
            righthud = content.Load<Texture2D>("Content/HUD/righthud");
            gameover = content.Load<Texture2D>("Content/HUD/gameover");
            upgrade = content.Load<Texture2D>("Content/HUD/upgrade");
            towerdata = content.Load<Texture2D>("Content/HUD/towerdata");
            scout = content.Load<Texture2D>("Content/HUD/scout");
            hover = content.Load<Texture2D>("Content/HUD/hover");
            upgradehover = content.Load<Texture2D>("Content/HUD/upgradehover");
            upgradefade = content.Load<Texture2D>("Content/HUD/upgradefade");
            cash = content.Load<Texture2D>("Content/HUD/cash");
            upgradecost = content.Load<Texture2D>("Content/HUD/upgradecost");

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
            dollar = content.Load<Texture2D>("Content/HUD/dollar");
            
            smallzero = content.Load<Texture2D>("Content/HUD/smallzero");
            smallone = content.Load<Texture2D>("Content/HUD/smallone");
            smalltwo = content.Load<Texture2D>("Content/HUD/smalltwo");
            smallthree = content.Load<Texture2D>("Content/HUD/smallthree");
            smallfour = content.Load<Texture2D>("Content/HUD/smallfour");
            smallfive = content.Load<Texture2D>("Content/HUD/smallfive");
            smallsix = content.Load<Texture2D>("Content/HUD/smallsix");
            smallseven = content.Load<Texture2D>("Content/HUD/smallseven");
            smalleight = content.Load<Texture2D>("Content/HUD/smalleight");
            smallnine = content.Load<Texture2D>("Content/HUD/smallnine");
            smalldollar = content.Load<Texture2D>("Content/HUD/smalldollar");

            selectedTower = null;
        }

        public void setSelectedTower(Tower selected)
        {
            selectedTower = selected;
        }

        public void Draw(int score, long cash, int mouseXPos, int mouseYPos)
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

            onesImage = getNumberImg(ones, false);
            tensImage = getNumberImg(score / 10, false);

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

            drawNumber(cash, Window.ClientBounds.Width - 5, 75, true);
                        
            drawTowerInfo(mouseXPos, mouseYPos, cash);
            
            spriteBatch.End();
        }

        private void drawTowerInfo(int mouseXPos, int mouseYPos, long cash)
        {
            if (selectedTower == null)
                return;

            drawNumber((selectedTower.getCost() * selectedTower.getLevel()), Window.ClientBounds.Width - 5, 95, true);

            if(selectedTower.getTileType() == Tile.TileType.NORMALTOWER)
                spriteBatch.Draw(scout, new Rectangle((Window.ClientBounds.Width - scout.Width - 30), (235), scout.Width, scout.Height), Color.White);
            else if(selectedTower.getTileType() == Tile.TileType.FASTTOWER)
                spriteBatch.Draw(hover, new Rectangle((Window.ClientBounds.Width - hover.Width - 30), (235), hover.Width, hover.Height), Color.White);

            spriteBatch.Draw(towerdata, new Rectangle((Window.ClientBounds.Width - towerdata.Width - 90), (265), towerdata.Width, towerdata.Height), Color.White);

            drawNumber(selectedTower.getLevel(), Window.ClientBounds.Width - 10, 277, false);
            drawNumber(selectedTower.getRange(), Window.ClientBounds.Width - 10, 298, false);
            drawNumber((long)((1.0f / selectedTower.getFireSpeed()) * 10000), Window.ClientBounds.Width - 10, 319, false);
            drawNumber(selectedTower.getAngle(), Window.ClientBounds.Width - 10, 340, false);

            if(selectedTower.getTarget() != null)
                drawNumber(selectedTower.getTarget().getHealth(), Window.ClientBounds.Width - 10, 360, false);
            else drawNumber(0, Window.ClientBounds.Width - 10, 360, false);

            if(selectedTower.getLevel() >= 5 || cash < (selectedTower.getCost() * selectedTower.getLevel()))
                spriteBatch.Draw(upgradefade, new Rectangle((Window.ClientBounds.Width - upgrade.Width - 45), (375), upgrade.Width, upgrade.Height), Color.White);
            else if(mouseXPos > (Window.ClientBounds.Width - upgrade.Width - 45) && 
               mouseXPos < (Window.ClientBounds.Width - 45) &&
               mouseYPos > 375 &&
               mouseYPos < (375 + upgrade.Height))
                spriteBatch.Draw(upgradehover, new Rectangle((Window.ClientBounds.Width - upgrade.Width - 45), (375), upgrade.Width, upgrade.Height), Color.White);
            else spriteBatch.Draw(upgrade, new Rectangle((Window.ClientBounds.Width - upgrade.Width - 45), (375), upgrade.Width, upgrade.Height), Color.White);
        }

        private void drawNumber(long number, int xPos, int yPos, bool dollars) // xPos and YPos are bottom right corner of number, numbers must be >= 0 and <= 999,999
        {
            int ones, tens, hundreds, thousands, tenT, hundredT, places;
            ones = tens = hundreds = thousands = tenT = hundredT = 0;
            Texture2D onesImg, tensImg, hundredsImg, thousandsImg, tenTImg, hundredTImg;
            
            number += 1000000;

            while (number % 10 != 0)
            {
                ones++;
                number--;
            }

            while (number % 100 != 0)
            {
                tens++;
                number -= 10;
            }

            while (number % 1000 != 0)
            {
                hundreds++;
                number -= 100;
            }

            while (number % 10000 != 0)
            {
                thousands++;
                number -= 1000;
            }

            while (number % 100000 != 0)
            {
                tenT++;
                number -= 10000;
            }

            while (number % 1000000 != 0)
            {
                hundredT++;
                number -= 100000;
            }

            places = 6;

            if (hundredT == 0)
            {                
                places--;

                if (tenT == 0)
                {
                    places--;

                    if (thousands == 0)
                    {
                        places--;

                        if (hundreds == 0)
                        {
                            places--;

                            if (tens == 0)
                                places--;
                        }
                    }
                }
            }

            System.Console.WriteLine(hundredT + "," + tenT + "," + thousands + "," + hundreds + "," + tens + "," + ones);

            onesImg = getNumberImg(ones, true);
            tensImg = getNumberImg(tens, true);
            hundredsImg = getNumberImg(hundreds, true);
            thousandsImg = getNumberImg(thousands, true);
            tenTImg = getNumberImg(tenT, true);
            hundredTImg = getNumberImg(hundredT, true);

            int xMod = 10;
                        
            spriteBatch.Draw(onesImg, new Rectangle((xPos - xMod), (yPos - onesImg.Height), onesImg.Width, onesImg.Height), Color.White);
            xMod += 10;

            if (places > 1)
            {
                spriteBatch.Draw(tensImg, new Rectangle((xPos - xMod), (yPos - onesImg.Height), tensImg.Width, tensImg.Height), Color.White);
                xMod += 10;
            }

            if (places > 2)
            {
                spriteBatch.Draw(hundredsImg, new Rectangle((xPos - xMod), (yPos - onesImg.Height), hundredsImg.Width, hundredsImg.Height), Color.White);
                xMod += 10;
            }

            if (places > 3)
            {
                spriteBatch.Draw(thousandsImg, new Rectangle((xPos - xMod), (yPos - onesImg.Height), thousandsImg.Width, thousandsImg.Height), Color.White);
                xMod += 10;
            }

            if (places > 4)
            {
                spriteBatch.Draw(tenTImg, new Rectangle((xPos - xMod), (yPos - onesImg.Height), tenTImg.Width, tenTImg.Height), Color.White);
                xMod += 10;
            }

            if (places > 5)
            {
                spriteBatch.Draw(hundredTImg, new Rectangle((xPos - xMod), (yPos - onesImg.Height), hundredTImg.Width, hundredTImg.Height), Color.White);
                xMod += 10;
            }

            if(dollars)
                spriteBatch.Draw(smalldollar, new Rectangle((xPos - xMod), (yPos - onesImg.Height), smalldollar.Width, smalldollar.Height), Color.White);

        }

        private Texture2D getNumberImg(int switchOn, bool small)
        {
            Texture2D img = null;

            if(small)
                switch (switchOn)
                {
                    case 0: img = smallzero; break;
                    case 1: img = smallone; break;
                    case 2: img = smalltwo; break;
                    case 3: img = smallthree; break;
                    case 4: img = smallfour; break;
                    case 5: img = smallfive; break;
                    case 6: img = smallsix; break;
                    case 7: img = smallseven; break;
                    case 8: img = smalleight; break;
                    case 9: img = smallnine; break;
                }
            else switch (switchOn)
                {
                    case 0: img = zero; break;
                    case 1: img = one; break;
                    case 2: img = two; break;
                    case 3: img = three; break;
                    case 4: img = four; break;
                    case 5: img = five; break;
                    case 6: img = six; break;
                    case 7: img = seven; break;
                    case 8: img = eight; break;
                    case 9: img = nine; break;
                }

            return img;
        }
    }
}
