using UnityEngine.UI;
using UnityEngine;

public class Martyer : MonoBehaviour
{
    
    public Text scoreText;

    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        scoreText.text = ("Score "+(EnemyManager.Deadbody*10).ToString());
    }
}
