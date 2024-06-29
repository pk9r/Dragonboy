namespace Mod.Constants
{
    /// <summary>
    /// Expresses the meaning of <see cref="ItemTemplate.type"/>.
    /// </summary>
    internal struct ItemTemplateType
    {
        /// <summary>
        /// Shirt (Wool t-shirt, Iron armor, etc.), used to increase your armor.
        /// </summary>
        internal static readonly sbyte Shirt = 0;

        /// <summary>
        /// Pants (Black trousers, Iron pants, etc.), used to increase your HP.
        /// </summary>
        internal static readonly sbyte Pants = 1;

        /// <summary>
        /// Gloves (Black gloves, Iron gloves, etc.), used to increase your damage.
        /// </summary>
        internal static readonly sbyte Gloves = 2;

        /// <summary>
        /// Shoes (Plastic shoes, Iron shoes, etc.), used to increase your MP.
        /// </summary>
        internal static readonly sbyte Shoes = 3;

        /// <summary>
        /// Items that increase your critical points (Radar, Ring of God, etc.).
        /// </summary>
        internal static readonly sbyte Radar = 4;

        /// <summary>
        /// Items that can change your appearance (Avatar, Disguise, etc.).
        /// </summary>
        internal static readonly sbyte AvatarAndDisguise = 5;

        /// <summary>
        /// Senzu Bean, used to restore your HP and MP.
        /// </summary>
        internal static readonly sbyte SenzuBean = 6;

        /// <summary>
        /// Skill books, used to learn new skills and increase your skill level.
        /// </summary>
        internal static readonly sbyte SkillBook = 7;

        /// <summary>
        /// Items relevant to your main quests (Chicken thighs, Comics, etc.).
        /// </summary>
        internal static readonly sbyte QuestItem = 8;

        /// <summary>
        /// Gold on the ground that you can pick up.
        /// </summary>
        internal static readonly sbyte Gold = 9;

        /// <summary>
        /// Green gem on the ground that you can pick up. It's no longer spawning in the game.
        /// </summary>
        internal static readonly sbyte GreenGem = 10;

        /// <summary>
        /// Items that you can carry on your back (Namek Dragonball, Black Star Dragonball, lantern, etc.).
        /// </summary>
        internal static readonly sbyte Backpack = 11;

        /// <summary>
        /// Dragonball 1–7 stars, which can be used to summon Shenron.
        /// </summary>
        internal static readonly sbyte DragonBall = 12;

        /// <summary>
        /// Charms that are sold at Urinai Obaba NPC.
        /// </summary>
        internal static readonly sbyte Charm = 13;

        /// <summary>
        /// Stones used to upgrade your equipment (Emerald, Sapphire, Ruby, Titan, etc.).
        /// </summary>
        internal static readonly sbyte UpgradeStone = 14;

        /// <summary>
        /// Rubble, can be combined to create <see cref="UpgradeStone"/>.
        /// </summary>
        internal static readonly sbyte Rubble = 15;

        /// <summary>
        /// Magic bottle, used to combine <see cref="Rubble"/> to create <see cref="UpgradeStone"/>.
        /// </summary>
        internal static readonly sbyte MagicBottle = 16;

        /// <summary>
        /// Satellite, can be placed on the ground to support you and your clan members that stand within the satellite's range.
        /// </summary>
        internal static readonly sbyte Satellite = 22;

        /// <summary>
        /// Items that help you fly without consuming MP (Flying Nimbus, Noel Helicopter, etc.).
        /// </summary>
        internal static readonly sbyte FlyPlatform = 23;

        /// <summary>
        /// <see cref="FlyPlatform"/> with additional buffs (increase your HP and MP, restore your HP and MP, etc.).
        /// </summary>
        internal static readonly sbyte VIPFlyPlatform = 24;

        /// <summary>
        /// 10 Radar Pack, used to find the location of Namek Dragonball.
        /// </summary>
        internal static readonly sbyte TenRadarsPack = 25;

        /// <summary>
        /// Other items that do not have specific uses.
        /// </summary>
        internal static readonly sbyte Miscellaneous = 27;

        /// <summary>
        /// Item that only appears and is exclusively used in events.
        /// </summary>
        internal static readonly sbyte EventItem = 27;

        /// <summary>
        /// Flag, turn on gray flag or flags that have different colors with your opponent to be able to fight each other.
        /// </summary>
        internal static readonly sbyte Flag = 28;

        /// <summary>
        /// Consumable buff item, used to increase your stats (Rage power, Cell Armor, Pudding Cake, Takoyaki, etc.).
        /// </summary>
        internal static readonly sbyte ConsumableBuffItem = 29;

        /// <summary>
        /// Star crystal, put into equipment that has crystal slots to increase the stats of that item.
        /// </summary>
        internal static readonly sbyte Crystal = 30;

        /// <summary>
        /// Event items that, when consumed, will increase your stats (Bánh Chýng, Bánh Tét, Bunny Moon Cake, Moon Cake, etc.).
        /// </summary>
        internal static readonly sbyte VietnameseCake = 31;

        /// <summary>
        /// Training suite, when equipped, reduce X% attack; when unequipped, increase X% attack. The duration of the effect depends on how long you equip it and the level of the training suite.
        /// </summary>
        internal static readonly sbyte TrainingSuite = 32;

        /// <summary>
        /// Collection card, upon picking will be added to the collection book.
        /// </summary>
        internal static readonly sbyte CollectionCard = 33;

        /// <summary>
        /// New type of gem that spawns on the ground and can be picked up, but unlike <see cref="GreenGem"/>, it cannot be given to other people and cannot be used to buy gem consignment items from the consignment shop (locked gem).
        /// </summary>
        internal static readonly sbyte Ruby = 34;

        /// <summary>
        /// Secret skills book, used to learn secret skills.
        /// </summary>
        internal static readonly sbyte SecretSkillsBook = 35;

        /// <summary>
        /// Title, that when equipped, will display on top of your character and increase your stats.
        /// </summary>
        internal static readonly sbyte Title = 36;

        /// <summary>
        /// Another type of skill book, used to learn new skills.
        /// </summary>
        /// <remarks>
        /// This type of skill book, unlike normal skill book, doesn't have levels.
        /// </remarks>
        internal static readonly sbyte SkillBook2 = 37;
    }
}