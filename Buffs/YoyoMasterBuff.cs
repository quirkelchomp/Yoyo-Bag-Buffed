using Terraria;
using Terraria.ModLoader;

namespace CalamityYoyoBagBuffed.Buffs
{
    public class YoyoMasterBuff : ModBuff
    {
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
            if (player.GetModPlayer<YoyoModPlayer>().yoyoMaster)
            {
                player.GetModPlayer<YoyoModPlayer>().yoyoMasterBuff = true;
            }
            if (!player.GetModPlayer<YoyoModPlayer>().yoyoMasterBuff) // If yoyoMasterBuff is false, remove buff.
            {
                player.DelBuff(buffIndex);
                buffIndex--; // Decrements the buffIndex parameter by 1
            }
        }
    }
}
