using UnityEngine.UI;
using UnityEngine;

public class bullets : MonoBehaviour
{
    public Text bullet;

    // Update is called once per frame
    void Update()
    {
        bullet.text = (WeaponManager.currentammo.ToString()+"/"+ WeaponManager.totalammo.ToString());
    }
}
