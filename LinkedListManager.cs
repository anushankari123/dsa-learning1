using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class LinkedListManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject linkPrefab;
    public Transform arSessionOrigin;
    public TMP_Text infoText;
    public TMP_Text codeText;  // New UI Text for Code Snippets

    private List<GameObject> nodes = new List<GameObject>();
    private List<GameObject> links = new List<GameObject>();

    private Vector3 startPosition = new Vector3(0, 0, 0.5f);
    private float nodeSpacing = 0.02f;

    void Start()
    {
        infoText.text = "Click Insert to add nodes.";
        codeText.text = "";  // Initially Empty
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

        public void InsertNode()
    {
        GameObject newNode = Instantiate(nodePrefab, startPosition + Vector3.right * nodes.Count * nodeSpacing, Quaternion.identity);
        nodes.Add(newNode);

        if (nodes.Count > 1)
        {
            GameObject newLink = Instantiate(linkPrefab, (nodes[nodes.Count - 2].transform.position + newNode.transform.position) / 2, Quaternion.identity);
            newLink.transform.LookAt(newNode.transform);
            links.Add(newLink);
        }

        infoText.text = "Inserted Node: " + nodes.Count + "\nHead → " + nodes[0].name;

        // Display Insert Code Snippet in C
        codeText.text = "struct Node {\n" +
                        "    int data;\n" +
                        "    struct Node* next;\n" +
                        "};\n\n" +
                        "void insertNode(struct Node** head, int value) {\n" +
                        "    struct Node* newNode = (struct Node*)malloc(sizeof(struct Node));\n" +
                        "    newNode->data = value;\n" +
                        "    newNode->next = *head;\n" +
                        "    *head = newNode;\n" +
                        "}";
    }

    public void DeleteNode()
    {
        if (nodes.Count == 0)
        {
            infoText.text = "List is empty!";
            return;
        }

        GameObject lastNode = nodes[nodes.Count - 1];
        nodes.RemoveAt(nodes.Count - 1);
        Destroy(lastNode);

        if (links.Count > 0)
        {
            GameObject lastLink = links[links.Count - 1];
            links.RemoveAt(links.Count - 1);
            Destroy(lastLink);
        }

        infoText.text = nodes.Count > 0 ? "Deleted last node. Head → " + nodes[0].name : "List is empty!";

        // Display Delete Code Snippet in C
        codeText.text = "void deleteNode(struct Node** head) {\n" +
                        "    if (*head == NULL) return;\n" +
                        "    struct Node* temp = *head;\n" +
                        "    *head = (*head)->next;\n" +
                        "    free(temp);\n" +
                        "}";
    }

    public void ReverseList()
    {
        if (nodes.Count <= 1)
        {
            infoText.text = "Not enough nodes to reverse!";
            return;
        }

        nodes.Reverse();
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].transform.position = startPosition + Vector3.right * i * nodeSpacing;
        }

        for (int i = 0; i < links.Count; i++)
        {
            Destroy(links[i]);
        }
        links.Clear();

        for (int i = 1; i < nodes.Count; i++)
        {
            GameObject newLink = Instantiate(linkPrefab, (nodes[i - 1].transform.position + nodes[i].transform.position) / 2, Quaternion.identity);
            newLink.transform.LookAt(nodes[i].transform);
            links.Add(newLink);
        }

        infoText.text = "Reversed List!";

        // Display Reverse Code Snippet in C
        codeText.text = "void reverseList(struct Node** head) {\n" +
                        "    struct Node* prev = NULL;\n" +
                        "    struct Node* current = *head;\n" +
                        "    struct Node* next = NULL;\n\n" +
                        "    while (current != NULL) {\n" +
                        "        next = current->next;\n" +
                        "        current->next = prev;\n" +
                        "        prev = current;\n" +
                        "        current = next;\n" +
                        "    }\n" +
                        "    *head = prev;\n" +
                        "}";
    }
}