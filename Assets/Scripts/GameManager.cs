using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public int x;
    public GameObject menu;
    public int gridStepSize = 10;
    public GameObject tileprefab;
    public GameObject Parent;
    void Start()
    {
        for (int i = -x; i < x + 1; i++)
        {
            for (int j = -x; j < x + 1; j++)
            {
                var tile = Instantiate(tileprefab, new Vector3(i * gridStepSize, 0, j * gridStepSize), Quaternion.identity);
                tile.transform.SetParent(Parent.transform);
            }
        }
    }

    public void resume()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
    }

    public void exit()
    {
        Application.Quit();
    }
}
