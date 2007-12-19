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
    public class SelectionTile : Tile
    {
        private string modelAsset = "Content/Cube";
        protected GraphicsDeviceManager graphics;
        protected ContentManager content;
        protected GraphicsDevice device;
        protected Vector4 color;

        public SelectionTile(GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice device)
        {
            this.graphics = graphics;
            this.content = content;
            this.device = device;
            this.color = new Vector4(1f, 0f, 0f, 1f);
            CompiledEffect compiledEffect = Effect.CompileEffectFromFile("@/../../../../Content/MetallicFlakes.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            this.effect = new Effect(graphics.GraphicsDevice, compiledEffect.GetEffectCode(), CompilerOptions.None, null);

            this.model = content.Load<Model>(modelAsset);
            foreach (ModelMesh modmesh in model.Meshes)
                foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
                    modmeshpart.Effect = this.effect.Clone(device);
        }

        public override bool isOccupied()
        {
            return true;
        }

        public override bool isTower()
        {
            return false;
        }

        public override Tile.TileType getTileType()
        {
            return TileType.SELECTION;
        }

        public override void updateState(float elapsedTime)
        {
            //;
        }

        public void setColor(Vector4 color)
        {
            this.color = color;
        }

        public override void draw(Microsoft.Xna.Framework.Matrix viewMatrix, Microsoft.Xna.Framework.Matrix projectionMatrix, bool showProjectile)
        {
            Matrix wm = Matrix.CreateScale(1.7f, 1.7f, .2f) * Matrix.CreateTranslation(new Vector3(this.getPosition().getX() + 1.0f, this.getPosition().getY() + 1.0f, 0.05f));

            foreach (ModelMesh modmesh in this.model.Meshes)
            {
                foreach (Effect currenteffect in modmesh.Effects)
                {
                    currenteffect.Parameters["I_a"].SetValue(this.color);
                    currenteffect.Parameters["I_d"].SetValue(new Vector4(.5f, .5f, .5f, 1f));
                    currenteffect.Parameters["I_s"].SetValue(new Vector4(.5f, .5f, .9f, 1f));
                    currenteffect.Parameters["k_a"].SetValue(new Vector4(.5f, .5f, .5f, 1f));
                    currenteffect.Parameters["k_d"].SetValue(new Vector4(.5f, .5f, .8f, 1f));
                    currenteffect.Parameters["k_s"].SetValue(new Vector4(.5f, .5f, .8f, 1f));
                    currenteffect.Parameters["k_r"].SetValue(new Vector4(.5f, .5f, .8f, 1f));
                    currenteffect.Parameters["noisescale"].SetValue(5f);
                    currenteffect.Parameters["alph"].SetValue(.5f);
                    currenteffect.Parameters["World"].SetValue(wm);
                    currenteffect.Parameters["View"].SetValue(viewMatrix);
                    currenteffect.Parameters["Projection"].SetValue(projectionMatrix);
                }
                modmesh.Draw();
            }
        }
    }
}
