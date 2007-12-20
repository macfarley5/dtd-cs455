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
                  upgrade, towerdata, scout, hover, gatling, upgradehover, upgradefade,
                  smallzero, smallone, smalltwo, smallthree, smallfour,
                  smallfive, smallsix, smallseven, smalleight, smallnine,
                  cashamount, upgradecost, smalldollar, level, notapplicable,
                  smallzerored, smallonered, smalltwored, smallthreered, smallfourred,
                  smallfivered, smallsixred, smallsevenred, smalleightred, smallninered,
                  smalldollarred;
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
            gatling = content.Load<Texture2D>("Content/HUD/gatling");
            upgradehover = content.Load<Texture2D>("Content/HUD/upgradehover");
            upgradefade = content.Load<Texture2D>("Content/HUD/upgradefade");
            cashamount = content.Load<Texture2D>("Content/HUD/cash");
            upgradecost = content.Load<Texture2D>("Content/HUD/upgradecost");
            level = content.Load<Texture2D>("Content/HUD/level");
            notapplicable = content.Load<Texture2D>("Content/HUD/notapplicable");

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

            smallzerored = content.Load<Texture2D>("Content/HUD/smallzerored");
            smallonered = content.Load<Texture2D>("Content/HUD/smallonered");
            smalltwored = content.Load<Texture2D>("Content/HUD/smalltwored");
            smallthreered = content.Load<Texture2D>("Content/HUD/smallthreered");
            smallfourred = content.Load<Texture2D>("Content/HUD/smallfourred");
            smallfivered = content.Load<Texture2D>("Content/HUD/smallfivered");
            smallsixred = content.Load<Texture2D>("Content/HUD/smallsixred");
            smallsevenred = content.Load<Texture2D>("Content/HUD/smallsevenred");
            smalleightred = content.Load<Texture2D>("Content/HUD/smalleightred");
            smallninered = content.Load<Texture2D>("Content/HUD/smallninered");
            smalldollarred = content.Load<Texture2D>("Content/HUD/smalldollarred");

            selectedTower = null;
        }

        public void setSelectedTower(Tower selected)
        {
            selectedTower = selected;
        }

        public void Draw(int score, long cash, int round, int mouseXPos, int mouseYPos, int towerNum)
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

            onesImage = getNumberImg(ones, false, false);
            tensImage = getNumberImg(score / 10, false, false);

            if (score < 10)
            {
                xModifier = -15;
            }
            else xModifier = 0;

            int levelMod = 0;

            if (round < 10)
                levelMod = -10;

            int towerValue;

            if (towerNum == 0)
                towerValue = 30;
            else if (towerNum == 1)
                towerValue = 200;
            else towerValue = 80;

            if (!(mouseXPos > (Window.ClientBounds.Width - 231) &&
               mouseYPos > 40 &&
               mouseYPos < (40 + 385)))
            {
                if(cash < towerValue)
                    drawNumber(towerValue, mouseXPos + 35, mouseYPos + 35, true, false);
                else
                    drawNumber(towerValue, mouseXPos + 35, mouseYPos + 35, true, true); // Draw dollar number by mouse pointer
            }

            spriteBatch.Draw(tophud, new Rectangle((Window.ClientBounds.Width - tophud.Width) / 2, 0, tophud.Width, tophud.Height), Color.White);
            spriteBatch.Draw(righthud, new Rectangle((Window.ClientBounds.Width - righthud.Width), 40, righthud.Width, righthud.Height), Color.White);
            spriteBatch.Draw(level, new Rectangle((Window.ClientBounds.Width - level.Width)/2 - 15, 10, level.Width, level.Height), Color.White);
            drawNumber(round, (Window.ClientBounds.Width)/2 + 70 + levelMod, 22, false, false);
            spriteBatch.Draw(cashamount, new Rectangle((Window.ClientBounds.Width - towerdata.Width - 90), 63, cashamount.Width, cashamount.Height), Color.White);

            if(score > 0)
                spriteBatch.Draw(tensImage, new Rectangle((Window.ClientBounds.Width - (5 * tensImage.Width)) / 2, 35, tensImage.Width, tensImage.Height), Color.White);

            spriteBatch.Draw(onesImage, new Rectangle((Window.ClientBounds.Width - (3 * onesImage.Width - 10)) / 2 + xModifier, 35, onesImage.Width, onesImage.Height), Color.White);
            spriteBatch.Draw(slash, new Rectangle((Window.ClientBounds.Width - (slash.Width)) / 2 + xModifier, 35, slash.Width, slash.Height), Color.White);
            spriteBatch.Draw(two, new Rectangle((Window.ClientBounds.Width + (1 * two.Width)) / 2 + xModifier, 35, two.Width, two.Height), Color.White);
            spriteBatch.Draw(zero, new Rectangle((Window.ClientBounds.Width + (3 * zero.Width)) / 2 + xModifier, 35, zero.Width, zero.Height), Color.White);

            drawNumber(cash, Window.ClientBounds.Width - 5, 75, true, false);
                        
            drawTowerInfo(mouseXPos, mouseYPos, cash);
            
            spriteBatch.End();
        }

        private void drawTowerInfo(int mouseXPos, int mouseYPos, long cash)
        {
            if (selectedTower == null)
                return;

            spriteBatch.Draw(upgradecost, new Rectangle((Window.ClientBounds.Width - towerdata.Width - 90), 83, upgradecost.Width, upgradecost.Height), Color.White);

            if(selectedTower.getLevel() < 5)
                drawNumber((selectedTower.getCost() * selectedTower.getLevel()), Window.ClientBounds.Width - 5, 95, true, false);
            else
                spriteBatch.Draw(notapplicable, new Rectangle((Window.ClientBounds.Width - notapplicable.Width - 5), (83), notapplicable.Width, notapplicable.Height), Color.White);

            if(selectedTower.getTileType() == Tile.TileType.NORMALTOWER)
                spriteBatch.Draw(scout, new Rectangle((Window.ClientBounds.Width - scout.Width - 30), (235), scout.Width, scout.Height), Color.White);
            else if(selectedTower.getTileType() == Tile.TileType.FASTTOWER)
                spriteBatch.Draw(gatling, new Rectangle((Window.ClientBounds.Width - gatling.Width - 10), (235), gatling.Width, gatling.Height), Color.White);
            else if(selectedTower.getTileType() == Tile.TileType.SPLASHTOWER)
                spriteBatch.Draw(hover, new Rectangle((Window.ClientBounds.Width - hover.Width - 30), (235), hover.Width, hover.Height), Color.White);

            spriteBatch.Draw(towerdata, new Rectangle((Window.ClientBounds.Width - towerdata.Width - 90), (265), towerdata.Width, towerdata.Height), Color.White);

            drawNumber(selectedTower.getLevel(), Window.ClientBounds.Width - 10, 277, false, false);
            drawNumber(selectedTower.getRange(), Window.ClientBounds.Width - 10, 298, false, false);
            drawNumber((long)((1.0f / selectedTower.getFireSpeed()) * 10000), Window.ClientBounds.Width - 10, 319, false, false);
            drawNumber(selectedTower.getDamage(), Window.ClientBounds.Width - 10, 340, false, false);

            if(selectedTower.getTarget() != null)
                drawNumber(selectedTower.getTarget().getHealth(), Window.ClientBounds.Width - 10, 360, false, false);
            else drawNumber(0, Window.ClientBounds.Width - 10, 360, false, false);

            if(selectedTower.getLevel() >= 5 || cash < (selectedTower.getCost() * selectedTower.getLevel()))
                spriteBatch.Draw(upgradefade, new Rectangle((Window.ClientBounds.Width - upgrade.Width - 45), (375), upgrade.Width, upgrade.Height), Color.White);
            else if(mouseXPos > (Window.ClientBounds.Width - upgrade.Width - 45) && 
               mouseXPos < (Window.ClientBounds.Width - 45) &&
               mouseYPos > 375 &&
               mouseYPos < (375 + upgrade.Height))
                spriteBatch.Draw(upgradehover, new Rectangle((Window.ClientBounds.Width - upgrade.Width - 45), (375), upgrade.Width, upgrade.Height), Color.White);
            else spriteBatch.Draw(upgrade, new Rectangle((Window.ClientBounds.Width - upgrade.Width - 45), (375), upgrade.Width, upgrade.Height), Color.White);
        }

        private void drawNumber(long number, int xPos, int yPos, bool dollars, bool red) // xPos and YPos are bottom right corner of number, numbers must be >= 0 and <= 999,999
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

            //System.Console.WriteLine(hundredT + "," + tenT + "," + thousands + "," + hundreds + "," + tens + "," + ones);

            onesImg = getNumberImg(ones, true, red);
            tensImg = getNumberImg(tens, true, red);
            hundredsImg = getNumberImg(hundreds, true, red);
            thousandsImg = getNumberImg(thousands, true, red);
            tenTImg = getNumberImg(tenT, true, red);
            hundredTImg = getNumberImg(hundredT, true, red);

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

            if(dollars && !red)            
                spriteBatch.Draw(smalldollar, new Rectangle((xPos - xMod), (yPos - onesImg.Height), smalldollar.Width, smalldollar.Height), Color.White);
            else if(dollars && red)
                spriteBatch.Draw(smalldollarred, new Rectangle((xPos - xMod), (yPos - onesImg.Height), smalldollarred.Width, smalldollarred.Height), Color.White);

        }

        private Texture2D getNumberImg(int switchOn, bool small, bool red)
        {
            Texture2D img = null;

            if(small && !red)
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
            else if(small && red)
                switch (switchOn)
                {
                    case 0: img = smallzerored; break;
                    case 1: img = smallonered; break;
                    case 2: img = smalltwored; break;
                    case 3: img = smallthreered; break;
                    case 4: img = smallfourred; break;
                    case 5: img = smallfivered; break;
                    case 6: img = smallsixred; break;
                    case 7: img = smallsevenred; break;
                    case 8: img = smalleightred; break;
                    case 9: img = smallninered; break;
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
