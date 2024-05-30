using System;
using Raylib_cs;

class Program
{
    // screen dimensions
    const int screenWidth = 800;
    const int screenHeight = 600;

    // game state
    static bool gameLost = false;
    static bool gameWon = false;
    static bool spacePressed = false;

    static int points = 0;
    static int targetPoints = 0;
    static Random random = new Random();

    static Player player = new Player(50, 50, 50, 50, 5);
    static Enemy enemy = new Enemy(300, 300, 50, 50, 9, 9);
    static Goal goal = new Goal(700, 500, 50, 50);

    static void Main(string[] args)
    {
        // initialization
        Raylib.InitWindow(screenWidth, screenHeight, "2D Block Game");
        Raylib.SetTargetFPS(60);

        ResetGame();

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

    static void ResetGame()
    {
        // reset points
        points = 0;

        // randomize target points between 5 and 30
        targetPoints = random.Next(5, 30);

        // reset player, enemy, and goal positions
        player.Reset();
        enemy.Reset();
        goal.Reset();
    }

    static void UpdateGame()
    {
        if (!gameLost && !gameWon)
        {
            // update player
            player.Update();

            // ensure player stays within the screen bounds
            player.EnsureInBounds(screenWidth, screenHeight);

            // update enemy
            enemy.Update(screenWidth, screenHeight);

            // check collisions
            if (player.CheckCollision(enemy))
            {
                gameLost = true;
            }
            if (player.CheckCollision(goal))
            {
                // move goal to a new random position and change shape, ensuring it spawns far away from the player
                goal.Respawn(player, screenWidth, screenHeight);

                // increase points
                points++;
            }

            // check if player has reached target points
            if (points >= targetPoints)
            {
                gameWon = true;
            }
        }
        else
        {
            // prompt player to press space to continue
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                // reset game state
                gameLost = false;
                gameWon = false;
                spacePressed = true;

                ResetGame();
            }
        }
    }

    static void DrawGame()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        if (gameLost)
        {
            Raylib.DrawText("You died! Press SPACE to continue.", screenWidth / 2 - 150, screenHeight / 2, 20, Color.Red);
        }
        else if (gameWon)
        {
            Raylib.DrawText("You win! Press SPACE to play again.", screenWidth / 2 - 150, screenHeight / 2, 20, Color.Green);
        }
        else
        {
            player.Draw();
            enemy.Draw();
            goal.Draw();

            // draw points
            Raylib.DrawText($"Points: {points}/{targetPoints}", 10, 10, 20, Color.White);
        }

        Raylib.EndDrawing();
    }
}

// base class for game objects
class GameObject
{
    public Rectangle Rect;
    protected Rectangle originalRect;

    public GameObject(float x, float y, float width, float height)
    {
        Rect = new Rectangle(x, y, width, height);
        originalRect = Rect;
    }

    public void Reset()
    {
        Rect = originalRect;
    }
}

// class for the player
class Player : GameObject
{
    private int speed;

    public Player(float x, float y, float width, float height, int speed) : base(x, y, width, height)
    {
        speed = speed;
    }

    public void Update()
    {
        if (Raylib.IsKeyDown(KeyboardKey.Right)) Rect.X += speed;
        if (Raylib.IsKeyDown(KeyboardKey.Left)) Rect.X -= speed;
        if (Raylib.IsKeyDown(KeyboardKey.Up)) Rect.Y -= speed;
        if (Raylib.IsKeyDown(KeyboardKey.Down)) Rect.Y += speed;
    }

    public void EnsureInBounds(int screenWidth, int screenHeight)
    {
        if (Rect.X < 0) Rect.X = 0;
        if (Rect.X > screenWidth - Rect.Width) Rect.X = screenWidth - Rect.Width;
        if (Rect.Y < 0) Rect.Y = 0;
        if (Rect.Y > screenHeight - Rect.Height) Rect.Y = screenHeight - Rect.Height;
    }

    public bool CheckCollision(GameObject other)
    {
        return Raylib.CheckCollisionRecs(Rect, other.Rect);
    }

    public void Draw()
    {
        Raylib.DrawRectangleRec(Rect, Color.Blue);
    }
}

// class for the enemy
class Enemy : GameObject
{
    private int speedX;
    private int speedY;
    private int originalSpeedX;
    private int originalSpeedY;

    public Enemy(float x, float y, float width, float height, int speedX, int speedY) : base(x, y, width, height)
    {
        speedX = speedX;
        speedY = speedY;
        originalSpeedX = speedX;
        originalSpeedY = speedY;
    }

    public void Update(int screenWidth, int screenHeight)
    {
        Rect.X += speedX;
        Rect.Y += speedY;

        // reverse direction if enemy reaches screen boundaries
        if (Rect.X > screenWidth - Rect.Width || Rect.X < 0)
        {
            speedX = -speedX;
        }
        if (Rect.Y > screenHeight - Rect.Height || Rect.Y < 0)
        {
            speedY = -speedY;
        }
    }

    public new void Reset()
    {
        base.Reset();
        speedX = originalSpeedX;
        speedY = originalSpeedY;
    }

    public void Draw()
    {
        Raylib.DrawRectangleRec(Rect, Color.Red);
    }
}

// class for the goal
class Goal : GameObject
{
    static Random random = new Random();
    private bool isCircle;

    public Goal(float x, float y, float width, float height) : base(x, y, width, height)
    {
        isCircle = false; // initially a rectangle
    }

    public void Respawn(Player player, int screenWidth, int screenHeight)
    {
        do
        {
            Rect.X = random.Next(0, screenWidth - (int)Rect.Width);
            Rect.Y = random.Next(0, screenHeight - (int)Rect.Height);
        } while (Math.Abs(Rect.X - player.Rect.X) < 200 && Math.Abs(Rect.Y - player.Rect.Y) < 200);

        Rect.Width = random.Next(20, 100);
        Rect.Height = random.Next(20, 100);

        // randomly choose between square and circle
        isCircle = random.Next(2) == 0;
    }

    public void Draw()
    {
        if (isCircle)
        {
            Raylib.DrawCircle((int)(Rect.X + Rect.Width / 2), (int)(Rect.Y + Rect.Height / 2), Rect.Width / 2, Color.Gold);
        }
        else
        {
            Raylib.DrawRectangleRec(Rect, Color.Gold);
        }
    }
}

