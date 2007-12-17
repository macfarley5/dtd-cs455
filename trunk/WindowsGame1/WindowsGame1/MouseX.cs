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
        }

        public ArrayList update(Vector3 cameraRot, float cameraDist, PathPlanner planner, ArrayList thePath, ArrayList creeps, long cash)
        {
            this.cash = cash;
            MouseState ms = Mouse.GetState();
            
            if (ms.X < 0 || ms.X > this.Window.ClientBounds.Width || 
                ms.Y < 0 || ms.Y > this.Window.ClientBounds.Height)
                return thePath;
            
            float fromX = (float)Math.Cos(cameraRot.X) * 
                          (float)Math.Cos(cameraRot.Z) * 
                          cameraDist + this.WIDTH / 2;
            float fromY = (float)Math.Sin(cameraRot.X) * 
                          (float)Math.Cos(cameraRot.Z) * 
                          cameraDist + this.HEIGHT / 2;
            float fromZ = (float)Math.Sin(cameraRot.Z) * cameraDist;

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
                        FastTower tow = new FastTower(graphics, content, device,creeps);
                        tow.setPosition(xPos, yPos);

                        if (!this.map.isOccupied(xPos, yPos))
                            this.cash = this.cash - 80;
                        else
                        {
                            Tower tmpTower = this.map.getTower(xPos, yPos);

                            if (tmpTower != null)
                                selectedTower = tmpTower;
                        }

                        this.map.placeTower(tow);
                        
                        if (planner.isPath())
                        {
                            thePath = planner.getPath();
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
                                this.cash = this.cash + 80;
                            }
                            else
                            {
                                selectedTower = this.map.getTower(xPos, yPos);

                                foreach (Creep creep in creeps)
                                {
                                    creep.setNewPath();
                                }
                            }
                        }
                        else
                        {
                            this.map.removeTower(tow);
                            this.cash = this.cash + 80;
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

        public Position getPos()
        {
            return mousePos;
        }

        public long getCash()
        {
            return cash;
        }

        public Tower getSelectedTower()
        {
            return selectedTower;
        }
    }
}
