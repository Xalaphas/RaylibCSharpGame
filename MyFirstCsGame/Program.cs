﻿using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Raylib_cs;

namespace Game {
class Program {
    const int WIN_WIDTH = 640;
    const int WIN_HEIGHT = 480;

    // center of window
    const int WIN_WIDTH_MID = WIN_WIDTH / 2;
    const int WIN_HEIGHT_MID = WIN_HEIGHT / 2;

    const int PLAYER_WIDTH = 10;
    const int PLAYER_HEIGHT = 50;
    const int BALL_SPEED = 50;
    const int SCORE_SIZE = 30;

    static readonly Random RandomGen = new Random( DateTime.Now.Microsecond ); // random number generator

    public static void Main() {
        var ball = GetNewBall( WIN_WIDTH_MID, WIN_HEIGHT_MID );

        var playerLeft = new Player( new Vector2( 10, WIN_HEIGHT_MID - ( PLAYER_WIDTH / 2 ) ), Color.Red, new Vector2( PLAYER_WIDTH, PLAYER_HEIGHT ), limitY: WIN_HEIGHT ) {
            Direction = Vector2.Zero,
            Speed = 240,
        };

        var playerRight = new Player( new Vector2( WIN_WIDTH - 20, WIN_HEIGHT_MID - ( PLAYER_WIDTH / 2 ) ), Color.Blue, new Vector2( PLAYER_WIDTH, PLAYER_HEIGHT ), limitY: WIN_HEIGHT ) {
            Direction = Vector2.Zero,
            Speed = 240,
        };

        Raylib.InitWindow( WIN_WIDTH, WIN_HEIGHT, "Multiplayer Pong" );
        Raylib.SetTargetFPS( 60 );
        Raylib.SetWindowIcon(Raylib.LoadImage("raylib.ico"));

        while (!Raylib.WindowShouldClose()) {
            var deltaTime = Raylib.GetFrameTime();

            // LEFT PLAYER
            if (Raylib.IsKeyPressed( KeyboardKey.W )) playerLeft.MoveUp();
            else if (Raylib.IsKeyPressed( KeyboardKey.S )) playerLeft.MoveDown();
            else if (Raylib.IsKeyReleased( KeyboardKey.W ) || Raylib.IsKeyReleased( KeyboardKey.S )) playerLeft.Stop();

            // RIGHT PLAYER
            if (Raylib.IsKeyPressed( KeyboardKey.Up )) playerRight.MoveUp();
            else if (Raylib.IsKeyPressed( KeyboardKey.Down )) playerRight.MoveDown();
            else if (Raylib.IsKeyReleased( KeyboardKey.Up ) || Raylib.IsKeyReleased( KeyboardKey.Down )) playerRight.Stop();

            Raylib.BeginDrawing();
            Raylib.ClearBackground( Color.Black );
            // draw center line ( before we draw ball so ball is above line)
            DrawCenterLineDashed( WIN_WIDTH_MID, WIN_HEIGHT );

            // DRAW SCORE
            Raylib.DrawText( $"{playerLeft.Score}", WIN_WIDTH_MID - ( SCORE_SIZE + 10 ), 10, SCORE_SIZE, Color.Red );
            Raylib.DrawText( $"{playerRight.Score}", WIN_WIDTH_MID + 10, 10, SCORE_SIZE, Color.Blue );

            // check for borders for colision and bouncing
            if (ball.Position.X - ball.Radius < 0 || ball.Position.X + ball.Radius > WIN_WIDTH) {
                ball.BounceVertical();
            } else if (ball.Position.Y - ball.Radius < 0 || ball.Position.Y + ball.Radius > WIN_HEIGHT) {
                ball.BounceHorizontal();
            }

            // lets move the ball
            ball.Move( deltaTime );

            // lets draw a ball
            ball.Draw();

            // move players (get keyboard inputs), check window colision
            playerLeft.Move( deltaTime );
            playerRight.Move( deltaTime );

            // show players
            playerLeft.Draw();
            playerRight.Draw();

            Raylib.EndDrawing();

            // check ball - blayer colision
            var ballRect = new Rectangle(
                ball.Position.X - ball.Radius,
                ball.Position.Y - ball.Radius,
                ball.Radius * 2,
                ball.Radius * 2
            );
            //  check: did ball hit player?
            var playerLeftColision = Raylib.CheckCollisionRecs( ballRect, playerLeft.ColisionBox );
            var playerRightColision = Raylib.CheckCollisionRecs( ballRect, playerRight.ColisionBox );
            if (playerLeftColision || playerRightColision) ball.BounceVertical();

            // check left right border colision for score
            PlayerScore score = CheckBallScorePosition( ball, WIN_WIDTH, WIN_HEIGHT );
            if (score == PlayerScore.Left) playerLeft.Score++;
            else if (score == PlayerScore.Right) playerRight.Score++;

            if (score != PlayerScore.None)// reset ball position
                ball = GetNewBall( WIN_WIDTH_MID, WIN_HEIGHT_MID );
        }

        Raylib.CloseWindow();
    }

    static Ball GetNewBall( int widthMid, int heightMid ) {
        var direction = GetRandomDirection( 4, 7 );
        return new Ball( new Vector2( widthMid, heightMid ), Color.Yellow, 10 ) {
            Direction = direction,
            Speed = BALL_SPEED
        };

    }
    static Vector2 GetRandomDirection( int min, int max ) {
        var x = RandomGen.Next( min, max );
        var y = RandomGen.Next( min, max );

        var directionX = RandomGen.Next() % 2 == 0 ? 1 : -1;
        var directionY = RandomGen.Next() % 2 == 0 ? 1 : -1;
        return new Vector2( x * directionX, y * directionY );
    }
    private static PlayerScore CheckBallScorePosition( Ball ball, int windowWidth, int wIN_HEIGHT ) {
        if (ball.Position.X - ball.Radius < 0) // ball hit left border, right player scores
            return PlayerScore.Right;
        else if (ball.Position.X + ball.Radius > windowWidth) // ball hit right border, left player scores
            return PlayerScore.Left;
        else return PlayerScore.None;
    }

    private static void DrawCenterLineDashed( int winCenter, int height ) {
        const int LINE_LENGTH = 20;
        const int LINE_SPACING = 10;

        for (var counter = 0; counter < height; counter += LINE_LENGTH + LINE_SPACING) {
            Raylib.DrawLineEx( new Vector2( winCenter, counter ), new Vector2( winCenter, counter + LINE_LENGTH ), 3, Color.Lime );
        }
    }

}

public enum PlayerScore { None, Left, Right };

}