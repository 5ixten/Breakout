using SFML.System;

namespace breakout;

using SFML.Graphics;

public static class Gui
{
    public static Text _mainText;
    public static Text _backgroundText;

    public static void InitGui()
    {
        _mainText = new Text();
        _mainText.CharacterSize = 34;
        _mainText.Font = new Font("Assets/future.ttf");
        
        _backgroundText = new Text();
        _backgroundText.CharacterSize = 44;
        _backgroundText.Font = new Font("Assets/future.ttf");
    }

    public static void Draw(RenderTarget target)
    {
        // Score text
        _mainText.DisplayedString = $"Score: {Game.Score}";
        _mainText.FillColor = Color.Black;
        _mainText.Position = new Vector2f(20, Game.WindowH - 54);
        target.Draw(_mainText);
        
        // Health text
        _mainText.DisplayedString = $"Health: {Game.Health}";
        _mainText.FillColor = Color.Red;
        _mainText.Position = new Vector2f(
            Game.WindowW - _mainText.GetGlobalBounds().Width - 20,
            Game.WindowH - 54);
        target.Draw(_mainText);

        if (!Game.RoundStarted)
        {
            _backgroundText.DisplayedString = $"Launch ball [space]";
            _backgroundText.FillColor = Color.White;
            _backgroundText.Position = new Vector2f(
                Game.WindowW/2 - _backgroundText.GetGlobalBounds().Width/2,
                Game.WindowH / 1.5f);
            target.Draw(_backgroundText);
        }
        
        if (Game.Health == 0)
        {
            _backgroundText.DisplayedString = $"Game over";
            _backgroundText.FillColor = Color.Red;
            _backgroundText.Position = new Vector2f(
                Game.WindowW/2 - _backgroundText.GetGlobalBounds().Width/2,
                Game.WindowH / 2);
            target.Draw(_backgroundText);
        }
    }
}