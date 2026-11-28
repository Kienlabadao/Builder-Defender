using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PrefabPool<T> where T : Component {
    private readonly ObjectPool<T> objectPool;
    private readonly List<T> prewarmedObjects = new();

    public PrefabPool(
        T prefab,
        Transform parent,
        int initialSize,
        int maxSize = 100,
        bool preWarm = true
    ) {
        objectPool = new ObjectPool<T>(
            createFunc: () => Object.Instantiate(prefab, parent),
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: pipeGroup => {
                if (Application.isPlaying) {
                    Object.Destroy(pipeGroup);
                } else {
                    Object.DestroyImmediate(pipeGroup);
                }
            },
            collectionCheck: true,
            defaultCapacity: initialSize,
            maxSize: maxSize
        );

        if (preWarm) {
            for (int i = 0; i < initialSize; i++) {
                T obj = objectPool.Get();
                obj.gameObject.SetActive(false);

                prewarmedObjects.Add(obj);
            }

            foreach (T obj in prewarmedObjects) {
                objectPool.Release(obj);
            }
        }
    }

    private IEnumerator ReleaseCoroutine(T obj, float time) {
        yield return new WaitForSeconds(time);
        objectPool.Release(obj);
    }

    public T Get() {
        return objectPool.Get();
    }

    public void Release(T obj) {
        objectPool.Release(obj);
    }

    public void Release(T obj, float time) {
        CoroutineRunner.Instance.RunCoroutine(ReleaseCoroutine(obj, time));
    }
}
