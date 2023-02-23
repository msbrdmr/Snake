using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StepController : MonoBehaviour
{
    public static StepController instance;
    public GameObject nodeprefab, InfoHeader, LoseHeader, replaybutton, foodprefab;
    [SerializeField] private int gridStepSize = 10, length = 5;
    private int score = 0;
    public TextMeshProUGUI scoretext, LoseHeaderScoreText;
    public float stepinterval = 15;
    private bool isStarted;
    private List<GameObject> nodelist = new List<GameObject>();
    private Vector3 temppos, foodPos, HeadPosition;
    private GameObject currentFood;
    private Vector2 dir;
    private void Start()
    {
        scoretext.text = score.ToString();
        if (instance == null) { instance = this; }
        spawnFood();
        for (int i = 0; i < length; i++)
        {
            var initialnode = Instantiate(nodeprefab, new Vector3(0, 10, 0 - i * gridStepSize), Quaternion.identity);

            pushStart(initialnode);
        }
        dir = Vector2.up;
        HeadPosition = gethead().transform.position;
    }
    private void Update()
    {

        if (!isStarted && Input.GetKeyDown(KeyCode.Space))
        {
            InfoHeader.SetActive(false);
            isStarted = true;
            StartCoroutine(Step());
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            Vector2 newdir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (dir * -1 == newdir) return;
            else dir = newdir;
        }
    }

    public void spawnFood()
    {
        Destroy(currentFood);
        currentFood = null;
        int randx = 10 * Random.Range(-5, 6);
        int randz = 10 * Random.Range(-5, 6);
        var pos = new Vector3(randx, 10, randz);
        //if pos is in occupied list, respawn
        var newfood = Instantiate(foodprefab, pos, Quaternion.identity);
        foodPos = newfood.transform.position;
        currentFood = newfood;
        if (occupied(pos, 0)) spawnFood();
    }
    private bool occupied(Vector3 pos, int mode)
    {
        if (mode == 0)
        {
            foreach (GameObject node in nodelist)
            {
                if (node.transform.position == pos)
                {
                    return true;
                }
            }
        }
        else if (mode == 1)
        {
            foreach (GameObject node in nodelist)
            {
                if (node.transform.position == pos)
                {
                    if (node == gethead()) return false;
                    return true;
                }
            }
        }
        return false;
    }
    private void pushStart(GameObject tailnode) // will be used when new node is eaten
    {
        nodelist.Insert(0, tailnode);
        // nodelist[0].GetComponent<SnakeData>().isTail = true;
    }
    public void pushEnd(GameObject headnode)
    {
        if (nodelist != null)
        {
            nodelist.Add(headnode);
            nodelist[nodelist.Count - 2].GetComponent<SnakeData>().isHead = false;
            nodelist[nodelist.Count - 1].GetComponent<SnakeData>().isHead = true;
            nodelist[nodelist.Count - 1].GetComponent<SnakeData>().isTail = false;
        }
    }
    public GameObject gettail(int mode)
    {
        if (mode == 0)
        {
            if (nodelist != null) return nodelist[0];
            else return null;
        }
        else if (mode == 1)
        {
            if (nodelist != null) return nodelist[1];
            else return null;
        }
        else return null;
    }
    public GameObject gethead()
    {
        if (nodelist != null) return nodelist[nodelist.Count - 1];
        else return null;
    }
    public void movestack()
    {
        if (nodelist != null)
        {
            var tempnode = nodelist[0];
            nodelist.RemoveAt(0);
            nodelist[0].GetComponent<SnakeData>().isTail = true;
            pushEnd(tempnode);
        }
    }

    public void EatFood()
    {
        length++;
        score++;
        scoretext.text = score.ToString();
        GameObject newnode = Instantiate(nodeprefab);
        pushStart(newnode);
        newnode.transform.position = temppos;
        Destroy(currentFood);
        spawnFood();
    }

    private IEnumerator Step()
    {
        yield return new WaitForSeconds(5f / stepinterval);
        GameObject tailnode = gettail(0);
        temppos = tailnode.transform.position;
        Vector3 pos = new Vector3(HeadPosition.x + gridStepSize * dir[0], 10, HeadPosition.z + gridStepSize * dir[1]);
        if (pos.x == -60) pos.x = -pos.x - gridStepSize;
        if (pos.x == 60) pos.x = -pos.x + gridStepSize;
        if (pos.z == -60) pos.z = -pos.z - gridStepSize;
        if (pos.z == 60) pos.z = -pos.z + gridStepSize;
        tailnode.transform.position = pos;
        HeadPosition = tailnode.transform.position;
        movestack();
        if (pos == foodPos) EatFood();
        if (occupied(pos, 1)) losegame();
        StartCoroutine(Step());
    }

    public void losegame()
    {
        Time.timeScale = 0;
        LoseHeader.SetActive(true);
        LoseHeaderScoreText.text = score.ToString();
        replaybutton.SetActive(true);
    }

    public void resetgame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

}
