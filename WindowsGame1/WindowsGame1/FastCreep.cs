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
        private string modelAsset = "Content/cube_ndx";
        private float scale = .5f;
        private float currentXSizeAngle = 0;
        private float currentYSizeAngle = 0;
        private float currentZSizeAngle = 0;

        public FastCreep(Position pos, int level, GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice device)
        {
            this.speed = .003f;;
            this.pos = pos;
            this.level = level;
            this.health = 150 + (15 * level);
            this.cash = 4 + (int)(0.5 * level);

            CompiledEffect compiledEffect = Effect.CompileEffectFromFile("@/../../../../Content/MetallicFlakes.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            this.effect = new Effect(graphics.GraphicsDevice, compiledEffect.GetEffectCode(), CompilerOptions.None, null);
            this.effect.Parameters["NoiseMap"].SetValue(content.Load<Texture3D>("Content/smallnoise3d"));


            this.model =  content.Load<Model>(modelAsset);
            foreach (ModelMesh modmesh in model.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                    modmeshpart.Effect = this.effect.Clone(device);
        }

        public override void updateState(float elapsedTime)
        {
            this.currentStateCountdown -= elapsedTime;
            if (this.currentStateCountdown < 0)
            {
                this.currentStateCountdown = 0;
                this.currentState = States.NOTHING;
            }
            if (this.currentState == States.DAMAGED)
            {
                currentXSizeAngle += elapsedTime / 60;
                currentYSizeAngle += elapsedTime / 90;
                currentZSizeAngle += elapsedTime / 75;
            }
            else
            {

                currentXSizeAngle += elapsedTime / 300;
                currentYSizeAngle += elapsedTime / 420;
                currentZSizeAngle += elapsedTime / 545;
            }
        }


        public override void draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            if (this.currentState == States.DAMAGED)
            {
                //scaleReducer = 25 + (100 / this.currentStateCountdown);
            }
            float xScale =  (float)Math.Cos(currentXSizeAngle);
            float yScale =  (float)Math.Cos(currentYSizeAngle);
            float zScale =  (float)Math.Cos(currentZSizeAngle);

            {
                Matrix worldMatrix = Matrix.CreateRotationX(xScale) *Matrix.CreateRotationY(yScale) *Matrix.CreateRotationZ(zScale) * Matrix.CreateScale(this.scale, this.scale, this.scale) * Matrix.CreateTranslation(new Vector3(this.getPosition().getX() + .5f, this.getPosition().getY() + .5f, 0.40f));
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
