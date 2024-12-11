using System.Numerics;
using Raylib_cs;

namespace Game {
    public abstract class Sprite( Vector2 position, Color color ) {
    public Vector2 Position { get; set; } = position;
    public Color Color { get; set; } = color;

    public abstract void Draw();    // draw sprite
}
}