using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityYoyoBagBuffed.Items.Accessories
{
    public class YoyoBagV4 : ModItem // Post-Devourer of Gods
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ultra Yoyo Bag"); // By default, capitalization in classnames will add spaces to the display name.
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
            player.counterWeight = 556 + Main.rand.Next(60) + ProjectileID.TerrarianBeam; // 556 is projectileID of black counterweight. By adding the projectileID of the TerrarianBeam (604), Main.rand.Next(60) uses the next 60 projectileIDs proceeding 604 instead.
            player.yoyoGlove = true;
            player.yoyoString = true;
            player.allDamage *= 1.15f; // multiplies damage by 1.15
            player.meleeDamage *= 1.15f; // multiplies melee damage by 1.15
            player.meleeCrit += 12; // add 12% crit chance
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
            // add Arcanum of the Void's abilities

            // To assign the player the YoyoMaster effect, we can't do player.YoyoMaster = true because Player doesn't have YoyoMaster. Be sure to remember to call the GetModPlayer method to retrieve the ModPlayer instance attached to the specified Player.
            player.GetModPlayer<YoyoModPlayer>().yoyoMaster = true;
        }
    }
}