using System;
using UnityEngine;

namespace Lucecita.HappinessBlossom
{
    public class StageSpriteRenderer : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public SpriteRenderer OverlaySpriteRenderer => _overlaySpriteRenderer;
        
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected SpriteRenderer _overlaySpriteRenderer;

        private Vector3 _originPos;
        private Vector3 _originOverlayPos;
        
        protected virtual void Awake()
        {
            _originPos = _spriteRenderer.transform.position;
            _originOverlayPos = _overlaySpriteRenderer.transform.position;
        }

        public void SetSpriteRendererAlpha(float alpha)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, alpha);
            _overlaySpriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, alpha);
        }

        public virtual void ResetSpriteRenderer()
        {
            _spriteRenderer.sprite = null;
            _spriteRenderer.color = new Color(1, 1, 1, 0);
            _spriteRenderer.transform.position = _originPos;
            
            _overlaySpriteRenderer.sprite = null;
            _overlaySpriteRenderer.color = new Color(1, 1, 1, 0);
            _overlaySpriteRenderer.transform.position = _originPos;
        }
    }
   
}