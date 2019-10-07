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
            DisplayName.SetDefault("Master Yoyo Bag");
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
                player.counterWeight = ProjectileID.BlackCounterweight + Main.rand.Next(6);
                player.yoyoGlove = false;
            }
            else
            {
                player.counterWeight = mod.ProjectileType<ModCounterweight>();
                player.yoyoGlove = false;
            }
            player.yoyoString = true;
            player.minionDamage *= 1.20f; // multiplies summoning damage by 1.20
            player.meleeDamage *= 1.20f; // multiplies melee damage by 1.20
            player.meleeCrit += 15; // add 15% crit chance. Player has a base crit chance of 4.
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
            player.maxMinions += 3;
            player.statDefense += 15;
            if (player.yoyoString && (ItemID.Sets.Yoyo[player.inventory[player.selectedItem].type] || player.inventory[player.selectedItem].channel && player.inventory[player.selectedItem].melee && player.inventory[player.selectedItem].useStyle == 5))
            {
                // To assign the player the yoyoMaster effect, we can't do player.yoyoMaster = true because the Player class doesn't have yoyoMaster as it is a vanilla class. 
                // So be sure to remember to call the GetModPlayer method to retrieve the ModPlayer instance attached to the specified Player.
                player.GetModPlayer<YoyoModPlayer>().yoyoMaster = true;
                player.GetModPlayer<YoyoModPlayer>().reflectProjectile = true;
            }
        }
    }
}