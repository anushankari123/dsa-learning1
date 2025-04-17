using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class QueueManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public Transform arSessionOrigin;
    public TMP_Text infoText;
    public TMP_Text codeText;    // UI Text to Display C Code

    private Queue<GameObject> queue = new Queue<GameObject>();
    private Vector3 queueStartPosition = new Vector3(0, 0, 0.5f);
    private float nodeSpacing = 0.025f;

    void Start()
    {
        infoText.text = "Click Enqueue to add elements.";
        codeText.text = "";
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void EnqueueNode()
    {
        GameObject newNode = Instantiate(nodePrefab, queueStartPosition + Vector3.right * queue.Count * nodeSpacing, Quaternion.identity);
        queue.Enqueue(newNode);

        infoText.text = "Enqueued Node: " + queue.Count;

        // Display Enqueue Code Snippet in C
        codeText.text = "struct Queue {\n" +
                        "    int data;\n" +
                        "    struct Queue* next;\n" +
                        "};\n\n" +
                        "void enqueue(struct Queue** front, struct Queue** rear, int value) {\n" +
                        "    struct Queue* newNode = (struct Queue*)malloc(sizeof(struct Queue));\n" +
                        "    newNode->data = value;\n" +
                        "    newNode->next = NULL;\n" +
                        "    if (*rear == NULL) {\n" +
                        "        *front = *rear = newNode;\n" +
                        "    } else {\n" +
                        "        (*rear)->next = newNode;\n" +
                        "        *rear = newNode;\n" +
                        "    }\n" +
                        "}";
    }

    public void DequeueNode()
    {
        if (queue.Count == 0)
        {
            infoText.text = "Queue is empty!";
            return;
        }

        GameObject frontNode = queue.Dequeue();
        Destroy(frontNode);

        // Shift remaining nodes left
        int index = 0;
        foreach (GameObject node in queue)
        {
            node.transform.position = queueStartPosition + Vector3.right * index * nodeSpacing;
            index++;
        }

        infoText.text = queue.Count > 0 ? "Dequeued Node. Front → " + queue.Peek().name : "Queue is empty!";

        // Display Dequeue Code Snippet in C
        codeText.text = "void dequeue(struct Queue** front, struct Queue** rear) {\n" +
                        "    if (*front == NULL) return;\n" +
                        "    struct Queue* temp = *front;\n" +
                        "    *front = (*front)->next;\n" +
                        "    if (*front == NULL) *rear = NULL;\n" +
                        "    free(temp);\n" +
                        "}";
    }
}
