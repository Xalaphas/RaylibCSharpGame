using System.Numerics;
using Raylib_cs;

namespace Game {
    public class Player( Vector2 position, Color color, Vector2 size, int limitY ) : Sprite( position, color ), IMovableSprite {
    public Vector2 Direction { get; set; }
    public float Speed { get; set; } = 1f;
    public Vector2 Size { get; set; } = size;
    public Rectangle ColisionBox => new Rectangle( this.Position, this.Size );

    public int Score { get; internal set; } = 0;

    public override void Draw() => Raylib.DrawRectangle( (int) Position.X, (int) Position.Y, (int) Size.X, (int) Size.Y, this.Color );

    public void Move( float frameDeltaTimeSec ) {
        var newPosition = this.Position + ( Direction * Speed * frameDeltaTimeSec );

        if (newPosition.Y < 0) Position = newPosition with { Y = 0 };
        else if (newPosition.Y + Size.Y > limitY) Position = newPosition with { Y = limitY - Size.Y };
        else Position = newPosition;
    }

    internal void MoveDown() => Direction = new Vector2( 0, 1 ); // increase Y to go down

    internal void MoveUp() => Direction = new Vector2( 0, -1 ); // reduce Y to go up

    internal void Stop() => Direction = Vector2.Zero;
}
}