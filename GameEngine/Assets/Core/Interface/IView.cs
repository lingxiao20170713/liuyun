using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView
{
    void Update();
    void Show();
    void Hide();
    int GetModuleType();
    void Destroy();
}
