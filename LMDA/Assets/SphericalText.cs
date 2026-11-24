using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class SphericalText : MonoBehaviour
{
    public float radius = 5f;
    public float curvature = 1f;

    private TextMeshPro textMesh;

    void LateUpdate()
    {
        if (!textMesh) textMesh = GetComponent<TextMeshPro>();

        textMesh.ForceMeshUpdate();
        var textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                Vector3 original = vertices[vertexIndex + j];

                float angle = (original.x / radius) * curvature;

                float newX = Mathf.Sin(angle) * radius;
                float newZ = Mathf.Cos(angle) * radius;

                vertices[vertexIndex + j] = new Vector3(
                    newX,
                    original.y,
                    newZ - radius // garde le texte devant
                );
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
