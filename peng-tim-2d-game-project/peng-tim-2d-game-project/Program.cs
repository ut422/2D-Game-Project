using Raylib_cs;
using System.Numerics;

public class Program
{
    const string title = "Basic Raylib Demo";
    const int width = 800;
    const int height = 600;

    static void Main(string[] args)
    {
        Raylib.InitWindow(width, height, title);
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);
            Vector2 position = Raylib.GetMousePosition();
            Raylib.DrawCircleV(position, 50, Color.Black);
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
}