using CalamityYoyoBagBuffed.Items.Weapons;
using CalamityYoyoBagBuffed.Projectiles;
using Terraria; // This line lets the program know what/where the Player class is/is located
using Terraria.ID; // TileID., ItemID., ProjectileID, etc. all pull from this directory
using Terraria.ModLoader; // The USING keyword tells the program the directories (i.e. the namespaces) that the classes used below are coming from. Without it, the computer will be lost on what the classes even mean or do.

namespace CalamityYoyoBagBuffed.Items.Accessories // The NAMESPACE is more or less is the "location" of this file
{
    public class TestYoyoBag : ModItem // The CLASS, known in C# as an OBJECT, is just the name of this code bundle that we're making below. It allows the game to call upon it, and its roles & functions all as one simple package.
    {
        public override void SetStaticDefaults() // This is a FUNCTION, known in C# as a METHOD. It's a statement that performs a specific task for the program. In this case, setting defaults that cannot be changed in-game.
        {
            //DisplayName.SetDefault("Test Yoyo Bag"); //By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("300% increased melee damage" +
                               "\n50% increased melee critical strike chance" +
                               "\nFantastically increases movement speed" +
                               "\nIncreases armor penetration by 80" +
                               "\nReduces damage taken by 90%" +
                               "\nNow with ENTROPY"); // These set the descriptors of the item in-game. \n makes the program write the string in a 'new line'
            //ItemID.Sets.SortingPriorityMaterials[item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults() // This is another function, also setting certain defaults for the item it's creating. (i.e. Functions work for the class they fall under.)
        {
            // these set the characteristics of the item that do not change in-game such as the in-game icon size, what kind of item it is, its value in pennies, and its rarity level on Terraria's pre-defined rarity chart.
            item.width = 24;
            item.height = 24;
            item.accessory = true;
            item.value = 10000000;
            item.rare = 11;
        }

        public override void AddRecipes() // This function defines the crafting recipe. If you mouse over any kind of function, you'll notice what class it belongs to (i.e. where it is coming from).
        {
            Mod CalamityMod = ModLoader.GetMod("CalamityMod");
            if (CalamityMod != null)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(mod.ItemType("YoyoBagV5"), 1);
                recipe.AddIngredient(ItemID.AvengerEmblem, 1);
                recipe.AddIngredient(CalamityMod, "Rock", 1); // Calamity Mod item
                recipe.AddIngredient(CalamityMod, "CheatTestThing", 1); // Calamity Mod accessory
                recipe.AddIngredient(CalamityMod, "ShadowspecBar", 1000); // Calamity Mod item
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual) // This function enacts changes to the game when you have this item equipped as an accessory. In parentheses, you'll notice that we're telling the program that "player" draws from the "Player" class in the vanilla Terraria files.
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
            player.minionDamage *= 3f; // multiplies summoning damage by 3
            player.meleeDamage *= 3f; // multiplies melee damage by 3
            player.meleeCrit += 50; // add 50% crit chance. Player has a base crit chance of 4.
            player.meleeDamageMult *= 5f; // changes crit damage from default of 2f (2x damage) to 5x
            while (player.meleeSpeed < 1.7f)
            {
                player.meleeSpeed += 0.01f; // For as long as this player's melee speed is below 170%, increase it by 1%
            }
            player.moveSpeed *= 1.6f; // multiplies movement speed by 1.6 (60% increase). Caps at 1.6
            player.runAcceleration *= 1.7f; // increases run acceleration by 70%
            player.armorPenetration += 80; // add 80 to armor penetration
            player.endurance = 0.90f; // all sources do 90% less damage to you
            player.lifeRegen *= 100; // 100x life regeneration
            player.maxMinions += 10;
            player.statDefense += 200; // adds 200 to defense stat
            //player.longInvince = true;
            //player.onHitDodge = true;
            if (player.yoyoString && (ItemID.Sets.Yoyo[player.inventory[player.selectedItem].type] || player.inventory[player.selectedItem].channel && player.inventory[player.selectedItem].melee && player.inventory[player.selectedItem].useStyle == 5))
            {
                // To assign the player the yoyoMaster effect, we can't do player.yoyoMaster = true because the Player class doesn't have yoyoMaster as it is a vanilla class. 
                // So be sure to remember to call the GetModPlayer method to retrieve the ModPlayer instance attached to the specified Player.
                player.GetModPlayer<YoyoModPlayer>().yoyoMaster = true;
                player.GetModPlayer<YoyoModPlayer>().reflectProjectile = true;

                // Alternate way of assigning the yoyoMaster effect:
                //YoyoModPlayer modPlayer = player.GetModPlayer<YoyoModPlayer>(mod);
                //modPlayer.yoyoMaster = true;
            }
        }
    }
}