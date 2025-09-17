namespace breakout;

using SFML.Graphics;
using SFML.System;
using SFML.Window;

public static class Game
{
    public const int WindowW = 700;
    public const int WindowH = 800;
    const int maxFps = 60;

    private static RenderWindow _window;
    private static Clock _clock;
    
    public static Ball Ball { get; private set; }
    public static Paddle Paddle { get; private set; }
    public static List<Tile> Tiles { get; private set; }
    public static List<Powerup> Powerups { get; private set; }
    
    
    public static bool RoundStarted = false;
    public static int Score { get; private set; }
    public static int Round { get; private set; }
    public static int Health { get; private set; } = 3;
    public static int Combo = 0;
    public static float PowerUpTime { get; private set; }

    public static void Start()
    {
        using (var newWindow = new RenderWindow(new VideoMode(WindowW, WindowH), "Breakout"))
        {
            _window = newWindow;
            _window.Closed += (s, e) => _window.Close();
            _window.SetFramerateLimit(maxFps);
        
            Gui.InitGui();
            _clock = new Clock();
        
            Ball = new Ball();
            Paddle = new Paddle();
            Tiles = new List<Tile>();
            Powerups = new List<Powerup>();
        
            PrepareRound();
            Iterate();
        }
    }

    public static void BallHitBottom()
    {
        Health -= 1;
        Ball.Reset();
        RoundStarted = false;
        
        // If lost
        if (Health <= 0)
        {
            Round = 1;
            SpawnTiles(3+Round, 3+Round);
        }
    }

    private static void PrepareRound()
    {
        Round++;
        RoundStarted = false;
        PowerUpTime = 0;
        
        Ball.Reset();
        SpawnTiles(3+Round, 3+Round);
    }

    private static void StartRound()
    {
        RoundStarted = true;
        Ball.Launch();
    }

    private static void Iterate()
    {
        while (_window.IsOpen)
        {
            _window.DispatchEvents();

            if (!RoundStarted && Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                if (Health <= 0)
                {
                    Score = 0;
                    Health = 3;
                }
                StartRound();
            }

            float dt = _clock.Restart().AsSeconds();
            PowerUpTime = Math.Clamp(PowerUpTime - dt, 0, 1000);

            // Update content
            Ball.Update(dt);
            Paddle.Update(dt);

            int i = 0;
            while (i < Powerups.Count)
            {
                Powerup powerup = Powerups[i];
                powerup.Update(dt);
                if (Powerups.Contains(powerup))
                    i++;
            }
            
            UpdateTiles();

            _window.Clear(new Color(19, 41, 11));
            
            // Render content
            Ball.Draw(_window);
            Paddle.Draw(_window);
            foreach (Tile tile in Tiles)
            {
                tile.Draw(_window);
            }
            foreach (Powerup powerup in Powerups)
            {
                powerup.Draw(_window);
            }
            Gui.Draw(_window);
    
            _window.Display();
        }
    }

    public static void PowerUpHitBottom(Powerup powerup)
    {
        Powerups.Remove(powerup);
    }
    
    public static void PowerUpCollected(Powerup powerup)
    {
        Powerups.Remove(powerup);
        PowerUpTime += 4;
    }

    private static void TileDestroyed(Tile tile)
    {
        Score = Score + 100 + (25*Combo);
        Combo++;
        
        // 10% chance to spawn powerup
        bool spawnPowerup = new Random().Next(1, 11) == 1;
        if (spawnPowerup)
        {
            Powerups.Add(new Powerup(tile));
        }
    }

    private static void UpdateTiles()
    {
        int i = 0;
        
        while (i < Tiles.Count)
        {
            bool colliding = Ball.CheckCollision(Tiles[i].Sprite);
            if (colliding)
            {
                TileDestroyed(Tiles[i]);
                Tiles.RemoveAt(i);
                continue;
            }
            i++;
        }

        if (Tiles.Count == 0)
        {
            PrepareRound();
        }
    }

    private static void SpawnTiles(int xTileCount, int yTileCount)
    {
        if (RoundStarted)
            return;
        
        Tiles.Clear();

        int startGap = 15;
        int tileGap = Math.Clamp(startGap-Round, 1, startGap);
        
        int tileWidth = (WindowW - tileGap - tileGap * xTileCount) / xTileCount ;
        int tileHeight = tileWidth / 2;

        // Spawn tiles
        for (int y = 0; y < yTileCount; y++)
        {
            for (int x = 0; x < xTileCount; x++)
            {
                string texture = Tile.Textures[Tiles.Count % 3];
                
                Tiles.Add(new Tile(
                    new Vector2f(
                        tileGap + tileWidth/2 + x*tileWidth + x*tileGap,
                        tileGap + tileHeight/2 + y*tileHeight + y*tileGap),
                    texture, tileWidth ));
            }
        }
    }
    
}