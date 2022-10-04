using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RetroGameEffects.Stylized
{
	public class DepthNormalFeature : ScriptableRendererFeature
	{
		class Pass : ScriptableRenderPass
		{
			RenderTargetHandle depthAttachmentHandle { get; set; }
			internal RenderTextureDescriptor descriptor { get; private set; }

			Material m_Mat = null;
			FilteringSettings m_FilteringSettings;
			ShaderTagId m_ShaderTagId = new ShaderTagId("DepthOnly");

			public Pass(RenderQueueRange renderQueueRange, LayerMask layerMask)
			{
				m_FilteringSettings = new FilteringSettings(renderQueueRange, layerMask);
				m_Mat = CoreUtils.CreateEngineMaterial("Hidden/Internal-DepthNormalsTexture");
				this.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
			}
			public void Setup(RenderTextureDescriptor baseDescriptor, RenderTargetHandle depthAttachmentHandle)
			{
				this.depthAttachmentHandle = depthAttachmentHandle;
				baseDescriptor.colorFormat = RenderTextureFormat.ARGB32;
				baseDescriptor.depthBufferBits = 32;
				descriptor = baseDescriptor;
			}
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				cmd.GetTemporaryRT(depthAttachmentHandle.id, descriptor, FilterMode.Point);
				ConfigureTarget(depthAttachmentHandle.Identifier());
				ConfigureClear(ClearFlag.All, Color.black);
			}
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer cmd = CommandBufferPool.Get("DepthNormals Prepass");
				{
					context.ExecuteCommandBuffer(cmd);
					cmd.Clear();

					var drawSettings = CreateDrawingSettings(m_ShaderTagId, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
					drawSettings.perObjectData = PerObjectData.None;

					ref CameraData data = ref renderingData.cameraData;
					Camera camera = data.camera;
					if (data.isStereoEnabled)
						context.StartMultiEye(camera);

					drawSettings.overrideMaterial = m_Mat;
					context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref m_FilteringSettings);

					cmd.SetGlobalTexture("_CameraDepthNormalsTexture", depthAttachmentHandle.id);
				}
				context.ExecuteCommandBuffer(cmd);
				CommandBufferPool.Release(cmd);
			}
			public override void FrameCleanup(CommandBuffer cmd)
			{
				cmd.ReleaseTemporaryRT(depthAttachmentHandle.id);
			}
		}

		Pass m_Pass;
		RenderTargetHandle m_RtDepthNormals;

		public override void Create()
		{
			m_Pass = new Pass(RenderQueueRange.opaque, -1);
			m_RtDepthNormals.Init("_CameraDepthNormalsTexture");
		}
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			m_Pass.Setup(renderingData.cameraData.cameraTargetDescriptor, m_RtDepthNormals);
			renderer.EnqueuePass(m_Pass);
		}
	}
}