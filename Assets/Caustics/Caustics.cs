using UnityEngine;
using System.Collections;
 
public class Caustics : MonoBehaviour
{
    private Projector projector;
    public UnityEngine.Video.VideoClip mt;
 
    void Start()
    {
        var vp = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
        vp.playOnAwake = false;
        vp.clip = mt;
        vp.isLooping = true;

        //vp.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        //vp.targetMaterialRenderer = GetComponent<Renderer>();
        //vp.targetMaterialProperty = "_ShadowTex";

        projector = GetComponent<Projector>();
        projector.material.SetTexture("_ShadowTex", vp.texture);
        
        vp.Play();
    }
}