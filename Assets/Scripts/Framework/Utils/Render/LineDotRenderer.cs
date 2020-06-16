using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 线（圆点组成）3d渲染
/// zouyu
/// </summary>
public class LineDotRenderer : MonoBehaviour
{
    public Material Material;
    public Camera Camera;
    /// <summary>
    /// 画笔大小
    /// </summary>
    public float BrushSize=1;

    private MeshRenderer _renderer;
    public MeshRenderer Renderer
    {
        get
        {
            return _renderer;
        }
    }
    private MeshFilter _filter;
    private Mesh _mesh;
    /// <summary>
    /// 顶点
    /// </summary>
    private List<Vector3> _vertices;

    /// <summary>
    /// 每帧渲染延时
    /// </summary>
    private const float UpdateDelayMax=0.1f;
    private float _updateDelay=0f;

    /// <summary>
    /// 点个数
    /// </summary>
    public int positionCount {
        get { return _vertices.Count; }
        set
        {
            if (_vertices == null)
            {
                Start();
            }

            if (value < _vertices.Count)
            {
                //_vertices.RemoveRange(0, _vertices.Count- value);
                //InitVertices();
            }
            else if (value > _vertices.Count)
            {
                int length = value - _vertices.Count;
                for (int i = 0; i < length; i++)
                {
                    _vertices.Add(Vector3.zero);
                }
                InitVertices();
            }
        }
    }

    

    // Use this for initialization
    void Start()
    {
        if (_vertices == null)
        {
            _renderer = gameObject.AddComponent<MeshRenderer>();
            _renderer.material = Material;

            _filter = gameObject.AddComponent<MeshFilter>();
            _mesh = new Mesh();
            _vertices = new List<Vector3>();
            InitVertices();
        }
    }


    /// <summary>
    /// 初始化顶点
    /// </summary>
    private void InitVertices()
    {
        //_mesh.Clear();
        float halfSize = BrushSize / 2f;
        int length = _vertices.Count;
        Vector3[] vertices = new Vector3[4* length];
        int[] triangles = new int[6 * length];
        Vector2[] uv = new Vector2[4 * length];
        for (int i = 0; i < length; i++) //每个顶点，两个三角形
        {
            Vector3 oreVertice = _vertices[i];
            vertices[0 + i*4] = new Vector3(oreVertice.x - halfSize, oreVertice.y - halfSize, oreVertice.z);
            vertices[1 + i * 4] = new Vector3(oreVertice.x + halfSize, oreVertice.y - halfSize, oreVertice.z);
            vertices[2 + i * 4] = new Vector3(oreVertice.x + halfSize, oreVertice.y + halfSize, oreVertice.z);
            vertices[3 + i * 4] = new Vector3(oreVertice.x - halfSize, oreVertice.y + halfSize, oreVertice.z);

            triangles[0 + i * 6] = 0 + i * 4;
            triangles[1 + i * 6] = 1 + i * 4;
            triangles[2 + i * 6] = 2 + i * 4;
            triangles[3 + i * 6] = 0 + i * 4;
            triangles[4 + i * 6] = 2 + i * 4;
            triangles[5 + i * 6] = 3 + i * 4;

            uv[0 + i * 4] = new Vector2(0,0);
            uv[1 + i * 4] = new Vector2(1,0);
            uv[2 + i * 4] = new Vector2(1,1);
            uv[3 + i * 4] = new Vector2(0,1);
        }

        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.uv = uv;
        _filter.mesh = _mesh;
    }

    /// <summary>
    /// 设置点坐标
    /// </summary>
    /// <param name="index">点索引</param>
    /// <param name="position">坐标</param>
    public void SetPosition(int index, Vector3 position)
    {
        if (index<_vertices.Count)
        {
            _vertices[index] = position;
        }
        else
        {
            Debug.LogError("LineDotRenderer 设置点坐标 溢出！");
        }
    }

    /// <summary>
    /// 旋转顶点
    /// </summary>
    /// <param name="point"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private Vector3 RotatePoint(Vector3 point, Vector3 rotation)
    {
        float radX = rotation.x * Mathf.Deg2Rad;
        float radY = rotation.y * Mathf.Deg2Rad;
        float radZ = rotation.z * Mathf.Deg2Rad;
        float sinX = Mathf.Sin(radX);
        float cosX = Mathf.Cos(radX);
        float sinY = Mathf.Sin(radY);
        float cosY = Mathf.Cos(radY);
        float sinZ = Mathf.Sin(radZ);
        float cosZ = Mathf.Cos(radZ);

        Vector3 xAxis = new Vector3(
            cosY * cosZ,
            cosX * sinZ + sinX * sinY * cosZ,
            sinX * sinZ - cosX * sinY * cosZ
        );
        Vector3 yAxis = new Vector3(
            -cosY * sinZ,
            cosX * cosZ - sinX * sinY * sinZ,
            sinX * cosZ + cosX * sinY * sinZ
        );
        Vector3 zAxis = new Vector3(
            sinY,
            -sinX * cosY,
            cosX * cosY
        );

        return xAxis * point.x + yAxis * point.y + zAxis * point.z;
    }

    // Update is called once per frame
    void Update()
    {
        _updateDelay -= Time.deltaTime;
        if (_updateDelay < 0)
        {
            _updateDelay = UpdateDelayMax;

            //顶点始终面向屏幕
            float halfSize = BrushSize / 2f;
            int length = _vertices.Count;
            Vector3[] vertices = _filter.mesh.vertices;
            int index = 0;
            for (int i = 0; i < length; i++) //每个顶点，两个三角形
            {
                //计算镜头和顶点的角度
                Vector3 oreVertice = _vertices[i];
                Vector3 targetPos = transform.position + (oreVertice) * transform.localScale.x;
                Vector3 targetDir = Camera.transform.position - targetPos; // 目标坐标与当前坐标差的向量
                Vector3 rotateAngle = Quaternion.FromToRotation(Vector3.right, targetDir).eulerAngles;

                vertices[0 + index] = RotatePoint(new Vector3(0, -halfSize, halfSize), rotateAngle) + oreVertice;
                vertices[1 + index] = RotatePoint(new Vector3(0, -halfSize, -halfSize), rotateAngle) + oreVertice;
                vertices[2 + index] = RotatePoint(new Vector3(0, halfSize, -halfSize), rotateAngle) + oreVertice;
                vertices[3 + index] = RotatePoint(new Vector3(0, halfSize, halfSize), rotateAngle) + oreVertice;
                index += 4;
            }
            _filter.mesh.vertices = vertices;
        }
    }
}
