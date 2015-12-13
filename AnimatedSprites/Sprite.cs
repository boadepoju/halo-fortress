// Abodunrin Adepoju

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimatedSprites
{
    abstract class Sprite
    {

        Texture2D textureImage;
        protected Point frameSize;
        Point currentFrame;
        Point sheetSize;
        int collisionOffset;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 100;
        protected Vector2 speed;
        protected Vector2 position;

        protected float bulletSpeed;
        protected float spriteSpeed;

        protected bool movementChecker;

        // Variables for tracking health and money earned
        protected float enemyHealth;
        protected int bountyGiven;

        //protected bool enemyIsDead;

        protected Queue<Vector2> waypoints = new Queue<Vector2>();
        
        public string collisionCueName
        {
            get;
            private set;
        }

        public bool IsOutOfBounds(Rectangle clientRect)
        {
            if (position.X < -frameSize.X ||
                position.X > clientRect.Width ||
                position.Y < -frameSize.Y ||
                position.Y > clientRect.Height)
            {
                return true;
            }

            return false;
        }

        // Returns the enemy health
        public float GetEnemyHealth
        {
            get
            {
                return enemyHealth;
            }
            set
            {
                enemyHealth = value;
            }
        }

        // Checks to see if enemy is "dead" 
        // i.e. enemyHealth is 0 or below
        public bool IsDead
        {
            get
            {
                return enemyHealth <= 0;
            }
        }

        // Returns the framesize of the sprite
        public Point GetFrameSize
        {
            get
            {
                return frameSize;
            }
        }

        // Returns the vector position of the sprite
        public Vector2 GetPosition
        {
            get
            {
                return position;
            }
      
        }

        // Sets the waypoints for a "ChasingSprite" to traverse
        public void SetWaypoints(Queue<Vector2> waypoints)
        {

            foreach (Vector2 waypoint in waypoints)
                this.waypoints.Enqueue(waypoint);

        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame, collisionCueName)
        {

        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, string collisionCueName)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.collisionCueName = collisionCueName;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, string collisionCueName, float bulletSpeed)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.collisionCueName = collisionCueName;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.bulletSpeed = bulletSpeed;
        }

        public Sprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int millisecondsPerFrame, string collisionCueName,
            SpriteManager spriteManager, float enemyHealth, int bountyGiven, float spriteSpeed, bool movementChecker)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.collisionCueName = collisionCueName;
            this.millisecondsPerFrame = millisecondsPerFrame;
        
            // Parameters to track health and money given per enemy
            this.enemyHealth = enemyHealth;
            this.bountyGiven = bountyGiven;

            // Speed for the sprite
            this.spriteSpeed = spriteSpeed;

            // Check for movement
            this.movementChecker = movementChecker;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(textureImage,
                position,
                new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1f, SpriteEffects.None, 0);
            
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, float reference)
        {

            spriteBatch.Draw(textureImage,
                position,
                new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1f, SpriteEffects.None, reference);

        }

        public abstract Vector2 direction
        {
            get;
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    frameSize.X,
                    frameSize.Y);
            }
        }

    }
}