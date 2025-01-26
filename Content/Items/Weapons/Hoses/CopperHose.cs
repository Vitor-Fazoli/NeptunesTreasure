using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TritonsHydrants.Common;
using TritonsHydrants.Content.Buffs;
using TritonsHydrants.Content.Items.Ammo;
using TritonsHydrants.Content.Projectiles;

namespace TritonsHydrants.Content.Items.Weapons.Hoses
{
	[AutoloadEquip(EquipType.Back)]
	public class CopperHose : HoseBase
	{
		override protected int ManaCost => 20;
		override protected int BurstDamage => 10;
		override protected int BurstKnockback => 2;

		public override void SetDefaults()
		{
			Item.mana = 0;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.shootSpeed = 10f;
			Item.useAnimation = 90;
			Item.useTime = 90;
			Item.damage = BurstDamage;
			Item.knockBack = BurstKnockback;
			Item.noUseGraphic = false;
			Item.shoot = ModContent.ProjectileType<AquaBurst>();
			Item.useAmmo = ModContent.ItemType<WaterCapsule>();
			Item.DamageType = DamageClass.Ranged;
			Item.buffType = 0;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse is 2)
			{
				if (player.statMana < MIN_MANA_COST + (ManaCost - Item.damage / 3))
					return false;

				Item.mana = MIN_MANA_COST + (ManaCost - Item.damage / 3);
				Item.useStyle = ItemUseStyleID.Swing;
				Item.shootSpeed = 0.1f;
				Item.useTime = 30;
				Item.useAnimation = 30;
				Item.noUseGraphic = true;
				Item.shoot = ModContent.ProjectileType<Hydrant>();
				Item.useAmmo = 0;
				Item.noMelee = true;
				Item.buffType = ModContent.BuffType<HydrantBuff>();
			}
			else
			{
				Item.mana = 0;
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noMelee = true;
				Item.shootSpeed = 10f;
				Item.useAnimation = 90;
				Item.useTime = 90;
				Item.damage = BurstDamage;
				Item.knockBack = BurstKnockback;
				Item.noUseGraphic = false;
				Item.shoot = ModContent.ProjectileType<AquaBurst>();
				Item.useAmmo = ModContent.ItemType<WaterCapsule>();
				Item.DamageType = DamageClass.Ranged;
				Item.buffType = 0;
			}

			return base.CanUseItem(player);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse is not 2)
				return true;

			player.AddBuff(Item.buffType, 2);

			Projectile projectile = Projectile.NewProjectileDirect(source, position, velocity, type, 0, 0, Main.myPlayer);
			projectile.originalDamage = 0;

			return false;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}