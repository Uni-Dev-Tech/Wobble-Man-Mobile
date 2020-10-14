using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] UIPanels; // 0 - спойман, 1 - выйграл
    static public UIManager instance;
    private void Awake()
    {
        if (UIManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        UIManager.instance = this;
    }
    /// <summary>
    /// Включает выбранный элемент Канваса
    /// </summary>
    /// <param name="UI">Элемент канваса</param>
    /// <returns></returns>
    public IEnumerator SelectUI(GameObject UI)
    {
        yield return new WaitForSeconds(1f);
        UI.SetActive(true);
    }
}
