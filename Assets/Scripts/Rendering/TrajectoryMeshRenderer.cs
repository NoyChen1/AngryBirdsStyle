using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TrajectoryMeshRenderer : MonoBehaviour
{
    public float width = 0.2f;

    private Mesh _m;

    private void Start()
    {
        _m = new Mesh();

        GetComponent<MeshFilter>().mesh = _m;
    }

    public void RenderPath(IEnumerable<Vector3> p)
    {
        var pArray = p as Vector3[] ?? p.ToArray();

        _m.Clear();
        var vs = new Vector3[pArray.Length * 2];
        var uvs = new Vector2[pArray.Length * 2];
        var ts = new int[(pArray.Length - 1) * 2 * 3 * 2];

        for (var i = 0; i < pArray.Length; i++)
        {
            var curP = pArray[i];
            var prevP = i > 0 ? pArray[i - 1] : curP;
            var nextP = i < pArray.Length - 1 ? pArray[i + 1] : curP;

            var fd = (nextP - prevP).normalized;
            var rd = (Vector3.Cross(Vector3.up, fd)).normalized;

            var vr = curP + rd * (width * 0.5f);
            var vl = curP - rd * (width * 0.5f);

            vs[i * 2] = vr;
            vs[i * 2 + 1] = vl;

            uvs[i * 2] = new Vector2(1f, (float)i / pArray.Length);
            uvs[i * 2 + 1] = new Vector2(0f, (float)i / pArray.Length);

            if (i >= pArray.Length - 1) break;

            ts[i * 12] = i * 2;
            ts[i * 12 + 1] = i * 2 + 1;
            ts[i * 12 + 2] = i * 2 + 2;
            ts[i * 12 + 3] = i * 2 + 2;
            ts[i * 12 + 4] = i * 2 + 1;
            ts[i * 12 + 5] = i * 2 + 3;

            ts[i * 12 + 6] = i * 2;
            ts[i * 12 + 7] = i * 2 + 2;
            ts[i * 12 + 8] = i * 2 + 1;
            ts[i * 12 + 9] = i * 2 + 1;
            ts[i * 12 + 10] = i * 2 + 2;
            ts[i * 12 + 11] = i * 2 + 3;

        }

        _m.vertices = vs;
        _m.uv = uvs;
        _m.triangles = ts;
    }
}
