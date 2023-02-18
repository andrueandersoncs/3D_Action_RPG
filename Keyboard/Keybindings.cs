using System;
using UniRx;
using UnityEngine;

namespace Keyboard
{
    public static class Keybindings
    {
        public static IDisposable Bind(KeyCode key, Action action) =>
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetKeyDown(key))
                .Subscribe(_ => action());
    }
}