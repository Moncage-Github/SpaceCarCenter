using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class AddressableManager : MonoBehaviour
{
    [SerializeField] protected AssetLabelReference AssetLabel;

    private AsyncOperationHandle<IList<Object>> _handle;

    public void StartLoadAsset()
    {
        if (_handle.IsValid()) return;
        StartCoroutine(LoadAssetAsync());
    }

    public void ReleaseAsset()
    {
        if (!_handle.IsValid()) return;

        Addressables.Release(_handle);
    }

    private IEnumerator LoadAssetAsync()
    {
        _handle = Addressables.LoadAssetsAsync<Object>(AssetLabel, null);

        while (!_handle.IsDone)
        {
            yield return null;
        }

        CompleteLoadAsset(_handle);
    }

    public abstract void CompleteLoadAsset(AsyncOperationHandle<IList<Object>> handle);
}
