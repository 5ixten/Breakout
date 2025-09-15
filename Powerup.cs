namespace breakout;

using SFML.Graphics;
using SFML.System;

public class Powerup
{
    public Sprite Sprite;
    private int _width = 50;
    private int _fallSpeed = 50;

    public Powerup(Tile tile)
    {
        Sprite = new Sprite();
        Sprite.Texture = new Texture(tile.Sprite.Texture);
        
        Vector2f textureSize = (Vector2f)Sprite.Texture.Size;
        Sprite.Origin = 0.5f * textureSize;
        
        // Scale and position
        float xScale = _width / textureSize.X;
        Sprite.Scale = new Vector2f(xScale, xScale);
        Sprite.Position = tile.Sprite.Position;
    }

    public void Update(float dt)
    {
        Sprite.Position += new Vector2f(0, _fallSpeed * dt);
        float height = Sprite.GetGlobalBounds().Height;

        // Check if hit bottom
        if (Sprite.Position.Y - height / 2 > Game.WindowH)
        {
            Game.PowerUpHitBottom(this);
        }

        // Check if hit paddle
        if (Sprite.GetGlobalBounds().Intersects(Game.Paddle.Sprite.GetGlobalBounds()))
        {
            Game.PowerUpCollected(this);
        }
    }

    public void Draw(RenderTarget target)
    {
        target.Draw(Sprite);
    }
}