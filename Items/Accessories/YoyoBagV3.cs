using CalamityYoyoBagBuffed.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityYoyoBagBuffed.Items.Accessories
{
    public class YoyoBagV3 : ModItem // Post-Providence
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mega Yoyo Bag");
            Tooltip.SetDefault("12% increased melee damage" +
                               "\n12% increased melee critical strike chance" +
                               "\nMildly increases movement speed" +
                               "\nIncreases armor penetration by 5" +
                               "\nReduces damage taken by 10%" +
                               "\nNow with extra BOOM");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.accessory = true;
            item.value = 250000;
            item.rare = 10;
        }

        public override void AddRecipes()
        {
            Mod CalamityMod = ModLoader.GetMod("CalamityMod");
            if (CalamityMod != null)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(mod.ItemType("YoyoBagV2"), 1);
                recipe.AddIngredient(CalamityMod, "AeroStone", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "BadgeofBravery", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "LifeJelly", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "FleshTotem", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "UeliaceBar", 75); // Calamity Mod item
                recipe.AddIngredient(CalamityMod, "BloodstoneCore", 15); // Calamity Mod item
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (Main.player[item.owner].inventory[Main.player[item.owner].selectedItem].type == ItemType<NucleusItem>() && item.owner == Main.myPlayer)
            {
                player.counterWeight = ProjectileID.BlackCounterweight + Main.rand.Next(6);
                player.yoyoGlove = false;
            }
            else
            {
                player.counterWeight = Main.rand.Next(GetInstance<YoyoGlobalProjectile>().counterWeightTypes);
                player.yoyoGlove = true;
            }
            player.yoyoString = true;
            player.minionDamage *= 1.12f; // multiplies summoning damage by 1.12
            player.meleeDamage *= 1.12f; // multiplies melee damage by 1.12
            player.meleeCrit += 12; // add 12% crit chance. Player has a base crit chance of 4.
            player.meleeDamageMult *= 3f; // changes crit damage from default of 2f (2x damage) to 3x
            while (player.meleeSpeed < 1.6f)
            {
                player.meleeSpeed += 0.01f;
            }
            player.moveSpeed *= 1.4f; // multiplies movement speed by 1.40 (40% increase). Caps at 1.6
            player.runAcceleration *= 1.25f; // increases run acceleration by 25%
            player.armorPenetration += 5; // add 5 to armor penetration
            player.endurance = 0.10f; // all sources do 10% less damage to you
            player.lifeRegen *= 2; // doubles life regeneration
        }
    }
}