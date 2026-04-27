using UnityEngine;
using BLINK.RPGBuilder.AI;
using UnityEngine.SceneManagement;
public class HandleHighlight : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private GameObject currentHighlighted;
    public GameObject player;
    private AIEntity entity;

    private void Start()
    {
        entity = transform.parent.GetComponent<AIEntity>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 500))
        {
            if(hit.transform && player)
            {
                //player = GameObject.FindGameObjectWithTag("Player");
                Vector3 delta = hit.transform.position - player.transform.position;
                currentHighlighted = hit.transform.gameObject;
                if (delta.magnitude <= 3 && hit.transform.gameObject.name == transform.parent.gameObject.name && Input.GetMouseButton(1))
                {
                    entity.ResetMovement();
                    entity.LookAtPlayer();
                }
            }
        }
    }

    bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName && scene.isLoaded)
            {
                return true;
            }
        }
        return false;
    }
}