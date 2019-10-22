using CalamityYoyoBagBuffed.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityYoyoBagBuffed.Items.Accessories
{
    public class YoyoBagV4 : ModItem // Post-Devourer of Gods
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ultra Yoyo Bag");
            Tooltip.SetDefault("15% increased melee damage" +
                               "\n12% increased melee critical strike chance" +
                               "\nModerately increases movement speed" +
                               "\nIncreases armor penetration by 8" +
                               "\nReduces damage taken by 10%" +
                               "\nNow with extra BOOM");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.accessory = true;
            item.value = 1000000;
            item.rare = 11;
        }

        public override void AddRecipes()
        {
            Mod CalamityMod = ModLoader.GetMod("CalamityMod");
            if (CalamityMod != null)
            {

                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(mod.ItemType("YoyoBagV3"), 1);
                recipe.AddIngredient(CalamityMod, "EldritchSoulArtifact", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "DimensionalSoulArtifact", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "ArcanumoftheVoid", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "ElementalGauntlet", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "CosmiliteBar", 50); // Calamity Mod item
                recipe.AddIngredient(CalamityMod, "EndothermicEnergy", 10); // Calamity Mod item
                recipe.AddIngredient(CalamityMod, "NightmareFuel", 10); // Calamity Mod item
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
            player.minionDamage *= 1.15f; // multiplies summoning damage by 1.15
            player.meleeDamage *= 1.15f; // multiplies melee damage by 1.15
            player.meleeCrit += 12; // add 12% crit chance. Player has a base crit chance of 4.
            player.meleeDamageMult *= 4f; // changes crit damage from default of 2f (2x damage) to 4x
            while (player.meleeSpeed < 1.7f)
            {
                player.meleeSpeed += 0.01f;
            }
            player.moveSpeed *= 1.6f; // multiplies movement speed by 1.60 (60% increase). Caps at 1.6
            player.runAcceleration *= 1.5f; // increases run acceleration by 50%
            player.armorPenetration += 8; // add 8 to armor penetration
            player.endurance = 0.10f; // all sources do 10% less damage to you
            player.lifeRegen *= 3; // triples life regeneration
            player.maxMinions += 1;
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