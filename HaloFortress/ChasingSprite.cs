using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimatedSprites
{
    class ChasingSprite : Sprite
    {
     
        protected bool alive = true;

        protected Vector2 position1 = Vector2.Zero;
        protected Vector2 velocity;
        protected float speed1 = 1.5f;

        protected bool enemyIsDead = false;

        protected bool isDeadFlag = false;

        SpriteManager spriteManager;

        public float GetSpriteSpeed
        {
            get
            {
                return spriteSpeed;
            }
            set
            {
                spriteSpeed = value;
            }
        }

        public ChasingSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int millisecondsPerFrame, string collisionCueName,
            SpriteManager spriteManager, float enemyHealth, int bountyGiven, float spriteSpeed, bool movementChecker)
            : base(textureImage, position, frameSize, collisionOffset,
            currentFrame, sheetSize, speed, millisecondsPerFrame, collisionCueName, spriteManager, enemyHealth, bountyGiven, spriteSpeed, movementChecker)
        {
            this.spriteManager = spriteManager;
        }

        public ChasingSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int millisecondsPerFrame,
            string collisionCueName,
            SpriteManager spriteManager)
            : base(textureImage, position, frameSize, collisionOffset,
            currentFrame, sheetSize, speed, millisecondsPerFrame, collisionCueName)
        {
            this.spriteManager = spriteManager;
        }

        public float DistanceToDestination
        {
            get { return Vector2.Distance(position, waypoints.Peek()); }
        }

        public override Vector2 direction
        {
            get { return speed; }
        }

        public bool enemyIsDeadTest
        {
            get
            {
                return enemyIsDead;
            }
            set
            {
                enemyIsDead = value;
            }
        }

        public bool IsDeadFlagTest
        {
            get
            {
                return isDeadFlag;
            }
            set
            {
                isDeadFlag = value;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (movementChecker == true)
            {
                if (waypoints.Count > 0)
                {
                    if (DistanceToDestination < 6f)
                    {
                        position = waypoints.Peek();
                        waypoints.Dequeue();
                    }
                    else
                    {
                        Vector2 direction1 = waypoints.Peek() - position;
                        direction1.Normalize();

                        velocity = Vector2.Multiply(direction1, spriteSpeed);

                        position += velocity;
                    }
                }
            }
            else
            {
                position.X += spriteSpeed;
            }



            if (enemyIsDead == true)
            {
                enemyHealth -= 10;
                enemyIsDead = false;
            }

            if (enemyHealth < 0)
            {
                isDeadFlag = true;
            }


            base.Update(gameTime, clientBounds);
        }

       
    }
}
