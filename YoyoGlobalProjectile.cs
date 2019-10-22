using CalamityYoyoBagBuffed.Items.Weapons;
using CalamityYoyoBagBuffed.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityYoyoBagBuffed
{
    public class YoyoGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity
        {
            get { return true; }
        }

        //public override bool CloneNewInstances
        //{
        //    get { return true; }
        //}

        public override void PostAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            Item selectedItem = player.inventory[player.selectedItem];
            Mod CalamityMod = ModLoader.GetMod("CalamityMod");
            if (projectile.owner == Main.myPlayer && player.GetModPlayer<YoyoModPlayer>().yoyoMaster && player.yoyoString)
            {
                if ((ItemID.Sets.Yoyo[selectedItem.type] || projectile.aiStyle == 99) && (player.channel || selectedItem.channel) && projectile.melee && !projectile.counterweight && projectile.active)
                {
                    //Main.NewText("projectile.type is " + projectile.type);
                    //Main.NewText("player.heldProj is " + player.heldProj);
                    //Main.NewText("projectile.identity is " + projectile.identity); // The projectile's universal unique identifier, which is the same on all clients and the server. Usually used to find the same projectile on multiple clients and/or the server.
                    //Main.NewText("Projectile is " + projectile); // Prints a message telling you the projectile's type (ID#), name, active status (t/f), whoAmI, identity, ai0, and uuid.
                    if (selectedItem.type != ItemType<NucleusItem>() && projectile.type != ProjectileType<ModCounterweight>())
                    {
                        if (selectedItem.modItem != null) // This property is null if the held item is not a modded item and not null if it is.
                        {
                            if (CalamityMod != null)
                            {
                                //projectile.modProjectile.Clone();
                                projectile.modProjectile.aiType = 0; // The Calamity yoyos use aiType to copy the AI of vanilla yoyos. Setting aiType to 0, resets them so as to not copy any behavior, allowing YoyosMaximumRange to take effect on them.
                            }
                        }
                        //Main.NewText("YoyosMaximumRange[projectile.type] is " + ProjectileID.Sets.YoyosMaximumRange[projectile.type]);
                        //Main.NewText("player.meleeSpeed is " + player.meleeSpeed);
                        ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 800f;
                        ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1f;
                        projectile.extraUpdates = 3;
                    }
                }
            }
        }

        public int[] counterWeightTypes = new int[]
        {
            ProjectileID.UnholyTridentFriendly,
            ProjectileID.SwordBeam,
            ProjectileID.FrostBoltSword,
            ProjectileID.TerraBeam,
            ProjectileID.LightBeam,
            ProjectileID.NightBeam,
            ProjectileID.CannonballFriendly,
            ProjectileID.FrostburnArrow,
            ProjectileID.EnchantedBeam,
            ProjectileID.PossessedHatchet,
            ProjectileID.PygmySpear,
            ProjectileID.ChlorophyteBullet,
            ProjectileID.CrystalLeafShot,
            ProjectileID.BoulderStaffOfEarth,
            ProjectileID.DeathSickle,
            ProjectileID.IchorArrow,
            ProjectileID.IchorBullet,
            ProjectileID.ShadowBeamFriendly,
            ProjectileID.PaladinsHammerFriendly,
            ProjectileID.VampireKnife,
            ProjectileID.BloodyMachete,
            ProjectileID.FruitcakeChakram,
            ProjectileID.PineNeedleFriendly,
            ProjectileID.Blizzard,
            ProjectileID.NorthPoleSpear,
            ProjectileID.Flairon,
            ProjectileID.Tempest,
            ProjectileID.Typhoon,
            ProjectileID.ElectrosphereMissile,
            ProjectileID.InfluxWaver,
            ProjectileID.ChargedBlasterLaser,
            ProjectileID.NurseSyringeHurt,
            ProjectileID.Arkhalis,
            ProjectileID.SolarWhipSword,
            ProjectileID.NebulaArcanum,
            ProjectileID.LastPrismLaser,
            ProjectileID.Daybreak,
            ProjectileID.MonkStaffT2Ghast,
            ProjectileID.DD2PhoenixBowShot,
            ProjectileID.MonkStaffT3_AltShot
        };
    }
}
