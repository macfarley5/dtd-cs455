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

        struct spritestruct
        {
            public Vector3 position;
            public Quaternion rotation;
        }

        private struct ourspritevertexformat
        {
            private Vector3 position;
            private float pointSize;

            public ourspritevertexformat(Vector3 position, float pointSize)
            {
                this.position = position;
                this.pointSize = pointSize;
            }

            public static VertexElement[] Elements =
              {
                  new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0),
                  new VertexElement(0, sizeof(float)*3, VertexElementFormat.Single, VertexElementMethod.Default, VertexElementUsage.PointSize, 0),
              };
            public static int SizeInBytes = sizeof(float) * (3 + 1);
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
        Effect effect;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        Texture2D scenerytexture;

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
            LoadFloorplan();
            SetUpVertices();
            AddTargets();
            base.Initialize();
        }

        private void LoadFloorplan()
        {
            this.theMap = new Map(WIDTH, HEIGHT);

             Random r = new Random(0);
            for (int i = 0; i < 30; i++)
            {
                Tower t = new Tower();
                t.setPosition(r.Next(WIDTH-1), r.Next(HEIGHT-1));
                this.theMap.placeTower(t);
            }

            for (int i = 0; i < 30; i++)
            {
                int x = r.Next(WIDTH - 1);
                int y = r.Next(HEIGHT - 1);
                if (this.theMap.layout[x, y] == null)
                {
                    Creep c = new FastCreep(new Position(x, y), 0);
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
            CompiledEffect compiledEffect = Effect.CompileEffectFromFile("@/../../../../effects.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            effect = new Effect(graphics.GraphicsDevice, compiledEffect.GetEffectCode(), CompilerOptions.None, null);

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

        private void AddTargets()
        {

        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                SetUpXNADevice();
                LoadEffect();


                scenerytexture = content.Load<Texture2D>("texturemap");

                bullettexture = content.Load<Texture2D>("bullet");

                LoadModels();
            }
        }

        private void LoadModels()
        {
            spacemodel = FillModelFromFile("dwarfWithEffectInstance_ndx");
            targetmodel = FillModelFromFile("bigship1_ndx");


            skybox = content.Load<Model>("skybox2");

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

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            ProcessKeyboard(gameTime.ElapsedGameTime.Milliseconds / 500.0f * gamespeed);
            UpdatePosition(ref cameraposition, spacemeshrotation, gameTime.ElapsedGameTime.Milliseconds / 20000.0f * gamespeed);
            UpdateCamera();
            UpdateSpritePositions(gameTime.ElapsedGameTime.Milliseconds / 500.0f * gamespeed);

            if (CheckCollision(cameraposition, spacemodel.Meshes[0].BoundingSphere.Radius * 0.0005f) > 0)
            {
                cameraposition = new Vector3(1, 2, 11);
                spacemeshrotation = Quaternion.CreateFromYawPitchRoll(0, -1f, 0);
                gamespeed /= 1.1f;
            }

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
            }
            if (keys.IsKeyDown(Keys.Down))
            {
                this.cameraRot.Z -= .02f;
            }
            if (keys.IsKeyDown(Keys.S))
            {
                this.cameraDist += .5f;
            }
            if (keys.IsKeyDown(Keys.W))
            {
                this.cameraDist -= .5f;
                if (this.cameraDist < .25f)
                    this.cameraDist = .25f;
            }
        }

        private void UpdatePosition(ref Vector3 position, Quaternion rotationquat, float speed)
        {
            Vector3 addvector = new Vector3(0, 1, 0);
            addvector = Vector3.Transform(addvector, Matrix.CreateFromQuaternion(rotationquat));
            addvector.Normalize();

            position += addvector*speed;
        }

        private int CheckCollision(Vector3 position, float radius)
        {
            if (position.Z - radius < 0) return 1;
            if (position.Z + radius > 35) return 10;

            if ((position.X < -15) || (position.X > WIDTH + 15)) return 2;
            if ((position.Y < -15) || (position.Y > HEIGHT + 15)) return 2;

            return 0;
        }

        private void UpdateSpritePositions(float speed)
        {
            for (int i = 0; i < spritelist.Count; i++)
            {
                spritestruct currentsprite = (spritestruct)spritelist[i];
                UpdatePosition(ref currentsprite.position, currentsprite.rotation, speed * 55);
                spritelist[i] = currentsprite;

                int collisionkind = CheckCollision(currentsprite.position, 0.05f);

                if (collisionkind > 0)
                {
                    spritelist.RemoveAt(i);
                    i--;

                    if (collisionkind == 3)
                    {
                        gamespeed *= 1.02f;
                        AddTargets();
                    }
                }
            }
        }

        private void DrawSprites()
        {
            if (spritelist.Count > 0)
            {
                ourspritevertexformat[] spritecoordsarray = new ourspritevertexformat[spritelist.Count];
                foreach (spritestruct currentsprite in spritelist)
                {
                    spritecoordsarray[spritelist.IndexOf(currentsprite)] = new ourspritevertexformat(currentsprite.position, 50.0f);
                }

                effect.CurrentTechnique = effect.Techniques["PointSprites"];
                Matrix worldMatrix = Matrix.Identity;
                effect.Parameters["xWorld"].SetValue(worldMatrix);
                effect.Parameters["xView"].SetValue(viewMatrix);
                effect.Parameters["xProjection"].SetValue(projectionMatrix);
                this.effect.Parameters["xTexture"].SetValue(bullettexture);

                device.RenderState.AlphaBlendEnable = true;
                device.RenderState.SourceBlend = Blend.One;
                device.RenderState.DestinationBlend = Blend.One;

                effect.Begin();
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    device.VertexDeclaration = new VertexDeclaration(device, ourspritevertexformat.Elements);
                    device.DrawUserPrimitives(PrimitiveType.PointList, spritecoordsarray, 0, spritecoordsarray.Length);
                    pass.End();
                }
                effect.End();

                device.RenderState.AlphaBlendEnable = false;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);
            if (gameTime.ElapsedGameTime.Milliseconds>0)
              Console.WriteLine("FPS: " + 1000d /(double)gameTime.ElapsedGameTime.Milliseconds);

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

            foreach (Tower t in this.theMap.towers)
            {
                worldMatrix = Matrix.CreateRotationX(3.14f / 2) * Matrix.CreateScale(2.0f, 2.0f, 2.0f) * Matrix.CreateTranslation(new Vector3(t.getPosition().getX() + 1, t.getPosition().getY() + 1, 0.0f));
                foreach (ModelMesh modmesh in spacemodel.Meshes)
                {
                    foreach (Effect e in modmesh.Effects)
                    {
                        e.CurrentTechnique = e.Techniques[0];
                        if (e is BasicEffect)
                        {
                            BasicEffect basicEffect = (BasicEffect)e;                            
                            basicEffect.World = worldMatrix;
                            basicEffect.View = viewMatrix;
                            basicEffect.Projection = projectionMatrix;
                        }
                        else
                        {
                           /*foreach (EffectParameter ep in e.Parameters)
                           {
                               String n = ep.Name;
                               Console.Out.WriteLine(n);
                           }*/
                            e.Parameters["g_mWorld"].SetValue(worldMatrix);
                           e.Parameters["g_mView"].SetValue(viewMatrix);
                           e.Parameters["g_mProj"].SetValue(projectionMatrix);
                           //basicEffect.Projection = projectionMatrix;
                        }

                    }
                    modmesh.Draw();
                }
            }

            foreach (Creep creep in this.creeps)
            {

                creep.move(gameTime.ElapsedGameTime.Milliseconds);
                worldMatrix = Matrix.CreateRotationX(3.14f / 2) * Matrix.CreateScale(0.1f, 0.1f, 0.1f) * Matrix.CreateTranslation(new Vector3(creep.getPosition().getX() + .25f, creep.getPosition().getY() + .25f, 0.40f));
                foreach (ModelMesh modmesh in targetmodel.Meshes)
                {
                    foreach (BasicEffect basicEffect in modmesh.Effects)
                    {
                        
                        basicEffect.World = worldMatrix;
                        basicEffect.View = viewMatrix;
                        basicEffect.Projection = projectionMatrix;

                        basicEffect.SpecularPower = 5.0f;
                        basicEffect.LightingEnabled = true;
                        basicEffect.DirectionalLight0.Enabled = true;
                        basicEffect.DirectionalLight0.DiffuseColor = Vector3.One;
                        basicEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(-1.0f, -1.0f, -1.0f));
                        basicEffect.DirectionalLight0.SpecularColor = Vector3.One;
                    }
                    modmesh.Draw();
                }
            }



            int i = 0;
            foreach (ModelMesh modmesh in skybox.Meshes)
            {
                foreach (Effect currenteffect in modmesh.Effects)
                {
                    currenteffect.CurrentTechnique = currenteffect.Techniques["Textured"];
                    worldMatrix = Matrix.CreateRotationX((float)Math.PI / 2) * Matrix.CreateScale(15, 15, 15) * Matrix.CreateTranslation(cameraposition);
                    currenteffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currenteffect.Parameters["xView"].SetValue(viewMatrix);
                    currenteffect.Parameters["xProjection"].SetValue(projectionMatrix);
                    currenteffect.Parameters["xTexture"].SetValue(skyboxtextures[i++]);
                }
                modmesh.Draw();
            }


            DrawSprites();

            base.Draw(gameTime);
        }
    }
}