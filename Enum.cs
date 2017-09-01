namespace DreamCatcher
{
    //Все перечисления в одном файле. Так ведь удобнее ^^

    /// <summary>
    /// Used to change between game states
    /// </summary>
    public enum GameState { Menu, Gameplay, Pause, Options, LevelWin, GameOver, Help, Loading, Cutscene }

    /// <summary>
    /// Type of game screen. Movable - for background. Static - for scenes
    /// </summary>
    public enum ScreenType { Movable, Static }

    /// <summary>
    /// Player and enemy state
    /// </summary>
    public enum State { Stay, Walk, Jump, Attack, Landed }

    /// <summary>
    /// Direction
    /// </summary>
    public enum Dir { Left_Down, Left, Left_Up, Right_Down, Right, Right_Up }

    /// <summary>
    /// Class of enemy
    /// </summary>
    public enum EnemyClass { Basic, Special, Boss }

    /// <summary>
    /// Supported fonts
    /// </summary>
    public enum Fonts { Arial, CurlzMT, Papyrus, Sans }

    /// <summary>
    /// In-game regions
    /// </summary>
    public enum Region { Dream, Forest, City, Cave, Ruins }


    /// <summary>
    /// In-game obstacles
    /// </summary>
    public enum ObstaclesID { Leaf_Pile, Rock_Pile, Stump, Bush, Tree, Crystal, Big_Mushroom, Giant_Mushroom };

    /// <summary>
    /// In-game enemies
    /// </summary>
    public enum EnemiesID { Shadow_Hunter, Shadow_Spider, Shadow_Bull, Shadow_Flier, Shadow_Croc, Shadow_Oak_Boss};

    /// <summary>
    /// Basic in-game objects
    /// </summary>
    public enum ObjectsID { Stoneberry, Green_Potion, Blue_Potion, Crystal, Lantern, Broken_Lantern };

    /// <summary>
    /// Special in-game objects
    /// </summary>
    public enum SpecialObjectsID { Shadow_Hunter_Mask, Shadow_Spider_Mask, Shadow_Bull_Mask, Shadow_Boss_Mask };
}