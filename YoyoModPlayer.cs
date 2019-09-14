using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityYoyoBagBuffed
{
    // This file shows the very basics of using ModPlayer classes since ExamplePlayer can be a bit overwhelming.
    // ModPlayer classes provide a way to attach data to Players and act on that data. 
    // This example will hopefully provide you with an understanding of the basic building blocks of how ModPlayer works. 
    // This example will teach the most commonly sought after effect: "How to do X if the player has Y?"
    // X in this example will be "Apply a buff to your character."
    // Y in this example will be "Wearing an accessory."
    // After studying this example, you can change X to other effects by changing the "hook" you use or the code within the hook you use. For example, you could use OnHitByNPC and call Projectile.NewProjectile within that hook to change X to "When the player is hit by NPC, spawn Projectiles".
    // We can change Y to other conditions as well. For example, you could give the player the effect by having a "potion" ModItem give a ModBuff that sets the ModPlayer variable in ModBuff.Update
    // Another example would be an armor set effect. Simply use the ModItem.UpdateArmorSet hook 

    // This is the ModPlayer class.
    public class YoyoModPlayer : ModPlayer
    {
        // Here we declare the yoyoMaster variable which will represent whether this player has the effect or not.
        public bool yoyoMaster;
        public bool yoyoMasterBuff;
        //private static int nextProjectile = ProjectileID.Count;
        public Projectile projectile { get; private set; } // This won't work as a definition for projectile because you don't know which instances of Projectile in the world are projectiles owned by the player and which are yoyo projectiles
        //public ModProjectile projectile { get; private set; }
        public GlobalProjectile globalProjectile { get; private set; }


        // ResetEffects is used to reset effects back to their default value. Terraria resets all effects every frame back to defaults so we will follow this design. 
        // (You might think to set a variable when an item is equipped and unassign the value when the item is unequipped, but Terraria is not designed that way.)
        public override void ResetEffects()
        {
            yoyoMaster = false;
            yoyoMasterBuff = false;
        }


        // Here we use a "hook" to actually let our YoyoMaster status take effect. This hook is called upon immediately after all equipment effects are loaded(updated) on the player.
        // This can be used to modify the effects that the equipment updates had on this player, but can also be used for general update tasks.
        public override void PostUpdateEquips()
        {
            if (yoyoMaster)
            {
                YoyoMasterEffect();
                player.AddBuff(mod.BuffType<Buffs.YoyoMasterBuff>(), 60, true); // Buff time is # of frames (@60FPS)
                //player.AddBuff(BuffID.Inferno, 60, false); // For testing purposes only - gives visible indication that the buff has activated
            }
            else
            {
                return;
            }
        }
        // To recap, make a class variable, reset that variable in ResetEffects, and use that variable in the logic of whatever hooks you use.


        private void YoyoMasterEffect()
        {
            // Goal is: if player is you && held projectile is a yoyo-type, create a clone of that projectile. My mod will then modify its characteristics, like reach length or extra updates. Use GlobalProjectile class?

            ////Projectile projectile = Main.projectile[i];
            ////Main.LoadProjectile(projectile.type);
            ////float modifiedYoyoRange = new ProjectileID.Sets.YoyosMaximumRange[Projectile.type];
            while (player.meleeSpeed < 1.7f)
            {
                player.meleeSpeed += 0.01f;
            }
            if (player.meleeSpeed > 1.7f) // If buffs and equipment effects boost melee speed past 70%...
            {
                player.meleeSpeed = 1.7f; // Bring it back down to no more than 70% (because any higher breaks yoyo functionality)
            }
            //player.AddBuff(BuffID.LeafCrystal, 60, false); // For testing purposes only - gives visible indication that the effect has activated
            ////modProjectile.projectile.extraUpdates += 2; // Doesn't work. Breaks things.

            //int i = 0;
            //if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer /*&& Main.player[projectile.owner].yoyoString*/)
            //{
            //    player.AddBuff(BuffID.Titan, 60, false); // For testing purposes only - gives visible indication that the effect has activated
            //    if ((i <= 556 && i >= 561) && modProjectile.projectile.aiStyle == 99)
            //    {
            //        player.AddBuff(BuffID.Thorns, 60, false); // For testing purposes only - gives visible indication that the effect has activated
            //        for (i = 0; i > ProjectileID.Count; i++)
            //        {
            //            ProjectileID.Sets.YoyosLifeTimeMultiplier[i] = -1f;
            //            ProjectileID.Sets.YoyosMaximumRange[i] += 200f;
            //            ProjectileID.Sets.YoyosTopSpeed[i] = 17.5f;
            //            ProjectileID.Sets.CanDistortWater[i] = true;
            //            Main.projectileLoaded[i] = true;
            //            Main.projFrames[i] = 1;
            //            ProjectileID.Sets.TrailingMode[i] = -1;
            //            ProjectileID.Sets.TrailCacheLength[i] = 10;
            //            player.AddBuff(BuffID.Werewolf, 60, false); // For testing purposes only - gives visible indication that the effect has activated
            //        }
            //        for (i = ProjectileID.Count; i < nextProjectile; i++)
            //        {
            //            ProjectileID.Sets.YoyosLifeTimeMultiplier[i] = -1;
            //            ProjectileID.Sets.YoyosMaximumRange[i] += 100f;
            //            ProjectileID.Sets.YoyosTopSpeed[i] += 5f;
            //            ProjectileID.Sets.CanDistortWater[i] = true;
            //            Main.projectileLoaded[i] = true;
            //            Main.projFrames[i] = 1; // This is the projectile's animation, switching(i.e. setting) which frame of the sprite to draw on sprites with multiple animation frames
            //            ProjectileID.Sets.TrailingMode[i] = -1;
            //            ProjectileID.Sets.TrailCacheLength[i] = 10;
            //            player.AddBuff(BuffID.Merfolk, 60, false); // For testing purposes only - gives visible indication that the effect has activated
            //        }
            //        //ProjectileID.Sets.YoyosMaximumRange[projectile.type] += 200f;

            //        //float NewYoyoRange = ProjectileID.Sets.YoyosMaximumRange[projectile.type];
            //        //NewYoyoRange += NewYoyoRange * 1.25f + 200f;
            //        //ProjectileID.Sets.YoyosMaximumRange[projectile.type] = NewYoyoRange;
            //    }
            //}
        }

        //public override bool CanHitPvpWithProj(Projectile proj, Player target)
        //{
        //    return base.CanHitPvpWithProj(proj, target);
        //}
        //
        //public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)
        //{
        //    base.ModifyHitPvpWithProj(proj, target, ref damage, ref crit);
        //}
        //
        //public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        //{
        //    return base.Shoot(item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        //}

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (pvp)
            {
                player.immune = true;
                player.immuneTime += 60;
                player.pvpDeath = false;
                player.shadowDodge = true;
                player.statDefense += 400;
                quiet = true;
                damage = 0;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }
    }
}
