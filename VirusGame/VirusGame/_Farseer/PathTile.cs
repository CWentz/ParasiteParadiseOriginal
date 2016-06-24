using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;

namespace VirusGame
{
    /// <summary>
    /// Class representing a collidable path of vectors that
    /// it creates from information given by MyLevel.cs
    /// </summary>
    class PathTile
    {
        public Body body;

        public Vector2 Position
        {
            get { return body.Position; }
        }

        public PathTile(List<Vector2> localPoints, Vector2 position, World world, bool MakroCollision = false)
        {
            body = new Body(world);
            body.Position = ConvertUnits.ToSimUnits(position);
            body.UserData = "Wall";
            body.IsStatic = true;

            Vertices terrain = new Vertices();

            foreach (Vector2 point in localPoints)
            {
                terrain.Add(ConvertUnits.ToSimUnits(point));
            }

            for (int i = 0; i < terrain.Count - 1; ++i)
            {
                FixtureFactory.AttachEdge(terrain[i], terrain[i + 1], body);
                body.FixtureList[i].UserData = "Wall";
            }
            body.Restitution = 0f;
            body.Friction = float.MaxValue;
            if (!MakroCollision)
                body.CollisionCategories = Category.Cat15 & ~Category.Cat3;
            else
                body.CollidesWith = Category.Cat29;
            
        }

        public void Draw(LineBatch lineBatch)
        {
            //this is straigth from Farseer 3.3.1 game samples
            //just to indicate where the path lies, and actually it draws it too low
            //lineBatch.Begin();
            //// draw ground
            //for (int i = 0; i < body.FixtureList.Count; ++i)
            //{
            //    lineBatch.DrawLineShape(body.FixtureList[i].Shape, Color.Black);
            //}
            //lineBatch.End();
        }
    }
}
