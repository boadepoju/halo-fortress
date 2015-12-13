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
    /// This is the main type for your game
    /// </summary>
    /// 
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum GameState { Start, Tutorial1, Tutorial2, inGame, Paused, GameOver };
        GameState currentGameState = GameState.Start;

        Level level = new Level();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteManager spriteManager;
        int highscore;
        int highWaveScore;
        int elapsedTime;

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;

        // Cues for the various audio pieces within the game
        Cue menuCue;
        Cue inGameCue;
        Cue gameOverCue;
        Cue elite_taunt;
        Cue wortwortwort;
        Cue killEnemies;
        Cue getSuitedSpartan;
        Cue tryAgainSpartan;

        // Fonts used for the menus
        SpriteFont menuFont;
        SpriteFont inductionFont;



        public Random rnd
        {
            get;
            private set;
        }
  
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            rnd = new Random();
            graphics.PreferredBackBufferHeight = 623;
            graphics.PreferredBackBufferWidth = 1024;
            
        }

        public void PlayCue(string cueName)
        {
            soundBank.PlayCue(cueName);
        }

        public int highScore()
        { 
            int tempScore = spriteManager.finalScore();
            if (highscore < tempScore)
                highscore = tempScore;

            return highscore;
        }

        //displays the highest wave achieved
        public int highWave()
        {
            int tempWave = spriteManager.waveCountDisplay;
            if (highWaveScore < tempWave)
                highWaveScore = tempWave;

            return highWaveScore;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            spriteManager.Enabled = false;
            spriteManager.Visible = false;

         //   Components.Add(buttonManager);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");

            //the game menu font
            menuFont = Content.Load<SpriteFont>(@"Fonts\menuFont");
            inductionFont = Content.Load<SpriteFont>(@"Fonts\inductionFont");

            // set the bgm cues
            menuCue = soundBank.GetCue("Fortress_main_menu");
            inGameCue = soundBank.GetCue("in_game_track");
            gameOverCue = soundBank.GetCue("Ashes_game_over_ver");
            elite_taunt = soundBank.GetCue("elite-taunt-1");
            wortwortwort = soundBank.GetCue("wort-2");
            killEnemies = soundBank.GetCue("kill_all_the_enemies");
            getSuitedSpartan = soundBank.GetCue("get_suited_spartan"); 
            tryAgainSpartan = soundBank.GetCue("try_again_spartan");

            //begins the main menu bgm
            menuCue.Play();

            Texture2D grass, path;

            // Tile textures
            grass = Content.Load<Texture2D>(@"Images\grass");
            path = Content.Load<Texture2D>(@"Images\grass_texture");

  
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
           
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            switch (currentGameState)
            {
                case GameState.Start:
                    
                    // Gamestate for the start of the game. Takes user to the "tutorial"(1/2) screen
                    #region Option: Startgame
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        elapsedTime -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (elapsedTime <= 0)
                        {
                            currentGameState = GameState.Tutorial1;
                            getSuitedSpartan.Play();
                            elapsedTime = 200;
                        }
                    }
                    else
                    {
                        elapsedTime = 0;

                    }
                    #endregion 

                    // Directs user to our website when "tab" is pressed
                    #region Option: Our Website
                    if(Keyboard.GetState().IsKeyDown(Keys.Tab))
                    {
                        System.Diagnostics.Process.Start("http://www.teamalwaysblock.tk"); 
                    }
                    #endregion
                    #region Option: Exit game
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        this.Exit();
                    }
                    #endregion

                    break;
                case GameState.Tutorial1:

                    // Gamestate for the second "tutorial"(2/2) screen
                    #region Proceed to next Tutorial Screen

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        elapsedTime -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (elapsedTime <= 0)
                        {
                            currentGameState = GameState.Tutorial2;
                            elapsedTime = 200;
                        }
                    }
                    else
                    {
                        elapsedTime = 0;
                    }
                    break;

                    #endregion
                case GameState.Tutorial2:
                    // GameState to start the running game
                    #region Proceed to inGame

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        elapsedTime -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (elapsedTime <= 0)
                        {
                            //stops old song and plays new song
                            menuCue.Stop(AudioStopOptions.Immediate);
                            killEnemies.Play();
                            inGameCue.Play();

                            currentGameState = GameState.inGame;
                            spriteManager.Enabled = true;
                            spriteManager.Visible = true;
                            spriteManager.buttonManager.Enabled = true;
                            spriteManager.buttonManager.Visible = true;
                            elapsedTime = 200;
                        }
                    }
                    else
                    {
                        elapsedTime = 0;

                    }

                    #endregion

                    #region Go back to the first tutorial screen

                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        currentGameState = GameState.Tutorial1;
                    }

                    #endregion

                    break;
                case GameState.inGame:
                    // In-game gamestate
                    #region Gamestate: inGame
                    if (spriteManager.isGameOver() || Keyboard.GetState().IsKeyDown(Keys.Tab))
                    {
                        //stops old song and plays new song
                        inGameCue.Stop(AudioStopOptions.Immediate);
                        gameOverCue.Play();

                        currentGameState = GameState.GameOver;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                        spriteManager.buttonManager.Enabled = false;
                        spriteManager.buttonManager.Visible = false;
                        elite_taunt.Play();
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        currentGameState = GameState.Paused;
                        inGameCue.Pause();
                    }
                    break;
                    #endregion
                case GameState.Paused:
                    // Gamestate associated with the "pause" screen of the game
                    #region Gamestate: Paused
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                    spriteManager.buttonManager.Enabled = false;
                    spriteManager.buttonManager.Visible = false;

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        inGameCue.Resume();
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                        spriteManager.buttonManager.Enabled = true;
                        spriteManager.buttonManager.Visible = true;
                        currentGameState = GameState.inGame;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        soundBank.Dispose();
                        Initialize();
                        currentGameState = GameState.Start;
                    }
                    break;
                #endregion
                case GameState.GameOver:
                    #region Gamestate: GameOver
                    // Gamestate associated with the "game over" screen of the game
                    if (Keyboard.GetState().IsKeyDown(Keys.Delete))
                    {
                        //stops old song and plays new song
                        gameOverCue.Stop(AudioStopOptions.Immediate);
                        Initialize();

                        currentGameState = GameState.Start;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        //stops old song and plays new song
                        gameOverCue.Stop(AudioStopOptions.Immediate);
                        Initialize();
                        menuCue.Stop(AudioStopOptions.Immediate);
                        //LoadContent();
                        tryAgainSpartan.Play();
                        currentGameState = GameState.inGame;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                        spriteManager.buttonManager.Enabled = true;
                        spriteManager.buttonManager.Visible = true;
                        inGameCue.Play();
                    }
                    break;
                    #endregion
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            audioEngine.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.Start:
                    //Default "start" menu
                    GraphicsDevice.Clear(Color.AliceBlue);
                    Texture2D menubackground;
                    menubackground = Content.Load<Texture2D>(@"Images\space");

                    // Draw text for intro splash screen
                    spriteBatch.Begin(  );
                    string text = 
                        "Halo Fortress: Defense Evolved"+
                        "\n\r\n\r\n\r\n\rWelcome to the Beta." +
                        "\n\rStop the covenant from entering the base!" +
                        "\n\r\n\rOptions:" +
                        "\n\r\n\r\n\rPress SPACE BAR to start game" +
                        "\n\rPress TAB to go to our Website (Internet Req.)" +
                        "\n\rPress ESCAPE to exit the game";
                    spriteBatch.Draw(menubackground, Vector2.Zero, Color.White);

                    spriteBatch.DrawString(inductionFont, text,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (inductionFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (inductionFont.MeasureString(text).Y / 2) - 60),
                        Color.White);

                    spriteBatch.End(  );
                    break;

                case GameState.Tutorial1:
                    GraphicsDevice.Clear(Color.White);
                    Texture2D tutorial1background;
                    tutorial1background = Content.Load<Texture2D>(@"Images\tutorial1");

                    spriteBatch.Begin();
                    spriteBatch.Draw(tutorial1background, Vector2.Zero, Color.White);
                    spriteBatch.End();

                    break;
                case GameState.Tutorial2:
                    GraphicsDevice.Clear(Color.White);
                    Texture2D tutorial2background;
                    tutorial2background = Content.Load<Texture2D>(@"Images\tutorial2");

                    spriteBatch.Begin();
                    spriteBatch.Draw(tutorial2background, Vector2.Zero, Color.White);
                    spriteBatch.End();

                    break;
                case GameState.inGame:
                    GraphicsDevice.Clear(Color.White);  
                    Texture2D background;

                    background = Content.Load<Texture2D>(@"Images\background_standard");

                    spriteBatch.Begin();

                    // Draws the background for the game
                    spriteBatch.Draw(background, Vector2.Zero, Color.White);
            
                    spriteBatch.End();
                    break;
                case GameState.Paused:
                    GraphicsDevice.Clear(Color.Black);
                    Texture2D pausebackground;

                    pausebackground = Content.Load<Texture2D>(@"Images\pause");
                    text =
                        "Paused\n\r" +
                        "\n\r\n\r\n\r\n\rPress SPACE to resume\n\r" +
                        "Press ENTER to leave to main menu";
                    spriteBatch.Begin();
                    spriteBatch.Draw(pausebackground, Vector2.Zero, Color.White);
                    spriteBatch.DrawString(inductionFont, text,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (inductionFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (inductionFont.MeasureString(text).Y / 2) - 60),
                        Color.White);
                    spriteBatch.End();
                    break;
                case GameState.GameOver:
                    GraphicsDevice.Clear(Color.AliceBlue);
                    Texture2D gameoverbackground;

                    gameoverbackground = Content.Load<Texture2D>(@"Images\gameover");
                    spriteBatch.Begin();
                    spriteBatch.Draw(gameoverbackground, Vector2.Zero, Color.White);
                    
                    string gameover = "Game Over! YOU LOSE!!";
                    spriteBatch.DrawString(inductionFont, gameover,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (inductionFont.MeasureString(gameover).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (inductionFont.MeasureString(gameover).Y / 2)),
                        Color.White);

                    //SCORE will be based on player's score + $$$ * .5
                    gameover = "Your score: " + spriteManager.finalScore().ToString();
                    spriteBatch.DrawString(inductionFont, gameover,
                        new Vector2(((Window.ClientBounds.Width / 2) - (inductionFont.MeasureString(gameover).X / 2) - 150),
                        (Window.ClientBounds.Height / 2) - (inductionFont.MeasureString(gameover).Y / 2) + 30),
                        Color.White);

                    gameover = "Your Wave: " + spriteManager.waveCountDisplay.ToString();
                    spriteBatch.DrawString(inductionFont, gameover,
                        new Vector2(((Window.ClientBounds.Width / 2) - (inductionFont.MeasureString(gameover).X / 2) - 150),
                        (Window.ClientBounds.Height / 2) - (inductionFont.MeasureString(gameover).Y / 2) + 60),
                        Color.White);

                    gameover = "Highest Score: " + highScore().ToString();
                    spriteBatch.DrawString(inductionFont, gameover,
                        new Vector2(((Window.ClientBounds.Width / 2) - (inductionFont.MeasureString(gameover).X / 2) + 150),
                        (Window.ClientBounds.Height / 2) - (inductionFont.MeasureString(gameover).Y / 2) + 30),
                        Color.White);

                    gameover = "Highest Wave: " + highWave().ToString();
                    spriteBatch.DrawString(inductionFont, gameover,
                        new Vector2(((Window.ClientBounds.Width / 2) - (inductionFont.MeasureString(gameover).X / 2) + 150),
                        (Window.ClientBounds.Height / 2) - (inductionFont.MeasureString(gameover).Y / 2) + 60),
                        Color.White);

                    gameover = "Press R to retry and FINISH THIS FIGHT";
                    spriteBatch.DrawString(inductionFont, gameover,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (inductionFont.MeasureString(gameover).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (inductionFont.MeasureString(gameover).Y / 2) + 90),
                        Color.White);

                    gameover = "Press delete to return to the main menu";
                    spriteBatch.DrawString(menuFont, gameover,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (menuFont.MeasureString(gameover).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (menuFont.MeasureString(gameover).Y / 2) + 120),
                        Color.White);

                    spriteBatch.End(  );

                    break;
            }          
            base.Draw(gameTime);
        }
    }
}
