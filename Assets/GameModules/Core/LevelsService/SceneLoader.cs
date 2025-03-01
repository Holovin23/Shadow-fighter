using System;
using Cysharp.Threading.Tasks;
using TFPlay.SceneFader;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneLoader : IInitializable
{
    public event Action OnLevelLoaded;
    public event Action OnLevelStartLoading;

    public int CurrentScene { private set; get; }

    private int levelsCount;

    [Inject] private SceneFaderController _sceneFaderController;
    
    public void Initialize()
    {
        levelsCount = SceneManager.sceneCountInBuildSettings - 1;
    }

    public async UniTask LoadScene(int sceneId)
    {
        _sceneFaderController.Show();
        await UniTask.WaitUntil(() => _sceneFaderController.Shown);

        OnLevelStartLoading?.Invoke();

        await SceneManager.LoadSceneAsync(sceneId);
        CurrentScene = sceneId;

        OnLevelLoaded?.Invoke();
        
        _sceneFaderController.Hide();
   }

    private async UniTask UnloadScenes()
    {
        for (var i = 0; i < SceneManager.sceneCount; i++)
            await SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
    }
}