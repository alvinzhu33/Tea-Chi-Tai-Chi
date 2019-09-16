using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndirectComponent : IDisposable {
    private class Proxy : MonoBehaviour
    {
        public event Action onStart;
        public event Action onUpdate;
        private void Start()
        {
            
        }
        private void Update()
        {
            
        }
    }

    Proxy proxy;

    public IndirectComponent(string constructorArgument0, int constructorArgument1)
    {
        proxy = new GameObject("Proxy").AddComponent<Proxy>();
        proxy.onUpdate += HandleUnityUpdate;
    }

    private void HandleUnityUpdate()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        GameObject.Destroy(proxy.gameObject);
    }
}
