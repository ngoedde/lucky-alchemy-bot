﻿using System;

namespace LuckyAlchemyBot.Objects
{
    //Copyright to DaxterSoul!
    public struct ItemVariance : IEquatable<ItemVariance>
    {
        #region Constants

        public const int IVS_WEAPON_DURABILITY = 0;
        public const int IVS_WEAPON_PHY_SPECIALIZE = 1;
        public const int IVS_WEAPON_MAG_SPECIALIZE = 2;
        public const int IVS_WEAPON_HIT_RATIO = 3;
        public const int IVS_WEAPON_PHY_DMG = 4;
        public const int IVS_WEAPON_MAG_DMG = 5;
        public const int IVS_WEAPON_CRITICAL_HITRATIO = 6;

        public const int IVS_ARMOR_DURABILITY = 0;
        public const int IVS_ARMOR_PHY_SPECIALIZE = 1;
        public const int IVS_ARMOR_MAG_SPECIALIZE = 2;
        public const int IVS_ARMOR_PHY_DEFENSE = 3;
        public const int IVS_ARMOR_MAG_DEFENSE = 4;
        public const int IVS_ARMOR_EVASION_RATIO = 5;

        public const int IVS_SHIELD_DURABILITY = 0;
        public const int IVS_SHIELD_PHY_SPECIALIZE = 1;
        public const int IVS_SHIELD_MAG_SPECIALIZE = 2;
        public const int IVS_SHIELD_BLOCK_RATIO = 3;
        public const int IVS_SHIELD_PHY_DEFENSE = 4;
        public const int IVS_SHIELD_MAG_DEFENSE = 5;

        public const int IVS_ACCESSORY_PHY_ABSORB_RATIO = 0;
        public const int IVS_ACCESSORY_MAG_ABSORB_RATIO = 1;

        private const byte SLOT_SIZE = 5;

        #endregion Constants

        #region Members

        private ulong _value;

        #endregion Members

        #region Constructor

        public ItemVariance(ulong value)
        {
            _value = value;
        }

        #endregion Constructor

        #region Methods

        public byte this[byte slot]
        {
            get
            {
                var offset = slot * SLOT_SIZE;
                var mask = (1ul << SLOT_SIZE) - 1ul << offset;

                return (byte)((_value & mask) >> offset);
            }
            set
            {
                var offset = slot * SLOT_SIZE;
                var mask = (1ul << SLOT_SIZE) - 1ul << offset;

                _value = (_value & ~mask) | ((ulong)(value << offset) & mask);
            }
        }

        public override bool Equals(object obj) => obj is ItemVariance variance && this.Equals(variance);

        public bool Equals(ItemVariance other) => _value == other._value;

        public override int GetHashCode() => _value.GetHashCode();

        public static explicit operator ItemVariance(ulong value) => new ItemVariance(value);

        public static implicit operator ulong(ItemVariance variance) => variance._value;

        public static bool operator ==(ItemVariance left, ItemVariance right) => left.Equals(right);

        public static bool operator !=(ItemVariance left, ItemVariance right) => !(left == right);

        #endregion Methods
    }
}