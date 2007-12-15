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
    class Tower : Tile
    {
        Creep target;
        private string modelAsset = "Content/tower";
        private float scale = .13f;
        protected float fireSpeed = 1f;
        protected float fireCounter = 1f;
        private float rot = 0;
        protected ArrayList projectiles = new ArrayList();

        public Tower(GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice device)
        {
            target = null;
            CompiledEffect compiledEffect = Effect.CompileEffectFromFile("@/../../../../Content/MetallicFlakes.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            this.effect = new Effect(graphics.GraphicsDevice, compiledEffect.GetEffectCode(), CompilerOptions.None, null);
            this.effect.Parameters["NoiseMap"].SetValue(content.Load<Texture3D>("Content/smallnoise3d"));
            

            this.model = content.Load<Model>(modelAsset);
            foreach (ModelMesh modmesh in model.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                    modmeshpart.Effect = this.effect.Clone(device);
        }

        public void setTarget(Creep creep)
        {
            target = creep;
        }

        public Creep getTarget()
        {
            return target;
        }

        private double getTurretAngle()
        {
            if (target != null)
            {
                double yOverX = (target.getPosition().getY() - getPosition().getY()) /
                                (target.getPosition().getX() - getPosition().getX());
                return Math.Atan(yOverX);
            }
            else return 0;
        }
        
        public override bool isOccupied()
        {
            return true;
        }

        public override Tile.TileType getTileType()
        {
            return TileType.Tower;
        }

        public override void updateState(float elapsedTime)
        {
            
            rot += elapsedTime / 700;
            foreach (Projectile p in this.projectiles)
            {
                p.updateState(elapsedTime);
            }
        }

        public override void draw(Matrix vm,Matrix pm)
        {
            Matrix wm = Matrix.CreateRotationX((float)Math.PI)*Matrix.CreateScale(this.scale, this.scale, this.scale) * Matrix.CreateTranslation(new Vector3(this.getPosition().getX() + 1.0f, this.getPosition().getY() + 1.0f, 0f));
           // rot += .01f;
            int count = 0;
            foreach (ModelMesh modmesh in this.model.Meshes)
            {
                foreach (Effect currenteffect in modmesh.Effects)
                {
                    currenteffect.Parameters["I_a"].SetValue(new Vector4(.015f, .05f, .015f, .1f));
                    currenteffect.Parameters["I_d"].SetValue(new Vector4(.5f, .5f, .5f, 1f));
                    currenteffect.Parameters["I_s"].SetValue(new Vector4(.35f, .35f, .45f, 1f));
                    currenteffect.Parameters["k_a"].SetValue(new Vector4(.05f, .05f, .05f, 1f));
                    currenteffect.Parameters["k_d"].SetValue(new Vector4(.5f, .5f, .5f, 1f));
                    currenteffect.Parameters["k_s"].SetValue(new Vector4(.3f, .3f, .4f, 1f));
                    currenteffect.Parameters["k_r"].SetValue(new Vector4(.1f, .2f, .3f, 1f));
                    currenteffect.Parameters["noisescale"].SetValue(.50f);
                    if (count == 2)
                    {
                        currenteffect.Parameters["World"].SetValue(Matrix.CreateRotationZ(this.rot) * wm);
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
