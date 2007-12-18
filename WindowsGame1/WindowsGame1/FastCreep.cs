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
    class FastCreep : Creep
    {
        private string modelAsset = "Content/sphere0";
        private float scale = .1f;

        public FastCreep(Position pos, int level, GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice device)
        {
            this.speed = .003f;;
            this.pos = pos;
            this.level = level;
            this.health = 200 + (20 * level);
            this.cash = 4 + (int)(1.2 * level);

            CompiledEffect compiledEffect = Effect.CompileEffectFromFile("@/../../../../Content/MetallicFlakes.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            this.effect = new Effect(graphics.GraphicsDevice, compiledEffect.GetEffectCode(), CompilerOptions.None, null);
            this.effect.Parameters["NoiseMap"].SetValue(content.Load<Texture3D>("Content/smallnoise3d"));


            this.model =  content.Load<Model>(modelAsset);
            foreach (ModelMesh modmesh in model.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                    modmeshpart.Effect = this.effect.Clone(device);
        }

        public override void draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            {
                Matrix worldMatrix = Matrix.CreateRotationX(3.14f / 2) * Matrix.CreateScale(this.scale, this.scale, this.scale) * Matrix.CreateTranslation(new Vector3(this.getPosition().getX() + .5f, this.getPosition().getY() + .5f, 0.20f));
                foreach (ModelMesh modmesh in model.Meshes)
                {
                    foreach (Effect currenteffect in modmesh.Effects)
                    {
                        currenteffect.Parameters["I_a"].SetValue(new Vector4(.15f, .05f, .15f, 1f));
                        currenteffect.Parameters["I_d"].SetValue(new Vector4(.5f, .5f, .5f, 1f));
                        currenteffect.Parameters["I_s"].SetValue(new Vector4(.5f, .5f, .9f, 1f));
                        currenteffect.Parameters["k_a"].SetValue(new Vector4(.2f, .3f, .9f, 1f));
                        currenteffect.Parameters["k_d"].SetValue(new Vector4(.2f, .3f, .9f, 1f));
                        currenteffect.Parameters["k_s"].SetValue(new Vector4(.7f, .7f, .1f, 1f));
                        currenteffect.Parameters["k_r"].SetValue(new Vector4(.1f, .1f, .7f, 1f));
                        
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
