namespace breakout;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

public class Tile
{
    public Sprite Sprite;
    public static string[] Textures = new string[]
    {
        "Assets/tilePink.png",
        "Assets/tileGreen.png",
        "Assets/tileBlue.png",
    };

    public Tile(Vector2f pos, string texture, int width = 50)
    {
        Sprite = new Sprite();
        Sprite.Texture = new Texture(texture);
        Sprite.Position = pos;
        
        Vector2f textureSize = (Vector2f)Sprite.Texture.Size;
        Sprite.Origin = 0.5f * textureSize;
        
        float xScale = width / textureSize.X;
        Sprite.Scale = new Vector2f(xScale, xScale);
    }
    
    public void Draw(RenderTarget target)
    {
        target.Draw(Sprite);
    }
}