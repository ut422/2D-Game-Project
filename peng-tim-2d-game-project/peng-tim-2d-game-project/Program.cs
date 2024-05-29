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

 