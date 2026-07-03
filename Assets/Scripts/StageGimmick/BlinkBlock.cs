using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BlinkBlock : MonoBehaviour
{
    public bool exist;
    private MeshRenderer meshRenderer;
    private Collider blockCollider;
    private Material blockMaterial;
    [SerializeField] private AudioClip lowSound;
    [SerializeField] private AudioClip highSound;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        blockCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        blockMaterial = meshRenderer.material;
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator Blink()
    {
        while (true)
        {
            for(int i=0; i<3; i++)
            {
                yield return new WaitForSeconds(1);
                audioSource.PlayOneShot(lowSound);
            }
            exist = !exist;
            yield return new WaitForSeconds(1);
            audioSource.PlayOneShot(highSound);
            Color color = blockMaterial.color;
            color.a = exist ? 1.0f: 0.5f;
            blockCollider.enabled = exist;
            blockMaterial.color = color;
            blockCollider.enabled = exist;
        }
    }
}
