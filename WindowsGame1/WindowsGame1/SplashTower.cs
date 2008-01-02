using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace TD3d
{
    class SplashTower : Tower
    {
        private string modelAsset = "Content/newTower";

        public SplashTower(GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice device, ArrayList creeps)
        {
            target = null;
            this.creeps = creeps;
            this.graphics = graphics;
            this.content = content;
            this.device = device;
            this.fireSpeed = 500f;
            CompiledEffect compiledEffect = Effect.CompileEffectFromFile("@/../../../../Content/MetallicFlakes.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            this.effect = new Effect(graphics.GraphicsDevice, compiledEffect.GetEffectCode(), CompilerOptions.None, null);
            this.effect.Parameters["NoiseMap"].SetValue(content.Load<Texture3D>("Content/smallnoise3d"));
            this.alphaVal = 1f;

            this.model = content.Load<Model>(modelAsset);
            foreach (ModelMesh modmesh in model.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                    modmeshpart.Effect = this.effect.Clone(device);

            this.cost = 80;
            this.damage = 30;
        }       

        public override bool isOccupied()
        {
            return true;
        }

        public override Tile.TileType getTileType()
        {
            return TileType.SPLASHTOWER;
        }

        public override void updateState(float elapsedTime)
        {
            this.fireSpeed = 2500 / level;
            this.range = 5 + 2*level;
            this.damage = 30 + (5 * level);

            base.updateState(elapsedTime);
        }

        protected override void fireProjectile(Vector2 velocity, Vector2 iniPos)
        {
            //Game1.AudioSystem.PlaySound("laser.wav");
            this.projectiles.Add(new ProjectileCannon(new Position(iniPos.X, iniPos.Y),
                new Position(velocity.X, velocity.Y), this.creeps, this.graphics, this.content, this.device, this.target, this.damage));
        }

        public override void draw(Matrix vm, Matrix pm, bool showProjectile)
        {
            Matrix wm = Matrix.CreateRotationX((float)Math.PI) * Matrix.CreateScale(this.scale, this.scale, this.scale) * Matrix.CreateTranslation(new Vector3(this.getPosition().getX() + 1.0f, this.getPosition().getY() + 1.0f, 0f));
            // rot += .01f;
            int count = 0;

            if (showProjectile)
                foreach (Projectile p in this.projectiles)
                {
                    p.draw(vm, pm);
                }
            float blueval = (this.level / 5f);
            foreach (ModelMesh modmesh in this.model.Meshes)
            {
                foreach (Effect currenteffect in modmesh.Effects)
                {
                    currenteffect.Parameters["I_a"].SetValue(new Vector4(.015f, .05f, .015f, .1f));
                    currenteffect.Parameters["I_d"].SetValue(new Vector4(.5f, .5f, .5f, 1f));
                    currenteffect.Parameters["I_s"].SetValue(new Vector4(.35f, .35f, .45f, 1f));
                    currenteffect.Parameters["k_a"].SetValue(new Vector4(.05f, .05f, .05f, 1f));
                    currenteffect.Parameters["k_d"].SetValue(new Vector4(.2f + blueval, .05f, .15f, 1f));
                    currenteffect.Parameters["k_s"].SetValue(new Vector4(.3f, .3f, .4f, 1f));
                    currenteffect.Parameters["k_r"].SetValue(new Vector4(.1f, .2f, .3f, 1f));
                    currenteffect.Parameters["alph"].SetValue(this.alphaVal);
                    currenteffect.Parameters["noisescale"].SetValue(.50f);
                    if (count == 3 || count == 4 || count == 10) // switch to 3, 4, 5 if using comp_newTower.x
                    {
                        currenteffect.Parameters["World"].SetValue(Matrix.CreateRotationZ(-this.rot) * wm);
                    }
                    else
                    {
                        currenteffect.Parameters["World"].SetValue(wm);
                    }
                    currenteffect.Parameters["View"].SetValue(vm);
                    currenteffect.Parameters["Projection"].SetValue(pm);
                    count++;
                }
                modmesh.Draw();
            }
        }
    }
}
