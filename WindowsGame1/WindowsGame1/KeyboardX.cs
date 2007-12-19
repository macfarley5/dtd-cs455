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
    class KeyboardX
    {
        int WIDTH;
        Vector3 cameraRot;
        float cameraDist;
        int towerNum = 0;

        public KeyboardX(int WIDTH)
        {
            this.WIDTH = WIDTH;
        }

        public void update(Vector3 cameraRot, float cameraDist, float speed)
        {
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Right))
                cameraRot.X += .05f;
            if (keys.IsKeyDown(Keys.Left))
                cameraRot.X -= .05f;
            if (keys.IsKeyDown(Keys.Up))
            {
                cameraRot.Z += .02f;

                if (cameraRot.Z > 1.5f)
                    cameraRot.Z = 1.5f;
            }
            if (keys.IsKeyDown(Keys.Down))
            {
                cameraRot.Z -= .02f;

                if (cameraRot.Z < 0.2f)
                    cameraRot.Z = 0.2f;
            }
            if (keys.IsKeyDown(Keys.S))
            {
                cameraDist += .5f;

                if (cameraDist > this.WIDTH * 3)
                    cameraDist = this.WIDTH * 3;
            }
            if (keys.IsKeyDown(Keys.W))
            {
                cameraDist -= .5f;

                if (cameraDist < 5.25f)
                    cameraDist = 5.25f;
            }

            if (keys.IsKeyDown(Keys.D1))
            {
                this.towerNum = 0;
            }
            if (keys.IsKeyDown(Keys.D2))
            {
                this.towerNum = 1;
            }
            if (keys.IsKeyDown(Keys.D3))
            {
                this.towerNum = 2;
            }

            this.cameraRot = cameraRot;
            this.cameraDist = cameraDist;
        }

        public Vector3 getCameraRot()
        {
            return this.cameraRot;
        }

        public float getCameraDist()
        {
            return this.cameraDist;
        }

        public int getTowerNum()
        {
            return this.towerNum;
        }
    }
}
