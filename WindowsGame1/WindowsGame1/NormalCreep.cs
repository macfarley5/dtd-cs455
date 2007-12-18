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

        private string modelAsset = "Content/sphere0_ndx";
        private float scale = .1f;
        private float currentXSizeAngle = 0;
        private float currentYSizeAngle = 0;
        private float currentZSizeAngle = 0;

        public NormalCreep(Position pos, int level, GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice device)
        {
            this.speed = .001f;
            this.pos = pos;
            this.level = level;

            this.health = 200 + (20 * level);
            this.cash = 3 + (int)(0.3 * level);

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
                currentXSizeAngle += elapsedTime / 20;
                currentYSizeAngle += elapsedTime / 35;
                currentZSizeAngle += elapsedTime / 50;
            }
            else
            {

                currentXSizeAngle += elapsedTime / 75;
                currentYSizeAngle += elapsedTime / 100;
                currentZSizeAngle += elapsedTime / 125;
            }
        }

        public override void draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            {
                float xScale, yScale, zScale;
                float scaleReducer = 40;
                if (this.currentState == States.DAMAGED)
                {
                    scaleReducer = 25 + (100/this.currentStateCountdown);
                }
                xScale = this.scale + (1 + (float)Math.Cos(currentXSizeAngle)) / scaleReducer;
                yScale = this.scale + (1 + (float)Math.Cos(currentYSizeAngle)) / scaleReducer;
                zScale = this.scale + (1 + (float)Math.Cos(currentZSizeAngle)) / scaleReducer;


                Matrix worldMatrix = Matrix.CreateRotationX(3.14f / 2) *
                                Matrix.CreateScale(xScale,yScale,zScale) * Matrix.CreateTranslation(new Vector3(this.getPosition().getX() + .5f, this.getPosition().getY() + .5f, .20f));
                foreach (ModelMesh modmesh in model.Meshes)
                {
                    foreach (Effect currenteffect in modmesh.Effects)
                    {
                        currenteffect.Parameters["I_a"].SetValue(new Vector4(.15f, .05f, .15f, 1f));
                        currenteffect.Parameters["I_d"].SetValue(new Vector4(.5f, .5f, .5f, 1f));
                        currenteffect.Parameters["I_s"].SetValue(new Vector4(.5f, .5f, .9f, 1f));
                        currenteffect.Parameters["k_a"].SetValue(new Vector4(.2f, .9f, .3f, 1f));
                        currenteffect.Parameters["k_d"].SetValue(new Vector4(.2f, .9f, .3f, 1f));
                        currenteffect.Parameters["k_s"].SetValue(new Vector4(.7f, .7f, .1f, 1f));
                        currenteffect.Parameters["k_r"].SetValue(new Vector4(.1f, .7f, .1f, 1f));

                        if (this.currentState == States.DAMAGED)
                        {
                            currenteffect.Parameters["k_a"].SetValue(new Vector4(.8f, .3f, .5f, 1f));
                            currenteffect.Parameters["k_d"].SetValue(new Vector4(.8f, .3f, .5f, 1f));
                            currenteffect.Parameters["I_s"].SetValue(new Vector4(.9f, .8f, .9f, 1f));
                        }
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
