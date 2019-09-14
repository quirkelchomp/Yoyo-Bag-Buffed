using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityYoyoBagBuffed.Buffs
{
    public class YoyoMasterBuff : ModBuff
    {
        //public Projectile projectile { get; } // This won't work as a definition for projectile because you don't know which instances of Projectile in the world are projectiles owned by the player and which are yoyo projectiles
        //public ModProjectile modProjectile { get; } // This won't work as a definition for projectile because you don't know which instances of Projectile in the world are projectiles owned by the player and which are yoyo projectiles

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Yoyo Master");
            Description.SetDefault("You are unstoppable");
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            YoyoModPlayer modPlayer = player.GetModPlayer<YoyoModPlayer>();
            if (/*(ItemID.Sets.Yoyo[player.inventory[player.selectedItem].type] || projectile.aiStyle == 99) && projectile.owner == Main.myPlayer && */ modPlayer.yoyoMaster)
            {
                modPlayer.yoyoMasterBuff = true;
            }

            if (!modPlayer.yoyoMasterBuff) // If yoyoMasterBuff is false, remove buff.
            {
                player.DelBuff(buffIndex);
                buffIndex--; // Decrements the buffIndex parameter by 1
            }
            else
            {
                //player.buffTime[buffIndex] = 18000; // Buff time is # of frames (@60FPS). 18000 calculates to be 5 mins long.
                player.AddBuff(BuffID.Midas, 60, false); // For testing purposes only - gives visible indication that the buff has activated
                //ProjectileID.Sets.YoyosMaximumRange[projectile.type] += 800f; // Replace projectile.type with i?

                //float NewYoyoRange = ProjectileID.Sets.YoyosMaximumRange[projectile.type];
                //NewYoyoRange += NewYoyoRange * 1.25f + 800f;
                //ProjectileID.Sets.YoyosMaximumRange[projectile.type] = NewYoyoRange;
            }
        }
    }
}
