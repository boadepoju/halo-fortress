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
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        UserControlledSprite player;

        // Fonts used for the game
        SpriteFont font;
        SpriteFont descriptionFont;

        // Lists that holds instantiations of the various classes that comprise the gameplay
        // Lists of "AutomatedSprite" are towers
        // Lists of "ChasingSprite" are enemies
        // Lists of "Bullet" are bullets
        List<ChasingSprite> spriteList = new List<ChasingSprite>();
        List<AutomatedSprite> towerList = new List<AutomatedSprite>();
        List<AutomatedSprite> slowTowerList = new List<AutomatedSprite>();
        List<AutomatedSprite> superTowerList = new List<AutomatedSprite>();
        List<AutomatedSprite> upgrade1TowerList = new List<AutomatedSprite>();
        List<AutomatedSprite> upgrade2TowerList = new List<AutomatedSprite>();
        List<AutomatedSprite> upgrade3TowerList = new List<AutomatedSprite>();
        List<AutomatedSprite> upgrade4TowerList = new List<AutomatedSprite>();
        List<Bullet> bulletList = new List<Bullet>();
        List<ExplosionSprite> explosionList = new List<ExplosionSprite>();

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;        
        Cue bonusRound;
        Cue wortwortwort;

        public ButtonManager buttonManager;

        // Rectangle boundary for the GUI upper section of the map
        Rectangle skyBox = new Rectangle(0, 0, 1500, 150);

        // Timers associated with bullet spawntimes
        float bulletTimer;
        float bulletTimer2;
        float bulletAge;
        float slowBulletTimer;
        float superBulletTimer;
        float upgrade1BulletTimer;
        float upgrade2BulletTimer;
        float upgrade3BulletTimer;
        float upgrade4BulletTimer;

        // Boolean logic to call different waves of enemies
        // "newWave" is responsible for the regular grunt enemy class
        // "bossWave" is responsible for the regular hunter enemy class
        // "nextWave" is responsible for the regular elite enemy class
        // "nextWave2" is responsible for the red elite enemy class
        // "nextWave3" is responsible for the blue grunt enemy class
        // "nextWave4" is responsible for the red jackal enemy class
        // "nextWave5" is responsible for the red hunter enemy class
        bool newWave = false;
        bool bossWave = false;
        bool nextWave = false;
        bool nextWave2 = false;
        bool nextWave3 = false;
        bool nextWave4 = false;
        bool nextWave5 = false;
        bool nextWave6 = false;
        bool nextWave7 = false;
        bool waveSweep = false;
        bool sweepBool = false;

        Level level = new Level();

        // Integers used for random enemy spawns
        int enemySpawnMinMilliseconds = 100;
        int enemySpawnMaxMilliseconds = 1000;

        // Counts the number of enemies for different wave logic
        int enemyCounter = 0;
        int enemyCounterIncreaser = 1;
        int countIncrease = 5;
        int overallWaveCounter = 0;
        int waveCount = 0;
        public int waveCountDisplay = 0;
        int waveSweepCount = 0;
        int waveBoolCount = 0;
        int waveBoolCount2 = 0;
        int waveBoolCount3 = 0;
        int waveBoolCount4 = 0;
        int waveBoolCount5 = 0;
        int waveBoolCount6 = 0;
        int waveBoolCount7 = 0;
        int waveBoolCount8 = 0;
        int waveBoolCount9 = 0;
        int redHunterCount = 0;
        int soundCueWaveElite = 0;
        int soundCueWaveRedElite = 0;

        int sweepCounter = 0;
        int sweepCounterIncreaser = 2;

        int hunterCounter = 0;
        int hunterCounterIncreaser = 3;
   

        // Health variables for enemies, used to increase enemy health periodically
        int bossHealth = 2200;
        int eliteHealth = -10;
        int sweepEnemyHealth = -10;
       
        // On-screen variables for score and running total of money
        int money = 200;
        int score = 0;

        float nextSpawnTime = 0;
        float nextSpawnTimeFF = 13000 / 2;

        // Counts your health
        float yourHealth = 100;

        // Texture for your healthbar
        Texture2D healthBar;
        
        // Vector2 for the initial position a "Bullet" class is placed at
        Vector2 positionForBullet;

        // Speed variables for bullets
        float baseTowerBulletSpeed = 3.4f * 2;
        float slowTowerBulletSpeed = 2.2f * 2;
        float superTowerBulletSpeed = 3.1f * 2;
        float upgrade1TowerBulletSpeed = 3.1f * 2;
        float upgrade2TowerBulletSpeed = 3.3f * 2;
        float upgrade3TowerBulletSpeed = 3.6f * 2;
        float upgrade4TowerBulletSpeed = 3f * 2;

        // Speed variables for enemies
        float baseJackal = 1.8f;
        float baseGhost = 1.5f;
        float baseGrunt = 3f;
        float baseHunter = 1f;
        float baseElite = 1.5f;
        float superElite = 2.1f;
        float baseFlood = 1.1f * 2;
        float superHunter = 1.2f;
        float superJackal = 3f;
        float superGrunt = 3.3f;
        float baseTowerRate = .8f / 2;
        float slowTowerRate = .33f / 2;
        float superTowerRate = .15f / 2;
        float upgrade1TowerRate = .20f / 2;
        float upgrade2TowerRate = .10f / 2;
        float upgrade3TowerRate = .08f / 2;
        float upgrade4TowerRate = .072f / 2;

        // Speed variables for bullet-rate
        float bulletAgeTimer = 2f / 2;
        

        float radius = 1000;
        float shootingRadius = 300;

        bool testBool = true;

        #region Functions

        // Function to calculate the final score
        public int finalScore()
        {
            if (score == 0)
                return 0;
            else
                return score;
        }

        /// <summary>
        /// A function that checks whether the health is zero. Returns true if so.
        /// </summary>
        /// <returns>A bool value of whether the game should end or not.</returns>
        public bool isGameOver()
        {
            if (yourHealth < 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Resets the respawn time.
        /// </summary>
        private void ResetSpawnTime()
        {
            nextSpawnTime = ((Game1)Game).rnd.Next(
                enemySpawnMinMilliseconds,
                enemySpawnMaxMilliseconds);
        }

        /// <summary>
        /// Spawns an "AutomatedSprite" base tower class with use of the "ButtonManager" class
        /// </summary>
        private void SpawnTower()
        {
           
            Vector2 speed = Vector2.Zero;
          
            Vector2 position = buttonManager.GetPosition;
            SpawnCreation(position);
            positionForBullet = position;

            Point frameSize = new Point(32, 32);
            Vector2 testposition = new Vector2(
                    -frameSize.X, ((Game1)Game).rnd.Next(250,
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight - frameSize.Y));
            
            towerList.Add(
                new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\Tower-42x42"),
                    position, new Point(42, 42), 10, new Point(0, 0), new Point(3, 1),
                    speed, 100, null));

            money -= 15;         

        }

        // Function to spawn the "AutomatedSprite" red tower class using the "ButtonManager" class
        private void SpawnSlowTower()
        {
            Vector2 speed = Vector2.Zero;

            Vector2 position = buttonManager.GetPosition;
            SpawnCreation(position);
            positionForBullet = position;

            Point frameSize = new Point(32, 32);
            Vector2 testposition = new Vector2(
                    -frameSize.X, ((Game1)Game).rnd.Next(250,
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight - frameSize.Y));

            slowTowerList.Add(
                new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\Tower_slow_42x42"),
                    position, new Point(42, 42), 10, new Point(0, 0), new Point(3, 1),
                    speed, 100, null));

            money -= 45;
        }

        // Function to spawn the "AutomatedSprite" blue tower class using the "ButtonManager" class
        private void SpawnSuperTower()
        {
            Vector2 speed = Vector2.Zero;

            Vector2 position = buttonManager.GetPosition;
            SpawnCreation(position);
            positionForBullet = position;

            Point frameSize = new Point(32, 32);
            Vector2 testposition = new Vector2(
                    -frameSize.X, ((Game1)Game).rnd.Next(250,
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight - frameSize.Y));

            superTowerList.Add(
                new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\Tower_super_42x42"),
                    position, new Point(42, 42), 10, new Point(0, 0), new Point(3, 1),
                    speed, 100, null));

            money -= 70;
        }

        // Function to spawn the "AutomatedSprite" first upgrade tower
        private void SpawnUpgrade1Tower(Vector2 upgrade1Position)
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = upgrade1Position;
            positionForBullet = position;

            Point frameSize = new Point(32, 32);
            Vector2 testposition = new Vector2(
                    -frameSize.X, ((Game1)Game).rnd.Next(250,
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight - frameSize.Y));

            upgrade1TowerList.Add(
                new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\Tower_upgrade1_42x42"),
                    position, new Point(42, 42), 10, new Point(0, 0), new Point(3, 1),
                    speed, 100, null));
        }

        // Function to spawn the "AutomatedSprite" second upgrade tower
        private void SpawnUpgrade2Tower(Vector2 upgrade1Position)
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = upgrade1Position;
            positionForBullet = position;
            SpawnCreation(position);

            Point frameSize = new Point(32, 32);
            Vector2 testposition = new Vector2(
                    -frameSize.X, ((Game1)Game).rnd.Next(250,
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight - frameSize.Y));

            upgrade2TowerList.Add(
                new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\Tower_upgrade2_42x42"),
                    position, new Point(42, 42), 10, new Point(0, 0), new Point(3, 1),
                    speed, 100, null));
        }

        // Function to spawn the "AutomatedSprite" third upgrade tower
        private void SpawnUpgrade3Tower(Vector2 upgrade1Position)
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = upgrade1Position;
            positionForBullet = position;
            SpawnCreation(position);

            Point frameSize = new Point(32, 32);
            Vector2 testposition = new Vector2(
                    -frameSize.X, ((Game1)Game).rnd.Next(250,
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight - frameSize.Y));

            upgrade3TowerList.Add(
                new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\Tower_upgrade3_42x42"),
                    position, new Point(42, 42), 10, new Point(0, 0), new Point(3, 1),
                    speed, 100, null));
        }

        // Function to spawn the "AutomatedSprite" fourth upgrade tower
        private void SpawnUpgrade4Tower(Vector2 upgrade1Position)
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = upgrade1Position;
            positionForBullet = position;
            SpawnCreation(position);

            Point frameSize = new Point(32, 32);
            Vector2 testposition = new Vector2(
                    -frameSize.X, ((Game1)Game).rnd.Next(250,
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight - frameSize.Y));

            upgrade4TowerList.Add(
                new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\Tower_upgrade4_42x42"),
                    position, new Point(42, 42), 10, new Point(0, 0), new Point(3, 1),
                    speed, 100, null));
         }

        // Spawns an explosion
        private void SpawnCollision(Vector2 explosionPosition)
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = explosionPosition;

            Point frameSize = new Point(32, 32);

            explosionList.Add(
                new ExplosionSprite(Game.Content.Load<Texture2D>(@"images\Explosion-42x42"),
                    position, new Point(42, 42), 10, new Point(0, 0), new Point(5, 1),
                    speed, 100, null));
        }

        // Spawns a creation sprite animation
        private void SpawnCreation(Vector2 creationPosition)
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = creationPosition;

            Point frameSize = new Point(32, 32);

            explosionList.Add(
                new ExplosionSprite(Game.Content.Load<Texture2D>(@"images\poof-42x42"),
                    position, new Point(42, 42), 10, new Point(0, 0), new Point(6, 1),
                    speed, 100, null));
        }

        // Function to spawn a "Bullet" class for the base tower,
        // taking in a position given by an "AutomatedSprite" class
        private void SpawnBullet(Vector2 bulletPos)
        {     
            Vector2 bulletSpeed = Vector2.Zero;

            bulletList.Add(
                new Bullet(Game.Content.Load<Texture2D>(@"images\superbullet"),
                    bulletPos, new Point(32, 32), 0, new Point(0, 0), new Point(1, 1),
                    Vector2.Zero, 100, null, baseTowerBulletSpeed));
        }

        // Function to spawn a "Bullet" class for the red tower, 
        // taking in a position given by an "AutomatedSprite" class
        private void slowSpawnBullet(Vector2 bulletPos)
        {      
            Vector2 bulletSpeed = Vector2.Zero;

            bulletList.Add(
                new Bullet(Game.Content.Load<Texture2D>(@"images\bullet"),
                    bulletPos, new Point(32, 32), 0, new Point(0, 0), new Point(1, 1),
                    Vector2.Zero, 100, null, slowTowerBulletSpeed));
        }

        // Function to spawn a "Bullet" class for the blue tower,
        // taking in a position given by an "AutomatedSprite" class
        private void superSpawnBullet(Vector2 bulletPos)
        {
            Vector2 bulletSpeed = Vector2.Zero;

            bulletList.Add(
                new Bullet(Game.Content.Load<Texture2D>(@"images\slowbullet"),
                    bulletPos, new Point(32, 32), 0, new Point(0, 0), new Point(1, 1),
                    Vector2.Zero, 100, null, superTowerBulletSpeed));
        }

         private void upgrade1SpawnBullet(Vector2 bulletPos)
        {
            Vector2 bulletSpeed = Vector2.Zero;

            bulletList.Add(
                new Bullet(Game.Content.Load<Texture2D>(@"images\upgrade1bullet"),
                    bulletPos, new Point(32, 32), 0, new Point(0, 0), new Point(1, 1),
                    Vector2.Zero, 100, null, upgrade1TowerBulletSpeed));
        }

         private void upgrade2SpawnBullet(Vector2 bulletPos)
         {
             Vector2 bulletSpeed = Vector2.Zero;

             bulletList.Add(
                 new Bullet(Game.Content.Load<Texture2D>(@"images\upgrade2bullet"),
                     bulletPos, new Point(32, 32), 0, new Point(0, 0), new Point(1, 1),
                     Vector2.Zero, 100, null, upgrade2TowerBulletSpeed));
         }

         private void upgrade3SpawnBullet(Vector2 bulletPos)
         {
             Vector2 bulletSpeed = Vector2.Zero;

             bulletList.Add(
                 new Bullet(Game.Content.Load<Texture2D>(@"images\upgrade3bullet"),
                     bulletPos, new Point(32, 32), 0, new Point(0, 0), new Point(1, 1),
                     Vector2.Zero, 100, null, upgrade3TowerBulletSpeed));
         }

         private void upgrade4SpawnBullet(Vector2 bulletPos)
         {
             Vector2 bulletSpeed = Vector2.Zero;

             bulletList.Add(
                 new Bullet(Game.Content.Load<Texture2D>(@"images\upgrade4bullet-alt"),
                     bulletPos, new Point(32, 32), 0, new Point(0, 0), new Point(1, 1),
                     Vector2.Zero, 100, null, upgrade4TowerBulletSpeed));
         }

        /// <summary>
        /// Spawns the sweeping enemies which start on the left side of the screen
        /// and move directly across
        /// </summary>
        private void SpawnSweepEnemy()
        {

            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;

            // Default size
            Point frameSize = new Point(42, 42);

            // Position for the sprite
            position = new Vector2(
                    -frameSize.X, ((Game1)Game).rnd.Next(180,
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight - frameSize.Y));

            // Create the sprite
            spriteList.Add(
                new ChasingSprite(Game.Content.Load<Texture2D>(@"images\Ghost-42x42"),
                    position, new Point(42, 42), 10, new Point(0, 0),
                    new Point(3, 1), speed, 100, "ghost-deathcry", this, sweepEnemyHealth, 10, baseGhost, false));  
                          
        }

        /// <summary>
        /// Spawns the "ChasingSprite"regular jackal enemy that occurs 
        /// within the first few waves of the game
        /// </summary>
        private void SpawnEnemy()
        {

            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;

            // Default size
            Point frameSize = new Point(42, 42);



            // Create the sprite
            spriteList.Add(
                new ChasingSprite(Game.Content.Load<Texture2D>(@"images\Jackal-42x42"),
                    level.Waypoints.Peek(), new Point(42, 42), 10, new Point(0, 0),
                    new Point(3, 1), speed, 100, "jackal-deathcry", this, 260, 10, baseJackal, true));

        }

        /// <summary>
        /// Returns the player position.
        /// </summary>
        /// <returns>Vector2 player position</returns>
        public Vector2 GetPlayerPosition()
        {
            return player.GetPosition;
        }

        #endregion Functions

        public SpriteManager(Game game)
            : base(game)
        {
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");

            player = new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"Images/refhand"),
                Vector2.Zero, new Point(32, 32), 10, new Point(0, 0),
                new Point(1, 1), new Vector2(6, 6));
            
            healthBar = Game.Content.Load<Texture2D>(@"Images\HealthBar");

            font = Game.Content.Load<SpriteFont>(@"Fonts\testSpriteFont");
            descriptionFont = Game.Content.Load<SpriteFont>(@"Fonts\descriptionFont");
            bonusRound = soundBank.GetCue("bonus_round");
            wortwortwort = soundBank.GetCue("wort-2");
            
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            buttonManager = new ButtonManager(Game);
            buttonManager.Visible = false;
            buttonManager.Enabled = false;
            Game.Components.Add(buttonManager);
            ResetSpawnTime();
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            MouseState mouse_state = Mouse.GetState();

            #region Bullet Wave Logic
            // Various timers for bullet logic; 
            // i.e. when "Bullet" classes will be instantiated
            // based upon which "AutomatedSprite" class position
            // it was given
            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            bulletTimer2 += (float)gameTime.ElapsedGameTime.TotalSeconds;
            bulletAge += (float)gameTime.ElapsedGameTime.TotalSeconds;
            slowBulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            superBulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            upgrade1BulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            upgrade2BulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            upgrade3BulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            upgrade4BulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (waveBoolCount > 2)
            {
                newWave = true;
            }

            if (waveBoolCount2 > 4)
            {
                bossWave = true;
            }

            if (waveSweepCount > 5)
            {
                waveSweep = true;
            }

            if (waveBoolCount3 > 7)
            {
                nextWave = true;
            }

            if (soundCueWaveElite > 7)
            {
                if (countIncrease == 6)
                {
                    wortwortwort.Play();
                    soundCueWaveElite = -10000;
                }
            }

            if (waveBoolCount6 > 11)
            {
                nextWave4 = true;
            }

            if (waveBoolCount4 > 14)
            {
                nextWave2 = true;
            }

            if (soundCueWaveRedElite > 27)
            {
               
                 bonusRound.Play();
                 soundCueWaveRedElite = -10000;
                
            }

            if (waveBoolCount5 > 9)
            {
                nextWave3 = true;
            }

            if (waveBoolCount6 > 20)
            {
            
                nextWave5 = true;
                //enemyCounter = enemyCounterIncreaser - 8;
            }

            if (waveBoolCount8 > 24)
            {
                nextWave6 = true;
            }

            if (waveBoolCount9 > 27)
            {
                nextWave7 = true;
                sweepBool = true;
            }

            if (redHunterCount > 20)
            {
                enemyCounter = enemyCounterIncreaser - 3;
                redHunterCount = 0;
            }

            // Timer logic for the regular bullet spawned by an "AutomatedSprite" class
            if (bulletTimer > baseTowerRate)
            {
                foreach (AutomatedSprite a in towerList)
                {
                    if (spriteList.Count() != 0)
                    SpawnBullet(a.GetPosition);
                  
                }

                bulletTimer = 0;
            }

            // Timer logic for a red bullet spawned by an "AutomatedSprite" class
            if (slowBulletTimer > slowTowerRate)
            {
                foreach (AutomatedSprite g in slowTowerList)
                {
                     if (spriteList.Count() != 0)
                        slowSpawnBullet(g.GetPosition);
                }
                slowBulletTimer = 0;
            }

            // Timer logic for a blue bullet spawned by an "AutomatedSprite" class
            if (superBulletTimer > superTowerRate)
            {
                foreach (AutomatedSprite f in superTowerList)
                {
                    if (spriteList.Count() != 0)
                        superSpawnBullet(f.GetPosition);
                }
                superBulletTimer = 0;
            }

            // Timer logic for an upgrade1 tower bullet
            if (upgrade1BulletTimer > upgrade1TowerRate)
            {
                foreach (AutomatedSprite e in upgrade1TowerList)
                {
                    if (spriteList.Count() != 0)
                        upgrade1SpawnBullet(e.GetPosition);
                }
                upgrade1BulletTimer = 0;
            }

            // Timer logic for an upgrade2 tower bullet
            if (upgrade2BulletTimer > upgrade2TowerRate)
            {
                foreach (AutomatedSprite e in upgrade2TowerList)
                {
                    if (spriteList.Count() != 0)
                        upgrade2SpawnBullet(e.GetPosition);
                }
                upgrade2BulletTimer = 0;
            }

            // Timer logic for an upgrade3 tower bullet
            if (upgrade3BulletTimer > upgrade3TowerRate)
            {
                foreach (AutomatedSprite e in upgrade3TowerList)
                {
                    if (spriteList.Count() != 0)
                        upgrade3SpawnBullet(e.GetPosition);
                }
                upgrade3BulletTimer = 0;
            }

            // Timer logic for an upgrade4 tower bullet
            if (upgrade4BulletTimer > upgrade4TowerRate)
            {
                foreach (AutomatedSprite e in upgrade4TowerList)
                {
                    if (spriteList.Count() != 0)
                        upgrade4SpawnBullet(e.GetPosition);
                }
                upgrade4BulletTimer = 0;
            } 

            #endregion Bullet Wave Logic

            #region Enemy Wave Logic
            // This region contains all of the wave spawn logic
            if (enemyCounter >= enemyCounterIncreaser)
            {
                nextSpawnTime = nextSpawnTimeFF;
                enemyCounter = 0;
                enemyCounterIncreaser += countIncrease;
                waveBoolCount += 1;
                waveBoolCount2 += 1;
                waveBoolCount3 += 1;
                waveBoolCount4 += 1;
                waveBoolCount5 += 1;
                waveBoolCount6 += 1;
                waveBoolCount7 += 1;
                waveBoolCount8 += 1;
                waveBoolCount9 += 1;
                waveSweepCount += 1;
                soundCueWaveElite += 1;
                soundCueWaveRedElite += 1;
                overallWaveCounter += 1;
                redHunterCount += 1;
                eliteHealth += 10;
                sweepEnemyHealth += 20;
                waveCountDisplay += 1;

                if (nextWave5 == true)
                {
                    sweepEnemyHealth -= 10;
                }
                if (nextWave7 == true)
                {
                    sweepCounterIncreaser += 1 ;
                    //eliteHealth += 10;
                //    sweepEnemyHealth -= 20;
                    waveSweep = true;
                }
            }
            else
               nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (nextSpawnTime < 0)
            {
                if (waveBoolCount2 > 3 || newWave == false && bossWave == false)
                {   if (nextWave == false)
                    
                        {
                            SpawnEnemy();
                            ++enemyCounter;

                            // Reset spawn timer
                            ResetSpawnTime();
                        }
                }
                if (newWave == true)
                {
                    spriteList.Add(new ChasingSprite(Game.Content.Load<Texture2D>(@"images\Grunt-42x42"),
                    level.Waypoints.Peek(), new Point(42, 42), 10, new Point(0, 0),
                    new Point(4, 1), new Vector2(1), 100, "grunt-deathcry", this, 70, 10, baseGrunt, true));

                    ++enemyCounter;

                    ++waveCount;

                    if (waveCount > 35)
                    {
                        waveBoolCount = 0;
                        newWave = false;
                        waveCount = 0;
                    }

                    ResetSpawnTime();

                }
                if (bossWave == true)
                {
                    spriteList.Add(new ChasingSprite(Game.Content.Load<Texture2D>(@"images\Hunter-42x42"),
                        level.Waypoints.Peek(), new Point(42, 42), 10, new Point(0, 0),
                        new Point(4, 1), new Vector2(1), 100, "ghost-deathcry", this, bossHealth, 10, baseHunter, true));

                    enemyCounter = enemyCounterIncreaser;
                    waveBoolCount2 = 0;
                    bossWave = false;
                    bossHealth += 200;
                    ResetSpawnTime();
                }

                if (nextWave == true && nextWave4 == false)
                {
                    spriteList.Add(new ChasingSprite(Game.Content.Load<Texture2D>(@"images\Elite-42x42"),
                        level.Waypoints.Peek(), new Point(42, 42), 10, new Point(0, 0),
                        new Point(3, 1), new Vector2(1), 100, "elite-deathcry", this, 400, 10, baseElite, true));
                    
                    ++enemyCounter;
                    countIncrease = 4;

                    ResetSpawnTime();
                }

                if (nextWave2 == true && nextWave5 == false)
                {
                    spriteList.Add(new ChasingSprite(Game.Content.Load<Texture2D>(@"images\Elite_red-42x42"),
                        level.Waypoints.Peek(), new Point(42, 42), 10, new Point(0, 0),
                        new Point(3, 1), new Vector2(1), 100, "elite-deathcry", this, eliteHealth * 1.5f, 10, superElite, true));

                   
                    ++enemyCounter;
                    countIncrease = 3;

                    ResetSpawnTime();
                }

                if (nextWave3 == true)
                {
                    spriteList.Add(new ChasingSprite(Game.Content.Load<Texture2D>(@"images\Grunt_blue-42x42"),
                        level.Waypoints.Peek(), new Point(42, 42), 10, new Point(0, 0),
                        new Point(3, 1), new Vector2(1), 100, "grunt-deathcry", this, 180, 10, superGrunt, true));

                    ++enemyCounter;

                    if (waveBoolCount5 > 15)
                    {
                        waveBoolCount5 = 11;
                    }

                    ResetSpawnTime();
                }

                if (waveSweep == true)
                {
                    SpawnSweepEnemy();

                    ++enemyCounter;
                    
                    sweepCounter++;

                    if (sweepCounter > sweepCounterIncreaser)
                    {
                        waveSweep = false;
                        if (sweepBool == false)
                        {
                            waveSweepCount = 3;
                            sweepCounter = 0;
                            sweepCounterIncreaser += 1;
                        }
                        else
                        {
                            waveSweepCount = 5;
                            sweepCounter = 0;
                            sweepCounterIncreaser += 1;
                        }
                    }

                    ResetSpawnTime();
                }

                if (nextWave4 == true && nextWave2 == false)
                {
                    spriteList.Add(new ChasingSprite(Game.Content.Load<Texture2D>(@"images\Jackal-red-42x42"),
                        level.Waypoints.Peek(), new Point(42, 42), 10, new Point(0, 0),
                        new Point(3, 1), new Vector2(1), 100, "jackal-deathcry", this, 220, 10, superJackal, true));

                    ++enemyCounter;

                    ResetSpawnTime();
                }

                if (nextWave5 == true && nextWave6 == false)
                {
                    spriteList.Add(new ChasingSprite(Game.Content.Load<Texture2D>(@"images\Flood_Jump-42x42"),
                        level.Waypoints.Peek(), new Point(42, 42), 10, new Point(0, 0), new Point(5, 1),
                        new Vector2(1), 100, "flood-deathcry", this, 750, 10, baseFlood, true));

                    ++enemyCounter;    

                    ResetSpawnTime();
                }

                if (nextWave6 == true)
                {
               
                    spriteList.Add(new ChasingSprite(Game.Content.Load<Texture2D>(@"images\Elite_red-42x42"),
                        level.Waypoints.Peek(), new Point(42, 42), 10, new Point(0, 0),
                        new Point(3, 1), new Vector2(1), 100, "elite-deathcry", this, eliteHealth * 1.75f, 10, superElite, true));

                   
                    ++enemyCounter;
                    countIncrease = 0;
                    

                    ResetSpawnTime();
                }

                if (nextWave7 == true)
                {
                    spriteList.Add(new ChasingSprite(Game.Content.Load<Texture2D>(@"images\Hunter-red-42x42"),
                        level.Waypoints.Peek(), new Point(42, 42), 10, new Point(0, 0),
                        new Point(4, 1), new Vector2(1), 100, "ghost-deathcry", this, bossHealth-1400, 10, superHunter, true));

                    ++enemyCounter;

                    hunterCounter++;

                    if (hunterCounter > hunterCounterIncreaser)
                    {
                        nextWave7 = false;
                        waveBoolCount9 = 25;
                        hunterCounter = 0;
                        hunterCounterIncreaser += 1;
                    }

                    ResetSpawnTime();
                }

            #endregion Enemy Wave Logic
            }

            //Update player
            player.Update(gameTime, Game.Window.ClientBounds);

            //Update all sprites
            for (int i = 0; i < spriteList.Count; ++i)
            {
                ChasingSprite s = spriteList[i];

                s.SetWaypoints(level.Waypoints);
                s.Update(gameTime, Game.Window.ClientBounds);

                // Checks for collisions between all "ChasingSprite" classes
                // and "AutomatedSprite" classes stored in towerList
                for (int count = 0; count < towerList.Count; ++count)
                {

                    AutomatedSprite t = towerList[count];

                    if (s.collisionRect.Intersects(t.collisionRect))
                    {
                        SpawnCollision(t.GetPosition);
                        towerList.RemoveAt(count);
                        --count;
                    }

                    //if (Vector2.Distance(t.GetPosition, s.GetPosition) <= shootingRadius)
                    //    t.IsInRange = true;
                    //else
                    //    t.IsInRange = false;


                }

                // Checks for collisions between all "ChasingSprite" classes
                // and "AutomatedSprite" classes stored in slowTowerList.
                // If a collision between their hitboxes occurs, the "AutomatedSprite"
                // is removed.
                for (int count9 = 0; count9 < slowTowerList.Count; ++count9)
                {
                    
                    AutomatedSprite h = slowTowerList[count9];

                    if (s.collisionRect.Intersects(h.collisionRect))
                    {
                        SpawnCollision(h.GetPosition);
                        slowTowerList.RemoveAt(count9);
                        --count9;
                    }

                }

                // Checks for collisions between all "ChasingSprite" classes
                // and "AutomatedSprite" classes stored in superTowerList.
                // If a collision between their hitboxes occurs, the "AutomatedSprite"
                // is removed.
                for (int count9 = 0; count9 < superTowerList.Count; ++count9)
                {
                    
                    AutomatedSprite h = superTowerList[count9];

                    if (s.collisionRect.Intersects(h.collisionRect))
                    {
                        SpawnCollision(h.GetPosition);
                        superTowerList.RemoveAt(count9);
                        --count9;
                    }
                
                }

                // Checks for collisions between all "ChasingSprite" classes
                // and "AutomatedSprite" classes stored in upgrade1TowerList.
                // If a collision between their hitboxes occurs, the "AutomatedSprite"
                // is removed.
                for (int count9 = 0; count9 < upgrade1TowerList.Count; ++count9)
                {

                    AutomatedSprite h = upgrade1TowerList[count9];

                    if (s.collisionRect.Intersects(h.collisionRect))
                    {
                        SpawnCollision(h.GetPosition);
                        upgrade1TowerList.RemoveAt(count9);
                        --count9;
                        
                    }

                }

                // Checks for collisions between all "ChasingSprite" classes
                // and "AutomatedSprite" classes stored in upgrade2TowerList.
                // If a collision between their hitboxes occurs, the "AutomatedSprite"
                // is removed.
                for (int count9 = 0; count9 < upgrade2TowerList.Count; ++count9)
                {

                    AutomatedSprite h = upgrade2TowerList[count9];


                    if (s.collisionRect.Intersects(h.collisionRect))
                    {
                        SpawnCollision(h.GetPosition);
                        upgrade2TowerList.RemoveAt(count9);
                        --count9;
                    }

                }

                // Checks for collisions between all "ChasingSprite" classes
                // and "AutomatedSprite" classes stored in upgrade3TowerList.
                // If a collision between their hitboxes occurs, the "AutomatedSprite"
                // is removed.
                for (int count9 = 0; count9 < upgrade3TowerList.Count; ++count9)
                {

                    AutomatedSprite h = upgrade3TowerList[count9];

                    if (s.collisionRect.Intersects(h.collisionRect))
                    {
                        SpawnCollision(h.GetPosition);
                        upgrade3TowerList.RemoveAt(count9);
                        --count9;
                    }

                }

                // Checks for collisions between all "ChasingSprite" classes
                // and "AutomatedSprite" classes stored in upgrade4TowerList.
                // If a collision between their hitboxes occurs, the "AutomatedSprite"
                // is removed.
                for (int count9 = 0; count9 < upgrade4TowerList.Count; ++count9)
                {

                    AutomatedSprite h = upgrade4TowerList[count9];

                    if (s.collisionRect.Intersects(h.collisionRect))
                    {
                        SpawnCollision(h.GetPosition);
                        upgrade4TowerList.RemoveAt(count9);
                        --count9;
                    }

                }
                
                // Checks for collisions between all "ChasingSprite" classes
                // and "Bullet" classes stored in bulletList.
                // If a collision between their hitboxes occurs, the "ChasingSprite"
                // class has it's 'enemyHealth' variable reduced.
                for (int count2 = 0; count2 < bulletList.Count; ++count2)
                {
                    
                    Bullet b = bulletList[count2];

                    if (s.collisionRect.Intersects(b.collisionRect))
                    {
                                bulletList.RemoveAt(count2);
                                --count2;
                                s.enemyIsDeadTest = true;          
                    }

                }
                
                // Remove object if it is out of bounds
                if (s.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    spriteList.RemoveAt(i);
                    yourHealth -= 1f;
                    --i;
                }
        
            }

            // Update score and money counters
            // Delete 'ChasingSprite" enemies that are "dead"
            for (int count6 = 0; count6 < spriteList.Count; count6++)
            {

                ChasingSprite z = spriteList[count6];
                if (z.IsDeadFlagTest == true)
                {
                    spriteList.RemoveAt(count6);
                    //plays the "ChasingSprite" deathcry
                    ((Game1)Game).PlayCue(z.collisionCueName);
                    --count6;
                    score += 1;
                    money += 2;
  
                }

            }

            // Function to aim the bullets at incoming "ChasingSprite" enemies
            // Also, deletes bullets that have been on the field for too long
            // and clears bullets when there aren't anymore "ChasingSprite"s
            for (int count7 = 0; count7 < bulletList.Count; count7++)
            {
                float smallestRange = radius;
                float smallestRange2 = shootingRadius;
                Bullet u = bulletList[count7];
                if (u.GetBulletAge > bulletAgeTimer)
                {
                    bulletList.RemoveAt(count7);
                    --count7;
                }

                if (spriteList.Count() != 0)
                {

                    foreach (ChasingSprite enemy in spriteList)
                    {
                        
                        
                        if (Vector2.Distance(u.GetPosition, enemy.GetPosition) < smallestRange)
                        {
                            smallestRange = Vector2.Distance(u.GetPosition, enemy.GetPosition);
                            u.getCollisionRect3(enemy, new Point(42, 42));
                            
                        }
                    }
                    // u.getCollisionRect3(spriteList.ElementAt<ChasingSprite>(0), new Point(42, 42));
                }
                else
                {
                    bulletList.Clear();
                }

                u.Update(gameTime, Game.Window.ClientBounds);

            }

            // Handles deletion of "ExplosionSprite" animations
            for (int count = 0; count < explosionList.Count; count++)
            {
                ExplosionSprite e = explosionList[count];
                if (e.GetExplosionAge > .65f)
                {
                    explosionList.RemoveAt(count);
                    --count;
                }
            }

            // Checks to see if a regular tower is placed on top of a secondary tower or vice-versa
            for (int count8 = 0; count8 < towerList.Count; count8++)
            {
                AutomatedSprite u = towerList[count8];

                for (int count9 = 0; count9 < slowTowerList.Count; count9++)
                {
                    AutomatedSprite z = slowTowerList[count9];

                    if (u.collisionRect == z.collisionRect)
                    {
                        u.IsPlaced = true;
                        z.IsPlaced = true;
                    }
                }
            }

            // Checks to see if a regular tower is placed on top of a super tower or vice-versa
            for (int count = 0; count < towerList.Count; count++)
            {
                AutomatedSprite t = towerList[count];

                for (int count1 = 0; count1 < superTowerList.Count; count1++)
                {
                    AutomatedSprite h = superTowerList[count1];
                    if (t.collisionRect == h.collisionRect)
                    {
                        t.IsPlaced1 = true;
                        h.IsPlaced1 = true;
                    }
                }
            }

            // Checks to see if a secondary tower is placed on top of a super tower
            for (int count = 0; count < slowTowerList.Count; count++)
            {
                AutomatedSprite t = slowTowerList[count];

                for (int count1 = 0; count1 < superTowerList.Count; count1++)
                {
                    AutomatedSprite h = superTowerList[count1];
                    if (t.collisionRect == h.collisionRect)
                    {
                        t.IsPlaced2 = true;
                        h.IsPlaced2 = true;
                    }
                }
            }

            // Checks to see if a super tower is placed on top of an upgrade1 tower
            for (int count = 0; count < upgrade1TowerList.Count; count++)
            {
                AutomatedSprite t = upgrade1TowerList[count];

                for (int count1 = 0; count1 < superTowerList.Count; count1++)
                {
                    AutomatedSprite h = superTowerList[count1];
                    if (t.collisionRect == h.collisionRect)
                    {
                        t.IsPlaced3 = true;
                        h.IsPlaced3 = true;
                    }
                }
            }

            // Removes base towers placed on top of another tower
            for (int count10 = 0; count10 < towerList.Count; count10++)
            {
                AutomatedSprite a = towerList[count10];

                if (a.collisionRect.Intersects(skyBox))
                {
                    towerList.RemoveAt(count10);
                    --count10;
                    money += 15;
                }

                if (a.IsPlaced == true)
                {
                    towerList.RemoveAt(count10);
                    --count10;
               //     SpawnCreation(a.GetPosition);
                    SpawnUpgrade1Tower(a.GetPosition);
                }

                if (a.IsPlaced1 == true)
                {
                    towerList.RemoveAt(count10);
                    --count10;
            //        SpawnCreation(a.GetPosition);
                    SpawnUpgrade2Tower(a.GetPosition);
                }

            }

            // Removes secondary towers placed on top of another tower
            for (int count11 = 0; count11 < slowTowerList.Count; count11++)
            {
                AutomatedSprite s = slowTowerList[count11];

                if (s.collisionRect.Intersects(skyBox))
                {
                    slowTowerList.RemoveAt(count11);
                    --count11;
                    money += 45;
                }

                if (s.IsPlaced == true)
                {
                    slowTowerList.RemoveAt(count11);
                    --count11;
                }

                if (s.IsPlaced2 == true)
                {
                    slowTowerList.RemoveAt(count11);
                    --count11;
              //      SpawnCreation(s.GetPosition);
                    SpawnUpgrade3Tower(s.GetPosition);
                }
            }

            // Removes super towers placed on top of another tower
            for (int count = 0; count < superTowerList.Count; count++)
            {
                AutomatedSprite s = superTowerList[count];

                if (s.collisionRect.Intersects(skyBox))
                {
                    superTowerList.RemoveAt(count);
                    --count;
                    money += 70;
                }

                if (s.IsPlaced1 == true)
                {
                    superTowerList.RemoveAt(count);
                    --count;
                }

                if (s.IsPlaced2 == true)
                {
                    superTowerList.RemoveAt(count);
                    --count;
                }
                if (s.IsPlaced3 == true)
                {
                    superTowerList.RemoveAt(count);
                    --count;
           //         SpawnCreation(s.GetPosition);
                    SpawnUpgrade4Tower(s.GetPosition);
                }
            }

            // Removes upgrade1 towers that have another tower placed on top of them
            for (int count = 0; count < upgrade1TowerList.Count; count++)
            {
                AutomatedSprite z = upgrade1TowerList[count];

                if (z.IsPlaced3 == true)
                {
                    upgrade1TowerList.RemoveAt(count);
                    --count;
                }
            }

            // Updates every "AutomatedSprite" base tower class
            foreach (AutomatedSprite e in towerList)
            {
                e.Update(gameTime, Game.Window.ClientBounds);
            }

            // Updates every "AutomatedSprite" red tower class
            foreach (AutomatedSprite f in slowTowerList)
            {
                f.Update(gameTime, Game.Window.ClientBounds);
            }

            // Updates every "AutomatedSprite" blue tower class
            foreach (AutomatedSprite g in superTowerList)
            {
                g.Update(gameTime, Game.Window.ClientBounds);
            }

            // Updates every "AutomatedSprite" upgrade1 tower class
            foreach (AutomatedSprite g in upgrade1TowerList)
            {
                g.Update(gameTime, Game.Window.ClientBounds);
            }

            // Updates every "AutomatedSprite" upgrade2 tower class
            foreach (AutomatedSprite g in upgrade2TowerList)
            {
                g.Update(gameTime, Game.Window.ClientBounds);
            }

            // Updates every "AutomatedSprite" upgrade3 tower class
            foreach (AutomatedSprite g in upgrade3TowerList)
            {
                g.Update(gameTime, Game.Window.ClientBounds);
            }

            // Updates every "AutomatedSprite" upgrade4 tower class
            foreach (AutomatedSprite g in upgrade4TowerList)
            {
                g.Update(gameTime, Game.Window.ClientBounds);
            }

            // Updates every "ExplosionSprite" animation
            foreach (ExplosionSprite e in explosionList)
            {
                e.Update(gameTime, Game.Window.ClientBounds);
            }

            // If a new "AutomatedTower" base tower class
            // is to be instantiated, SpawnTower() is called
            if (buttonManager.GetTowerCheck == true)
            {
                if (mouse_state.LeftButton == ButtonState.Released)
                {
                    if (money - 15 >= 0)
                    SpawnTower();
                }
            }

            // If a new "AutomatedTower" red tower class
            // is to be instantiated, SpawnTower() is called
            if (buttonManager.GetSlowTowerCheck == true)
            {
                if (mouse_state.LeftButton == ButtonState.Released)
                {
                    if (money - 45 >= 0)
                        SpawnSlowTower();
                }
            }

            // If a new "AutomatedTower" blue tower class
            // is to be instantiated, SpawnTower() is called
            if (buttonManager.GetSuperTowerCheck == true)
            {
                if (mouse_state.LeftButton == ButtonState.Released)
                {
                    if (money - 70 >= 0)
                        SpawnSuperTower();
                }
            }

            // Checks to see if game should be fast-forwarded
            // and contains code to fast-forward all game logic
            if (buttonManager.GetFastForwardCheck == true)
            {
                if (testBool == true)
                {
                    baseTowerBulletSpeed *= 2f;
                    slowTowerBulletSpeed *= 2f;
                    superTowerBulletSpeed *= 2f;
                    upgrade1TowerBulletSpeed *= 2f;
                    upgrade2TowerBulletSpeed *= 2f;
                    upgrade3TowerBulletSpeed *= 2f;
                    upgrade4TowerBulletSpeed *= 2f;

                    baseJackal *= 2f;
                    baseGhost *= 2f;
                    baseGrunt *= 2f;
                    baseHunter *= 2f;
                    baseElite *= 2f;
                    superElite *= 2f;
                    baseFlood *= 2f;
                    superHunter *= 2f;
                    superJackal *= 2f;
                    superGrunt *= 2f;

                    foreach (ChasingSprite enemy in spriteList)
                    {
                        enemy.GetSpriteSpeed *= 2f;
                    }

                    baseTowerRate /= 2f;
                    slowTowerRate /= 2f;
                    superTowerRate /= 2f;
                    upgrade1TowerRate /= 2f;
                    upgrade2TowerRate /= 2f;
                    upgrade3TowerRate /= 2f;
                    upgrade4TowerRate /= 2f;

                    bulletAgeTimer /= 1.985f;

                    nextSpawnTimeFF /= 2f;

                    testBool = false;
                }
                else
                {
                    baseTowerBulletSpeed /= 2f;
                    slowTowerBulletSpeed /= 2f;
                    superTowerBulletSpeed /= 2f;
                    upgrade1TowerBulletSpeed /= 2f;
                    upgrade2TowerBulletSpeed /= 2f;
                    upgrade3TowerBulletSpeed /= 2f;
                    upgrade4TowerBulletSpeed /= 2f;

                    baseJackal /= 2f;
                    baseGhost /= 2f;
                    baseGrunt /= 2f;
                    baseHunter /= 2f;
                    baseElite /= 2f;
                    superElite /= 2f;
                    baseFlood /= 2f;
                    superHunter /= 2f;
                    superJackal /= 2f;
                    superGrunt /= 2f;

                    foreach (ChasingSprite enemy in spriteList)
                    {
                        enemy.GetSpriteSpeed /= 2f;
                    }

                    baseTowerRate *= 2f;
                    slowTowerRate *= 2f;
                    superTowerRate *= 2f;
                    upgrade1TowerRate *= 2f;
                    upgrade2TowerRate *= 2f;
                    upgrade3TowerRate *= 2f;
                    upgrade4TowerRate *= 2f;

                    bulletAgeTimer *= 1.985f;

                    nextSpawnTimeFF *= 2f;

                    testBool = true;
                }

                buttonManager.GetFastForwardCheck = false;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            // Draw the player
            player.Draw(gameTime, spriteBatch, 1f);

            // Draw all enemy sprites
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);

            // Draw all towers
            foreach (AutomatedSprite a in towerList)
                a.Draw(gameTime, spriteBatch);

            // Draw all bullets
            foreach (Bullet b in bulletList)
                b.Draw(gameTime, spriteBatch);

            // Draw all secondary towers
            foreach (AutomatedSprite f in slowTowerList)
            {
                f.Draw(gameTime, spriteBatch);
            }

            // Draw all super towers
            foreach (AutomatedSprite i in superTowerList)
            {
                i.Draw(gameTime, spriteBatch);
            }

            // Draw all upgrade1 towers
            foreach (AutomatedSprite i in upgrade1TowerList)
            {
                i.Draw(gameTime, spriteBatch);
            }

            // Draw all upgrade2 towers
            foreach (AutomatedSprite i in upgrade2TowerList)
            {
                i.Draw(gameTime, spriteBatch);
            }

            // Draw all upgrade3 towers
            foreach (AutomatedSprite i in upgrade3TowerList)
            {
                i.Draw(gameTime, spriteBatch);
            }

            // Draw all upgrade4 towers
            foreach (AutomatedSprite i in upgrade4TowerList)
            {
                i.Draw(gameTime, spriteBatch);
            }

            // Draw all "ExplosionSprite" animations
            foreach (ExplosionSprite e in explosionList)
            {
                e.Draw(gameTime, spriteBatch);
            }

            // Draw the health for the health bar
            spriteBatch.Draw(healthBar, new Rectangle(240,
                45, (int)(healthBar.Width * ((double)yourHealth / 100)), 22), new Rectangle(0, 45, healthBar.Width, 44), Color.Red, 0f, Vector2.Zero, SpriteEffects.None, .01f);

            // Draw the box around the health bar
            spriteBatch.Draw(healthBar, new Rectangle(240,
                45, healthBar.Width, 22), new Rectangle(0, 0, healthBar.Width, 44), Color.White, 0f, Vector2.Zero, SpriteEffects.None, .1f);

            // Draw the tower placement buttons
            for (int i = 0; i < buttonManager.GetNUM_OF_BUTTONS; i++)
                spriteBatch.Draw(buttonManager.GetButtonTexture[i], buttonManager.GetButtonRectangle[i], null, buttonManager.GetButtonColor[i], 0f, Vector2.Zero, SpriteEffects.None, .01f);

            // Strings for descriptive text drawn on screen
            string scoreText = string.Format("Score: ");
            string moneyText = string.Format("Money:  $");
            string tower1Text = string.Format("Cost = $15");
            string tower2Text = string.Format("Cost = $45");
            string tower3Text = string.Format("Cost = $70");
            string waveCountText = string.Format("Wave: ");

            // Draw "Score: "
            spriteBatch.DrawString(font, scoreText, new Vector2(20, 30), Color.Black);
            
            // Draw score total
            spriteBatch.DrawString(font, score.ToString(), new Vector2(160, 30), Color.Black);
            
            // Draw "Money: "
            spriteBatch.DrawString(font, moneyText, new Vector2(20, 80), Color.Black);

            // Draw money total
            spriteBatch.DrawString(font, money.ToString(), new Vector2(160, 80), Color.Black);

            // Draw "Cost = $15"
            spriteBatch.DrawString(descriptionFont, tower1Text, new Vector2(930, 13), Color.Black);
            
            // Draw "Cost = $30"
            spriteBatch.DrawString(descriptionFont, tower2Text, new Vector2(845, 13), Color.Black);

            // Draw "Cost = $150"
            spriteBatch.DrawString(descriptionFont, tower3Text, new Vector2(760, 13), Color.Black);

            // Draw "Wave: "
            spriteBatch.DrawString(font, waveCountText, new Vector2(20, 130), Color.Black);

            // Draw wave total
            spriteBatch.DrawString(font, waveCountDisplay.ToString(), new Vector2(160, 130), Color.Black);
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
