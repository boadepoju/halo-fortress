using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimatedSprites
{
    class AutomatedSprite : Sprite
    {
        // How much the tower costs to make
        protected int cost; 
        
        // Damage counter that the enemies take
        protected int damage;

        // Variables to check tower placement
        private bool placementCheck = false;
        private bool placementCheck1 = false;
        private bool placementCheck2 = false;
        private bool placementCheck3 = false;

        // Variable to check range of tower
        private bool isInRange = false;

        public int Cost
        {
            get
            {
                return cost;
            }
        }

        public int Damage
        {
            get
            {
                return damage;
            }
        }

        // Checks to see if tower has been placed on top of another tower 
        public bool IsPlaced
        {
            get
            {
                return placementCheck;
            }
            set
            {
                placementCheck = value;
            }
        }

        public bool IsPlaced1
        {
            get
            {
                return placementCheck1;
            }
            set
            {
                placementCheck1 = value;
            }
        }

        public bool IsPlaced2
        {
            get
            {
                return placementCheck2;
            }
            set
            {
                placementCheck2 = value;
            }
        }

        public bool IsPlaced3
        {
            get
            {
                return placementCheck3;
            }
            set
            {
                placementCheck3 = value;
            }
        }

        // Checks to see if a tower is placed within range to shoot at a 
        // "ChasingSprite" enemy
        public bool IsInRange
        {
            get
            {
                return isInRange;
            }
            set
            {
                isInRange = value;
            }
        }

        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName)
                : base(textureImage, position, frameSize, collisionOffset, currentFrame,
                sheetSize, speed, collisionCueName)
        {
        }

        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize,
           int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, string collisionCueName)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame, collisionCueName)
        {
        }

        public override Vector2 direction
        {
            get { return speed;  }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            base.Update(gameTime, clientBounds);
        }

    }
}
