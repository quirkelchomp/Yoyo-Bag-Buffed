using CalamityYoyoBagBuffed.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityYoyoBagBuffed.Items.Weapons
{
    public class NucleusItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nucleus");
            Tooltip.SetDefault("Atomic decay");

            // These are all related to gamepad controls and don't seem to affect anything else
            ItemID.Sets.Yoyo[item.type] = true;
            ItemID.Sets.GamepadExtraRange[item.type] += 15;
            ItemID.Sets.GamepadSmartQuickReach[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.width = 24;
            item.height = 24;
            item.useAnimation = 25;
            item.useTime = 25;
            item.shootSpeed = 48f;
            item.knockBack = 6.5f;
            item.damage = 1200;
            item.rare = 11;

            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.noUseGraphic = true;

            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(platinum: 100);
            item.shoot = mod.ProjectileType<NucleusProjectile>();
        }

        public override void AddRecipes()
        {
            Mod Modloader = ModLoader.GetMod("ModLoader");
            if (Modloader != null)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(mod.ItemType("TestYoyoBag"), 10);
                recipe.AddIngredient(ItemID.Terrarian, 1);
                recipe.AddIngredient(Modloader.ItemType("AprilFools"), 1);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}
