using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AnimatedSprites 
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    class Bullet : Sprite
    {
       
        protected Vector2 privatePosition;
        protected Point privateFrameSize;

        protected Vector2 velocity;

        protected Vector2 speed2 = new Vector2(1f);

        private ChasingSprite s;

        private int damage;
        private float age;

        public int Damage
        {
            get { return damage; }
        }

        // Method to "Kill" or destruct a bullet
        public void Kill()
        {
            this.age = 200;
        }

        // Returns the "age" (timer count) of a bullet
        public float GetBulletAge
        {
            get { return age; }
        }

        public Bullet(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, collisionCueName)
        {
            
        }

        public Bullet(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, string collisionCueName, float bulletSpeed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame, collisionCueName, bulletSpeed)
        {

        }

        public Bullet(Texture2D textureImage, Vector2 position, Point frameSize,
           int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame,
            string collisionCueName,
           SpriteManager spriteManager)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame, collisionCueName)
        {
        
        }

        public override Vector2 direction
        {
            get { return speed; }
        }

        // Method (1) to retrieve the destination parameters 
        public void getCollisionRect(Vector2 position1, Point frameSize1)
        {
            privatePosition = position1;
            privateFrameSize = frameSize1;
        }

        // Method (2) to retrieve destination parameters
        public void getCollisionRect3(ChasingSprite t, Point frameSize2)
        {
            privatePosition = t.GetPosition;
            privateFrameSize = frameSize2;
        }

        // Method to retrieve a destination rectangle
        public Rectangle collisionRect1
        {
            get
            {
                return new Rectangle(
                    (int)privatePosition.X,
                    (int)privatePosition.Y,
                    privateFrameSize.X,
                    privateFrameSize.Y);
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            age += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            Rectangle endzoneRecDestination = new Rectangle(990, 360, 35, 35);

            Vector2 direction1 = privatePosition - position;

            direction1.Normalize();

            // Makes the bullet move towards it's direction vector
            velocity = Vector2.Multiply(direction1, bulletSpeed);
            position += velocity;

            base.Update(gameTime, clientBounds);
        }

    }
}
