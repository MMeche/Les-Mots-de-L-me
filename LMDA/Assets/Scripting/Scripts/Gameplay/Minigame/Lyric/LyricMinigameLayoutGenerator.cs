using System.Collections.Generic;
using System.Drawing;
using Ink.Parsed;
using NUnit.Framework.Internal;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;


public class LyricMinigameLayoutGenerator : MonoBehaviour
{

    public int NbOfPoints;
    public int NbOfAlternativePaths;
    public float HeightAmplitude;
    public int MaxLengthOfAlternativePaths;

    public SplineContainer _splineContainer;
    SplineExtrude _splineMesh;
    MeshFilter _meshFilter;

    Spline _spline;
    CinemachineCamera _camera;

    BattleManager _battleManager;

    //[SerializeField] SplineContainer splineContainer;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _battleManager.BattleManagerEventBus.OnKnobEnabled += OnKnobEnabled;
        _battleManager.BattleManagerEventBus.OnLyricSplineEnabled += OnLyricSplineEnabled;
        _spline = RuntimeSplineGenerator.Generate(
            origin: Vector3.zero,
            minSpacing: 0.5f,
            heightAmplitude: 10f,
            maxSpacing: 3f,
            controlPointCount:NbOfPoints,
            seed: 0);

        
        
        _splineMesh = GetComponentInChildren<SplineExtrude>();
        _meshFilter = GetComponentInChildren<MeshFilter>();
        
    }

    void OnLyricSplineEnabled(BattleManagerEventContext ctx)
    {
        _splineContainer = transform.GetChild(0).GetComponentInChildren<SplineContainer>();
        _splineContainer.Spline = _spline;
        _splineMesh.FlipNormals = true;
        _splineMesh.Rebuild();
        PlaceObjectAndCam();
    }

    void OnKnobEnabled(BattleManagerEventContext ctx)
    {
        SplineAnimate animate = GetComponentInChildren<SplineAnimate>();
        animate.Container = _splineContainer;
        //animate.enabled = false;
        animate.transform.position = _splineContainer.Spline[0].Position;
        animate.ElapsedTime = 0;
        animate.NormalizedTime = 0;
        
    }

    private void PlaceObjectAndCam()
    {
        
        Vector3 startSplineInWorldPosition = transform.TransformPoint(_meshFilter.mesh.vertices[_meshFilter.mesh.vertexCount - 1]);
        Vector3 endSplineInWorldPosition = transform.TransformPoint(_meshFilter.mesh.vertices[0]);


        //Bien placer le centre du spline sur le centre de l'objet que l'on va bouger pour simplifier les manip !!        
        Transform _child = transform.GetChild(0).transform;
        _child.position = new(_child.position.x - endSplineInWorldPosition.x, _child.position.y, _child.position.z);
        _child.position = new((_child.position.x - startSplineInWorldPosition.x) / 2, _child.position.y, _child.position.z);
        float _objectLength = GetComponentInChildren<Renderer>().bounds.size.x;
        float _objectHeight = GetComponentInChildren<Renderer>().bounds.size.y;

        
        _camera = (CinemachineCamera)FindFirstObjectByType<CinemachineBrain>().ActiveVirtualCamera;
        transform.rotation = _camera.transform.rotation;
        _camera.Follow = transform;
        transform.rotation *= Quaternion.Euler(new(90, 0, 0));
        

        float requiredDistance = Mathf.Max((_objectLength/ 2) / Mathf.Tan(_camera.Lens.FieldOfView * 0.5f * Mathf.Deg2Rad), (_objectHeight/ 2) / Mathf.Tan(91f * 0.5f * Mathf.Deg2Rad)); //91f is the horizontal field of view somehow we can't access it smh...
        //transform.position = _camera.transform.position + _camera.transform.forward * requiredDistance;
        //transform.position = new Vector3(transform.position.x, transform.position.y, _objectLength / 2);
    }
    private void OnDisable()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.GetChild(0).position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(0,0,0);
        _spline.Clear();
    }

    public Transform GetKnob()
    {
        return GetComponentInChildren<SplineAnimate>().transform;
    }
}
