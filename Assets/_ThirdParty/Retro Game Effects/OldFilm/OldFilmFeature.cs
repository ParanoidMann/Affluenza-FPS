using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.OldFilm
{
	public class OldFilmFeature : ScriptableRendererFeature
	{
		public enum EDrawStyle { PassOne = 0, PassTwo = 1, PassThree = 2, PassFour = 3 }
		public class Pass : ScriptableRenderPass
		{
			public Material m_Mat;
			public RenderTargetIdentifier m_Source;
			public EDrawStyle m_DrawStyle = EDrawStyle.PassOne;
			public float m_Jitter = 0f;
			RenderTargetIdentifier m_RtID;
			int m_RtPropID = 0;

			public Pass()
			{
				this.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
			}
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor rtDesc)
			{
				m_RtPropID = Shader.PropertyToID("tmpRT");
				cmd.GetTemporaryRT(m_RtPropID, rtDesc.width, rtDesc.height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				m_RtID = new RenderTargetIdentifier(m_RtPropID);
				ConfigureTarget(m_RtID);
			}
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer cmd = CommandBufferPool.Get("OldFilm");

				Vector2 offset = new Vector2(m_Jitter * Random.value, m_Jitter * Random.value);
				m_Mat.SetVector("_Offset", offset);

				if (m_DrawStyle == EDrawStyle.PassOne)
					cmd.Blit(m_Source, m_RtID, m_Mat, 0);
				else if (m_DrawStyle == EDrawStyle.PassTwo)
					cmd.Blit(m_Source, m_RtID, m_Mat, 1);
				else if (m_DrawStyle == EDrawStyle.PassThree)
					cmd.Blit(m_Source, m_RtID, m_Mat, 2);
				else if (m_DrawStyle == EDrawStyle.PassFour)
					cmd.Blit(m_Source, m_RtID, m_Mat, 3);

				cmd.Blit(m_RtID, m_Source);
				context.ExecuteCommandBuffer(cmd);
				CommandBufferPool.Release(cmd);
			}
			public override void FrameCleanup(CommandBuffer cmd)
			{
				cmd.ReleaseTemporaryRT(m_RtPropID);
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public EDrawStyle m_DrawStyle = EDrawStyle.PassOne;
		public Material m_Mat;
		[Range(0.0f, 0.01f)] public float m_Jitter = 0f;
		Pass m_Pass;

		public override void Create()
		{
			m_Pass = new Pass();
		}
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			RenderTargetIdentifier src = renderer.cameraColorTarget;
			m_Pass.m_Mat = m_Mat;
			m_Pass.m_Source = src;
			m_Pass.m_DrawStyle = m_DrawStyle;
			m_Pass.m_Jitter = m_Jitter;
			renderer.EnqueuePass(m_Pass);
		}
	}
}