using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityYoyoBagBuffed.Items.Accessories
{
    public class YoyoBagV2 : ModItem // Post-Moonlord
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Yoyo Bag"); // By default, capitalization in classnames will add spaces to the display name.
            Tooltip.SetDefault("5% increased melee damage" +
                               "\n5% increased melee critical strike chance" +
                               "\nSlightly increases movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.accessory = true;
            item.value = 350000;
            item.rare = 9;
        }

        public override void AddRecipes()
        {
            Mod CalamityMod = ModLoader.GetMod("CalamityMod");
            if (CalamityMod != null)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.YoyoBag, 1);
                recipe.AddIngredient(ItemID.MechanicalGlove, 1);
                recipe.AddIngredient(ItemID.TitanGlove, 1);
                recipe.AddIngredient(CalamityMod, "VitalJelly", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "BloodyWormScarf", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "DeificAmulet", 1); // Calamity Mod accessory
                recipe.AddIngredient(ItemID.LunarBar, 20);
                recipe.AddIngredient(CalamityMod, "MLGRune2", 3); // Calamity Mod item
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.counterWeight = 556 + Main.rand.Next(6); // 556 is projectileID of black counterweight. Main.rand.Next(6) makes it so it has a chance to be any one of the 6 projectileIDs including 556 and thereafter (i.e. the other counterweight colors)
            player.yoyoGlove = true;
            player.yoyoString = true;
            player.allDamage *= 1.05f; // multiplies damage by 1.05
            player.meleeDamage *= 1.05f; // multiplies melee damage by 1.05
            player.meleeCrit += 5; // add 5% crit chance
            player.meleeDamageMult *= 3f; // changes crit damage from default of 2f (2x damage) to 3x
            while (player.meleeSpeed < 1.5f)
            {
                player.meleeSpeed += 0.01f;
            }
            player.moveSpeed *= 1.2f; // multiplies movement speed by 1.20 (20% increase). Caps at 1.6
            player.runAcceleration *= 1.1f; // increases run acceleration by 10%
        }
    }
}