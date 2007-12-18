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
                  upgrade, towerdata, scout, hover, upgradehover,
                  smallzero, smallone, smalltwo, smallthree, smallfour,
                  smallfive, smallsix, smallseven, smalleight, smallnine;
        Tower selectedTower;

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
            dollar = content.Load<Texture2D>("Content/HUD/dollar");
            upgrade = content.Load<Texture2D>("Content/HUD/upgrade");
            towerdata = content.Load<Texture2D>("Content/HUD/towerdata");
            scout = content.Load<Texture2D>("Content/HUD/scout");
            hover = content.Load<Texture2D>("Content/HUD/hover");
            upgradehover = content.Load<Texture2D>("Content/HUD/upgradehover");

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
            Texture2D cash1img = empty;
            Texture2D cash10img = empty;
            Texture2D cash100img = empty;
            Texture2D cash1000img = empty;
            Texture2D cash10000img = empty;
            int ones = 0;
            int xModifier;
            int cash1, cash10, cash100, cash1000, cash10000;
            cash1 = cash10 = cash100 = cash1000 = cash10000 = 0;
            cash += 1000000;

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

            while (cash % 10 != 0)
            {
                cash--;
                cash1++;
            }

            while (cash % 100 != 0)
            {
                cash = cash - 10;
                cash10++;
            }

            while (cash % 1000 != 0)
            {
                cash = cash - 100;
                cash100++;
            }

            while (cash % 10000 != 0)
            {
                cash = cash - 1000;
                cash1000++; 
            }

            while (cash % 100000 != 0)
            {
                cash = cash - 10000;
                cash10000++;
            }

            switch(cash1)
            {
                case 0: cash1img = zero; break;
                case 1: cash1img = one; break;
                case 2: cash1img = two; break;
                case 3: cash1img = three; break;
                case 4: cash1img = four; break;
                case 5: cash1img = five; break;
                case 6: cash1img = six; break;
                case 7: cash1img = seven; break;
                case 8: cash1img = eight; break;
                case 9: cash1img = nine; break;
            }

            switch(cash10)
            {
                case 0:
                    if(cash10000 == 0 && cash1000 == 0 && cash100 == 0)
                        cash10img = dollar;
                    else cash10img = zero; break;
                case 1: cash10img = one; break;
                case 2: cash10img = two; break;
                case 3: cash10img = three; break;
                case 4: cash10img = four; break;
                case 5: cash10img = five; break;
                case 6: cash10img = six; break;
                case 7: cash10img = seven; break;
                case 8: cash10img = eight; break;
                case 9: cash10img = nine; break;
            }

            switch(cash100)
            {
                case 0: 
                    if(cash10 == 0 && cash1000 == 0 && cash10000 == 0)                        
                        cash100img = empty;
                    else if(cash10000 == 0 && cash1000 == 0)
                        cash100img = dollar; 
                    else cash100img = zero; break;
                case 1: cash100img = one; break;
                case 2: cash100img = two; break;
                case 3: cash100img = three; break;
                case 4: cash100img = four; break;
                case 5: cash100img = five; break;
                case 6: cash100img = six; break;
                case 7: cash100img = seven; break;
                case 8: cash100img = eight; break;
                case 9: cash100img = nine; break;
            }

            switch(cash1000)
            {
                case 0: 
                    if(cash100 == 0 && cash10000 == 0)
                         cash1000img = empty;
                    else if(cash10000 == 0)
                        cash1000img = dollar; 
                    else cash1000img = zero; break;
                case 1: cash1000img = one; break;
                case 2: cash1000img = two; break;
                case 3: cash1000img = three; break;
                case 4: cash1000img = four; break;
                case 5: cash1000img = five; break;
                case 6: cash1000img = six; break;
                case 7: cash1000img = seven; break;
                case 8: cash1000img = eight; break;
                case 9: cash1000img = nine; break;
            }

            switch(cash10000)
            {
                case 0:
                    if(cash1000 == 0)
                        cash10000img = empty;
                    else cash10000img = dollar; break;
                case 1: cash10000img = one; break;
                case 2: cash10000img = two; break;
                case 3: cash10000img = three; break;
                case 4: cash10000img = four; break;
                case 5: cash10000img = five; break;
                case 6: cash10000img = six; break;
                case 7: cash10000img = seven; break;
                case 8: cash10000img = eight; break;
                case 9: cash10000img = nine; break;
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

            spriteBatch.Draw(cash1img, new Rectangle((Window.ClientBounds.Width - cash1img.Width - 5), (55), cash1img.Width, cash1img.Height), Color.White);
            spriteBatch.Draw(cash10img, new Rectangle((Window.ClientBounds.Width - (cash10img.Width + cash1img.Width) - 10), (55), cash10img.Width, cash10img.Height), Color.White);
            spriteBatch.Draw(cash100img, new Rectangle((Window.ClientBounds.Width - (cash100img.Width + cash10img.Width + cash1img.Width) - 15), (55), cash100img.Width, cash100img.Height), Color.White);
            spriteBatch.Draw(cash1000img, new Rectangle((Window.ClientBounds.Width - (cash1000img.Width + cash100img.Width + cash10img.Width + cash1img.Width) - 20), (55), cash1000img.Width, cash1000img.Height), Color.White);
            spriteBatch.Draw(cash10000img, new Rectangle((Window.ClientBounds.Width - (cash10000img.Width + cash1000img.Width + cash100img.Width + cash10img.Width + cash1img.Width) - 25), (55), cash10000img.Width, cash10000img.Height), Color.White);

            if(cash10000 != 0)
                spriteBatch.Draw(dollar, new Rectangle((Window.ClientBounds.Width - (dollar.Width + cash10000img.Width + cash1000img.Width + cash100img.Width + cash10img.Width + cash1img.Width) - 30), (55), dollar.Width, dollar.Height), Color.White);

            drawTowerInfo(mouseXPos, mouseYPos);
            
            spriteBatch.End();
        }

        private void drawTowerInfo(int mouseXPos, int mouseYPos)
        {
            if (selectedTower == null)
                return;

            if(selectedTower.getTileType() == Tile.TileType.NORMALTOWER)
                spriteBatch.Draw(scout, new Rectangle((Window.ClientBounds.Width - scout.Width - 30), (235), scout.Width, scout.Height), Color.White);
            else if(selectedTower.getTileType() == Tile.TileType.FASTTOWER)
                spriteBatch.Draw(hover, new Rectangle((Window.ClientBounds.Width - hover.Width - 30), (235), hover.Width, hover.Height), Color.White);

            spriteBatch.Draw(towerdata, new Rectangle((Window.ClientBounds.Width - towerdata.Width - 90), (265), towerdata.Width, towerdata.Height), Color.White);

            drawNumber(selectedTower.getLevel(), Window.ClientBounds.Width - 10, 277);
            drawNumber(selectedTower.getRange(), Window.ClientBounds.Width - 10, 298);
            drawNumber((long)((1.0f / selectedTower.getFireSpeed()) * 10000), Window.ClientBounds.Width - 10, 319);
            drawNumber(selectedTower.getAngle(), Window.ClientBounds.Width - 10, 340);

            if(selectedTower.getTarget() != null)
                drawNumber(selectedTower.getTarget().getHealth(), Window.ClientBounds.Width - 10, 360);
            else drawNumber(0, Window.ClientBounds.Width - 10, 360);

            if(mouseXPos > (Window.ClientBounds.Width - upgrade.Width - 45) && 
               mouseXPos < (Window.ClientBounds.Width - 45) &&
               mouseYPos > 375 &&
               mouseYPos < (375 + upgrade.Height))
                spriteBatch.Draw(upgradehover, new Rectangle((Window.ClientBounds.Width - upgrade.Width - 45), (375), upgrade.Width, upgrade.Height), Color.White);
            else spriteBatch.Draw(upgrade, new Rectangle((Window.ClientBounds.Width - upgrade.Width - 45), (375), upgrade.Width, upgrade.Height), Color.White);
        }

        private void drawNumber(long number, int xPos, int yPos) // xPos and YPos are bottom right corner of number, numbers must be >= 0 and <= 999,999
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

            if(places > 1)
                spriteBatch.Draw(tensImg, new Rectangle((xPos - xMod), (yPos - onesImg.Height), tensImg.Width, tensImg.Height), Color.White);
            xMod += 10;

            if(places > 2)
                spriteBatch.Draw(hundredsImg, new Rectangle((xPos - xMod), (yPos - onesImg.Height), hundredsImg.Width, hundredsImg.Height), Color.White);
            xMod += 10;

            if(places > 3)
                spriteBatch.Draw(thousandsImg, new Rectangle((xPos - xMod), (yPos - onesImg.Height), thousandsImg.Width, thousandsImg.Height), Color.White);
            xMod += 10;

            if(places > 4)
                spriteBatch.Draw(tenTImg, new Rectangle((xPos - xMod), (yPos - onesImg.Height), tenTImg.Width, tenTImg.Height), Color.White);
            xMod += 10;

            if(places > 5)
                spriteBatch.Draw(hundredTImg, new Rectangle((xPos - xMod), (yPos - onesImg.Height), hundredTImg.Width, hundredTImg.Height), Color.White);
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