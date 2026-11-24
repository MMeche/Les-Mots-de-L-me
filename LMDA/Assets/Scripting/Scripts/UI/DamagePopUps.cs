using System.Collections;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.ShaderGraph.Internal;
#endif
using UnityEngine;

public class DamagePopUps : MonoBehaviour
{
    float _damage;
    TextMeshPro _tmp;
    DamageType _type;
    Animation _anim;
    private void OnEnable()
    {
        transform.localScale = new Vector3(1 / transform.parent.localScale.x, 1 / transform.parent.localScale.y, 1 / transform.parent.localScale.z);
        //if (_type == 0) transform.localScale *= 3;
        //else transform.localScale *= 1.5f;
    }
    void Start()
    {
        _tmp = GetComponent<TextMeshPro>();
        _tmp.text = _damage.ToString();
        transform.rotation = FindFirstObjectByType<CinemachineBrain>().OutputCamera.transform.rotation;
        _anim = GetComponent<Animation>();
        if (_type == 0) _anim.clip = _anim.GetClip("PlayerDamageAnimation");
        else _anim.clip = _anim.GetClip("EnemyDamageAnimation");
        _anim.Play();

    }

    void Update()
    {
        transform.rotation = FindFirstObjectByType<CinemachineBrain>().OutputCamera.transform.rotation;
    }

    private void DestroyGameObject()
    {

        Destroy(transform.parent.gameObject);
    }
    public void SetDamage(float d)
    {
        _damage = d;
    }
    public void SetType(DamageType type)
    {
        _type = type;
    }

    public Animation GetAnim()
    {
        return _anim;
    }
}

public enum DamageType
    {
        PlayerDamage, //pour les dégâts qu'inglige le joueur à l'ennemi
        EnemyDamage // pour les dégâts qu'inflige l'ennemi au joueur.
    }
