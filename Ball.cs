namespace breakout;

using SFML.System;
using SFML.Graphics;

public class Ball
{
    private Sprite _sprite;
    public const float Diameter = 30;
    public const float Radius = Diameter * 0.5f;
    private Vector2f _direction;

    private bool _launched;

    public Ball()
    {
        // Create sprite
        _sprite = new Sprite();
        _sprite.Texture = new Texture("Assets/ball.png");
        _sprite.Position = new Vector2f(250, 300);
        
        // Scale sprite
        Vector2f ballTextureSize = (Vector2f) _sprite.Texture.Size;
        _sprite.Origin = 0.5f * ballTextureSize;
        _sprite.Scale = new Vector2f(
            Diameter / ballTextureSize.X,
            Diameter / ballTextureSize.Y);
    }

    public void Reset()
    {
        _launched = false;
    }
    
    public void Launch()
    {
        // Get random upward direction
        int xDir = new Random().Next(1, 3) == 1 ? 1 : -1;
        _direction = new Vector2f(xDir, 1) / MathF.Sqrt(2);
        
        _launched = true;
    }

    public void Update(float dt)
    {
        // Stick to paddle if ball hasn't been launched
        if (!Game.RoundStarted || !_launched)
        {
            _sprite.Position = new Vector2f(
                Game.Paddle.Position.X,
                Game.Paddle.Position.Y - 40);
            return;
        }
        
        // Get new pos
        Vector2f newPos = _sprite.Position;
        newPos += _direction * dt * 650.0f;

        // Stay within window bounds
        if (newPos.X > Game.WindowW - Radius)
        {
            newPos.X = Game.WindowW - Radius;
            Bounce(new Vector2f(-1, 0));
        } 
        else if (newPos.X < Radius)
        {
            newPos.X = Radius;
            Bounce(new Vector2f(1, 0));
        } 
        else if (newPos.Y > Game.WindowH - Radius)
        {
            Game.BallHitBottom(); // If ball hit bottom of window
        } 
        else if (newPos.Y < Radius)
        {
            newPos.Y = Radius;
            Bounce(new Vector2f(0, 1));
        }
        
        _sprite.Position = newPos;
    }

    public bool CheckCollision(List<Sprite> other_sprites)
    {
        foreach (Sprite other_sprite in other_sprites)
        {
            bool colliding = CheckCollision(other_sprite);
            if (colliding)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckCollision(Sprite otheSprite)
    {
        Vector2f otherSize = new Vector2f(
            otheSprite.GetGlobalBounds().Width,
            otheSprite.GetGlobalBounds().Height
        );

        Vector2f otherPos = otheSprite.Position;

        if (Collision.CircleRectangle(
                _sprite.Position, Radius,
                otherPos, otherSize, out Vector2f hit))
        {
            // Only bounce if direction and surface hit are pointing toward each other
            Vector2f normal = hit.Normalized();
            if (Collision.Dot(_direction, normal) < 0)
            {
                _sprite.Position += hit;
                Bounce(normal);
                return true;
            }
        }
        return false;
    }

    public void Bounce(Vector2f normal)
    {
        _direction -= normal * (2 * (_direction.X * normal.X + _direction.Y * normal.Y));
    }

    public void Draw(RenderTarget target)
    {
        target.Draw(_sprite);
    }
}