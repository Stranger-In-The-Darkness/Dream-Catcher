namespace DreamCatcher
{
    //Все перечисления в одном файле. Так ведь удобнее ^^

    /// <summary>
    /// Used to change between game states
    /// </summary>
    public enum GameState
    {
        Menu,
        Gameplay,
        Pause,
        Options,
        LevelWin,
        GameOver,
        Help,
        Loading,
        Cutscene
    }

    /// <summary>
    /// Type of game screen
    /// </summary>
    public enum ScreenType
    {
        /// <summary>
        /// На задний фон
        /// </summary>
        Movable,

        /// <summary>
        /// Для статичных изображений
        /// </summary>
        Static
    }

    /// <summary>
    /// Player and enemy state
    /// </summary>
    public enum State
    {
        Stay,
        Walk,
        Jump,
        Attack,
        Landed
    }

    /// <summary>
    /// Direction
    /// </summary>
    public enum Dir
    {
        Down,
        Left_Down,
        Left,
        Left_Up,
        Up,
        Right_Up,
        Right,
        Right_Down
    }

    /// <summary>
    /// Class of enemy
    /// </summary>
    public enum EnemyClass
    {
        /// <summary>
        /// Просто ходит по заданому маршруту
        /// </summary>
        Basic,

        /// <summary>
        /// Преследует игрока
        /// </summary>
        Special,

        /// <summary>
        /// Босс. Поведение зависит от самого босса
        /// </summary>
        Boss
    }

    /// <summary>
    /// Supported fonts
    /// </summary>
    public enum Fonts
    {
        Arial,
        CurlzMT,
        Papyrus,
        Sans,
        Gigi,
        Chiller
    }

    /// <summary>
    /// In-game regions
    /// </summary>
    public enum Region
    {
        Dream,
        Forest,
        City,
        Cave,
        Ruins
    }


    /// <summary>
    /// In-game obstacles
    /// </summary>
    public enum ObstaclesID { Leaf_Pile, Rock_Pile, Stump, Bush, Tree, Crystal, Big_Mushroom, Giant_Mushroom };

    /// <summary>
    /// In-game enemies
    /// </summary>
    public enum EnemiesID
    {
        Shadow_Hunter,
        Shadow_Spider,
        Shadow_Horner,

        /// <summary>
        /// Предполагаемый летучий враг
        /// </summary>
        Shadow_Flier,

        /// <summary>
        /// Как варинат, Тень-крокодил
        /// </summary>
        Shadow_Croc,
        Shadow_Oak_Boss };

    /// <summary>
    /// Basic in-game objects
    /// </summary>
    public enum ObjectsID
    {
        /// <summary>
        /// Оно же валюта
        /// </summary>
        Stoneberry,

        /// <summary>
        /// "Лечилка"
        /// </summary>
        Green_Potion,

        /// <summary>
        /// Мана
        /// </summary>
        Blue_Potion,

        /// <summary>
        /// Кристалл. Можно ипользовать как аккумулятор
        /// </summary>
        Crystal,

        /// <summary>
        /// Фонарик
        /// </summary>
        Lantern,

        /// <summary>
        /// Сломанный фонарик. При попытке активировать, взрывается
        /// </summary>
        Broken_Lantern,

        /// <summary>
        /// Бумажний фонарик. Хорошо горит :)
        /// </summary>
        Paper_Lantern
    };

    /// <summary>
    /// Special in-game objects
    /// </summary>
    public enum SpecialObjectsID
    {
        Shadow_Hunter_Mask,
        Shadow_Spider_Mask,
        Shadow_Horner_Mask,
        Shadow_Boss_Mask
    };
}