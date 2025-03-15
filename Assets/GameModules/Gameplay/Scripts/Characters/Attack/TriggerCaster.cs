using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters
{
    public class TriggerCaster : MonoBehaviour
{
    [SerializeField] private List<CastPointInfo> castPoints;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private int maxColliders;

    private Collider[] colliders;

    private void Start()
    {
        colliders = new Collider[maxColliders];
    }
#if UNITY_EDITOR
    
    private void OnValidate()
    {
        for (int i = 0; i < castPoints.Count; i++)
        {
            if (!castPoints[i].isInited)
            {
                GameObject gO = new GameObject("Sphere");
                gO.transform.parent = transform;

                castPoints[i].transform = gO.transform;
                EditorUtility.SetDirty(gO);

                castPoints[i].radius = 1;

                castPoints[i].isInited = true;
            }
        }
    }
    
#endif

    public HashSet<Collider> Cast()
    {
        HashSet<Collider> hits = new HashSet<Collider>();


        for (int i = 0; i < castPoints.Count; i++)
        {
            var collCount = CastSphere(castPoints[i].transform.position, castPoints[i].radius, interactionLayer, out Collider[] overlapHit);
            if (collCount>0)
            {
                for (int j = 0; j < collCount; j++)
                {
                    hits.Add(overlapHit[j]);
                }
            }
        }
        return hits;
    }

    public void TryCollectCollidersForTime(float time, Action<HashSet<Collider>> CollidersList)
    {
        StartCoroutine(CollectColliders(time, CollidersList));
    }


    private IEnumerator CollectColliders(float time, Action<HashSet<Collider>> CollidersList)
    {
        HashSet<Collider> hits = new HashSet<Collider>();
        
        var currentTime = time;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            
            for (int i = 0; i < castPoints.Count; i++)
            {
                var collCount = CastSphere(castPoints[i].transform.position, castPoints[i].radius, interactionLayer, out Collider[] overlapHit);
                if (collCount>0)
                {
                    for (int j = 0; j < collCount; j++)
                    {
                        hits.Add(overlapHit[j]);
                    }
                }
            }

            CollidersList.Invoke(hits);
            yield return new WaitForSeconds(0.5f);
        }

        yield break;
    }

    public int CastSphere(Vector3 position, float radius, LayerMask layer, out Collider[] hits)
    {
        hits = colliders;
        int hitCount = Physics.OverlapSphereNonAlloc(position, radius, colliders, layer);
        return hitCount;
    }


    private void OnDrawGizmos()
    {
        for (int i = 0; i < castPoints.Count; i++)
        {
            if (castPoints[i].isInited)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(castPoints[i].transform.position, castPoints[i].radius);
            }
        }
    }
}

[System.Serializable]
public class CastPointInfo
{
    public bool isInited;
    public Transform transform;

    public float radius;
}
}