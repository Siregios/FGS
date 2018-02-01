using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    [System.Serializable]
    public struct HitBox
    {
        [SerializeField]
        private Vector2 _origin;
        public Vector2 origin { get { return _origin; } }
        [SerializeField]
        private Vector2 _size;
        public Vector2 size { get { return _size; } }
    }

    [System.Serializable]
    public struct Attack
    {
        [SerializeField]
        private string _attackName;
        public string attackName { get { return _attackName; } }
        [SerializeField]
        public bool _debug;
        public bool debug { get { return _debug; } }
        [SerializeField]
        private HitBox[] _hitBoxes;
        public HitBox[] hitBoxes { get { return _hitBoxes; } }
        [SerializeField]
        private float[] _values;            //0 damage, 1 stun
        public float[] values { get { return _values; } }
    }

    [SerializeField]
    private Attack[] _attackList;
    public Attack[] attackList { get { return _attackList; } }

    private bool attackActive;
    private bool clearCache;
    private Collider2D[] cacheTemp;
    private Hashtable cache;
    private int mask;
    private int index;
    private GameObject tempObject;
    private Vector3 tempVector;

    // Use this for initialization
    private void Start () {
        cache = new Hashtable();
        index = -1;
        if (gameObject.tag == "Player")
        {
            mask = LayerMask.GetMask(new string[] { "EnemyHitbox" });
            cacheTemp = new Collider2D[1000];
        }
        else if (gameObject.tag == "Enemy")
        {
            mask = LayerMask.GetMask(new string[] { "PlayerHitbox" });
            cacheTemp = new Collider2D[10];
        }
    }

    private void FixedUpdate() //Called before animation event
        //call animation events the frame before it starts and the frame it ends
    {
        if (clearCache)
        {
            cache.Clear();
            clearCache = false;
        }
        if (index != -1)
        {
            CallAttack(index);
        }
    }

    private void AttackIndex(int input)
    {
        index = input;
    }

    private void CallAttack(int input)
    {
        if (input != -1)
        {
            int len;
            for (int i = 0; i < _attackList[input].hitBoxes.Length; i++)
            {
                tempVector = new Vector3(_attackList[input].hitBoxes[i].origin.x * transform.localScale.x, _attackList[input].hitBoxes[i].origin.y, 0);
                len = Physics2D.OverlapBoxNonAlloc(tempVector + transform.position, _attackList[input].hitBoxes[i].size * 2, 0, cacheTemp, mask);
                for (int j = 0; j < len; j++)
                {
                    //checks if collider is a child gameobject of the main one
                    if (cacheTemp[j].gameObject.GetComponent<Health>() != null)
                    {
                        tempObject = cacheTemp[j].gameObject;
                    }
                    else
                    {
                        tempObject = cacheTemp[j].transform.parent.gameObject;
                        //code for headshot/bodyshot marker
                           //prioritize headshot vs bodyshot
                            //throw hitbox
                    }
                    if (!cache.ContainsValue(tempObject))
                    {
                        Debug.Log(input);
                        cache.Add(tempObject.GetHashCode(), tempObject);
                        RegisterHit(input, tempObject);
                        return;
                    }
                }
            }
        }
    }

    private void RegisterHit(int input, GameObject target)
    {
        Debug.Log(target.name);
        target.GetComponent<Health>().stun = _attackList[input].values[1];
        target.GetComponent<Health>().health -= _attackList[input].values[0];
    }

    private void ClearCache() //use when animation cancelling
    {
        clearCache = true;
    }
}
