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
        int score;
        long cash;
        bool newWave = false;
        Cue musicCue;

        // OTHER VARS
        GraphicsDeviceManager graphics;
        ContentManager content;
        GraphicsDevice device;
        Effect effect, effect2;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        Texture2D scenerytexture;        

        int WIDTH = 22;
        int HEIGHT = 26;
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

        int maxCreeps = 10;
        int numCreeps = 0;
        int waveNum = 1;
        ArrayList thePath = null;
        int totalTime = 0;
        PathPlanner planner;

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

            score = 20;
            cash = 800;

            Music.Initialize();
            musicCue = Music.Play("sbtechno");
        }

        private void LoadFloorplan()
        {
            this.map = new Map(WIDTH, HEIGHT);
            
            planner = new PathPlanner(WIDTH, HEIGHT, 0, HEIGHT / 2, WIDTH - 1, HEIGHT / 2, this.map);
            planner.isPath();
            thePath = planner.getPath();
        }

        private void SetUpXNADevice()
        {
            device = graphics.GraphicsDevice;

            graphics.PreferredBackBufferWidth = 1400;
            graphics.PreferredBackBufferHeight = 900;
            graphics.IsFullScreen = true;
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
               // bullettexture = content.Load<Texture2D>("Content/bullet");

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
            if (score == 0)
                return;

            totalTime += (int)(gameTime.ElapsedGameTime.TotalMilliseconds);

            if (numCreeps == 10)
                newWave = true;

            if (numCreeps < maxCreeps && totalTime > 1000)
            {
                totalTime = 0;
                int x = 0;
                int y = HEIGHT / 2;

                if (this.map.layout[x, y] == null)
                {
                    int whichCreep = waveNum % 2;
                    Creep c;

                    if (whichCreep == 1)
                    {
                        c = new NormalCreep(new Position(x, y), waveNum, graphics, content, device);
                        c.setPlanner(new PathPlanner(WIDTH, HEIGHT, 0, HEIGHT / 2, WIDTH - 1, HEIGHT / 2, this.map));
                        c.setPath(thePath);
                    }
                    else
                    {
                        c = new FastCreep(new Position(x, y), waveNum, graphics, content, device);
                        c.setPlanner(new PathPlanner(WIDTH, HEIGHT, 0, HEIGHT / 2, WIDTH - 1, HEIGHT / 2, this.map));
                        c.setPath(thePath);
                    }
                    this.creeps.Add(c);
                    numCreeps++;
                }
            }
            if (newWave && this.creeps.Count == 0)
            {
                this.waveNum++;
                this.numCreeps = 0;
                newWave = false;
            }

            thePath = mouse.update(cameraRot, cameraDist, planner, thePath, creeps, cash);
            cash = mouse.getCash();

            foreach (Tower t in this.map.towers)
            {
                t.updateState(gameTime.ElapsedGameTime.Milliseconds);
            }

            for (int i = 0; i < creeps.Count; i++)
            {
                Creep c = (Creep)creeps[i];
                if (c.getPosition().getX() > (WIDTH - 1) - .1f && c.getPosition().getY() > (HEIGHT / 2) - .1f)
                {
                    c.injure(Int32.MaxValue);
                    creeps.RemoveAt(i--);
                    score--;
                }
                else if (c.getHealth() <= 0)
                {
                    creeps.RemoveAt(i--);
                    this.cash += c.getCashValue();
                }
                else
                {
                    c.updateState(gameTime.ElapsedGameTime.Milliseconds);
                    c.move(gameTime.ElapsedGameTime.Milliseconds);
                }
            }
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            keyboard.update(cameraRot, cameraDist, gameTime.ElapsedGameTime.Milliseconds / 500.0f * gamespeed);
            this.cameraRot = keyboard.getCameraRot();
            this.cameraDist = keyboard.getCameraDist();

            UpdateCamera();
            base.Update(gameTime);

            if (!musicCue.IsPlaying)
            {
                musicCue.Play();
            }

            Music.Update();
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
            Position mousePos = mouse.getPos();

            foreach (Tower t in this.map.towers)
            {
                t.draw(viewMatrix, projectionMatrix, true);
            }
            
            for(int i=0;i<creeps.Count;i++)
            {
                Creep c = (Creep)creeps[i];
                c.draw(viewMatrix, projectionMatrix);
            }

            int j = 0;
            foreach (ModelMesh modmesh in skybox.Meshes)
            {
                worldMatrix = Matrix.CreateRotationX((float)Math.PI / 2) * Matrix.CreateScale(15, 15, 15) * Matrix.CreateTranslation(cameraposition);

                foreach (Effect currenteffect in modmesh.Effects)
                {
                    currenteffect.CurrentTechnique = currenteffect.Techniques["Textured"];

                    currenteffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currenteffect.Parameters["xView"].SetValue(viewMatrix);
                    currenteffect.Parameters["xProjection"].SetValue(projectionMatrix);
                    currenteffect.Parameters["xTexture"].SetValue(skyboxtextures[j++]);
                }

                modmesh.Draw();
            }


            for (int x = 0; x < this.WIDTH / 2; x++)
            {
                for (int y = 0; y < this.HEIGHT / 2; y++)
                {
                    foreach (ModelMesh modmesh in mapModel.Meshes)
                    {
                        //worldMatrix = Matrix.CreateScale(this.WIDTH, this.HEIGHT, .3f) * Matrix.CreateTranslation(this.WIDTH / 2, this.HEIGHT / 2, -.15f);
                        worldMatrix = Matrix.CreateScale(1.7f, 1.7f, .1f) * Matrix.CreateTranslation((float)(2 * x) + 1f, (float)(2 * y) + 1f, -.15f);
                        foreach (Effect currenteffect in modmesh.Effects)
                        {
                            //currenteffect.CurrentTechnique = currenteffect.Techniques["Colored"];
                            float red = (float)(x % 2) / 4 + .25f;
                            float blue = (float)(y % 2) / 4 + .25f;
                            currenteffect.Parameters["I_a"].SetValue(new Vector4(red, .5f, blue, 1f));
                            currenteffect.Parameters["I_d"].SetValue(new Vector4(.5f, .5f, .5f, 1f));
                            currenteffect.Parameters["I_s"].SetValue(new Vector4(.5f, .5f, .9f, 1f));
                            currenteffect.Parameters["k_a"].SetValue(new Vector4(.5f, .5f, .5f, 1f));
                            currenteffect.Parameters["k_d"].SetValue(new Vector4(.5f, .5f, .8f, 1f));
                            currenteffect.Parameters["k_s"].SetValue(new Vector4(.5f, .5f, .8f, 1f));
                            currenteffect.Parameters["k_r"].SetValue(new Vector4(.5f, .5f, .8f, 1f));
                            currenteffect.Parameters["noisescale"].SetValue(5f);
                            currenteffect.Parameters["alph"].SetValue(.75f);
                            //currenteffect.Parameters["NoiseMap"].SetValue(content.Load<Texture3D>("Content/smallnoise3d"));

                            currenteffect.Parameters["World"].SetValue(worldMatrix);
                            currenteffect.Parameters["View"].SetValue(viewMatrix);
                            currenteffect.Parameters["Projection"].SetValue(projectionMatrix);
                            //currenteffect.Parameters["xTexture"].SetValue(skyboxtextures[i++]);
                        }
                        modmesh.Draw();
                    }
                }
            }

            if (mousePos != null)
            {
                int xPos = (int)(mousePos.getX());
                int yPos = (int)(mousePos.getY());

                mouse.setTowerType(keyboard.getTowerNum());
                Tile.TileType towerType = mouse.getTowerType();
                Tower tow = null;
                if (towerType == Tile.TileType.NORMALTOWER)
                {
                    tow = new NormalTower(graphics, content, device, creeps);
                }
                else if (towerType == Tile.TileType.FASTTOWER)
                {
                    tow = new FastTower(graphics, content, device, creeps);
                }
                else if (towerType == Tile.TileType.SPLASHTOWER)
                {
                    tow = new SplashTower(graphics, content, device, creeps);
                }
                tow.setPosition(xPos, yPos);

                if (this.map.canPlaceTower(tow))
                {
                    tow.setAlpha(.5f);
                    tow.draw(viewMatrix, projectionMatrix, true);
                }
                else
                {
                    SelectionTile sel = new SelectionTile(graphics, content, device);
                    sel.setPosition(xPos, yPos);
                    sel.draw(viewMatrix, projectionMatrix, true);
                }
            }

            Tower selectedTower = mouse.getSelectedTower();
            if (selectedTower != null)
            {
                SelectionTile sel = new SelectionTile(graphics, content, device);
                sel.setPosition((int)selectedTower.getPosition().getX(), (int)selectedTower.getPosition().getY());
                sel.setColor(new Vector4(1f, 1f, 0f, 1f));
                sel.draw(viewMatrix, projectionMatrix, true);
            }

            base.Draw(gameTime);
            hud.Draw(score, cash, waveNum, mouse.getScreenXPos(), mouse.getScreenYPos());

            Matrix hudView = Matrix.CreateLookAt(new Vector3(0, 0, 8), new Vector3(this.WIDTH / 2 - 7.1f, this.HEIGHT / 2, -2.7f), new Vector3(0, -0.35f, 1));
            Matrix hudProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)this.Window.ClientBounds.Width / (float)this.Window.ClientBounds.Height, 0.2f, 500.0f);

            if (mouse.getSelectedTower() != null)
            {
                Tower temp = (Tower)mouse.getSelectedTower();
                Tile.TileType towType = temp.getTileType();
                hud.setSelectedTower(temp);

                if(towType == Tile.TileType.NORMALTOWER)
                {
                    NormalTower display = (NormalTower)mouse.getSelectedTower();
                    Position tmp = display.getPosition();
                    display.setPosition(10, 10);
                    display.draw(hudView, hudProjection, false);
                    display.setPosition((int)tmp.getX(), (int)tmp.getY());
                }
                else if (towType == Tile.TileType.FASTTOWER)
                {
                    FastTower display = (FastTower)mouse.getSelectedTower();
                    Position tmp = display.getPosition();
                    display.setPosition(10, 10);
                    display.draw(hudView, hudProjection, false);
                    display.setPosition((int)tmp.getX(), (int)tmp.getY());
                }
                else if (towType == Tile.TileType.SPLASHTOWER)
                {
                    SplashTower display = (SplashTower)mouse.getSelectedTower();
                    Position tmp = display.getPosition();
                    display.setPosition(10, 10);
                    display.draw(hudView, hudProjection, false);
                    display.setPosition((int)tmp.getX(), (int)tmp.getY());
                }
            }
        }

        
    }
}