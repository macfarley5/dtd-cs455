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
    class NormalCreep : Creep
    {

        private string modelAsset = "sphere0";
        private float scale = .2f;

        public NormalCreep(Position pos, int level, GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice device)
        {
            //this.speed = 200;
            this.pos = pos;
            this.level = level;

            CompiledEffect compiledEffect = Effect.CompileEffectFromFile("@/../../../../MetallicFlakes.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            this.effect = new Effect(graphics.GraphicsDevice, compiledEffect.GetEffectCode(), CompilerOptions.None, null);
            this.effect.Parameters["NoiseMap"].SetValue(content.Load<Texture3D>("smallnoise3d"));


            this.model =  content.Load<Model>(modelAsset);
            foreach (ModelMesh modmesh in model.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                    modmeshpart.Effect = this.effect.Clone(device);
        }

        public override void draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            {
                Matrix worldMatrix = Matrix.CreateRotationX(3.14f / 2) * Matrix.CreateScale(this.scale, this.scale, this.scale) * Matrix.CreateTranslation(new Vector3(this.getPosition().getX() + .25f, this.getPosition().getY() + .25f, 0.40f));
                foreach (ModelMesh modmesh in model.Meshes)
                {
                    foreach (Effect eff in modmesh.Effects)
                    {
                        // eff.CurrentTechnique = eff.Techniques[1];
                        foreach (EffectParameter ep in eff.Parameters)
                        {
                            String n = ep.Name;
                            //Console.Out.WriteLine(n);
                        }
                        eff.Parameters["World"].SetValue(worldMatrix);
                        eff.Parameters["View"].SetValue(viewMatrix);
                        eff.Parameters["Projection"].SetValue(projectionMatrix);
                        /*basicEffect.World = worldMatrix;
                        basicEffect.View = viewMatrix;
                        basicEffect.Projection = projectionMatrix;

                        basicEffect.SpecularPower = 5.0f;
                        basicEffect.LightingEnabled = true;
                        basicEffect.DirectionalLight0.Enabled = true;
                        basicEffect.DirectionalLight0.DiffuseColor = Vector3.One;
                        basicEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(-1.0f, -1.0f, -1.0f));
                        basicEffect.DirectionalLight0.SpecularColor = Vector3.One;*/
                    }
                    modmesh.Draw();
                }
            }
        }

    }
}
