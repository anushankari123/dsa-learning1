using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HeapManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject linkPrefab;
    public Transform arSessionOrigin;
    public TMP_Text infoText;
    public TMP_Text codeText;    // UI Text for Code Snippets

    private List<GameObject> nodes = new List<GameObject>();
    private List<GameObject> links = new List<GameObject>();
    
    // Starting position for the root node
    private Vector3 rootPosition = new Vector3(0, 0.2f, 0.5f);
    private float horizontalSpacing = 0.08f;
    private float verticalSpacing = 0.12f;

    void Start()
    {
        infoText.text = "Click Insert to add elements to heap.";
        codeText.text = "";
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");  // Note the full path
    }

    public void InsertNode()
    {
        int nodeIndex = nodes.Count;
        
        // Calculate position based on heap structure
        int level = Mathf.FloorToInt(Mathf.Log(nodeIndex + 1, 2));
        int position = nodeIndex - (int)Mathf.Pow(2, level) + 1;
        float xOffset = position * horizontalSpacing * Mathf.Pow(2, 3 - level);
        
        if (level == 0)
        {
            // Root node
            xOffset = 0;
        }
        else
        {
            // Center the level
            xOffset -= (Mathf.Pow(2, level) - 1) * horizontalSpacing * Mathf.Pow(2, 2 - level);
        }
        
        Vector3 nodePosition = rootPosition + new Vector3(xOffset, -level * verticalSpacing, 0);
        
        GameObject newNode = Instantiate(nodePrefab, nodePosition, Quaternion.identity);
        nodes.Add(newNode);
        
        // Add link to parent if not root
        if (nodeIndex > 0)
        {
            int parentIndex = (nodeIndex - 1) / 2;
            GameObject parentNode = nodes[parentIndex];
            
            GameObject newLink = Instantiate(linkPrefab, parentNode.transform.position, Quaternion.identity);

// Calculate direction vector from parent to child
Vector3 direction = newNode.transform.position - parentNode.transform.position;
            
// Orient link to look at child node
newLink.transform.LookAt(newNode.transform.position);
            
// Set the scale of the link to match the distance
float distance = direction.magnitude;
newLink.transform.localScale = new Vector3(newLink.transform.localScale.x, 
                                           newLink.transform.localScale.y, 
                                           distance);
            
// Move the pivot point to start from parent (assuming the link prefab has its pivot at one end)
newLink.transform.position = parentNode.transform.position + direction * 0.5f;
        }
        
        infoText.text = "Inserted Node: " + (nodeIndex + 1);
        
        // Show heap insertion code
        codeText.text = "void heapInsert(int heap[], int* size, int value) {\n" +
                        "    // Insert at the end\n" +
                        "    int i = *size;\n" +
                        "    heap[i] = value;\n" +
                        "    (*size)++;\n\n" +
                        "    // Bubble up\n" +
                        "    while (i > 0 && heap[parent(i)] < heap[i]) {\n" +
                        "        swap(&heap[i], &heap[parent(i)]);\n" +
                        "        i = parent(i);\n" +
                        "    }\n" +
                        "}\n\n" +
                        "int parent(int i) {\n" +
                        "    return (i - 1) / 2;\n" +
                        "}";
    }
    
    public void ExtractMax()
    {
        if (nodes.Count == 0)
        {
            infoText.text = "Heap is empty!";
            return;
        }
        
        // Remove root node (max element in max heap)
        GameObject rootNode = nodes[0];
        
        if (nodes.Count == 1)
        {
            // Last node in heap
            nodes.RemoveAt(0);
            Destroy(rootNode);
            infoText.text = "Heap is now empty";
        }
        else
        {
            // Replace root with last node and heapify down
            GameObject lastNode = nodes[nodes.Count - 1];
            
            // Move last node to root position
            lastNode.transform.position = rootPosition;
            
            // Remove the last node from list and put it at root
            nodes.RemoveAt(nodes.Count - 1);
            nodes[0] = lastNode;
            
            // Remove link to the last node
            if (links.Count > 0)
            {
                GameObject lastLink = links[links.Count - 1];
                links.RemoveAt(links.Count - 1);
                Destroy(lastLink);
            }
            
            // Destroy the original root
            Destroy(rootNode);
            
            infoText.text = "Extracted max element";
            
            // A full heapify-down implementation would go here
            // For visualization purposes, we're just moving the node
        }
        
        codeText.text = "int extractMax(int heap[], int* size) {\n" +
                        "    if (*size <= 0) return -1;\n\n" +
                        "    // Store the maximum value\n" +
                        "    int max = heap[0];\n\n" +
                        "    // Replace root with last element\n" +
                        "    heap[0] = heap[*size - 1];\n" +
                        "    (*size)--;\n\n" +
                        "    // Heapify-down\n" +
                        "    maxHeapify(heap, *size, 0);\n\n" +
                        "    return max;\n" +
                        "}\n\n" +
                        "void maxHeapify(int heap[], int size, int i) {\n" +
                        "    int largest = i;\n" +
                        "    int left = 2 * i + 1;\n" +
                        "    int right = 2 * i + 2;\n\n" +
                        "    if (left < size && heap[left] > heap[largest])\n" +
                        "        largest = left;\n\n" +
                        "    if (right < size && heap[right] > heap[largest])\n" +
                        "        largest = right;\n\n" +
                        "    if (largest != i) {\n" +
                        "        swap(&heap[i], &heap[largest]);\n" +
                        "        maxHeapify(heap, size, largest);\n" +
                        "    }\n" +
                        "}";
    }
    
    public void BuildHeap()
    {
        if (nodes.Count <= 1)
        {
            infoText.text = "Need more nodes to build heap!";
            return;
        }
        
        // For visualization, we'll just reposition the nodes in a proper heap structure
        // In a real heap, this would perform heapify operations
        
        for (int i = 0; i < nodes.Count; i++)
        {
            int level = Mathf.FloorToInt(Mathf.Log(i + 1, 2));
            int position = i - (int)Mathf.Pow(2, level) + 1;
            float xOffset = position * horizontalSpacing * Mathf.Pow(2, 3 - level);
            
            if (level == 0)
            {
                xOffset = 0;
            }
            else
            {
                xOffset -= (Mathf.Pow(2, level) - 1) * horizontalSpacing * Mathf.Pow(2, 2 - level);
            }
            
            Vector3 nodePosition = rootPosition + new Vector3(xOffset, -level * verticalSpacing, 0);
            nodes[i].transform.position = nodePosition;
        }
        
        infoText.text = "Built heap structure";
        
        codeText.text = "void buildMaxHeap(int heap[], int size) {\n" +
                        "    // Build heap (rearrange array)\n" +
                        "    for (int i = size / 2 - 1; i >= 0; i--)\n" +
                        "        maxHeapify(heap, size, i);\n" +
                        "}\n\n" +
                        "void heapSort(int arr[], int n) {\n" +
                        "    // Build heap\n" +
                        "    buildMaxHeap(arr, n);\n\n" +
                        "    // Extract elements from heap one by one\n" +
                        "    for (int i = n - 1; i > 0; i--) {\n" +
                        "        // Move current root to end\n" +
                        "        swap(&arr[0], &arr[i]);\n\n" +
                        "        // Max heapify on the reduced heap\n" +
                        "        maxHeapify(arr, i, 0);\n" +
                        "    }\n" +
                        "}";
    }
}