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
    class ProjectileLaser:Projectile
    {
        public ProjectileLaser(Position pos, Position velocity,ArrayList creeps,GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice device, Creep target, int damage)
        {
            this.modelAsset = "Content/laser";
            this.scale = 3f;
            this.doesSplash = false;
            this.speed = .015f;
            this.target = target;
            this.pos = pos;
            this.creeps = creeps;
            this.velocity = velocity;
            CompiledEffect compiledEffect = Effect.CompileEffectFromFile("@/../../../../Content/MetallicFlakes.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            this.effect = new Effect(graphics.GraphicsDevice, compiledEffect.GetEffectCode(), CompilerOptions.None, null);
            this.effect.Parameters["NoiseMap"].SetValue(content.Load<Texture3D>("Content/smallnoise3d"));
            this.damageDone = damage;

            this.model = content.Load<Model>(modelAsset);
            foreach (ModelMesh modmesh in model.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                    modmeshpart.Effect = this.effect.Clone(device);
        }

        public override void draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            //float length = this.speed;
            if (isActive){
                Matrix worldMatrix = Matrix.CreateScale(this.scale, this.scale, this.scale)* Matrix.CreateRotationZ(this.rot) * Matrix.CreateTranslation(new Vector3(this.getPosition().getX() + .5f, this.getPosition().getY() + .5f, 0.40f));
                foreach (ModelMesh modmesh in model.Meshes)
                {
                    foreach (Effect currenteffect in modmesh.Effects)
                    {
                        currenteffect.Parameters["I_a"].SetValue(new Vector4(.25f, .25f, .25f, 1f));
                        currenteffect.Parameters["I_d"].SetValue(new Vector4(.95f, .95f, .95f, 1f));
                        currenteffect.Parameters["I_s"].SetValue(new Vector4(.99f, .99f, .9f, 1f));
                        currenteffect.Parameters["k_a"].SetValue(new Vector4(.3f, .8f, .05f, 1f));
                        currenteffect.Parameters["k_d"].SetValue(new Vector4(.3f, .8f, .08f, 1f));
                        currenteffect.Parameters["k_s"].SetValue(new Vector4(.2f, .9f, .04f, 1f));
                        currenteffect.Parameters["k_r"].SetValue(new Vector4(.4f, .9f, .02f, 1f));
                        currenteffect.Parameters["noisescale"].SetValue(.70f);
                        //currenteffect.Parameters["alph"].SetValue(.85f);
                        currenteffect.Parameters["World"].SetValue(worldMatrix);
                        currenteffect.Parameters["View"].SetValue(viewMatrix);
                        currenteffect.Parameters["Projection"].SetValue(projectionMatrix);
                    }
                    modmesh.Draw();
                }
            }
        }


    }
}
