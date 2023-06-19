using Sirenix.OdinInspector;
using UnityEngine;

public class EyeController : MonoBehaviour
{
    public Eye LeftEye, RightEye;
    public Transform target;  // The target object

    public void Init()
    {
        LeftEye.Initialize();
        RightEye.Initialize();
    }

    public void Tick()
    {
        LeftEye.Update(target.position);
        RightEye.Update(target.position);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget.transform;
    }

    public void SetTarget(Vector3 newTarget)
    {
        target.position = newTarget;
    }

    [Button]
    public void Test()
    {
        LeftEye.Initialize();
        RightEye.Initialize();
        LeftEye.Update(target.position);
        RightEye.Update(target.position);
    }

    [Button]
    public void CreateEyes()
    {
        var lefteye = new GameObject("LeftEye");
        lefteye.transform.SetParent(transform);
        var lmeshfilter = lefteye.AddComponent<MeshFilter>();
        var lmeshrenderer = lefteye.AddComponent<MeshRenderer>();

        var righteye = new GameObject("RightEye");
        righteye.transform.SetParent(transform);
        var rmeshfilter = righteye.AddComponent<MeshFilter>();
        var rmeshrenderer = righteye.AddComponent<MeshRenderer>();

        var leftpupil = new GameObject("LeftPupil");
        leftpupil.transform.SetParent(lefteye.transform);
        LeftEye._pupil = leftpupil.transform;
        LeftEye.Initialize();

        var rightpupil = new GameObject("RightPupil");
        rightpupil.transform.SetParent(righteye.transform);
        RightEye._pupil = rightpupil.transform;
        RightEye.Initialize();
    }
}

[System.Serializable]
public class Eye
{
    public Transform _pupil;
    public float _radius;
    private Vector3 _pupilCenter = Vector3.zero;
    public float _pupilZOffset = -0.0002f;
    public Eye(Transform pupil, float radius)
    {
        _pupil = pupil;
        _radius = radius;
    }

    public void Initialize()
    {
        Reset();
        _pupilCenter = Vector3.zero;
    }

    public void Update(Vector3 target)
    {
        // Calculate the direction to the target in local space
        Vector3 directionToTarget = _pupil.parent.InverseTransformPoint(target) - _pupilCenter;
        directionToTarget.z = _pupilZOffset;  // If you only want 2D movement

        // Normalize the direction to the target
        directionToTarget = directionToTarget.normalized;

        // Scale the direction by the eye radius only if its magnitude is less than _radius
        if (directionToTarget.magnitude < _radius)
        {
            directionToTarget = directionToTarget * _radius;
        }
        else
        {
            directionToTarget = directionToTarget * _radius;
        }


        // Make sure to respect the pupil's z position
        Vector3 pupiltarget = _pupilCenter + directionToTarget;
        pupiltarget.z = _pupilZOffset;
        _pupil.localPosition = pupiltarget;


    }

    [Button]
    public void Reset()
    {
        _pupil.localPosition = new Vector3(0, 0, _pupilZOffset);
    }



}
