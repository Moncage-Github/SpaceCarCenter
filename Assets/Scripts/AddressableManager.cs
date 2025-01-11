using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour
{
    [SerializeField] protected AssetLabelReference AssetLabel;

    private SerializableDictionary<string, GameObject> _prefabs = new();
    private AsyncOperationHandle<IList<Object>> _handle;

    public SerializableDictionary<string, GameObject> Prefabs
    {
        get => _prefabs;
        protected set => _prefabs = value;
    }

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
        _handle.Completed += CompleteLoadAsset;

        while (!_handle.IsDone)
        {
            yield return null;
        }
    }

    public virtual void CompleteLoadAsset(AsyncOperationHandle<IList<Object>> handle)
    {
        if (_handle.Status == AsyncOperationStatus.Succeeded)
        {
            var result = _handle.Result;
            foreach (var obj in result)
            {
                Prefabs[obj.name] = (GameObject)obj;
            }
        }
    }
}
