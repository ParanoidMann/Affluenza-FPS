using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.Stylized
{
	public class StylizedBehaviour : MonoBehaviour
	{
		public StylizedFeature m_Feature;
		public StylizedFeature.EDrawStyle m_DrawStyle = StylizedFeature.EDrawStyle.One;
		public StylizedFeature.EBlendMode m_BlendMode = StylizedFeature.EBlendMode.AlphaBlend;
		public Material m_Mat;
//		[Header("Brightness Saturation Contrast")]
//		public float m_Saturation = 1f;
//		public float m_Brightness = 1f;
//		public float m_Contrast = 1f;
		[Header("Dot")]
		public Color m_Color = Color.white;
		public float m_Angle = 1.57f;
		public float m_Scale = 1f;
		[Header("Outline")]
		public Color m_LineColor = Color.black;
		[Range(0f, 1f)] public float m_LineNoise = 1f;
		[Range(0f, 3f)] public float m_LineIntensity = 2f;
		public float m_SensitivityDepth = 1f;
		public float m_SensitivityNormals = 1f;
		public float m_SampleDist = 1f;
		[Header("DrawNoise")]
		public float m_Step = 5f;
		[Range(0f, 1f)] public float m_StepNoiseScale = 0.15f;
		[Header("Blend")]
		[Range(0f, 1f)] public float m_Intensity = 0f;
		public Texture2D m_PaperOverlay;

		void Start()
		{
			m_Feature.m_Mat = m_Mat;
		}
		void Update()
		{
			if (m_DrawStyle == StylizedFeature.EDrawStyle.One)
			{
				m_Mat.SetFloat("_Saturation", 1f);
				m_Mat.SetFloat("_Brightness", 1.5f);
				m_Mat.SetFloat("_Contrast", 1f);
				m_BlendMode = StylizedFeature.EBlendMode.AlphaBlend;
			}
			if (m_DrawStyle == StylizedFeature.EDrawStyle.Two)
			{
				m_Mat.SetFloat("_Saturation", 1f);
				m_Mat.SetFloat("_Brightness", 0.6f);
				m_Mat.SetFloat("_Contrast", 1f);
				m_BlendMode = StylizedFeature.EBlendMode.AlphaBlend;
			}
			if (m_DrawStyle == StylizedFeature.EDrawStyle.Three)
			{
				m_Mat.SetFloat("_Saturation", 1f);
				m_Mat.SetFloat("_Brightness", 0.6f);
				m_Mat.SetFloat("_Contrast", 1f);
				m_BlendMode = StylizedFeature.EBlendMode.Overlay;
			}
//			m_Mat.SetFloat("_Saturation", m_Saturation);
//			m_Mat.SetFloat("_Brightness", m_Brightness);
//			m_Mat.SetFloat("_Contrast", m_Contrast);
			
			m_Mat.SetColor("_Color", m_Color);
			m_Mat.SetFloat("_Angle", m_Angle);
			m_Mat.SetFloat("_Scale", m_Scale);

			m_Mat.SetFloat("_Step", m_Step);
			m_Mat.SetFloat("_StepNoiseScale", m_StepNoiseScale);

			Vector2 sensitivity = new Vector2(m_SensitivityDepth, m_SensitivityNormals);
			m_Mat.SetColor("_LineColor", m_LineColor);
			m_Mat.SetVector("_Sensitivity", new Vector4(sensitivity.x, sensitivity.y, 1f, sensitivity.y));
			m_Mat.SetFloat("_SampleDistance", m_SampleDist);
			m_Mat.SetFloat("_LineNoise", m_LineNoise);
			m_Mat.SetFloat("_LineIntensity", m_LineIntensity);

			m_Mat.SetTexture("_PaperTex", m_PaperOverlay);
			m_Mat.SetFloat("_Intensity", m_Intensity);

			m_Feature.m_Style = m_DrawStyle;
			m_Feature.m_BlendMode = m_BlendMode;
		}
	}
}