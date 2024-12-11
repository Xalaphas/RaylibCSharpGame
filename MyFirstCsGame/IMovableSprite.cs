using System.Numerics;
using Raylib_cs;

namespace Game {
    public interface IMovableSprite {

    Vector2 Direction { get; set; }
    float Speed { get; set; }
    void Move( float frameDeltaTimeSec );
}
}