using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.CRT
{
	public class CRTFeature : ScriptableRendererFeature
	{
		public class Pass : ScriptableRenderPass
		{
			public RenderTargetIdentifier m_Source;
			public Material m_MatFunc;
			public Material m_MatPostPro;
			public bool m_TurnOff = false;
			public int m_DownsampleScale = 1;
			RenderTargetIdentifier m_RtID1;
			RenderTargetIdentifier m_RtID2;
			int m_RtPropID1 = 0;
			int m_RtPropID2 = 0;

			public Pass()
			{
				this.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
			}
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				int width = cameraTextureDescriptor.width / m_DownsampleScale;
				int height = cameraTextureDescriptor.height / m_DownsampleScale;

				m_RtPropID1 = Shader.PropertyToID("tmpRT1");
				m_RtPropID2 = Shader.PropertyToID("tmpRT2");
				cmd.GetTemporaryRT(m_RtPropID1, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				cmd.GetTemporaryRT(m_RtPropID2, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				m_RtID1 = new RenderTargetIdentifier(m_RtPropID1);
				m_RtID2 = new RenderTargetIdentifier(m_RtPropID2);
				ConfigureTarget(m_RtID1);
				ConfigureTarget(m_RtID2);
			}
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer cmd = CommandBufferPool.Get("CRTFeature");
				cmd.Blit(m_Source, m_RtID1, m_MatFunc, 0);

				cmd.SetGlobalTexture("_BlurTex", m_RtID1);
				cmd.Blit(m_Source, m_RtID2, m_MatPostPro);
				if (m_TurnOff)
				{
					cmd.Blit(m_RtID2, m_RtID1, m_MatFunc, 1);
					cmd.Blit(m_RtID1, m_RtID2, m_MatFunc, 2);
					cmd.Blit(m_RtID2, m_Source, m_MatFunc, 3);
				}
				else
				{
					cmd.Blit(m_RtID2, m_RtID1, m_MatFunc, 2);
					cmd.Blit(m_RtID1, m_Source, m_MatFunc, 3);
				}
				context.ExecuteCommandBuffer(cmd);
				CommandBufferPool.Release(cmd);
			}
			public override void FrameCleanup(CommandBuffer cmd)
			{
				cmd.ReleaseTemporaryRT(m_RtPropID1);
				cmd.ReleaseTemporaryRT(m_RtPropID2);
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		Material m_MatFunc;
		Material m_MatPostPro;
		Pass m_Pass;
		bool m_TurnOff;
		public override void Create()
		{
			m_Pass = new Pass();
		}
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			RenderTargetIdentifier src = renderer.cameraColorTarget;
			m_Pass.m_Source = renderer.cameraColorTarget;
			m_Pass.m_MatFunc = m_MatFunc;
			m_Pass.m_MatPostPro = m_MatPostPro;
			m_Pass.m_TurnOff = m_TurnOff;
			renderer.EnqueuePass(m_Pass);
		}
		public void SetupMaterial(Material matFunc, Material matPostPro)
		{
			m_MatFunc = matFunc;
			m_MatPostPro = matPostPro;
		}
		public void SetTurnOff(bool enable)
		{
			m_TurnOff = enable;
		}
	}
}