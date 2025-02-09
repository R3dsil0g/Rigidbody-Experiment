using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEditor.Callbacks;
using Unity.VisualScripting;

public class Displacement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float time;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int resolution = 30;
    [SerializeField] bool draw = true;
    public float gravity = -9.81f;

    void Start(){
        lineRenderer.startWidth = 0.2f;  
        lineRenderer.endWidth = 0.2f;  
    }

    void Update(){
        if (Input.GetKey(KeyCode.Space))
        {
            draw = false;
            //lineRenderer.enabled = false;
            Test();
        }

        if (draw)
        {
            DrawTrajectory();
        }
        
        targetPosition();
        power();
    }

    public void Test()
    {
        rb.linearVelocity = new Vector2(GetXVelocity(), GetYVelocity());
    }

    public float GetXVelocity()
    {
        var distance = target.transform.position - transform.position;
        return distance.x / time;
    }

    public float GetYVelocity()
    {
        var distance = target.transform.position - transform.position;
        return (distance.y - DisplacementFormula()) / time;
    }

    public float DisplacementFormula()
    {
        return (0 * time) + (0.5f * gravity * time * time);
    }

    public void targetPosition()
    {
        float xAxis = 0f; 
        float yAxis = 0f;
        float moveSpeed = 5f;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            yAxis += 1;
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            xAxis -= 1;
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            yAxis -= 1;
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            xAxis += 1;
        }

        target.position += new Vector3(xAxis, yAxis, 0) * moveSpeed * Time.deltaTime;
    } 

    public void power()
    {
        if (Input.GetKey(KeyCode.E))
        {
            time -= 0.5f * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.Q))
        {
            time += 0.5f * Time.deltaTime;
        }
    }

    private void DrawTrajectory()
    {
        Vector2[] points = new Vector2[resolution];
        Vector2 startPosition = transform.position;
        Vector2 startVelocity = new Vector2(GetXVelocity(), GetYVelocity());

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)resolution * time;
            points[i] = startPosition + startVelocity * t + 0.5f * new Vector2(0, gravity) * t * t;

            Vector3[] linePositions = System.Array.ConvertAll(points, p => (Vector3)p);
            lineRenderer.positionCount = resolution;
            lineRenderer.SetPositions(linePositions);
        }
    }
}