using UnityEngine;

public class MenuButtonScript : MonoBehaviour
{
    public GameObject menu;
    public void showmenu()
    {
        if (!menu.activeSelf)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;

            menu.SetActive(false);
        }
    }
}
