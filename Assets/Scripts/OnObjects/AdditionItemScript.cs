using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OnMergeScript))]
[RequireComponent(typeof(OnClickManager))]
[RequireComponent(typeof(CollisionDetector))]
[RequireComponent(typeof(DamageNumberSpawner))]


public class AdditionItemScript : MonoBehaviour
{
    [System.Serializable]
    public class AdditionItemClass
    {
        public string name;
        public string description;
        public enum JoinType { Ranged, Melee, Fire, Arcane }
        public JoinType joinType;

        public enum Rarity { Common, Uncommon, Rare };
        public Rarity rarity;
    }
}
