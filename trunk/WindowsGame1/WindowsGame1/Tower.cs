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
    class Tower : Tile
    {
        Creep target;
        private string modelAsset = "Content/bigship1_ndx";
        private float scale = .13f;
        private float rot = 0;

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

        public override void draw(Matrix vm,Matrix pm)
        {
            Matrix wm = Matrix.CreateRotationZ(this.rot)*Matrix.CreateScale(this.scale, this.scale, this.scale) * Matrix.CreateTranslation(new Vector3(this.getPosition().getX() + 1.0f, this.getPosition().getY() + 1.0f, 1.0f));
            rot += .01f;
            foreach (ModelMesh modmesh in this.model.Meshes)
            {
                foreach (Effect e in modmesh.Effects)
                {
                    e.Parameters["k_s"].SetValue(new Vector4(.2f, .1f, .7f, 1f));
                    e.Parameters["World"].SetValue(wm);
                    e.Parameters["View"].SetValue(vm);
                    e.Parameters["Projection"].SetValue(pm);
                }
                modmesh.Draw();
            }
        }
    }
}
