namespace breakout;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

public class Paddle
{
    public Sprite Sprite;
    private float _xVelocity;

    private float _width = 100;
    private float _friction = 0.9f;

    private Vector2f originalScale;
    
    public Vector2f Position { get { return Sprite.Position; } }

    public Paddle()
    {
        Sprite = new Sprite();
        Sprite.Texture = new Texture("Assets/paddle.png");
        Sprite.Position = new Vector2f(Game.WindowW / 2, 700);
        
        Vector2f textureSize = (Vector2f)Sprite.Texture.Size;
        Sprite.Origin = 0.5f * textureSize;
        
        float xScale = _width / textureSize.X;
        Sprite.Scale = new Vector2f(xScale, xScale);
        originalScale = Sprite.Scale;
    }

    public void Update(float dt)
    {
        Sprite.Scale = Game.PowerUpTime <= 0 ? originalScale : originalScale * 1.5f;
        
        bool leftPressed = Keyboard.IsKeyPressed(Keyboard.Key.Left) || Keyboard.IsKeyPressed(Keyboard.Key.A);
        bool rightPressed = Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed(Keyboard.Key.D);
        
        int direction = 0;
        direction += leftPressed ? -1 : 0;
        direction += rightPressed ? 1 : 0;
        _xVelocity += direction * dt * 75;

        _xVelocity *= (_friction * (1+dt));
        
        StayInBounds();
        Sprite.Position += new Vector2f(_xVelocity, 0);
        
        bool hit = Game.Ball.CheckCollision(Sprite);
        if (hit)
        {
            Game.Combo = 0;
        
        Console.WriteLine(hit);
            
        }
    }

    private void StayInBounds()
    {
        if (Sprite.Position.X - _width / 2 < 0)
        {
            Sprite.Position = new Vector2f(_width / 2, Sprite.Position.Y);
            _xVelocity = Math.Clamp(-_xVelocity, 6, 1000);
        } else if (Sprite.Position.X + _width / 2 > Game.WindowW)
        {
            Sprite.Position = new Vector2f(Game.WindowW - _width / 2, Sprite.Position.Y);
            _xVelocity = Math.Clamp(-_xVelocity, -1000, -6);
        }
    }
    
    public void Draw(RenderTarget target)
    {
        target.Draw(Sprite);
    }
}