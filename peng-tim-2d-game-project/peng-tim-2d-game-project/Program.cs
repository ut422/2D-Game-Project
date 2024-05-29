using System;
using Raylib_cs;

class Program
{
    // screen dimensions
    const int screenWidth = 800;
    const int screenHeight = 600;

    // player/enemy settings
    static Rectangle player = new Rectangle(50, 50, 50, 50);
    static Rectangle enemy = new Rectangle(300, 300, 50, 50);
    static Rectangle goal = new Rectangle(700, 500, 50, 50);
    static int playerSpeed = 5;
    static int enemySpeed = 9;

    // original positions
    static Rectangle originalPlayer = player;
    static Rectangle originalEnemy = enemy;
    static Rectangle originalGoal = goal;

    // game state
    static bool gameLost = false;
    static bool spacePressed = false;

    static Random random = new Random();

    static void Main(string[] args)
    {
        // initialization
        Raylib.InitWindow(screenWidth, screenHeight, "Simple 2D Game");
        Raylib.SetTargetFPS(60);

        // game loop
        while (!Raylib.WindowShouldClose())
        {
            // update game state
            UpdateGame();

            // draw everything
            DrawGame();
        }

        // close window and unload resources
        Raylib.CloseWindow();
    }

    static void UpdateGame()
    {
        if (!gameLost)
        {
            // player movement
            if (Raylib.IsKeyDown(KeyboardKey.Right)) player.X += playerSpeed;
            if (Raylib.IsKeyDown(KeyboardKey.Left)) player.X -= playerSpeed;
            if (Raylib.IsKeyDown(KeyboardKey.Up)) player.Y -= playerSpeed;
            if (Raylib.IsKeyDown(KeyboardKey.Down)) player.Y += playerSpeed;

            // ensure player stays within the screen bounds
            if (player.X < 0) player.X = 0;
            if (player.X > screenWidth - player.Width) player.X = screenWidth - player.Width;
            if (player.Y < 0) player.Y = 0;
            if (player.Y > screenHeight - player.Height) player.Y = screenHeight - player.Height;

            // enemy movement (simple horizontal oscillation)
            enemy.X += enemySpeed;
            if (enemy.X > screenWidth - enemy.Width || enemy.X < 0)
            {
                enemySpeed = -enemySpeed;
            }

            // check collisions
            if (Raylib.CheckCollisionRecs(player, enemy))
            {
                gameLost = true;
            }
            if (Raylib.CheckCollisionRecs(player, goal))
            {
                // move goal to a new random position and change size, ensuring it spawns far away from the player
                do
                {
                    goal.X = random.Next(0, screenWidth - (int)goal.Width);
                    goal.Y = random.Next(0, screenHeight - (int)goal.Height);
                } while (Math.Abs(goal.X - player.X) < 200 && Math.Abs(goal.Y - player.Y) < 200);

                goal.Width = random.Next(20, 100);
                goal.Height = random.Next(20, 100);
            }
        }
        else
        {
            // prompt player to press space to continue
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                // reset game state
                gameLost = false;
                spacePressed = true;

                // reset player, enemy, and goal positions
                player = originalPlayer;
                enemy = originalEnemy;
                goal = originalGoal;
            }
        }
    }

    static void DrawGame()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        // you died
        if (gameLost)
        {
            Raylib.DrawText("you the died! press SPACE to continue.", screenWidth / 2 - 150, screenHeight / 2, 20, Color.Red);
        }
        else
        {
            Raylib.DrawRectangleRec(player, Color.Blue);
            Raylib.DrawRectangleRec(enemy, Color.Red);
            Raylib.DrawRectangleRec(goal, Color.Gold);
        }

        Raylib.EndDrawing();
    }
}
