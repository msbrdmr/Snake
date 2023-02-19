using UnityEngine;
public class FoodController : MonoBehaviour
{
    void Start()
    {
        var rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.red);
    }
}
