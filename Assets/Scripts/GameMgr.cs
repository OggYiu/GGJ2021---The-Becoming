using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    [SerializeField] private ParticleSystem sfxDeath;
    [SerializeField] private GameObject fishSprite;

    public bool IsGameOver() => _gameOver;

    private bool _gameOver = false;

    public void KillFish()
    {
        Fish fish = FindObjectOfType<Fish>();
        if(fish != null)
        {
            sfxDeath.transform.position = fish.transform.position;
            sfxDeath.Play();
            Destroy(fish.gameObject.GetComponent<Collider>());
            Destroy(fish.gameObject.GetComponent<MeshRenderer>());
            Destroy(fishSprite);
        }

        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(2.0f);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
