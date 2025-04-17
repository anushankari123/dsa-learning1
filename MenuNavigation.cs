using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MenuNavigation : MonoBehaviour
{
    public void OpenStack()
    {
        Debug.Log("Attempting to load Stack Scene");
        SceneManager.LoadScene(1);
    }

    public void OpenQueue()
    {
        Debug.Log("Attempting to load Queue Scene");
        SceneManager.LoadScene(2);
    }

    public void OpenLinkedList()
    {
        Debug.Log("Attempting to load LinkedList Scene");
        SceneManager.LoadScene(3);
    }

    public void OpenHeap()
    {
        Debug.Log("Attempting to load Heap Scene");
        SceneManager.LoadScene(4);
    }
}