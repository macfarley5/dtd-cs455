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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        struct targetstruct
        {
            public Vector3 position;
            public float radius;
        }

        /// <summary>
        /// ///////////////////////////// OUR VARIABLES //////////////////////////
        /// </summary>

        Map theMap;
        ArrayList creeps = new ArrayList();

        // OTHER VARS
        GraphicsDeviceManager graphics;
        ContentManager content;
        GraphicsDevice device;
        Effect effect, effect2;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        Texture2D scenerytexture;
        Position mousePos = null;

        int WIDTH = 50;
        int HEIGHT = 40;
        int differentbuildings = 5;
        private int[] buildingheights = new int[] { 0, 10, 1, 3, 2, 5 };
        Vector3 cameraposition = new Vector3(5, -2, 20);
        Quaternion spacemeshrotation = Quaternion.CreateFromYawPitchRoll(0,-.8f, 0f);
        float gamespeed = 1.0f;
        ArrayList targetlist = new ArrayList();
        Model targetmodel;
        ArrayList spritelist = new ArrayList();
        Texture2D bullettexture;

        Model skybox;
        Texture2D[] skyboxtextures;

        Vector3 cameraRot = new Vector3(0, 0, 1);
        float cameraDist = 5;

        VertexPositionNormalTexture[] verticesarray;
        ArrayList verticeslist = new ArrayList();
        Model spacemodel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);
        }

        protected override void Initialize()
        {
            base.Initialize();
            LoadFloorplan();
            SetUpVertices();
            this.IsMouseVisible = true;
            this.cameraDist = this.WIDTH;
        }

        private void LoadFloorplan()
        {
            this.theMap = new Map(WIDTH, HEIGHT);

             Random r = new Random(0);
            for (int i = 0; i < 30; i++)
            {
                Tower t = new Tower(graphics, content, device);
                t.setPosition(r.Next(WIDTH-1), r.Next(HEIGHT-1));
                this.theMap.placeTower(t);
            }

            for (int i = 0; i < 30; i++)
            {
                int x = r.Next(WIDTH - 1);
                int y = r.Next(HEIGHT - 1);
                if (this.theMap.layout[x, y] == null)
                {
                    int whichCreep = r.Next(2);
                    Creep c;
                    if (whichCreep==0)
                        c = new FastCreep(new Position(x, y), 0,graphics,content,device);
                    else
                        c = new NormalCreep(new Position(x, y), 0,graphics,content,device);
                    this.creeps.Add(c);
                }
            }
        }

        private void SetUpXNADevice()
        {
            device = graphics.GraphicsDevice;

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Tower Defense";
        }

        private void LoadEffect()
        {
            CompiledEffect compiledEffect = Effect.CompileEffectFromFile("@/../../../../Content/effects.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            effect = new Effect(graphics.GraphicsDevice, compiledEffect.GetEffectCode(), CompilerOptions.None, null);

            CompiledEffect compiledEffect2 = Effect.CompileEffectFromFile("@/../../../../Content/MetallicFlakes.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            effect2 = new Effect(graphics.GraphicsDevice, compiledEffect2.GetEffectCode(), CompilerOptions.None, null);
            effect2.Parameters["NoiseMap"].SetValue(content.Load<Texture3D>("Content/smallnoise3d"));

            viewMatrix = Matrix.CreateLookAt(new Vector3(20, 5, 13), new Vector3(8, 7, 0), new Vector3(0, 0, 1));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, this.Window.ClientBounds.Width / this.Window.ClientBounds.Height, 0.2f, 500.0f);

            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);

            effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xLightDirection"].SetValue(new Vector3(0.5f, -1, -1));
            effect.Parameters["xAmbient"].SetValue(0.4f);

        }

        private void SetUpVertices()
        {
            float imagesintexture = 1 + differentbuildings * 2;

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    int currentbuilding = 0;// int_Floorplan[x, y];
                    if (this.theMap.layout[x,y] != null)
                        currentbuilding = 1;
                    verticeslist.Add(new VertexPositionNormalTexture(new Vector3(x + 1, y + 1, 0), new Vector3(0, 0, 1), new Vector2(currentbuilding * 2 / imagesintexture, 0)));
                    verticeslist.Add(new VertexPositionNormalTexture(new Vector3(x + 1, y, 0), new Vector3(0, 0, 1), new Vector2(currentbuilding * 2 / imagesintexture, 1)));
                    verticeslist.Add(new VertexPositionNormalTexture(new Vector3(x, y, 0), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2 + 1) / imagesintexture, 1)));

                    verticeslist.Add(new VertexPositionNormalTexture(new Vector3(x, y, 0), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2 + 1) / imagesintexture, 1)));
                    verticeslist.Add(new VertexPositionNormalTexture(new Vector3(x, y + 1, 0), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2 + 1) / imagesintexture, 0)));
                    verticeslist.Add(new VertexPositionNormalTexture(new Vector3(x + 1, y + 1, 0), new Vector3(0, 0, 1), new Vector2(currentbuilding * 2 / imagesintexture, 0)));
                }
            }
            verticesarray = (VertexPositionNormalTexture[])verticeslist.ToArray(typeof(VertexPositionNormalTexture));
        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                SetUpXNADevice();
                LoadEffect();


                scenerytexture = content.Load<Texture2D>("Content/texturemap");

                bullettexture = content.Load<Texture2D>("Content/bullet");

                LoadModels();
            }
        }

        private void LoadModels()
        {
            skybox = content.Load<Model>("Content/skybox2");

            int i = 0;
            skyboxtextures = new Texture2D[skybox.Meshes.Count];
            foreach (ModelMesh mesh in skybox.Meshes)
                foreach (BasicEffect currenteffect in mesh.Effects)
                    skyboxtextures[i++] = currenteffect.Texture;

            foreach (ModelMesh modmesh in skybox.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                {
                    modmeshpart.Effect = effect.Clone(device);
                    effect.Parameters["xEnableLighting"].SetValue(false);
                }


        }

        private Model FillModelFromFile(string asset)
        {

            Model mod = content.Load<Model>(asset);
            /*foreach (ModelMesh modmesh in mod.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                    modmeshpart.Effect = effect.Clone(device);*/
            return mod;
        }

        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent == true)
            {
                content.Unload();
            }
        }

        protected void doMouseStuff(){
            MouseState ms = Mouse.GetState();
            if (ms.X < 0 || ms.X > this.Window.ClientBounds.Width || ms.Y < 0 || ms.Y > this.Window.ClientBounds.Height)
            {
                return;
            }
            if (ms.LeftButton==ButtonState.Pressed){
//                Console.Out.WriteLine("It's down");
//                Console.Out.WriteLine(viewMatrix.ToString());
                Vector3 LookAt = new Vector3(this.WIDTH / 2, this.HEIGHT / 2, 0);
                Vector3 LookFrom = new Vector3((float)Math.Cos(this.cameraRot.X) * this.cameraDist + this.WIDTH / 2,
                                               (float)Math.Sin(this.cameraRot.X) * this.cameraDist + this.HEIGHT / 2,
                                               (float)Math.Sin(this.cameraRot.Z) * this.cameraDist);
                Vector3 ViewUp = new Vector3(0, 0, 1);
                Vector3 W = LookFrom - LookAt;
                W.Normalize();
                Vector3 U = Vector3.Cross(ViewUp, W);
                U.Normalize();
                Vector3 V = Vector3.Cross(W, U);
                V.Normalize();
//                Console.Out.WriteLine(W);
//                Console.Out.WriteLine(U);
//                Console.Out.WriteLine(V);

                float vMax = (float)(((Vector3)(LookAt - LookFrom)).Length() * Math.Tan(MathHelper.PiOver4 / 2));
                float vMin = -vMax;
                float uMax = vMax * (float)this.Window.ClientBounds.Width / (float)this.Window.ClientBounds.Height;
                float uMin = -uMax;
                Vector3 sView = new Vector3(ms.X * ((uMax - uMin)/(float)this.Window.ClientBounds.Width) + uMin,
                                            (this.Window.ClientBounds.Height - ms.Y) * ((vMax - vMin) / this.Window.ClientBounds.Height) + vMin, 0);
                Vector3 sWorld = LookAt + sView.X * U + sView.Y * V + sView.Z * W;
//                Console.Out.WriteLine(LookAt + " + " + sView.X + " * " + U + " + " + sView.Y + " * " + V + " + " + sView.Z + " * " + W + " = " + sWorld);
//                Console.Out.WriteLine(sWorld);
                Vector3 rayO = LookFrom;
                Vector3 rayD = sWorld - LookFrom;
                rayD.Normalize();
                float t = intersectBoard(rayO, rayD);
                //t = - rayO.Z / rayD.Z;
                if (t < 1e7)
                {
                    Vector3 iPoint = rayO + t * rayD;
                    int xPos = (int)(iPoint.X);
                    int yPos = (int)(iPoint.Y);
//                    Console.Out.WriteLine("X:" + ms.X + ", Y:" + (this.Window.ClientBounds.Height - ms.Y) + ", uMin:" + uMin + ", uMax:" + uMax + ", vMin:" + vMin + ", vMax:" + vMax + ", sView:" + sView + ", W:" +
//                                          this.Window.ClientBounds.Width + ", H:" + this.Window.ClientBounds.Height + ", sWorld:" + sWorld + ", iPoint:" + iPoint + ", X:" + xPos + ", Y:" + yPos);
                    if (xPos < WIDTH && xPos >= 0 && yPos < HEIGHT && yPos >= 0)
                    {
                        mousePos = new Position(xPos, yPos);
                    }
                    else
                    {
                        mousePos = null;
                    }
                }
                else
                {
                    mousePos = null;
                }
            }
            //Console.Out.WriteLine(ms.X + " " + ms.Y);
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

        protected override void Update(GameTime gameTime)
        {
            doMouseStuff();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            ProcessKeyboard(gameTime.ElapsedGameTime.Milliseconds / 500.0f * gamespeed);
            
            UpdateCamera();
           


            base.Update(gameTime);
        }

        private void UpdateCamera()
        {
            Vector3 campos = new Vector3((float)Math.Cos(this.cameraRot.X) * this.cameraDist + this.WIDTH / 2,
                                         (float)Math.Sin(this.cameraRot.X) * this.cameraDist + this.HEIGHT / 2,
                                         (float)Math.Sin(this.cameraRot.Z) * this.cameraDist);

            Vector3 camup = new Vector3(0, 0, 1);

            viewMatrix = Matrix.CreateLookAt(campos, new Vector3(this.WIDTH/2,this.HEIGHT/2,0), camup);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)this.Window.ClientBounds.Width / (float)this.Window.ClientBounds.Height, 0.2f, 500.0f);
        }

        private void ProcessKeyboard(float speed)
        {
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Right))
            {
                this.cameraRot.X += .05f;
            }
            if (keys.IsKeyDown(Keys.Left))
            {
                this.cameraRot.X -= .05f;
            }
            if (keys.IsKeyDown(Keys.Up))
            {
                this.cameraRot.Z += .02f;
                if (this.cameraRot.Z > 1.5f)
                    this.cameraRot.Z = 1.5f;
            }
            if (keys.IsKeyDown(Keys.Down))
            {
                this.cameraRot.Z -= .02f;
                if (this.cameraRot.Z < .25f)
                    this.cameraRot.Z = .25f;
            }
            if (keys.IsKeyDown(Keys.S))
            {
                this.cameraDist += .5f;
                if (this.cameraDist > this.WIDTH)
                    this.cameraDist = this.WIDTH;
            }
            if (keys.IsKeyDown(Keys.W))
            {
                this.cameraDist -= .5f;
                if (this.cameraDist < .25f)
                    this.cameraDist = .25f;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);
            //if (gameTime.ElapsedGameTime.Milliseconds>0)
            //  Console.WriteLine("FPS: " + 1000d /(double)gameTime.ElapsedGameTime.Milliseconds);

            Matrix worldMatrix = Matrix.Identity;
            effect.CurrentTechnique = effect.Techniques["Textured"];
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xTexture"].SetValue(scenerytexture);
            effect.Begin();

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                device.VertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
                device.DrawUserPrimitives(PrimitiveType.TriangleList, verticesarray, 0, verticesarray.Length / 3);

                pass.End();
            }
            effect.End();

            if (this.mousePos != null)
            {
                if (this.theMap.isOccupied((int)(mousePos.getX()), (int)(mousePos.getY())))
                {
                    //draw red square
                    Console.Out.WriteLine("Occupied");
                }
                else
                {
                    //draw green square
                    Console.Out.WriteLine("Empty");
                }
                this.mousePos = null;
            }

            foreach (Tower t in this.theMap.towers)
            {
                
                t.draw(viewMatrix, projectionMatrix);
            }

            foreach (Creep creep in this.creeps)
            {

                creep.move(gameTime.ElapsedGameTime.Milliseconds);
                creep.draw(viewMatrix,projectionMatrix);
            }



            int i = 0;
            foreach (ModelMesh modmesh in skybox.Meshes)
            {
                worldMatrix = Matrix.CreateRotationX((float)Math.PI / 2) * Matrix.CreateScale(15, 15, 15) * Matrix.CreateTranslation(cameraposition);
                foreach (Effect currenteffect in modmesh.Effects)
                {
                    currenteffect.CurrentTechnique = currenteffect.Techniques["Textured"];
                    
                    currenteffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currenteffect.Parameters["xView"].SetValue(viewMatrix);
                    currenteffect.Parameters["xProjection"].SetValue(projectionMatrix);
                    currenteffect.Parameters["xTexture"].SetValue(skyboxtextures[i++]);
                }
                modmesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}