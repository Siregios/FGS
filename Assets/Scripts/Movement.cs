using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    protected LayerMask walls, passing, notPassing, passable;
    
    protected Vector3 _velocity; 
    public Vector3 velocity { get { return _velocity; } }

    protected bool falling;
    protected bool jump;
    protected bool fallThrough;

    private RaycastHit2D hit;
    private CapsuleCollider2D col;

    protected void Start()
    {
        falling = true;
        col = gameObject.GetComponent<CapsuleCollider2D>();
        notPassing = LayerMask.GetMask(new string[] { "Unpassable", "PassableBottom", "Passable" });
        passing = LayerMask.GetMask(new string[] { "Unpassable", "PassableBottom" });
        walls = LayerMask.GetMask(new string[] { "Unpassable" });
        passable = LayerMask.GetMask(new string[] { "Passable" });
    }

	protected void FixedUpdate () {
        if (falling)
            _velocity += new Vector3(0, -0.009f, 0);
        transform.position += _velocity;
        CollisionAdjust();
    }

    protected void CollisionAdjust()
    {
        //Raycast Up
        if (!BoundCheck(new Vector2(0, 0), new Vector2(0, 1), walls))
            if (!BoundCheck(new Vector2(col.size.x / 2 - col.size.x / 8, 0), new Vector2(0, 1), walls))
                BoundCheck(new Vector2(-col.size.x / 2 + col.size.x / 8, 0), new Vector2(0, 1), walls);
        //Raycast Lefts
        if (!BoundCheck(new Vector2(0, 0), new Vector2(-1, 0), walls))
            if (!BoundCheck(new Vector2(0, col.size.y / 2 - col.size.y / 16), new Vector2(-1, 0), walls))
                BoundCheck(new Vector2(0, -col.size.y / 2 + col.size.y / 4), new Vector2(-1, 0), walls);
        //Raycast Right
        if (!BoundCheck(new Vector2(0, 0), new Vector2(1, 0), walls))
            if (!BoundCheck(new Vector2(0, col.size.y / 2 - col.size.y / 16), new Vector2(1, 0), walls))
                BoundCheck(new Vector2(0, -col.size.y / 2 + col.size.y / 4), new Vector2(1, 0), walls);

        //Raycast Down
        if (!jump)
        {
            if (fallThrough)
            {
                fallThrough = BoundCheck(new Vector2(0, 0), new Vector2(0, -1f), passable, false);
                if (!BoundCheck(new Vector2(0, 0), new Vector2(0, -1.5f), passing))
                    if (!BoundCheck(new Vector2(col.size.x / 2 - col.size.x / 16, 0), new Vector2(0, -1), passing))
                        if (!BoundCheck(new Vector2(-col.size.x / 2 + col.size.x / 16, 0), new Vector2(0, -1), passing))
                            falling = true;
            }
            else
            {
                if (!BoundCheck(new Vector2(0, 0), new Vector2(0, -1.5f), notPassing))
                    if (!BoundCheck(new Vector2(col.size.x / 2 - col.size.x / 16, 0), new Vector2(0, -1), notPassing))
                        if (!BoundCheck(new Vector2(-col.size.x / 2 + col.size.x / 16, 0), new Vector2(0, -1), notPassing))
                            falling = true;
            }
        }
        else
            jump = false;
    }

    private bool BoundCheck(Vector2 origin, Vector2 dir, int layerMask, bool tMove = true)
    {
        hit = Physics2D.Raycast(col.bounds.center + (Vector3)origin, dir, Mathf.Abs(dir.x) * col.size.x / 2 + Mathf.Abs(dir.y) * col.size.y / 2, layerMask);
        if (hit)
        {
            if (dir.normalized.y == -1)
            {
                if (dir.normalized.y == -1)
                if (!fallThrough && Vector2.Angle(hit.normal, _velocity.normalized) <= 90 && hit.transform.gameObject.layer == LayerMask.NameToLayer("Passable") && Vector2.Distance(hit.point, col.bounds.center + (Vector3)origin) <= col.size.y / 3)
                {
                    fallThrough = true;
                    return false;
                }
                if (!falling || (Vector2.Distance(hit.point, col.bounds.center + (Vector3)origin) <= col.size.y / 2 && (fallThrough || Vector2.Distance(hit.point, col.bounds.center + (Vector3)origin) >= col.size.y / 3) && Vector2.Angle(hit.normal, _velocity.normalized) > 90))
                {
                    if (tMove)
                    {
                        falling = false;
                        _velocity = new Vector3(0, 0, 0);
                    }
                }
                else
                {
                    if (dir.y == -1.5)
                        return true;
                    return false;
                }
            }
            if (dir.normalized.y == 1 && _velocity.y > 0 && tMove)
                _velocity.y = 0;
            if (tMove)
                transform.position = hit.point - origin + new Vector2(-dir.normalized.x * col.size.x / 2, -dir.normalized.y * col.size.y / 2) - col.offset;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Move(Vector2 dir)
    {
        _velocity += (Vector3)dir;
    }
}
