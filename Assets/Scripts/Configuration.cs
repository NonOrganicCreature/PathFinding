using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathFinding
{
    [CreateAssetMenu]
    public class Configuration : ScriptableObject
    {
        public Material team1Material;
        public Material team2Material;

        public UnitView rangeView;
        public UnitView meleeView;
        public GridFieldView fieldView;
        public BulletView bulletView;
        
        public Tile tileView;
        
        [Range(1, 10)]
        public float unitSpeed;

        [Range(2, 8)]
        public int rangeDamage;
        [Range(1, 4)]
        public int meleeDamage;
        [NonSerialized]
        public int gridWidth = 80;
        [NonSerialized]
        public int gridHeight = 80;

        [Range(1, 999)]
        public int respawnSecondsLeftSide;
        [Range(1, 999)]
        public int respawnSecondsRightSide;
        
        [Range(0, 2)]
        public float waitUntilRecalculateWalkPath;

        [Range(2, 8)]
        public float meleeAttackRadius = 2f;
        [Range(1, 8)]
        public float rangeAttackRadius = 6f;

        [Range(1, 8)]
        public int unitHealth = 4;
        
        [Range(0, 8)]
        public int unitAttackCooldown = 0;

        [Range(1, 10)] public int pathFindingInterpolationStep = 1;
    }
}

