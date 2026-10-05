using System.Collections;
using UnityEngine;

public class BlinkBlock : MonoBehaviour
{
    public bool exist;

    private MeshRenderer meshRenderer;
    private Collider blockCollider;
    private Material blockMaterial;
    private AudioSource audioSource;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioClip lowSound;
    [SerializeField] private AudioClip highSound;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        meshRenderer = GetComponent<MeshRenderer>();
        blockCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        blockMaterial = meshRenderer.material;


        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {

        while (true)
        {

            yield return new WaitUntil(() => gameManager.isGamePlaying);


            // 3秒待つ
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(1);

                if (!gameManager.isGamePlaying)
                {
                    break;
                }

                audioSource.PlayOneShot(lowSound);
            }

            if (!gameManager.isGamePlaying)
                continue;

            exist = !exist;

            yield return new WaitForSeconds(1);

            if (!gameManager.isGamePlaying)
                continue;


            audioSource.PlayOneShot(highSound);

            Color color2 = blockMaterial.color;
            color2.a = exist ? 1.0f : 0.5f;

            blockCollider.enabled = exist;
            blockMaterial.color = color2;
        }
    }
}