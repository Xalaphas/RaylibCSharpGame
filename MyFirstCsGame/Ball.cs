using System.Numerics;
using Raylib_cs;

namespace Game {
    public class Ball( Vector2 position, Color color, int radius ) : Sprite( position, color ), IMovableSprite {
    public int Radius { get; set; } = radius;
    public Vector2 Direction { get; set; } = Vector2.Zero;
    public float Speed { get; set; } = 1f;

    public override void Draw() => Raylib.DrawCircle( (int) Position.X, (int) Position.Y, Radius, this.Color );
    public void Move( float frameDeltaTimeSec ) => Position = this.Position + ( Direction * Speed * frameDeltaTimeSec );

    internal void BounceHorizontal() => Direction = this.Direction with { Y = -this.Direction.Y }; // bounce on Y

    internal void BounceVertical() => Direction = this.Direction with { X = -this.Direction.X }; // bounce on X

}
}