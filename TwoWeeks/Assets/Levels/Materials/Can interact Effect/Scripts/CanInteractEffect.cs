using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Environment
{
    public class ShaderChanger
    {
        private MaterialPropertyBlock PropertyBlock { get; }
        private int ParameterId { get; }
        private Renderer Renderer { get; }

        public ShaderChanger(string parameterName, Renderer renderer)
        {
            PropertyBlock = new MaterialPropertyBlock();
            ParameterId = Shader.PropertyToID(parameterName);
            Renderer = renderer;
        }

        public void SetShaderParameter(bool value)
        {
            if (Renderer == null)
                return;
            Renderer.GetPropertyBlock(PropertyBlock);
            PropertyBlock.SetFloat(ParameterId, value ? 1f : 0f);
            Renderer.SetPropertyBlock(PropertyBlock);
        }
    }

    public class CanInteractEffect : MonoBehaviour
    {
        [SerializeField] private List<Renderer> _targetRenderers = new();
        [SerializeField] private string _parameterName = "_Enable";
        private List<ShaderChanger> _shaderChangers = new();

        private void Start()
        {
            for (var i = 0; i < _targetRenderers.Count; i++)
            {
                _shaderChangers.Add(new ShaderChanger(_parameterName, _targetRenderers[i]));
                _shaderChangers.Last().SetShaderParameter(false);
            }
        }

        public void Activate()
        {
            foreach (var renderer in _shaderChangers)
            {
                renderer.SetShaderParameter(true);
            }
        }

        public void Deactivate()
        {
            foreach (var renderer in _shaderChangers)
            {
                renderer.SetShaderParameter(false);
            }
        }
    }
}