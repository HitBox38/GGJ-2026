using UnityEngine;
using System.Collections;

public class GooseMask : MaskObject 
{
    [SerializeField] private float debuffRestMinSeconds = 2f;
    [SerializeField] private float debuffRestMaxSeconds = 10f;
    
    [SerializeField, Space(5)] private float ravenPatrolRadius = 3f;
    [SerializeField] private float ravenPatrolHeight = 4f;
    
    public float DebuffRestMinTime => debuffRestMinSeconds;
    public float DebuffRestMaxTime => debuffRestMaxSeconds;
    public float RavenPatrolRadius => ravenPatrolRadius;
    public float RavenPatrolHeight => ravenPatrolHeight;
    
    public class GooseFriend : MaskEffect {}
    
    public class RavenCaller : MaskEffect
    {
        private RavenHazard[] _ravens;
        private RavenGoosePatrol _maskPatrol;
        private GooseMask _gooseMask;
        private Coroutine _activeDebuffRoutine;
        
        // Find the closest raven to the player
        private void Start()
        {
            _gooseMask = GetComponentInChildren<GooseMask>();
            _ravens = 
                FindObjectsByType<RavenHazard>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            // init patrol data
            _maskPatrol = new RavenGoosePatrol();
            // create the points
            CreateTransforms();

            _activeDebuffRoutine = StartCoroutine(DebuffRoutine());
        }

        private IEnumerator DebuffRoutine()
        {
            // wait for a random time between the min and max time (in seconds)
            var randInterval = Random.Range(_gooseMask.DebuffRestMinTime, _gooseMask.DebuffRestMaxTime);
            Debug.Log($"Goose mask's honks in {randInterval} seconds.");
            yield return new WaitForSeconds(randInterval);
            Debug.Log("Goose mask's HONKED!");
            UpdateTransforms();
            var closestRaven = GetClosestRaven();
            closestRaven.ActivateGooseMaskEffect(_maskPatrol);
            _activeDebuffRoutine = StartCoroutine(DebuffRoutine());
        }

        private RavenHazard GetClosestRaven()
        {
            RavenHazard closest = null;
            var minSqrDistance = float.MaxValue;
            var currentPos = transform.position;

            foreach (var t in _ravens)
            {
                if (!t) continue;
                var sqrDist = (t.transform.position - currentPos).sqrMagnitude;
                if (!(sqrDist < minSqrDistance)) continue;
                minSqrDistance = sqrDist;
                closest = t;
            }

            return closest;
        }
        
        private void CreateTransforms()
        {
            // create two points on the goose's path that the raven will patrol around
            _maskPatrol.GoosePatrolPointA = new GameObject("Goose Mask Point A").transform;
            _maskPatrol.GoosePatrolPointB = new GameObject("Goose Mask Point B").transform;
        }

        private void UpdateTransforms()
        {
            // update the points
            _maskPatrol.GoosePatrolPointA.position =
                transform.position + new Vector3(_gooseMask.RavenPatrolRadius, _gooseMask.RavenPatrolHeight);
            _maskPatrol.GoosePatrolPointB.position =
                transform.position + new Vector3(-_gooseMask.RavenPatrolRadius, _gooseMask.RavenPatrolHeight);
        }

        private void DestroyTransforms()
        {
            Destroy(_maskPatrol?.GoosePatrolPointA?.gameObject);
            Destroy(_maskPatrol?.GoosePatrolPointB?.gameObject);
        }
        
        private void OnDestroy()
        {
            // stop the routine
            StopCoroutine(_activeDebuffRoutine);
            // clean scene from new objects
            DestroyTransforms();
        }
    }

    public override void ApplyEffects()
    {
        // add the goose friend buff component
        AddBuff<GooseFriend>();
        // add the raven caller debuff component
        AddDebuff<RavenCaller>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
