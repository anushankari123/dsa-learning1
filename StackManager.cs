using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class StackManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public Transform arSessionOrigin;
    public TMP_Text infoText;
    public TMP_Text codeText;  // UI Text to Display C Code

    private Stack<GameObject> stack = new Stack<GameObject>();
    private Vector3 stackBasePosition = new Vector3(0, 0, 0.5f);
    private float nodeSpacing = 0.025f;

    void Start()
    {
        infoText.text = "Click Push to add elements.";
        codeText.text = "";
    }

     public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PushNode()
    {
        GameObject newNode = Instantiate(nodePrefab, stackBasePosition + Vector3.up * stack.Count * nodeSpacing, Quaternion.identity);
        stack.Push(newNode);

        infoText.text = "Pushed Node: " + stack.Count;

        // Display Push Code Snippet in C
        codeText.text = "struct Stack {\n" +
                        "    int data;\n" +
                        "    struct Stack* next;\n" +
                        "};\n\n" +
                        "void push(struct Stack** top, int value) {\n" +
                        "    struct Stack* newNode = (struct Stack*)malloc(sizeof(struct Stack));\n" +
                        "    newNode->data = value;\n" +
                        "    newNode->next = *top;\n" +
                        "    *top = newNode;\n" +
                        "}";
    }

    public void PopNode()
    {
        if (stack.Count == 0)
        {
            infoText.text = "Stack is empty!";
            return;
        }

        GameObject topNode = stack.Pop();
        Destroy(topNode);

        infoText.text = stack.Count > 0 ? "Popped Node. Stack Top: " + stack.Count : "Stack is empty!";

        // Display Pop Code Snippet in C
        codeText.text = "void pop(struct Stack** top) {\n" +
                        "    if (*top == NULL) return;\n" +
                        "    struct Stack* temp = *top;\n" +
                        "    *top = (*top)->next;\n" +
                        "    free(temp);\n" +
                        "}";
    }

    public void PeekNode()
    {
        if (stack.Count == 0)
        {
            infoText.text = "Stack is empty!";
            return;
        }

        infoText.text = "Top Node: " + stack.Count;

        // Display Peek Code Snippet in C
        codeText.text = "int peek(struct Stack* top) {\n" +
                        "    if (top == NULL) return -1;\n" +
                        "    return top->data;\n" +
                        "}";
    }
}
