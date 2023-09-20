public enum CameraState
{
    MainMenu = 0,
    InGame = 1,
    Shop = 2,
}

public enum LevelType
{
    Tutorial = 0,
    Normal = 1,
    Boss = 2,
}

public enum EnemyType
{
    // Enemy
    Archer = PoolType.Archer,
    Thrower = PoolType.Thrower,
    Mage = PoolType.Mage,
    Bat = PoolType.Bat,
    
    // Boss
    BossSpider = PoolType.BossSpider,
}

public enum BulletType
{
    // Weapon Bullet
    BulletArrowW = PoolType.BulletArrowW,
    BulletAxe0 = PoolType.BulletAxe0,
    BulletAxe1 = PoolType.BulletAxe1,
    BulletBoomerang = PoolType.BulletBoomerang,
    BulletCandy0 = PoolType.BulletCandy0,
    BulletCandy1 = PoolType.BulletCandy1,
    BulletCandy2 = PoolType.BulletCandy2,
    BulletCandy3 = PoolType.BulletCandy3,
    BulletHammer = PoolType.BulletHammer,
    BulletHammer2 = PoolType.BulletHammer2,
    BulletKnife = PoolType.BulletKnife,
    BulletUzi = PoolType.BulletUzi,
    BulletZ = PoolType.BulletZ,
    // Other Bullet
    BulletArrow = PoolType.BulletArrow,
    BulletBomb = PoolType.BulletBomb,
    BulletMageShoot = PoolType.BulletMageShoot,
    BulletBSpiderShoot = PoolType.BulletBSpiderShoot
}

public enum ShopType
{
    None = -1,
    Weapon = 0,
    Hat = 1,
    Pant = 2,
}

public enum WeaponType
{
    Arrow = PoolType.Arrow,
    Axe0 = PoolType.Axe0,
    Axe1 = PoolType.Axe1,
    Boomerang = PoolType.Boomerang,
    Candy0 = PoolType.Candy0,
    Candy1 = PoolType.Candy1,
    Candy2 = PoolType.Candy2,
    Candy3 = PoolType.Candy3,
    Hammer = PoolType.Hammer,
    Hammer2 = PoolType.Hammer2,
    Knife = PoolType.Knife,
    Uzi = PoolType.Uzi,
    Z = PoolType.Z,
}


public enum HatType
{
    None = -1,
    Arrow = PoolType.ArrowHat,
    BunnyEar = PoolType.BunnyEar,
    Cowboy = PoolType.Cowboy,
    Crown = PoolType.Crown,
    Hat = PoolType.Hat,
    HatCap = PoolType.HatCap,
    HatStraw = PoolType.HatStraw,
    Headphone = PoolType.Headphone,
    Horn = PoolType.Horn,
    Rau = PoolType.Rau,
}

public enum PantType
{
    None = -1,
    PantAmerica = PoolType.PantAmerica,
    PantBatman = PoolType.PantBatman,
    PantChickThigh = PoolType.PantChickThigh,
    PantDevil = PoolType.PantDevil,
    PantLeopard = PoolType.PantLeopard,
    PantPokeball = PoolType.PantPokeball,
    PantPurple = PoolType.PantPurple,
    PantRainbow = PoolType.PantRainbow,
    PantPinkDot = PoolType.PantPinkDot,
}

// Not Use

public enum SkillType
{
   None = 0, 
   MoreDamage = 1,
   MoreSpeed = 2,
   MoreBullet = 3,
   BounceBullet = 4,
   FireBullet = 5,
   IceBullet = 6,
}

public enum SkillLevel
{
    One = 1,
    Two = 2, 
    Three = 3
}

public enum StageName
{
    CityOfShadow = 0,
    CloudyBeach = 1,
    WhisperingWoods = 2,
}

public enum EnemyPhase
{
    Phase1 = 0,
    Phase2 = 1,
    Phase3 = 2,
}

public enum BgmType
{
    MainMenu = 0,
    InGame = 1,
    Boss = 2,
}

public enum SfxType
{
    // Boss Spider Venom
    BossSpiderShoot = 0,
    BossSpiderJump = 1,
    BossSpiderFall = 2,
    BossSpiderRoll = 3,
}

