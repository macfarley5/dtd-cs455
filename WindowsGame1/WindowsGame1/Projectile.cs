using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace TD3d
{
    class Projectile
    {
        protected Position pos;
        protected Position velocity;
        protected float speed;
        protected string modelAsset = "Content/sphere0";
        protected float scale = .1f;
        protected Effect effect;
        protected Model model;

        protected GraphicsDeviceManager graphics;
        protected ContentManager content;
        protected GraphicsDevice device;

        public Projectile(Position pos, Position velocity,GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice device)
        {
            this.pos = pos;
            this.velocity = velocity;
            CompiledEffect compiledEffect = Effect.CompileEffectFromFile("@/../../../../Content/MetallicFlakes.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            this.effect = new Effect(graphics.GraphicsDevice, compiledEffect.GetEffectCode(), CompilerOptions.None, null);
            this.effect.Parameters["NoiseMap"].SetValue(content.Load<Texture3D>("Content/smallnoise3d"));

            this.model = content.Load<Model>(modelAsset);
            foreach (ModelMesh modmesh in model.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                    modmeshpart.Effect = this.effect.Clone(device);
        }

        public Position getPosition()
        {
            return pos;
        }
        public void setPosition(Position pos)
        {
            this.pos = pos;
        }

        public void updateState(float elapsedTime)
        {
            float newX = this.pos.getX()+elapsedTime*this.velocity.getX()/1000;
            this.pos.setX(newX);
            float newY = this.pos.getY() + elapsedTime * this.velocity.getY() / 1000;
            this.pos.setY(newY);
        }

        public void draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            {
                Matrix worldMatrix = Matrix.CreateRotationX(3.14f / 2) * Matrix.CreateScale(this.scale, this.scale, this.scale) * Matrix.CreateTranslation(new Vector3(this.getPosition().getX() + .25f, this.getPosition().getY() + .25f, 0.40f));
                foreach (ModelMesh modmesh in model.Meshes)
                {
                    foreach (Effect currenteffect in modmesh.Effects)
                    {
                        currenteffect.Parameters["I_a"].SetValue(new Vector4(.15f, .05f, .15f, 1f));
                        currenteffect.Parameters["I_d"].SetValue(new Vector4(.5f, .5f, .5f, 1f));
                        currenteffect.Parameters["I_s"].SetValue(new Vector4(.5f, .5f, .9f, 1f));
                        currenteffect.Parameters["k_a"].SetValue(new Vector4(.9f, .2f, .3f, 1f));
                        currenteffect.Parameters["k_d"].SetValue(new Vector4(.9f, .2f, .3f, 1f));
                        currenteffect.Parameters["k_s"].SetValue(new Vector4(.2f, .1f, .7f, 1f));
                        currenteffect.Parameters["k_r"].SetValue(new Vector4(.7f, .2f, .1f, 1f));
                        currenteffect.Parameters["noisescale"].SetValue(.70f);
                        currenteffect.Parameters["alph"].SetValue(1f);
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
