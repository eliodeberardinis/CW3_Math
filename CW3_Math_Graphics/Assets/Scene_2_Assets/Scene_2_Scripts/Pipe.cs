using UnityEngine;

//Script to create the basic element of our track: The pipe
public class Pipe : MonoBehaviour
{
    //The Pipe's dimensions and segments
    public float pipeRadius;
    public int pipeSegmentCount;

    //Range in which the user can adjust the pipe's curvatur and radius
    public float minCurveRadius, maxCurveRadius;
    public int minCurveSegmentCount, maxCurveSegmentCount;

    //The radius and segment count of the curve
    private float curveRadius;
    private int curveSegmentCount;

    //Mesh Variables: Vertices, triangles and ring distances
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    public float ringDistance;
    private float curveAngle;

    //Relative rtation across different pipe segments
    private float relativeRotation;
    private Vector2[] uv;

    //An instance to generate obstacles inside the pipe
    public PipeItemGenerator[] generators;

    //A series of methods to obtain private variables outside this script
    public float CurveRadius
    {
        get
        {
            return curveRadius; //the radius of the current pipe's curve
        }
    }

    public float CurveAngle //To detect the end of the current pipe in the system
    {
        get
        {
            return curveAngle;
        }
    }

    public float RelativeRotation //Relative rotation compared to the previous pipe
    {
        get
        {
            return relativeRotation;
        }
    }

    public int CurveSegmentCount //The length of current pipe segment
    {
        get
        {
            return curveSegmentCount;
        }
    }

    //Method to obtain specific point on the pipe. Given the circular structure, Sin and Cos functions are used
    private Vector3 GetPointOnTorus(float u, float v)
    {
        Vector3 p;
        float r = (curveRadius + pipeRadius * Mathf.Cos(v));
        p.x = r * Mathf.Sin(u);
        p.y = r * Mathf.Cos(u);
        p.z = pipeRadius * Mathf.Sin(v);
        return p;
    }

    //Initialize the Mesh
    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Pipe";
    }

    //Randomly generates a new pipe within the range decided by the user
    public void Generate(bool withItems = true)
    {
        curveRadius = Random.Range(minCurveRadius, maxCurveRadius);  //The curve radius is randomly chosen between a amax and a min
        curveSegmentCount = Random.Range(minCurveSegmentCount, maxCurveSegmentCount + 1); //The length of the segment is also randomly chosen between a max and a min

        //Methods called to initialize the mesh, set the UVs, draw the triangles and calculate the normals
        mesh.Clear();
        SetVertices();
        SetUV();
        SetTriangles();
        mesh.RecalculateNormals();

        //Destroys the pipe the player has passed
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        //Instantiate obstacles inside the pipes when the bool with item is set to true. The forst 2 pipes are without objects for simplicity
        if (withItems)
        {
            generators[Random.Range(0, generators.Length)].GenerateItems(this);
        }
    }

    //The Uvs are set along the torus
    private void SetUV()
    {
        uv = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i += 4)
        {
            uv[i] = Vector2.zero;
            uv[i + 1] = Vector2.right;
            uv[i + 2] = Vector2.up;
            uv[i + 3] = Vector2.one;
        }
        mesh.uv = uv;
    }

    //The vertices of the meshare set here
    private void SetVertices() {
        vertices = new Vector3[pipeSegmentCount * curveSegmentCount * 4]; //Number of vertices depend on the parameters above
        float uStep = ringDistance / curveRadius;                         //Distance between vertices dipend on the radius and the distance between subsequent rings
        curveAngle = uStep * curveSegmentCount * (360f / (2f * Mathf.PI)); //Normalization of the angles

        CreateFirstQuadRing(uStep); //Creates the first quad: 2 triangles

        int iDelta = pipeSegmentCount * 4;
        for (int u = 2, i = iDelta; u <= curveSegmentCount; u++, i += iDelta) //Generates the remaining quads for each ring in the pipe
        {
            CreateQuadRing(u * uStep, i);
        }
        mesh.vertices = vertices;
    }

    //Creates the complete first quad ring
    private void CreateFirstQuadRing(float u)
    {
        float vStep = (2f * Mathf.PI) / pipeSegmentCount;

        Vector3 vertexA = GetPointOnTorus(0f, 0f);
        Vector3 vertexB = GetPointOnTorus(u, 0f);
        for (int v = 1, i = 0; v <= pipeSegmentCount; v++, i += 4)
        {
            vertices[i] = vertexA;
            vertices[i + 1] = vertexA = GetPointOnTorus(0f, v * vStep);
            vertices[i + 2] = vertexB;
            vertices[i + 3] = vertexB = GetPointOnTorus(u, v * vStep);
        }
    }

    //Creates the subsequent quad rings
    private void CreateQuadRing(float u, int i)
    {
        float vStep = (2f * Mathf.PI) / pipeSegmentCount;
        int ringOffset = pipeSegmentCount * 4;

        Vector3 vertex = GetPointOnTorus(u, 0f);
        for (int v = 1; v <= pipeSegmentCount; v++, i += 4)
        {
            vertices[i] = vertices[i - ringOffset + 2];
            vertices[i + 1] = vertices[i - ringOffset + 3];
            vertices[i + 2] = vertex;
            vertices[i + 3] = vertex = GetPointOnTorus(u, v * vStep);
        }
    }

    //Draws the triangles between the set vertices
    private void SetTriangles()
    {
        triangles = new int[pipeSegmentCount * curveSegmentCount * 6];
        for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4)
        {
            triangles[t] = i;
            triangles[t + 1] = triangles[t + 4] = i + 2;
            triangles[t + 2] = triangles[t + 3] = i + 1;
            triangles[t + 5] = i + 3;
        }
        mesh.triangles = triangles;
    }

    //Aligns the newly created pipe with the curren one at a random angle
    public void AlignWith(Pipe pipe)
    {
        relativeRotation = Random.Range(0, curveSegmentCount) * 360f / pipeSegmentCount;

        //This is achieved by temporarely switching between transforms parents and taking into account the relative rotation of the current pipe
        transform.SetParent(pipe.transform, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, -pipe.curveAngle);
        transform.Translate(0f, pipe.curveRadius, 0f);
        transform.Rotate(relativeRotation, 0f, 0f);
        transform.Translate(0f, -curveRadius, 0f);
        transform.SetParent(pipe.transform.parent);
        transform.localScale = Vector3.one;
    }
}