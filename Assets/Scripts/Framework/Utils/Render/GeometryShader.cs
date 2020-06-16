using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 虚拟几何shader
/// 可选择顶点或边线渲染
/// zouyu
/// </summary>
public class GeometryShader : MonoBehaviour
{
    /// <summary>
    /// 原模型
    /// </summary>
    public Mesh OreMesh;
    /// <summary>
    /// 实际显示的 mesh
    /// </summary>
    public Mesh NewMesh;
    public Material Material;
    public Camera Camera;
    /// <summary>
    /// 是否显示线， 否则显示顶点
    /// </summary>
    public bool IsShowLine = false;
    /// <summary>
    /// 画笔大小
    /// </summary>
    public float BrushSize=1;

    private Transform[] vertices;
    private MeshRenderer _renderer;
    private MeshFilter _filter;
    /// <summary>
    /// 顶点
    /// </summary>
    private List<Vector3> _vertices;
    /// <summary>
    /// 顶点连线 （起始点）
    /// </summary>
    private List<Vector3[]> _linesStartEndPt;
    /// <summary>
    /// 顶点连线 （线段平面， 四个点）
    /// </summary>
    //private List<Vector3[]> _linesPlanePt;
    /// <summary>
    /// 连线法线 
    /// </summary>
    private List<Vector3> _linesNormal;
    
    /// <summary>
    /// 每帧渲染延时
    /// </summary>
    private const float UpdateDelayMax=0.1f;
    private float _updateDelay=0f;


    // Use this for initialization
    void Start()
    {
        _renderer = gameObject.AddComponent<MeshRenderer>();
        _renderer.material = Material;

        _filter = gameObject.AddComponent<MeshFilter>();

        if (NewMesh == null) //生成mesh
        {
            NewMesh = new Mesh();
            //线列表  屏蔽重复线段
            if (IsShowLine)
            {
                _linesStartEndPt = new List<Vector3[]>();
                _linesNormal = new List<Vector3>();
                int[] triangles = OreMesh.triangles;
                Vector3[] normals = OreMesh.normals;
                Vector3[] vertices = OreMesh.vertices;
                int length = triangles.Length/3;
                for (int i = 0; i < length; i++) //每个三角形 取三条线
                {
                    int index = i*3;
                    for (int j = 0; j < 3; j++)
                    {
                        Vector3 pt1 = vertices[triangles[index + j]];
                        Vector3 pt2 = vertices[triangles[index + (j + 1)%3]];
                        if (_linesStartEndPt.Find((pts) =>
                            {
                                return ((pts[0].Equals(pt1) && pts[1].Equals(pt2)) ||
                                        (pts[0].Equals(pt2) && pts[1].Equals(pt1)));
                            }) == default(Vector3[]))
                        {
                            _linesStartEndPt.Add(new Vector3[] {pt1, pt2});
                            _linesNormal.Add(normals[triangles[index + j]]);
                        }
                    }
                }

                ShowLines();
            }
            else
            {
                //顶点列表  屏蔽重复点
                _vertices = new List<Vector3>();
                Vector3[] vertices = OreMesh.vertices;
                foreach (var vertice in vertices)
                {
                    if (_vertices.Find((item) => { return item.Equals(vertice); }) == default(Vector3))
                    {
                        _vertices.Add(vertice);
                    }
                }

                ShowVertices();
            }
        }
        else //读取 已导出的mesh
        {
            if (IsShowLine==false)
            {
                int length = NewMesh.vertices.Length / 4;
                _vertices = new List<Vector3>();
                Vector3[] vertices = NewMesh.vertices;
                int index = 0;
                for (int i = 0; i < length; i++)
                {
                    Vector3 vertice = Vector3.zero;
                    for (int j = 0; j < 4; j++)
                    {
                        vertice += vertices[j + index];
                    }
                    vertice /= 4;
                    _vertices.Add(vertice);
                    index += 4;
                }
            }
        }

        _filter.mesh = NewMesh;
    }

    /// <summary>
    /// 显示边线
    /// </summary>
    private void ShowLines()
    {
        float halfSize = BrushSize / 2f;
        int length = _linesStartEndPt.Count;
        Vector3[] vertices = new Vector3[4 * length];
        int[] triangles = new int[6 * length];
        Vector2[] uv = new Vector2[4 * length];
        for (int i = 0; i < length; i++) //每个顶点，两个三角形
        {
            Vector3[] pts = _linesStartEndPt[i];
            Vector3 ptStart = pts[0];
            Vector3 ptEnd = pts[1];
            
            Vector3 normalVec = _linesNormal[i];
            Vector3 targetDir = ptStart - ptEnd;
            Vector3 dist = Vector3.Cross(normalVec, targetDir).normalized*halfSize;
            vertices[0 + i * 4] = -dist + ptStart;
            vertices[1 + i * 4] = dist + ptStart;
            vertices[2 + i * 4] = dist + ptEnd;
            vertices[3 + i * 4] = -dist + ptEnd;

            triangles[0 + i * 6] = 0 + i * 4;
            triangles[1 + i * 6] = 1 + i * 4;
            triangles[2 + i * 6] = 2 + i * 4;
            triangles[3 + i * 6] = 0 + i * 4;
            triangles[4 + i * 6] = 2 + i * 4;
            triangles[5 + i * 6] = 3 + i * 4;

            uv[0 + i * 4] = new Vector2(0, 0);
            uv[1 + i * 4] = new Vector2(1, 0);
            uv[2 + i * 4] = new Vector2(1, 1);
            uv[3 + i * 4] = new Vector2(0, 1);
        }

        NewMesh.vertices = vertices;
        NewMesh.triangles = triangles;
        NewMesh.uv = uv;
    }

    /// <summary>
    /// 显示顶点(初始化)
    /// </summary>
    private void ShowVertices()
    {
        
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

        NewMesh.vertices = vertices;
        NewMesh.triangles = triangles;
        NewMesh.uv = uv;
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
        if (!IsShowLine)
        {
            _updateDelay -= Time.deltaTime;
            if (_updateDelay<0)
            {
                _updateDelay = UpdateDelayMax;

                //顶点始终面向屏幕
                float halfSize = BrushSize / 2f;
                int length = _vertices.Count;
                Vector3[] vertices = _filter.mesh.vertices;
                int index=0;
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
}
