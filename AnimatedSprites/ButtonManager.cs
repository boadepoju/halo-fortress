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
    public class ButtonManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
            // TODO: Construct any child components here

            public bool tower_check = false;
            public bool slowTower_check = false;
            public bool superTower_check = false;
            public bool fastForward_check = false;

            public bool fastForward_bool = false;
       

            Vector2 position;

            // Return tower_check
             public bool GetTowerCheck
             {
                 get
                 {
                     return tower_check;
                 }
             }

             public bool GetSlowTowerCheck
             {
                 get
                 {
                     return slowTower_check;
                 }
             }

             public bool GetSuperTowerCheck
             {
                 get
                 {
                     return superTower_check;
                 }
             }

             public bool GetFastForwardCheck
             {
                 get
                 {
                     return fastForward_check;
                 }
                 set
                 {
                     fastForward_check = value;
                 }
             }

             public bool GetFastForwardBool
             {
                 get
                 {
                     return fastForward_bool;
                 }
             }

             public Vector2 GetPosition
             {
                 get
                 {
                     return position;
                 }
             }

             public int GetNUM_OF_BUTTONS
             {
                 get
                 {
                     return NUM_OF_BUTTONS;
                 }
             }

             public Texture2D[] GetButtonTexture
             {
                 get
                 {
                     return button_texture;
                 }
             }

             public Color[] GetButtonColor
             {
                 get
                 {
                     return button_color;
                 }
             }

             public Rectangle[] GetButtonRectangle
             {
                 get
                 {
                     return button_rectangle;
                 }
             }



            // Variables for button start

            

            enum BState
            {
                HOVER,
                UP,
                JUST_RELEASED,
                DOWN
            }

            const int NUM_OF_BUTTONS = 4, 
                TOWER_BUTTON_INDEX = 0,
                SLOWTOWER_BUTTON_INDEX = 1,
                SUPERTOWER_BUTTON_INDEX = 2,
                FASTFORWARD_BUTTON_INDEX = 3,
                BUTTON_HEIGHT = 60,
                BUTTON_WIDTH = 50;

           Texture2D[] button_texture = new Texture2D[NUM_OF_BUTTONS];
           Rectangle[] button_rectangle = new Rectangle[NUM_OF_BUTTONS];
           double[] button_timer = new double[NUM_OF_BUTTONS];
           BState[] button_state = new BState[NUM_OF_BUTTONS];
           Color[] button_color = new Color[NUM_OF_BUTTONS];
           Color background_color;

        // mouse pressed and mouse just pressed
        bool mpressed, prev_mpressed = false;

        // mouse location in window
        int mx, my;
        double frame_time;

          // wrapper for hit_image_alpha taking Rectangle and Texture
         Boolean hit_image_alpha(Rectangle rect, Texture2D tex, int x, int y)
         {
             return hit_image_alpha(0, 0, tex, tex.Width * (x - rect.X) /
                 rect.Width, tex.Height * (y - rect.Y) / rect.Height);
         }

         // wraps hit_image then determines if hit a transparent part of image
         Boolean hit_image_alpha(float tx, float ty, Texture2D tex, int x, int y)
         {
             if (hit_image(tx, ty, tex, x, y))
             {
                 uint[] data = new uint[tex.Width * tex.Height];
                 tex.GetData<uint>(data);
                 if ((x - (int)tx) + (y - (int)ty) *
                     tex.Width < tex.Width * tex.Height)
                 {
                     return ((data[
                         (x - (int)tx) + (y - (int)ty) * tex.Width
                         ] &
                                 0xFF000000) >> 24) > 20;
                 }
             }
             return false;
         }

         // determine if x,y is within rectangle formed by texture located at tx,ty
         Boolean hit_image(float tx, float ty, Texture2D tex, int x, int y)
         {
             return (x >= tx &&
                 x <= tx + tex.Width &&
                 y >= ty &&
                 y <= ty + tex.Height);
         }

         // determine state and color of button
         void update_buttons()
         {
             for (int i = 0; i < NUM_OF_BUTTONS; i++)
             {

                 if (hit_image_alpha(
                     button_rectangle[i], button_texture[i], mx, my))
                 {
                     button_timer[i] = 0.0;
                     if (mpressed)
                     {
                         // mouse is currently down
                         button_state[i] = BState.DOWN;
                         button_color[i] = Color.Blue;
                         if (button_state[0] == BState.DOWN)
                             tower_check = true;
                         else if (button_state[1] == BState.DOWN)
                         {
                             slowTower_check = true;
                         }
                         else if (button_state[2] == BState.DOWN)
                         {
                             superTower_check = true;
                         }
                         else
                         {
                             fastForward_check = true;
                             //fastForward_bool = !fastForward_bool;
                             //fastForward_check = !fastForward_check;
                         }
                     }
                     else if (!mpressed && prev_mpressed)
                     {
                         // mouse was just released
                         if (button_state[i] == BState.DOWN)
                         {
                             // button i was just down
                             button_state[i] = BState.JUST_RELEASED;
                         }
                     }
                     else
                     {
                         button_state[i] = BState.HOVER;
                         button_color[i] = Color.LightBlue;
                     }
                 }
                 else
                 {
                     button_state[i] = BState.UP;
                     if (button_timer[i] > 0)
                     {
                         button_timer[i] = button_timer[i] - frame_time;
                     }
                     else
                     {
                         button_color[i] = Color.White;
                     }
                 }

                 if (button_state[i] == BState.JUST_RELEASED)
                 {
                     take_action_on_button(i);
                 }
             }
         }

         // Logic for each button click goes here
         void take_action_on_button(int i)
         {
             //take action corresponding to which button was clicked
             switch (i)
             {
                 case TOWER_BUTTON_INDEX:
                     background_color = Color.Green;
                     break;
                 case SLOWTOWER_BUTTON_INDEX:
                     background_color = Color.Green;
                     break;
                 case SUPERTOWER_BUTTON_INDEX:
                     background_color = Color.Green;
                     break;
                 case FASTFORWARD_BUTTON_INDEX:
                     background_color = Color.Green;
                     break;
                 default:
                     break;
             }
         }


        // Button code end

        public ButtonManager(Game game)
            : base(game)
        {

        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            button_texture[TOWER_BUTTON_INDEX] =
            Game.Content.Load<Texture2D>(@"images/tower-button");

            button_texture[SLOWTOWER_BUTTON_INDEX] =
            Game.Content.Load<Texture2D>(@"images\slowtower-button");

            button_texture[SUPERTOWER_BUTTON_INDEX] =
            Game.Content.Load<Texture2D>(@"images\supertower-button");

            button_texture[FASTFORWARD_BUTTON_INDEX] =
            Game.Content.Load<Texture2D>(@"images\fastforward2");
            
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // starting x and y locations to stack buttons
            // vertically in the middle of the screen
            //int x = Game.Window.ClientBounds.Width / 2 - BUTTON_WIDTH / 2;
            //int y = Game.Window.ClientBounds.Height / 2 -
            //    NUM_OF_BUTTONS / 2 * BUTTON_HEIGHT -
            //    (NUM_OF_BUTTONS % 2) * BUTTON_HEIGHT / 2;

            int x = 930;
            int y = 30;

            // TODO: Add your initialization code here
            for (int i = 0; i < NUM_OF_BUTTONS; i++)
            {
                button_timer[i] = 0.0;
                if (i < 3)
                {
                    button_rectangle[i] = new Rectangle(x, y, BUTTON_WIDTH, BUTTON_HEIGHT);
                }
                else
                {
                    button_rectangle[i] = new Rectangle(x, y, 30, 30);
                }
                if (i < 2)
                {
                    x -= 85;
                    y = 30;
                }
                else
                {
                    x -= 520;
                    y += 60;
                }
            }

            base.Initialize();
        }

        



        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            // get elapsed frame time in seconds
            frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

            // update mouse variables
            MouseState mouse_state = Mouse.GetState();
            mx = mouse_state.X;
            my = mouse_state.Y;
            prev_mpressed = mpressed;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;

            update_buttons();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            Texture2D square;
            Texture2D slowTowerSquare;
            Texture2D superTowerSquare;

            Vector2 squarePosition;

            // Tile-based snapping variables

            int TileX;
            int TileY;
            int tileWidth = 42;
            int tileHeight = 42;

            // Images for buttons

            square = Game.Content.Load<Texture2D>(@"Images\Tower-42x42-single");
            slowTowerSquare = Game.Content.Load<Texture2D>(@"Images\Tower_slow_single");
            superTowerSquare = Game.Content.Load<Texture2D>(@"Images\Tower_super_single");

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            //for (int i = 0; i < NUM_OF_BUTTONS; i++)
            //    spriteBatch.Draw(button_texture[i], button_rectangle[i], null, button_color[i], 0f, Vector2.Zero, SpriteEffects.None, .01f);

             //spriteBatch.Draw(healthBar, new Rectangle(240,
             //   45, healthBar.Width, 22), new Rectangle(0, 0, healthBar.Width, 44), Color.White, 0f, Vector2.Zero, SpriteEffects.None, .1f);

           // Color.White, 0f, Vector2.Zero, SpriteEffects.None, .1f);

            if (tower_check == true)
            {

                MouseState mouse = Mouse.GetState();

                TileX = (int)Math.Floor((float)(mouse.X / tileWidth));
                TileY = (int)Math.Floor((float)(mouse.Y / tileHeight));

                squarePosition.X = (TileX * tileWidth) + (tileWidth / 2);
                squarePosition.Y = (TileY * tileHeight) + (tileHeight / 2);

                position = new Vector2(squarePosition.X, squarePosition.Y);

                spriteBatch.Draw(square, squarePosition, null, Color.White, 0, new Vector2(square.Width / 2, square.Height / 2), 1, SpriteEffects.None, 1);

                if (mouse.LeftButton == ButtonState.Released)
                    tower_check = false;
            }

            if (slowTower_check == true)
            {
                MouseState mouse = Mouse.GetState();
                TileX = (int)Math.Floor((float)(mouse.X / tileWidth));
                TileY = (int)Math.Floor((float)(mouse.Y / tileHeight));

                squarePosition.X = (TileX * tileWidth) + (tileWidth / 2);
                squarePosition.Y = (TileY * tileHeight) + (tileHeight / 2);

                position = new Vector2(squarePosition.X, squarePosition.Y);

                spriteBatch.Draw(slowTowerSquare, squarePosition, null, Color.White, 0, new Vector2(square.Width / 2, square.Height / 2), 1, SpriteEffects.None, 1);

                if (mouse.LeftButton == ButtonState.Released)
                    slowTower_check = false;
            }

            if (superTower_check == true)
            {
                MouseState mouse = Mouse.GetState();
                TileX = (int)Math.Floor((float)(mouse.X / tileWidth));
                TileY = (int)Math.Floor((float)(mouse.Y / tileHeight));

                squarePosition.X = (TileX * tileWidth) + (tileWidth / 2);
                squarePosition.Y = (TileY * tileHeight) + (tileHeight / 2);

                position = new Vector2(squarePosition.X, squarePosition.Y);

                spriteBatch.Draw(superTowerSquare, squarePosition, null, Color.White, 0, new Vector2(square.Width / 2, square.Height / 2), 1, SpriteEffects.None, 1);

                if (mouse.LeftButton == ButtonState.Released)
                    superTower_check = false;

            }

            spriteBatch.End();
            base.Draw(gameTime);

        }
            
     }
}
