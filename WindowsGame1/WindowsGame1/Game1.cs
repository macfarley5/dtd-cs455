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
                
        // OUR VARIABLES
        
        Map map;
        ArrayList creeps = new ArrayList();
        MouseX mouse;
        KeyboardX keyboard;
        HUD hud;

        // OTHER VARS
        GraphicsDeviceManager graphics;
        ContentManager content;
        GraphicsDevice device;
        Effect effect, effect2;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        Texture2D scenerytexture;        

        int WIDTH = 40;
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

        Model skybox, mapModel;
        Texture2D[] skyboxtextures;

        Vector3 cameraRot = new Vector3(0, 0, 1);
        float cameraDist = 5;

        VertexPositionNormalTexture[] verticesarray;
        ArrayList verticeslist = new ArrayList();
        Model spacemodel;
        Random globalRand = new Random(0);

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
            
            mouse = new MouseX(this.Window, WIDTH, HEIGHT, graphics, content, device, map);
            keyboard = new KeyboardX(WIDTH);
            hud = new HUD(this.Window, graphics, content);
        }

        private void LoadFloorplan()
        {
            this.map = new Map(WIDTH, HEIGHT);
            
            ArrayList thePath = null;
            Random r = new Random(0);

            for (int i = 0; i < 30; i++)
            {
                Tower t = new Tower(graphics, content, device);
                t.setPosition(r.Next(WIDTH - 15), r.Next(HEIGHT - 15));
                this.map.placeTower(t);
            }

            PathPlanner planner = new PathPlanner(WIDTH, HEIGHT, 0, HEIGHT / 2, WIDTH - 1, HEIGHT / 2, this.map);
            thePath = null;
            thePath = planner.getPath();

            for (int i = 0; i < 1; i++)
            {
                int x = 0;// r.Next(WIDTH - 1);
                int y = HEIGHT / 2;// r.Next(HEIGHT - 1);

                if (this.map.layout[x, y] == null)
                {
                    int whichCreep = r.Next(1);
                    Creep c;
                    if (whichCreep == 0)
                    {
                        c = new NormalCreep(new Position(x, y), 0, graphics, content, device);
                        c.setPath(thePath);
                    }
                    else
                    {
                        c = new FastCreep(new Position(x, y), 0, graphics, content, device);
                        c.setPath(thePath);
                    }

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
                    if (this.map.layout[x,y] != null)
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
            skyboxtextures = new Texture2D[skybox.Meshes.Count];
            int i = 0;            
            
            foreach (ModelMesh mesh in skybox.Meshes)
                foreach (BasicEffect currenteffect in mesh.Effects)
                    skyboxtextures[i++] = currenteffect.Texture;

            foreach (ModelMesh modmesh in skybox.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                {
                    modmeshpart.Effect = effect.Clone(device);
                    effect.Parameters["xEnableLighting"].SetValue(false);
                }

            this.mapModel = content.Load<Model>("Content/cube_ndx");

            foreach (ModelMesh modmesh in mapModel.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                    modmeshpart.Effect = effect2.Clone(device);
            }

        private Model FillModelFromFile(string asset)
        {
            Model mod = content.Load<Model>(asset);

            foreach (ModelMesh modmesh in mod.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                    modmeshpart.Effect = effect.Clone(device);

            return mod;
        }

        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent == true)
                content.Unload();
        }
        
        protected override void Update(GameTime gameTime)
        {
            mouse.update(cameraRot, cameraDist);
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            keyboard.update(cameraRot, cameraDist, gameTime.ElapsedGameTime.Milliseconds / 500.0f * gamespeed);
            this.cameraRot = keyboard.getCameraRot();
            this.cameraDist = keyboard.getCameraDist();

            UpdateCamera();
            base.Update(gameTime);
        }

        private void UpdateCamera()
        {
            Vector3 campos = new Vector3((float)Math.Cos(this.cameraRot.X) * (float)Math.Cos(this.cameraRot.Z)* this.cameraDist + this.WIDTH / 2,
                                         (float)Math.Sin(this.cameraRot.X) * (float)Math.Cos(this.cameraRot.Z)* this.cameraDist + this.HEIGHT / 2,
                                         (float)Math.Sin(this.cameraRot.Z) * this.cameraDist);
            Vector3 camup = new Vector3(0, 0, 1);

            viewMatrix = Matrix.CreateLookAt(campos, new Vector3(this.WIDTH/2,this.HEIGHT/2,0), camup);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)this.Window.ClientBounds.Width / (float)this.Window.ClientBounds.Height, 0.2f, 500.0f);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);

            Matrix worldMatrix = Matrix.Identity;
            /**/effect.CurrentTechnique = effect.Techniques["Textured"];
            effect.Parameters["xEnableLighting"].SetValue(1);
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

            Position mousePos = mouse.getPos();

            if(mousePos != null)
            {
                int xPos = (int)(mousePos.getX());
                int yPos = (int)(mousePos.getY());

                Tower tow = new Tower(graphics, content, device);
                tow.setPosition(xPos, yPos);

                if (this.map.canPlaceTower(tow))
                {
                    tow.draw(viewMatrix, projectionMatrix);
                }
            }

            foreach (Tower t in this.map.towers)
            {
                t.updateState(gameTime.ElapsedGameTime.Milliseconds);
                t.draw(viewMatrix, projectionMatrix);
            }

            foreach (Creep creep in this.creeps)
            {
                if (globalRand.NextDouble() < .001 * gameTime.ElapsedGameTime.Milliseconds)
                    creep.injure(1);
                creep.updateState(gameTime.ElapsedGameTime.Milliseconds);
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
            hud.Draw(20);
        }
    }
}