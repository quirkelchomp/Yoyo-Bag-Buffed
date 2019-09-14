using CalamityYoyoBagBuffed.Items.Weapons;
using CalamityYoyoBagBuffed.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityYoyoBagBuffed.Items.Accessories
{
    public class YoyoBagV5 : ModItem // Post-Yharon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Master Yoyo Bag"); // By default, capitalization in classnames will add spaces to the display name.
            Tooltip.SetDefault("20% increased melee damage" +
                               "\n15% increased melee critical strike chance" +
                               "\nGreatly increases movement speed" +
                               "\nIncreases armor penetration by 15" +
                               "\nReduces damage taken by 20%" +
                               "\nNow with ENTROPY");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.accessory = true;
            item.value = 10000000;
            item.rare = 11;
        }

        public override void AddRecipes()
        {
            Mod CalamityMod = ModLoader.GetMod("CalamityMod");
            if (CalamityMod != null)
            {

                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(mod.ItemType("YoyoBagV4"), 1);
                recipe.AddIngredient(CalamityMod, "GodlySoulArtifact", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "DarkSunRing", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "ReaperToothNecklace", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "HellcasterFragment", 10); // Calamity Mod item
                recipe.AddIngredient(CalamityMod, "AuricOre", 100); // Calamity Mod item
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (Main.player[item.owner].inventory[Main.player[item.owner].selectedItem].type == mod.ItemType<NucleusItem>() && item.owner == Main.myPlayer)
            {
                player.counterWeight = 556 + Main.rand.Next(6);
            }
            else
            {
                player.counterWeight = mod.ProjectileType<ModCounterweight>();
            }
            player.yoyoGlove = true;
            player.yoyoString = true;
            player.allDamage *= 1.20f; // multiplies damage by 1.20
            player.meleeDamage *= 1.20f; // multiplies melee damage by 1.20
            player.meleeCrit += 15; // add 15% crit chance
            player.meleeDamageMult *= 4f; // changes crit damage from default of 2f (2x damage) to 4x
            while (player.meleeSpeed < 1.7f)
            {
                player.meleeSpeed += 0.01f;
            }
            player.moveSpeed *= 1.6f; // multiplies movement speed by 1.6 (60% increase). Caps at 1.6
            player.runAcceleration *= 1.7f; // increases run acceleration by 70%
            player.armorPenetration += 15; // add 15 to armor penetration
            player.endurance = 0.20f; // all sources do 20% less damage to you
            player.lifeRegen *= 4; // quadruples life regeneration
            // add Arcanum of the Void's abilities
            player.maxMinions += 3;
            player.statDefense += 15;

            // To assign the player the YoyoMaster effect, we can't do player.YoyoMaster = true because Player doesn't have YoyoMaster. Be sure to remember to call the GetModPlayer method to retrieve the ModPlayer instance attached to the specified Player.
            player.GetModPlayer<YoyoModPlayer>().yoyoMaster = true;
        }
    }
}