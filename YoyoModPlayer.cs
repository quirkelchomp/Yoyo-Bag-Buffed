using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityYoyoBagBuffed
{
    // This example will teach the most commonly sought after effect: "How to do X if the player has Y?" X in this example will be "Apply a buff to your character." Y in this example will be "Wearing an accessory."
    // After studying this example, you can change X to other effects by changing the "hook" you use or the code within the hook you use. For example, you could use OnHitByNPC and call Projectile.NewProjectile within that hook to change X to "When the player is hit by NPC, spawn Projectiles".
    // We can change Y to other conditions as well. For example, you could give the player the effect by having a "potion" ModItem give a ModBuff that sets the ModPlayer variable in ModBuff.Update
    // Another example would be an armor set effect. Simply use the ModItem.UpdateArmorSet hook 
    public class YoyoModPlayer : ModPlayer
    {
        // Here we declare the yoyoMaster variable that represents whether this player has the effect or not.
        public bool yoyoMaster;
        public bool yoyoMasterBuff;
        public bool reflectProjectile;

        // ResetEffects is used to reset effects back to their default value. Terraria resets all effects every tick back to defaults so we will follow this design. 
        // (You might think to set a variable when an item is equipped and unassign the value when the item is unequipped, but Terraria is not designed that way.)
        public override void ResetEffects()
        {
            yoyoMaster = false;
            yoyoMasterBuff = false;
            reflectProjectile = false;
        }

        // Here we use a "hook" to actually let our YoyoMaster status take effect. This hook is called upon immediately after all equipment effects are loaded (i.e. updated) on the player.
        // This can be used to modify the effects that the equipment updates had on this player, but can also be used for general update tasks.
        public override void PostUpdateEquips()
        {
            if (yoyoMaster)
            {
                YoyoMasterEffect();
                player.AddBuff(BuffType<Buffs.YoyoMasterBuff>(), 60, true); // Buff time is # of frames (@60FPS)
            }
        }
        // To recap, make a class variable, reset that variable in ResetEffects, and use that variable in the logic of whatever hooks you use.

        private void YoyoMasterEffect()
        {
            while (player.meleeSpeed < 1.7f)
            {
                player.meleeSpeed += 0.01f;
            }
            if (player.meleeSpeed > 1.7f) // If buffs and equipment effects boost melee speed past 70%...
            {
                player.meleeSpeed = 1.7f; // Bring it back down to no more than 70% (because any higher breaks yoyo functionality)
            }
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            if (reflectProjectile && proj.active && proj.hostile && !proj.friendly && damage > 0 && Main.rand.Next(40) == 0)
            {
                player.statLife += damage;
                player.HealEffect(damage, true);
                proj.hostile = false;
                proj.friendly = true;
                proj.velocity.X = -proj.velocity.X * 10;
                proj.velocity.Y = -proj.velocity.Y * 10;
            }
            for (int i = 0; i < ProjectileLoader.ProjectileCount; i++)
            {
                Projectile projectile = Main.projectile[i];
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && !Main.player[p].dead && ((Main.player[projectile.owner].hostile && Main.player[p].hostile) || Main.player[projectile.owner].team != Main.player[p].team))
                    {
                        Main.player[Main.myPlayer].statLife += damage;
                    }
                }
            }
        }
    }
}
