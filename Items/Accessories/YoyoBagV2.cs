using CalamityYoyoBagBuffed.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityYoyoBagBuffed.Items.Accessories
{
    public class YoyoBagV2 : ModItem // Post-Moonlord
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Yoyo Bag");
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
            if (Main.player[item.owner].inventory[Main.player[item.owner].selectedItem].type == ItemType<NucleusItem>() && item.owner == Main.myPlayer)
            {
                player.counterWeight = ProjectileID.BlackCounterweight + Main.rand.Next(6); // 556 (blackcounterweight ID) plus a random number from 0 to 6 (556 to 561)(i.e. all the counterweight colors)
                player.yoyoGlove = false;
            }
            else
            {
                player.counterWeight = ProjectileID.BlackCounterweight + Main.rand.Next(6);
                player.yoyoGlove = true;
            }
            player.yoyoString = true;
            player.minionDamage *= 1.05f; // multiplies summoning damage by 1.05
            player.meleeDamage *= 1.05f; // multiplies melee damage by 1.05
            player.meleeCrit += 5; // add 5% crit chance. Player has a base crit chance of 4.
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