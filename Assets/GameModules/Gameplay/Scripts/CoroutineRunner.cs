using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner instance;
    public static CoroutineRunner Instance => instance ??= GetInstance();

    private static CoroutineRunner GetInstance()
    {
        var coroutineRunner = new GameObject("Coroutine Runner").AddComponent<CoroutineRunner>();
        DontDestroyOnLoad(coroutineRunner.gameObject);
        return instance = coroutineRunner;
    }

    public static Coroutine StartRoutine(IEnumerator routine, MonoBehaviour monoBehaviour = null) =>
        monoBehaviour == null ? Instance.StartCoroutine(routine) : monoBehaviour.StartCoroutine(routine);

    public static void StopRoutine(Coroutine routine, MonoBehaviour monoBehaviour = null)
    {
        if (routine == null)
            return;

        if (monoBehaviour == null)
            Instance.StopCoroutine(routine);
        else
            monoBehaviour.StopCoroutine(routine);
    }
}