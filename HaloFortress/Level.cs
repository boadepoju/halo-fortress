using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// 
    public class Level
    {
        private List<Texture2D> tileTextures = new List<Texture2D>();

        private Queue<Vector2> waypoints = new Queue<Vector2>();

        public Queue<Vector2> Waypoints
        {
            get { return waypoints; }
        }

        public void AddTexture(Texture2D texture)
        {
            tileTextures.Add(texture);
        }

        // Constructs a "Level" class that contains waypoints 
        // used by a "ChasingSprite" class in order to traverse a path
        // on the map
        public Level()
        {
            waypoints.Enqueue(new Vector2(0, 357));
            waypoints.Enqueue(new Vector2(147, 357));
            waypoints.Enqueue(new Vector2(147, 525));
            waypoints.Enqueue(new Vector2(231, 525));
            waypoints.Enqueue(new Vector2(231, 441));
            waypoints.Enqueue(new Vector2(357, 441));
            waypoints.Enqueue(new Vector2(357, 357));
            waypoints.Enqueue(new Vector2(567, 357));
            waypoints.Enqueue(new Vector2(567, 483));
            waypoints.Enqueue(new Vector2(693, 483));
            waypoints.Enqueue(new Vector2(693, 315));
            waypoints.Enqueue(new Vector2(1100, 315));
        }
    }
}


               
        



