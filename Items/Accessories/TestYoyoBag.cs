using CalamityYoyoBagBuffed.Items.Weapons;
using CalamityYoyoBagBuffed.Projectiles;
using Terraria; // This line lets the program know what/where the Player class is/is located
using Terraria.ID; // TileID., ItemID., ProjectileID, etc. all pull from this directory
using Terraria.ModLoader; // The USING keyword tells the program the directories (i.e. the namespaces) that the classes used below are coming from. Without it, the computer will be lost on what the classes even mean or do.

namespace CalamityYoyoBagBuffed.Items.Accessories // The NAMESPACE is more or less is the location of this file
{
    public class TestYoyoBag : ModItem // The CLASS is just the name of this code bundle that we're making below. It allows the game to call upon it, and its roles & functions all as one simple package. If a program is simply a bunch of objects interracting w/each other via functions, classes would be the OBJECTs.
    {
        //public int owner = Main.myPlayer; // This is a VARIABLE. It lets the program know that everytime you mention "owner" down below, you're actually referring to Main.myPlayer, but also that the value for it is also an integer. Also, Main.myPlayer at default = 0
        //public int type; // This lets the program know that the "type"s written here are integer values, as opposed to true/false values, or decimal values, etc.


        // Variables in C# are categorized into 3 types: Value types, Reference Types, and Pointer Types. Value types variables can be assigned a value directly as they are derived from System.ValueType
        // Some value types contain data directly such as int, char, and float which store whole numbers, alphabets, and (single-precision) floating point (i.e. decimal) numbers respectively. Other common value types are bool, byte, and double.
        // Reference types do not contain data, but rather point the program to where the data is. Reference types can be split into 3 subcategories: object, dynamic, and string.
        // Object type example:
        //object obj; // specifying obj as an object type reference
        //obj = 100; // and converting the object type to a value type by giving it a value
        // Dynamic type example:
        //dynamic <variable_name> = value; //this is the syntax for a dynamic type
        //dynamic d = 20; // example of a dynamic type
        // String type allows you to assign any string values to a variable. 
        //String str = "Tutorials Point"; // Don't forget the quotation marks or else the compiler won't know it's a string
        // Pointer type variables store the memory address of another type. Hopefully you don't have to use these.

        // Syntax for variable definition is 
        //<data_type> <variable_list>;
        // For example: 
        //int i; 
        //float f;
        //double d;
        // You can initialize (assign a value) a variable at the time of definition with the following syntax:
        //<data_type> <variable_name> = value;
        // For example:
        //int i = 100; // intializes i
        //char x = 'x'; // the variable x has the value 'x'


        public override void SetStaticDefaults() // This is a FUNCTION, known in C# as a METHOD. It's a statement that performs a specific task for the program. In this case, setting defaults that cannot be changed in-game. If a program is simply a bunch of objects interracting w/each other via functions, then functions would be the METHODs.
        {
            //DisplayName.SetDefault("Test Yoyo Bag"); //By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("300% increased melee damage" +
                               "\n50% increased melee critical strike chance" +
                               "\nFantastically increases movement speed" +
                               "\nIncreases armor penetration by 80" +
                               "\nReduces damage taken by 90%" +
                               "\nNow with ENTROPY");
            // if you couldn't figure out by now, these set the descriptors of the item in-game. \n makes the program write the string in a 'new line'
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

        public override void UpdateAccessory(Player player, bool hideVisual) // This function enacts changes to the game when you have this item equipped as an accessory. In parentheses, you'll notice that we're telling the program that "player" refers to the "Player" class in the vanilla Terraria files. And also, that when (or if) we say "hideVisual" below, it is a boolean, meaning it can only have values of true or false. 
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
            player.allDamage *= 3f; // multiplies damage by 3
            player.meleeDamage *= 3f; // multiplies melee damage by 3
            player.meleeCrit += 50; // add 50% crit chance
            player.meleeDamageMult *= 5f; // changes crit damage from default of 2f (2x damage) to 5x
            while (player.meleeSpeed < 1.7f)
            {
                player.meleeSpeed += 0.01f;
            }
            player.moveSpeed *= 1.6f; // multiplies movement speed by 1.6 (60% increase). Caps at 1.6
            player.runAcceleration *= 1.7f; // increases run acceleration by 70%
            player.armorPenetration += 80; // add 80 to armor penetration
            player.endurance = 0.90f; // all sources do 90% less damage to you
            player.lifeRegen *= 100; // 100x life regeneration

            player.maxMinions += 10;

            player.statDefense += 300; // adds 300 to defense stat
            player.longInvince = true;
            player.immuneTime += 60;
            player.onHitDodge = true;
            player.immuneNoBlink = true;
            //player.extraAccessorySlots += 1; // Breaks your character when you already have maxed accessory slots
            player.onHitDodge = true;

            // To assign the player the YoyoMaster effect, we can't do player.YoyoMaster = true because Player is a vanilla class which of course doesn't have YoyoMaster. Be sure to remember to call the GetModPlayer method to instead retrieve the ModPlayer instance attached to the specified Player class.
            player.GetModPlayer<YoyoModPlayer>().yoyoMaster = true;

            // Just below is how the Calamity Modders did the same thing
            //YoyoModPlayer modPlayer = player.GetModPlayer<YoyoModPlayer>(mod);
            //modPlayer.yoyoMaster = true;
        }
    }
    // Operators are the arithmetic symbols such as +, -, *, /, ++, <, >, =, ==, etc. Most are intuitive. For reference the following mean:
    // % finds the remainder after a division (e.g. 20 % 10 = 0)
    // ++ or -- are increment/decrement operators that increase/decrease the integer value by 1 (e.g. if integer i = 3, then i++ would be 4 and i-- would be 2)
    // == checks if the two values are equal or not. If YES, the condition specified then becomes true.
    // != checks if the two values are equal or not. If NO, the condition specified then becomes true.
    // >= or <= checks if the value of left operand is greater than or equal to the value of right operand, if yes then condition becomes true... and vice versa.
    // && means "AND". If both values are non-zero, the condition then becomes true. The values could be an integer, for example, or even a boolean value (true is 1. false is 0)
    // || means "OR". If any of the two values are non-zero, the condition then becomes true.
    // ! means "NOT". Takes any of the relationships described above, and if true, automatically makes it false.
    // += or -= is an "add AND"/"subtract AND" operator. It adds/subtracts the value on the right of it to/from the value on the left of it.
    // *= or /= is an "multiply AND"/"divide AND" operator. It multiplies/divides the value on the right of it to/from the value on the left of it.

    // Loops can be the following:
    // while repeats a statement or a group of statements while a given condition is true. It checks the condition everytime BEFORE performing another loop.
    // for executes a sequence of statements multiple times and abbreviates the code that manages the loop variable.
    // do...while is similar to the while loop but it tests the condition at the END of the loop.
    // A loop becomes an infinite loop if the condition never becomes false. The for loop is traditionally used for this.
    // Programmers commonly use for (; ; ) {} to create an infinite loop.
}