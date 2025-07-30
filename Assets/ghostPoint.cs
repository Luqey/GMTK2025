using UnityEditor;
using UnityEngine;

public class ghostPoint
{
    public Vector3 position;
    public Vector3 eulerAngles;
    public Vector3 scale;
    public Sprite sprite;
    public bool facingRight;

    public ghostPoint(Vector3 p, Vector3 e, Vector3 sc, Sprite s, bool fR)
    {
        position = p;
        eulerAngles = e;
        scale = sc;
        sprite = s;
        facingRight = fR;
    }
}
