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
    class MouseX
    {
        GameWindow Window;
        int WIDTH, HEIGHT;
        Position mousePos = null;
        GraphicsDeviceManager graphics;
        ContentManager content;
        GraphicsDevice device;
        Map map;
        long cash;
        Tower selectedTower;
        bool pressButton = false;
        bool pressedBoard = false;
        Tile.TileType towerType = Tile.TileType.NORMALTOWER;
        float cameraDist;
        long previousWheel;

        public MouseX(GameWindow Window, int WIDTH, int HEIGHT, GraphicsDeviceManager graphics, 
                      ContentManager content, GraphicsDevice device, Map map)
        {
            this.Window = Window;
            this.WIDTH = WIDTH;
            this.HEIGHT = HEIGHT;
            this.graphics = graphics;
            this.content = content;
            this.device = device;
            this.map = map;
            selectedTower = null;
            previousWheel = 0;
            this.cameraDist = 30;
        }

        public ArrayList update(Vector3 cameraRot, float cameraDist, PathPlanner planner, ArrayList thePath, ArrayList creeps, long cash)
        {
            this.cash = cash;
            MouseState ms = Mouse.GetState();

            int dWheel = (int)(ms.ScrollWheelValue - previousWheel);
            this.cameraDist -= dWheel / 50;
            previousWheel = ms.ScrollWheelValue;

            if (this.cameraDist < 5)
                this.cameraDist = 5;
            else if (this.cameraDist > 100)
                this.cameraDist = 100;

            if (ms.X > (Window.ClientBounds.Width - 231) &&
               ms.Y > 40 &&
               ms.Y < (40 + 385)) // if the pointer is within the bounds of the right HUD
            {
                if (ms.X > (Window.ClientBounds.Width - 113 - 45) &&
                    ms.X < (Window.ClientBounds.Width - 45) &&
                    ms.Y > 375 &&
                    ms.Y < (375 + 30) && 
                    selectedTower != null && 
                    this.cash >= selectedTower.getCost() * selectedTower.getLevel()) // if the pointer is on the upgrade button
                {
                    if (ms.LeftButton == ButtonState.Pressed) // clicked on the upgrade button
                    {
                        pressButton = true;
                    }
                    else if (ms.LeftButton == ButtonState.Released && pressButton)
                    {
                        this.cash -= selectedTower.getCost() * selectedTower.getLevel();
                        selectedTower.incrementLevel();                        
                        pressButton = false;
                    }
                }
                else pressButton = false;
                
                return thePath;
            }

            pressButton = false;

            if (ms.X < 0 || ms.X > this.Window.ClientBounds.Width || 
                ms.Y < 0 || ms.Y > this.Window.ClientBounds.Height)
                return thePath;
            
            float fromX = (float)Math.Cos(cameraRot.X) * 
                          (float)Math.Cos(cameraRot.Z) * 
                          this.cameraDist + this.WIDTH / 2;
            float fromY = (float)Math.Sin(cameraRot.X) * 
                          (float)Math.Cos(cameraRot.Z) * 
                          this.cameraDist + this.HEIGHT / 2;
            float fromZ = (float)Math.Sin(cameraRot.Z) * this.cameraDist;

            Vector3 LookFrom = new Vector3(fromX, fromY, fromZ);
            Vector3 LookAt = new Vector3(this.WIDTH / 2, this.HEIGHT / 2, 0);            
            Vector3 ViewUp = new Vector3(0, 0, 1);
            
            Vector3 W = LookFrom - LookAt;
            W.Normalize();
            Vector3 U = Vector3.Cross(ViewUp, W);
            U.Normalize();
            Vector3 V = Vector3.Cross(W, U);
            V.Normalize();

            float vMax = (float)(((Vector3)(LookAt - LookFrom)).Length() * 
                                 Math.Tan(MathHelper.PiOver4 / 2));
            float vMin = -vMax;
            float uMax = vMax * (float)this.Window.ClientBounds.Width / 
                                (float)this.Window.ClientBounds.Height;
            float uMin = -uMax;
            
            float sViewX = ms.X * ((uMax - uMin) / (float)this.Window.ClientBounds.Width) + uMin;
            float sViewY = (this.Window.ClientBounds.Height - ms.Y) * ((vMax - vMin) / 
                            this.Window.ClientBounds.Height) + vMin;
            Vector3 sView = new Vector3(sViewX, sViewY, 0);

            Vector3 sWorld = LookAt + sView.X * U + sView.Y * V + sView.Z * W;
            Vector3 rayO = LookFrom;
            Vector3 rayD = sWorld - LookFrom;
            rayD.Normalize();            
            float t = intersectBoard(rayO, rayD);
            
            if (t < 1e7)
            {
                Vector3 iPoint = rayO + t * rayD;
                int xPos = (int)(iPoint.X - .5f);
                int yPos = (int)(iPoint.Y - .5f);
                
                if (xPos < WIDTH && xPos >= 0 && yPos < HEIGHT && yPos >= 0)
                {
                    mousePos = new Position(xPos, yPos);

                    if (ms.LeftButton == ButtonState.Pressed)
                    {
                        pressedBoard = true;
                    }
                    else if (ms.LeftButton == ButtonState.Released && pressedBoard)
                    {
                        pressedBoard = false;
                        Tower tow = null;
                        if (this.towerType == Tile.TileType.NORMALTOWER)
                        {
                            tow = new NormalTower(graphics, content, device, creeps);
                        }
                        else if (this.towerType == Tile.TileType.FASTTOWER)
                        {
                            tow = new FastTower(graphics, content, device, creeps);
                        }
                        else if (this.towerType == Tile.TileType.SPLASHTOWER)
                        {
                            tow = new SplashTower(graphics, content, device, creeps);
                        }
                        tow.setPosition(xPos, yPos);

                        if (!this.map.isOccupied(xPos, yPos) && this.map.canPlaceTower(tow))
                            this.cash = this.cash - tow.getCost();
                        else
                        {
                            Tower tmpTower = this.map.getTower((int)(iPoint.X), (int)(iPoint.Y));

                            if (tmpTower != null)
                                selectedTower = tmpTower;
                            return thePath;
                        }

                        this.map.placeTower(tow);
                        
                        if (planner.isPath())
                        {
                            ArrayList tempPath = planner.getPath();
                            bool anyBlocked = false;
                            
                            if (this.cash < 0)
                                anyBlocked = true;

                            foreach (Creep creep in creeps)
                            {
                                if (!creep.hasPath())
                                {
                                    anyBlocked = true;
                                }
                                //creep.setPath(thePath);
                            }
                            if (anyBlocked)
                            {
                                this.map.removeTower(tow);
                                this.cash = this.cash + tow.getCost();
                            }
                            else
                            {
                                thePath = tempPath;
                                selectedTower = this.map.getTower((int)(iPoint.X), (int)(iPoint.Y));

                                foreach (Creep creep in creeps)
                                {
                                    creep.setNewPath();
                                }
                            }
                        }
                        else
                        {
                            this.map.removeTower(tow);
                            this.cash = this.cash + tow.getCost();
                        }
                    }
                }
                else mousePos = null;
            }
            else mousePos = null;

            return thePath;
        }

        private float intersectBoard(Vector3 rayO, Vector3 rayD)
        {
            Vector3 pN = new Vector3(0, 0, 1);
            float normalDotRay = Vector3.Dot(pN, rayD);
            if (normalDotRay == 0) return 1e8f;
            float d = -(pN.X + pN.Y);
            float t = -(Vector3.Dot(pN, rayO) + d) / Vector3.Dot(pN, rayD);
            if (t <= 0) return 1e8f;
            return t;
        }

        public Tile.TileType getTowerType()
        {
            return this.towerType;
        }

        public void setTowerType(int towNum)
        {
            switch (towNum)
            {
                case 0:
                    this.towerType = Tile.TileType.NORMALTOWER;
                    break;
                case 1:
                    this.towerType = Tile.TileType.FASTTOWER;
                    break;
                case 2:
                    this.towerType = Tile.TileType.SPLASHTOWER;
                    break;
            }
        }

        public Position getPos()
        {
            return mousePos;
        }

        public int getScreenXPos()
        {
            MouseState ms = Mouse.GetState();
            return ms.X;
        }

        public int getScreenYPos()
        {
            MouseState ms = Mouse.GetState();
            return ms.Y;
        }

        public long getCash()
        {
            return cash;
        }

        public Tower getSelectedTower()
        {
            return selectedTower;
        }

        public float getCameraDist()
        {
            return this.cameraDist;
        }

        internal void setCameraDist(float p)
        {
            this.cameraDist = p;
        }
    }
}
