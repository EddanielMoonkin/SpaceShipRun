using UnityEngine;

public class MeshBallAsteroids : MonoBehaviour
{

	static int
		baseColorId = Shader.PropertyToID("_BaseColor");
		
	[SerializeField]
	Mesh mesh = default;

	[SerializeField]
	Material material = default;

	Matrix4x4[] matrices = new Matrix4x4[1023];
	Vector4[] baseColors = new Vector4[1023];

	MaterialPropertyBlock block;

	void Awake()
	{
		for (int i = 0; i < matrices.Length; i++)
		{
			matrices[i] = Matrix4x4.TRS(
				Random.insideUnitSphere * 1000f, 
				Quaternion.Euler(Random.value * 360f, Random.value * 360f, Random.value * 360f), 
				Vector3.one * Random.Range(0.1f, 1.5f));
			baseColors[i] =
				new Vector4(Random.Range(0.4f, 0.6f), Random.Range(0.4f, 0.6f), Random.Range(0.4f, 0.6f), Random.Range(0.1f, 1f));
			
		}
	}

	void Update()
	{
		if (block == null)
		{
			block = new MaterialPropertyBlock();
			block.SetVectorArray(baseColorId, baseColors);
		}
		Graphics.DrawMeshInstanced(mesh, 0, material, matrices, 1023, block);
	}
}