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

        public Tower(GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice device)
        {
            target = null;
            this.model = content.Load<Model>("dwarfWithEffectInstance_ndx");
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
            Matrix wm = Matrix.CreateRotationX(3.14f / 2) * Matrix.CreateScale(2.0f, 2.0f, 2.0f) * Matrix.CreateTranslation(new Vector3(this.getPosition().getX() + 1, this.getPosition().getY() + 1, 0.0f));
            foreach (ModelMesh modmesh in this.model.Meshes)
            {
                foreach (Effect e in modmesh.Effects)
                {
                    //ae.CurrentTechnique = e.Techniques[0];
                    if (e is BasicEffect)
                    {
                        BasicEffect basicEffect = (BasicEffect)e;
                        basicEffect.World = wm;
                        basicEffect.View = vm;
                        basicEffect.Projection = pm;
                    }
                    else
                    {
                        /*foreach (EffectParameter ep in e.Parameters)
                        {
                            String n = ep.Name;
                            Console.Out.WriteLine(n);
                        }*/
                        e.Parameters[this.worldEffectString].SetValue(wm);
                        e.Parameters[this.viewEffectString].SetValue(vm);
                        e.Parameters[this.projectionEffectString].SetValue(pm);
                        //basicEffect.Projection = projectionMatrix;
                    }

                }
                modmesh.Draw();
            }
        }
    }
}
