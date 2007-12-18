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
    class NormalTower : Tower
    {
        private string modelAsset = "Content/tower";
        public NormalTower(GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice device, ArrayList creeps)
        {
            target = null;
            this.creeps = creeps;
            this.graphics = graphics;
            this.content = content;
            this.device = device;
            CompiledEffect compiledEffect = Effect.CompileEffectFromFile("@/../../../../Content/MetallicFlakes.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            this.effect = new Effect(graphics.GraphicsDevice, compiledEffect.GetEffectCode(), CompilerOptions.None, null);
            this.effect.Parameters["NoiseMap"].SetValue(content.Load<Texture3D>("Content/smallnoise3d"));


            this.model = content.Load<Model>(modelAsset);
            foreach (ModelMesh modmesh in model.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                    modmeshpart.Effect = this.effect.Clone(device);
        }

        public override bool isOccupied()
        {
            return true;
        }

        public override Tile.TileType getTileType()
        {
            return TileType.NORMALTOWER;
        }

        public override void updateState(float elapsedTime)
        {
            this.fireCounter -= elapsedTime;
            if (this.fireCounter < 0)
            {
                //Fire!
                if (this.creeps.Count > 0)
                {
                    Position bestPos = ((Creep)creeps[0]).getVisualPosition();
                    Position myVisPos = this.getPosition();
                    myVisPos.setX(myVisPos.getX() + .5f);
                    myVisPos.setY(myVisPos.getY() + .5f);
                    float bestDist = 100000f;

                    foreach (Creep c in this.creeps)
                    {
                        float nowdist = Position.dist(myVisPos, c.getVisualPosition());
                        if (nowdist < bestDist)
                        {
                            bestDist = nowdist;
                            bestPos = c.getVisualPosition();
                        }
                    }
                    if (bestDist < this.range)
                    {
                        Vector2 velocity = new Vector2(bestPos.getX() - myVisPos.getX(), bestPos.getY() - myVisPos.getY());
                        velocity.Normalize();
                        rot = (float)Math.Atan2(velocity.Y, velocity.X);

                        this.fireCounter = this.fireSpeed;
                        this.projectiles.Add(new Projectile(myVisPos.Clone(),
                            new Position(velocity.X, velocity.Y), this.creeps, this.graphics, this.content, this.device));
                    }
                }
            }
            else
            {
                //rot += elapsedTime / 700;
            }

            foreach (Projectile p in this.projectiles)
            {
                p.updateState(elapsedTime);
            }

            if (target != null)
            {
                this.rot = (float)(Math.Atan2(target.getPosition().getY() - this.pos.getY(), target.getPosition().getX() - this.pos.getX()));
            }
            else
            {
                //this.rot += .001f * elapsedTime;
            }
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
